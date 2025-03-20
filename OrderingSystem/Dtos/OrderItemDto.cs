﻿using System.ComponentModel.DataAnnotations;

namespace OrderingSystem.API.Dtos
{
    public class OrderItemDto
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }

      

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

    }
}
