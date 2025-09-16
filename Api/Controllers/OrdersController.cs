using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.DTOs;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OnlineShopContext context, BackgroundOrderQueue queue, ILogger<OrdersController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string? orderNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(email))
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var query = context.Order
                    .Include(o => o.Customer)
                    .Where(o => o.Customer.Id == customer.Id)
                    .OrderByDescending(w => w.CreatedAt)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(orderNumber))
                    query = query.Where(o => o.OrderNumber == orderNumber);

                var totalCount = await query.CountAsync();

                var paged = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(
                    new
                    {
                        Orders = paged.ToList(),
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
                    return Unauthorized();

                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);

                if (customer == null)
                    return NotFound("Customer not found.");

                var order = await GetOrder(id, customer.Id);

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
                var basket = await context.Basket
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Product)
                .SingleOrDefaultAsync(b => b.Id == createOrderRequest.BasketId);

                var taxRate = await context.TaxRate
                    .Where(t =>
                        t.EffectiveFrom <= DateTime.UtcNow &&
                        (t.EffectiveTo == null || t.EffectiveTo > DateTime.UtcNow))
                    .OrderByDescending(t => t.EffectiveFrom)
                    .FirstOrDefaultAsync();

                var customer = await context.Customer
                    .SingleOrDefaultAsync(c => c.Id == createOrderRequest.Customer.Id);

                if (basket == null)
                    return BadRequest("Basket not found");

                if (taxRate == null)
                    return BadRequest("No applicable tax rate found");

                if (basket.Items.Count == 0)
                    return BadRequest("Basket is empty");

                if (customer == null)
                    return BadRequest("Customer not found");

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

                    context.Address.Add(customerBillingAddress);
                    context.Address.Add(customerShippingAddress);
                    await context.SaveChangesAsync();

                    customer.BillingAddress = customerBillingAddress;
                    customer.ShippingAddress = customerShippingAddress;
                }

                if(string.IsNullOrEmpty(customer.PhoneNumber))
                    customer.PhoneNumber = createOrderRequest.Customer.PhoneNumber;

                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();

                var totalPrice = basket.Items.Sum(bi => bi.TotalPrice);

                var orderBillingAddress = new Address
                {
                    AddressLineOne = createOrderRequest.Customer.BillingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.BillingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.BillingAddress.Town,
                    County = createOrderRequest.Customer.BillingAddress.County,
                    PostCode = createOrderRequest.Customer.BillingAddress.PostCode,
                    Country = createOrderRequest.Customer.BillingAddress.Country,
                };

                var orderShippingAddress = new Address
                {
                    AddressLineOne = createOrderRequest.Customer.ShippingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.ShippingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.ShippingAddress.Town,
                    County = createOrderRequest.Customer.ShippingAddress.County,
                    PostCode = createOrderRequest.Customer.ShippingAddress.PostCode,
                    Country = createOrderRequest.Customer.ShippingAddress.Country,
                };

                context.Address.Add(orderBillingAddress);
                context.Address.Add(orderShippingAddress);
                await context.SaveChangesAsync();

                var order = new Order
                {
                    Customer = customer,
                    BillingAddress = orderBillingAddress,
                    ShippingAddress = orderShippingAddress,
                    TotalPrice = totalPrice,
                    VATRate = taxRate.Rate,
                    Status = "Pending",
                    Payment = new Payment
                    {
                        Amount = totalPrice,
                        PaymentMethod = "Credit Card",
                        CardName = createOrderRequest.Payment.CardName,
                        CardNumber = createOrderRequest.Payment.CardNumber,
                        Expiry = createOrderRequest.Payment.Expiry,
                        CVV = createOrderRequest.Payment.CVV,
                    },
                    DeliveryMethod = "Royal Mail",
                    EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                    ContactPhoneNumber = customer.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                context.Order.Add(order);
                await context.SaveChangesAsync();

                order.OrderNumber = $"ORD{order.Id:D7}";
                await context.SaveChangesAsync();

                var orderItems = basket.Items.Select(bi => new OrderItem
                {
                    OrderId = order.Id,
                    Product = bi.Product,
                    Quantity = bi.Quantity,
                    UnitPrice = bi.Price,
                    TotalPrice = bi.TotalPrice,
                    VATRate = taxRate.Rate,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

                context.OrderItem.AddRange(orderItems);
                await context.SaveChangesAsync();

                context.BasketItem.RemoveRange(basket.Items);
                context.Basket.Remove(basket);
                await context.SaveChangesAsync();

                queue.Enqueue(order.Id);

                var createdOrder = await GetOrder(order.Id, customer.Id);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating order");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<Order> GetOrder(int orderId, int customerId)
        {
            var order = await context.Order
                .Include(o => o.Customer)
                .Include(o => o.BillingAddress)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.TrackingUpdates)
                .SingleOrDefaultAsync(o => o.Id == orderId && o.Customer.Id == customerId);

            return order;
        }
    }
}
