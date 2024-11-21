﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBSAIGONGLISTEN.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public string HouseAddress { get; set; }
        public string PickUpAddress { get; set; }
        public string Notes { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
