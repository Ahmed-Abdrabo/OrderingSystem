using Microsoft.AspNetCore.Identity;

namespace OrderingSystem.Core.Entities.Identity
{
    public class Customer : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
