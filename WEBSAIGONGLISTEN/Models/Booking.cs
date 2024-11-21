using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBSAIGONGLISTEN.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Range(0, 100)]
        public int QuantityUnder2 { get; set; }

        [Range(0, 100)]
        public int Quantity2To10 { get; set; }

        [Range(0, 100)]
        public int QuantityAbove10 { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Trạng thái: Pending, Paid, Canceled

        public string? CancellationReason { get; set; } // Lý do hủy
        public DateTime? CancellationDate { get; set; } // Ngày hủy
        public DateTime? AdminProcessedDate { get; set; } // Ngày admin xử lý
    }
}
