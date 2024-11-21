using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBSAIGONGLISTEN.Models;

namespace WEBSAIGONGLISTEN.Models
{
    public class TourDay
    {
        public int Id { get; set; }

        // Số thứ tự của ngày trong tour
        [Required]
        public int DayNumber { get; set; }

        // Mô tả hoạt động của ngày đó
        [Required, StringLength(500)]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }

        // Mối quan hệ với Category (một Category có thể có nhiều ngày tour)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Thêm thuộc tính ProductId
        public int ProductId { get; set; }

        public Product? Product { get; set; }

    }
}