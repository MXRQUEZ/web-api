using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.Entities
{
    public sealed class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
