using System;
using System.Collections.Generic;

namespace DAL.Models.Entities
{
    public sealed class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
