using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Core.Entities.Identity;
using System.Security.Claims;

namespace OrderingSystem.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<Customer?> FindUserWithAddressAsync(this UserManager<Customer> userManager,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await userManager.Users.Include(u=>u.Address).SingleOrDefaultAsync(u=>u.Email == email);
            return user;
        }
    }
}
