namespace Api.Models.Db
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public Address BillingAddress { get; set; } //navigation property to HouseAddress table
        public Address ShippingAddress { get; set; } //navigation property to HouseAddress table
    }
}
