using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.API.Dtos;
using OrderingSystem.API.Errors;
using OrderingSystem.API.Extensions;
using OrderingSystem.Core.Entities.Identity;
using OrderingSystem.Core.Services.Contract;
using System.Security.Claims;

namespace OrderingSystem.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IAuthService atuhService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = atuhService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<CustomerDto>> Login(LoginDto model)
        {
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401, "Invalid email or password"));
            }
            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Customer"; // Default to "Customer" if no role is found

            return Ok(new CustomerDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = role,
                Token =await _authService.GenerateTokenAsync(user, _userManager)
            }); 
        }


        [HttpPost("register")]
        public async Task<ActionResult<CustomerDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[] { "This Email Already Exists" } });
            }

            var user = new Customer()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiValidationErrorResponse { Errors = errors });
            }

            // Assign "Customer" role to the newly created user
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Error assigning role to user"));
            }

            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Customer"; // Default to "Customer" if no role is found

            return Ok(new CustomerDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = role,
                Token = await _authService.GenerateTokenAsync(user, _userManager)
            });
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault() ?? "Customer"; // Default to "Customer" if no role is found

            return Ok(new CustomerDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = role,
                Token = await _authService.GenerateTokenAsync(user, _userManager)
            });
        }
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);   
            var address= _mapper.Map<AddressDto>(user.Address);
            return Ok(address);
        }

        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<Address>(updatedAddress);

            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user.Address != null)
            {
                address.Id = user.Address.Id;
            }

            user.Address = address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
