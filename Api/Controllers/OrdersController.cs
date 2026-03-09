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
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly BasketRepository _basketRepository;
        private readonly TaxRateRepository _taxRateRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly BackgroundOrderQueue _queue;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderRepository orderRepository, CustomerRepository customerRepository, BasketRepository basketRepository,
            TaxRateRepository taxRateRepository, PaymentRepository paymentRepository, BackgroundOrderQueue queue, ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _basketRepository = basketRepository;
            _taxRateRepository = taxRateRepository;
            _paymentRepository = paymentRepository;
            _queue = queue;
            _logger = logger;
        }

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

                var customer = await _customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                var (orders, totalCount) = await _orderRepository.GetOrdersByCustomerId(customer.Id, orderNumber, page, pageSize);

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
                _logger.LogError(ex, "There was an error getting a list of orders.");
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

                var customer = await _customerRepository.GetCustomerByEmail(email);

                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var order = await _orderRepository.GetOrder(id, customer.Id);

                return order == null
                    ? NotFound()
                    : Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting an order with id {OrderId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO createOrderRequest)
        {
            try
            {
                var basket = await _basketRepository.GetBasketById(createOrderRequest.BasketId);

                if (basket == null)
                {
                    return BadRequest("Basket not found.");
                }

                if (basket.Items.Count == 0)
                {
                    return BadRequest("Basket is empty.");
                }

                var taxRate = await _taxRateRepository.GetCurrentTaxRate();

                if (taxRate == null)
                {
                    return BadRequest("No tax rate found.");
                }

                var customer = await _customerRepository.GetCustomerById(createOrderRequest.Customer.Id);

                if (customer == null)
                {
                    return BadRequest("Customer not found");
                }

                if (customer.BillingAddress == null && customer.ShippingAddress == null)
                {
                    var customerBillingAddress = CopyAddress(createOrderRequest.Customer.BillingAddress);
                    var customerShippingAddress = CopyAddress(createOrderRequest.Customer.ShippingAddress);

                    customer.BillingAddress = await _customerRepository.CreateAddress(customerBillingAddress);
                    customer.ShippingAddress = await _customerRepository.CreateAddress(customerShippingAddress);
                }

                if (string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    customer.PhoneNumber = createOrderRequest.Customer.PhoneNumber;
                }

                await _customerRepository.UpdateCustomer(customer);

                var totalPrice = basket.Items.Sum(bi => bi.TotalPrice);

                var orderBillingAddress = await _customerRepository.CreateAddress(CopyAddress(createOrderRequest.Customer.BillingAddress));
                var orderShippingAddress = await _customerRepository.CreateAddress(CopyAddress(createOrderRequest.Customer.ShippingAddress));

                var payment = await _paymentRepository.CreatePayment(new Payment
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

                var createdOrder = await _orderRepository.CreateOrder(order);

                await _orderRepository.UpdateOrderNumber(createdOrder.Id, $"ORD{createdOrder.Id:D7}");

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

                await _orderRepository.CreateOrderItems(orderItems);

                await _basketRepository.RemoveAllBasketItems(basket.Id);
                await _basketRepository.DeleteBasket(basket.Id);

                _queue.Enqueue(createdOrder.Id);

                var completeOrder = await _orderRepository.GetOrder(createdOrder.Id, customer.Id);
                return Ok(completeOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating the order.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private static Address CopyAddress(CreateAddressDTO addressDto)
        {
            return new Address
            {
                AddressLineOne = addressDto.AddressLineOne,
                AddressLineTwo = addressDto.AddressLineTwo,
                Town = addressDto.Town,
                County = addressDto.County,
                PostCode = addressDto.PostCode,
                Country = addressDto.Country,
            };
        }
    }
}
