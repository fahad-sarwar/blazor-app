using System.Security.Claims;
using Api.Data;
using Api.Models;
using Api.Models.Db;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OnlineShopContext context, BackgroundOrderQueue queue) : ControllerBase
    {
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<PagedOrderResult>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return NotFound("Customer not found.");

            var query = context.Order
                .Include(o => o.Customer)
                .Where(o => o.Customer.Id == customer.Id)
                .OrderByDescending(w => w.CreatedAt)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var paged = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedOrderResult
            {
                Orders = paged.ToList(),
                TotalCount = totalCount
            };
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return NotFound("Customer not found.");

            var order = await context.Order
                .Include(o => o.Customer)
                    .ThenInclude(c => c.ShippingAddress)
                .Include(o => o.Customer)
                    .ThenInclude(c => c.BillingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.TrackingUpdates)
                .SingleOrDefaultAsync(o => o.Id == id && o.Customer.Id == customer.Id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostBasketItem(CreateOrderRequest createOrderRequest)
        {
            // Check basket & tax rate exist
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
            {
                return BadRequest("Basket not found");
            }

            if (taxRate == null)
            {
                return BadRequest("No applicable tax rate found");
            }

            if (basket.Items.Count == 0)
            {
                return BadRequest("Basket is empty");
            }

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            try
            {
                // Create customer addresses
                var billingAddress = new Address
                {
                    AddressLineOne = createOrderRequest.Customer.BillingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.BillingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.BillingAddress.Town,
                    County = createOrderRequest.Customer.BillingAddress.County,
                    PostCode = createOrderRequest.Customer.BillingAddress.PostCode,
                    Country = createOrderRequest.Customer.BillingAddress.Country,
                };

                var shippingAddress = new Address
                {
                    AddressLineOne = createOrderRequest.Customer.ShippingAddress.AddressLineOne,
                    AddressLineTwo = createOrderRequest.Customer.ShippingAddress.AddressLineTwo,
                    Town = createOrderRequest.Customer.ShippingAddress.Town,
                    County = createOrderRequest.Customer.ShippingAddress.County,
                    PostCode = createOrderRequest.Customer.ShippingAddress.PostCode,
                    Country = createOrderRequest.Customer.ShippingAddress.Country,
                };

                context.Address.Add(billingAddress);
                context.Address.Add(shippingAddress);
                await context.SaveChangesAsync();

                customer.PhoneNumber = createOrderRequest.Customer.PhoneNumber;
                customer.BillingAddress = billingAddress;
                customer.ShippingAddress = shippingAddress;

                context.Entry(customer).State = EntityState.Modified;
                await context.SaveChangesAsync();
               
                // Create order
                var totalPrice = basket.Items.Sum(bi => bi.TotalPrice);
                var totalVAT = totalPrice * taxRate.Rate / 100;
                totalPrice += totalVAT; // Add VAT to total price

                var order = new Order
                {
                    Customer = customer,
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
                    EstimatedDelivery = DateTime.UtcNow.AddDays(3), // Example estimated delivery
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                context.Order.Add(order);
                await context.SaveChangesAsync();

                order.OrderNumber = $"ORD{order.Id:D7}";
                await context.SaveChangesAsync(); // Save the OrderNumber

                // Create order items
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

                context.BasketItem.RemoveRange(basket.Items); // Clear the basket items
                context.Basket.Remove(basket); // Clear the basket after order creation
                await context.SaveChangesAsync();

                queue.Enqueue(order.Id); // Enqueue the order for background processing

                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Order creation failed: {ex.Message}");
            }
        }
    }
}
