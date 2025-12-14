using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CheckoutService
    {
        public async Task<CheckoutViewModel> GetDummyCheckoutModel(UserInfoViewModel? userInfo, CustomerService customerService)
        {
            var checkoutViewModel = new CheckoutViewModel
            {
                CustomerId = userInfo.CustomerId.Value,
                FirstName = userInfo.FirstName ?? string.Empty,
                LastName = userInfo.LastName ?? string.Empty,
                Email = userInfo.Email ?? string.Empty
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
