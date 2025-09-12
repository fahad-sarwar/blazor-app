using Bogus;
using OnlineShopUI.ViewModels;
using System.Security.Claims;

namespace OnlineShopUI.Services
{
    public class CheckoutDummyDataService
    {
        public async Task<CheckoutViewModel> GetDummyCheckoutModel(ClaimsPrincipal user, CustomerService customerService)
        {
            var customerId = int.Parse(user.FindFirst("CustomerId")?.Value);
            var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
            var lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
            var email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

            var testUser = new Faker<CheckoutViewModel>("en_GB")
                .RuleFor(c => c.CustomerId, f => customerId)
                .RuleFor(c => c.FirstName, f => firstName)
                .RuleFor(c => c.LastName, f => lastName)
                .RuleFor(c => c.Email, f => email)
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.BillingAddressLineOne, f => f.Address.StreetAddress())
                .RuleFor(c => c.BillingTown, f => f.Address.City())
                .RuleFor(c => c.BillingCounty, f => f.Address.County())
                .RuleFor(c => c.BillingPostCode, f => f.Address.ZipCode("??# #??"))
                .RuleFor(c => c.ShippingAddressLineOne, f => f.Address.StreetAddress())
                .RuleFor(c => c.ShippingTown, f => f.Address.City())
                .RuleFor(c => c.ShippingCounty, f => f.Address.County())
                .RuleFor(c => c.ShippingPostCode, f => f.Address.ZipCode("??# #??"))
                .RuleFor(c => c.CardName, (f, vm) => $"{vm.FirstName} {vm.LastName}")
                .RuleFor(c => c.CardNumber, "4111111111111111")
                .RuleFor(c => c.Expiry, "12/28")
                .RuleFor(c => c.CVV, "123");

            var checkoutViewModel = testUser.Generate();

            var customer = await customerService.GetCustomerAsync();

            if (customer != null)
            {
                if (!string.IsNullOrEmpty(customer.PhoneNumber))
                    checkoutViewModel.PhoneNumber = customer.PhoneNumber;

                if (customer?.ShippingAddress != null)
                {
                    checkoutViewModel.ShippingAddressLineOne = customer.ShippingAddress.AddressLineOne;
                    checkoutViewModel.ShippingAddressLineTwo = customer.ShippingAddress.AddressLineTwo;
                    checkoutViewModel.ShippingTown = customer.ShippingAddress.Town;
                    checkoutViewModel.ShippingCounty = customer.ShippingAddress.County;
                    checkoutViewModel.ShippingPostCode = customer.ShippingAddress.PostCode;
                    checkoutViewModel.ShippingCountry = customer.ShippingAddress.Country;
                }

                if(customer?.BillingAddress != null)
                {
                    checkoutViewModel.BillingAddressLineOne = customer.BillingAddress.AddressLineOne;
                    checkoutViewModel.BillingAddressLineTwo = customer.BillingAddress.AddressLineTwo;
                    checkoutViewModel.BillingTown = customer.BillingAddress.Town;
                    checkoutViewModel.BillingCounty = customer.BillingAddress.County;
                    checkoutViewModel.BillingPostCode = customer.BillingAddress.PostCode;
                    checkoutViewModel.BillingCountry = customer.BillingAddress.Country;
                }
            }

            return checkoutViewModel;
        }
    }
}
