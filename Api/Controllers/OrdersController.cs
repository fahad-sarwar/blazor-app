using Api.Data;
using Api.Models;
using Api.Models.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OnlineShopContext _context;

        public OrdersController(OnlineShopContext context)
        {
            _context = context;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .SingleOrDefaultAsync(o => o.Id == id);

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
            var basket = await _context.Basket
                .Include(b => b.Items)
                .ThenInclude(bi => bi.Product)
                .SingleOrDefaultAsync(b => b.Id == createOrderRequest.BasketId);

            var taxRate = await _context.TaxRate
                .Where(t =>
                    t.EffectiveFrom <= DateTime.UtcNow &&
                    (t.EffectiveTo == null || t.EffectiveTo > DateTime.UtcNow))
                .OrderByDescending(t => t.EffectiveFrom)
                .FirstOrDefaultAsync();

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

                _context.Address.Add(billingAddress);
                _context.Address.Add(shippingAddress);
                await _context.SaveChangesAsync();

                // Create customer
                var customer = new Customer
                {
                    UserName = createOrderRequest.Customer.Email,
                    FirstName = createOrderRequest.Customer.FirstName,
                    LastName = createOrderRequest.Customer.LastName,
                    Email = createOrderRequest.Customer.Email,
                    PhoneNumber = createOrderRequest.Customer.PhoneNumber,
                    BillingAddress = billingAddress,
                    ShippingAddress = shippingAddress,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

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
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                order.OrderNumber = $"ORD{order.Id:D7}";
                await _context.SaveChangesAsync(); // Save the OrderNumber

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

                _context.OrderItem.AddRange(orderItems);
                await _context.SaveChangesAsync();

                _context.BasketItem.RemoveRange(basket.Items); // Clear the basket items
                _context.Basket.Remove(basket); // Clear the basket after order creation
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Order creation failed: {ex.Message}");
            }
        }
    }
}
