using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.API.Dtos
{
    public class OrderItemToReturnDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
