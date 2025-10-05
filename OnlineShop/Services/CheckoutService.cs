using OnlineShopUI.ViewModels;
using System.Security.Claims;

namespace OnlineShopUI.Services
{
    public class CheckoutService
    {
        public async Task<CheckoutViewModel> GetDummyCheckoutModel(ClaimsPrincipal user, CustomerService customerService)
        {
            var customerId = int.Parse(user.FindFirst("CustomerId")?.Value);
            var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
            var lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
            var email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

            var checkoutViewModel = new CheckoutViewModel
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var customer = await customerService.GetCustomer();

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
