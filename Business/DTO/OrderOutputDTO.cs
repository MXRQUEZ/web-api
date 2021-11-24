using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Entities;

namespace Business.DTO
{
    public sealed class OrderOutputDTO
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
