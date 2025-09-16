using System.Security.Claims;
using Api.Models;
using Api.Models.DTOs;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository orderRepository, ICustomerRepository customerRepository, IBasketRepository basketRepository,
        ITaxRateRepository taxRateRepository, IAddressRepository addressRepository, IPaymentRepository paymentRepository, IOrderItemRepository orderItemRepository,
        BackgroundOrderQueue queue, ILogger<OrdersController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? orderNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var (orders, totalCount) = await orderRepository.GetOrdersByCustomerId(customer.Id, orderNumber, page, pageSize);

                return Ok(
                    new
                    {
                        Orders = orders,
                        TotalCount = totalCount
                    }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving orders");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized();
                }

                var customer = await customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var order = await orderRepository.GetOrder(id, customer.Id);

                return order == null
                    ? NotFound()
                    : Ok(order);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving order with id {OrderId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO createOrderRequest)
        {
            try
            {
                var basket = await basketRepository.GetBasketById(createOrderRequest.BasketId);

                if (basket == null)
                {
                    return BadRequest("Basket not found");
                }

                if (basket.Items.Count == 0)
                {
                    return BadRequest("Basket is empty");
                }

                var taxRate = await taxRateRepository.GetCurrentTaxRate();

                if (taxRate == null)
                {
                    return BadRequest("No applicable tax rate found");
                }

                var customer = await customerRepository.GetCustomerById(createOrderRequest.Customer.Id);

                if (customer == null)
                {
                    return BadRequest("Customer not found");
                }

                if (customer.BillingAddress == null && customer.ShippingAddress == null)
                {
                    var customerBillingAddress = new Address
                    {
                        AddressLineOne = createOrderRequest.Customer.BillingAddress.AddressLineOne,
                        AddressLineTwo = createOrderRequest.Customer.BillingAddress.AddressLineTwo,
                        Town = createOrderRequest.Customer.BillingAddress.Town,
                        County = createOrderRequest.Customer.BillingAddress.County,
                        PostCode = createOrderRequest.Customer.BillingAddress.PostCode,
                        Country = createOrderRequest.Customer.BillingAddress.Country,
                    };

                    var customerShippingAddress = new Address
                    {
                        AddressLineOne = createOrderRequest.Customer.ShippingAddress.AddressLineOne,
                        AddressLineTwo = createOrderRequest.Customer.ShippingAddress.AddressLineTwo,
                        Town = createOrderRequest.Customer.ShippingAddress.Town,
                        County = createOrderRequest.Customer.ShippingAddress.County,
                        PostCode = createOrderRequest.Customer.ShippingAddress.PostCode,
                        Country = createOrderRequest.Customer.ShippingAddress.Country,
                    };

                    customer.BillingAddress = await addressRepository.CreateAddress(customerBillingAddress);
                    customer.ShippingAddress = await addressRepository.CreateAddress(customerShippingAddress);
                }

                if (string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    customer.PhoneNumber = createOrderRequest.Customer.PhoneNumber;
                }

                await customerRepository.UpdateCustomer(customer);

                var totalPrice = basket.Items.Sum(bi => bi.TotalPrice);

                var orderBillingAddress = await addressRepository.CreateAddress(new Address
                {
                    AddressLineOne = createOrderRequest.Customer.BillingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.BillingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.BillingAddress.Town,
                    County = createOrderRequest.Customer.BillingAddress.County,
                    PostCode = createOrderRequest.Customer.BillingAddress.PostCode,
                    Country = createOrderRequest.Customer.BillingAddress.Country,
                });

                var orderShippingAddress = await addressRepository.CreateAddress(new Address
                {
                    AddressLineOne = createOrderRequest.Customer.ShippingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.ShippingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.ShippingAddress.Town,
                    County = createOrderRequest.Customer.ShippingAddress.County,
                    PostCode = createOrderRequest.Customer.ShippingAddress.PostCode,
                    Country = createOrderRequest.Customer.ShippingAddress.Country,
                });

                var payment = await paymentRepository.CreatePayment(new Payment
                {
                    Amount = totalPrice,
                    PaymentMethod = "Credit Card",
                    CardName = createOrderRequest.Payment.CardName,
                    CardNumber = createOrderRequest.Payment.CardNumber,
                    Expiry = createOrderRequest.Payment.Expiry,
                    CVV = createOrderRequest.Payment.CVV,
                    CreatedAt = DateTime.UtcNow
                });

                var order = new Order
                {
                    Customer = customer,
                    BillingAddress = orderBillingAddress,
                    ShippingAddress = orderShippingAddress,
                    TotalPrice = totalPrice,
                    VATRate = taxRate.Rate,
                    Status = "Pending",
                    Payment = payment,
                    DeliveryMethod = "Royal Mail",
                    EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                    ContactPhoneNumber = customer.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                var createdOrder = await orderRepository.CreateOrder(order);

                await orderRepository.UpdateOrderNumber(createdOrder.Id, $"ORD{createdOrder.Id:D7}");

                var orderItems = new List<OrderItem>();

                foreach (var basketItem in basket.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = createdOrder.Id,
                        Product = basketItem.Product,
                        Quantity = basketItem.Quantity,
                        UnitPrice = basketItem.Price,
                        TotalPrice = basketItem.TotalPrice,
                        VATRate = taxRate.Rate,
                        CreatedAt = DateTime.UtcNow
                    };

                    orderItems.Add(orderItem);
                }

                await orderItemRepository.CreateOrderItems(orderItems);

                await basketRepository.RemoveAllBasketItems(basket.Id);
                await basketRepository.DeleteBasket(basket.Id);

                queue.Enqueue(createdOrder.Id);

                var completeOrder = await orderRepository.GetOrder(createdOrder.Id, customer.Id);
                return Ok(completeOrder);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
