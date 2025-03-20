using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.API.Dtos
{
    public class OrderDto
    {
        [Required]
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public decimal TotalAmount { get; set; }
    }
}
