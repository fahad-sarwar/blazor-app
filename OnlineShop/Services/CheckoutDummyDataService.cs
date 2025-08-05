using Bogus;
using CountryData.Bogus;
using OnlineShopUI.ViewModels;

namespace OnlineShopUI.Services
{
    public class CheckoutDummyDataService
    {
        public CheckoutViewModel GetDummyCheckoutModel()
        {
            // This method returns a dummy checkout model with pre-filled data.

            var testUser = new Faker<CheckoutViewModel>("en_GB")
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
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
                .RuleFor(c => c.CardNumber, "4111111111111111") // Test card number
                .RuleFor(c => c.Expiry, "12/28")
                .RuleFor(c => c.CVV, "123");

            return testUser.Generate();
        }
    }
}
