using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.API.Dtos;
using OrderingSystem.API.Errors;
using OrderingSystem.Core.Entities.Order;
using OrderingSystem.Core.Services.Contract;
using System.Security.Claims;

namespace OrderingSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(new ApiResponse(401, "Unauthorized user"));


            var order = _mapper.Map<Order>(orderDto);
            order.CustomerId = customerId;
            order = await _orderService.CreateOrderAsync(order);

            if (order is null)
                return BadRequest(new ApiResponse(400, "Failed to create order"));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [HttpGet("GetOrderForCustomer")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(new ApiResponse(401, "Unauthorized user"));

            var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(new ApiResponse(401, "Unauthorized user"));

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order is null)
                return NotFound(new ApiResponse(404, "Order not found"));

            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin"); // Check if the user is an admin

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized(new ApiResponse(401, "Unauthorized user"));

            var result = await _orderService.DeleteOrderAsync(id, customerId, isAdmin);
            if (!result)
                return NotFound(new ApiResponse(404, "Order not found or does not belong to the customer"));

            return NoContent();
        }
    }
}
