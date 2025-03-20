namespace OrderingSystem.Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string CustomerId { get; set; }  
        public Customer User { get; set; }
        public string Country { get; set; }
    }
}