using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.API.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }  
        public string CustomerName { get; set; }  

        public List<OrderItemToReturnDto> OrderItems { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
}
