using Microsoft.AspNetCore.Identity;
using OrderingSystem.Core.Entities.Identity;

namespace OrderingSystem.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string> GenerateTokenAsync(Customer User, UserManager<Customer> userManager);
    }
}
