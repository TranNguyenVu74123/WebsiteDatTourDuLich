using System;
using System.ComponentModel.DataAnnotations;

namespace WEBSAIGONGLISTEN.Models
{
    public class BookingHistory
    {
        public int Id { get; set; }

        public string UserId { get; set; } // Sử dụng kiểu string thay vì int

        public int ProductId { get; set; } // ID của tour

        public Product Product { get; set; } // Thông tin về tour

        [Required]
        public DateTime BookingDate { get; set; } // Ngày đặt tour

        [Required]
        public DateTime StartDate { get; set; } // Ngày khởi hành

        [Required]
        public DateTime EndDate { get; set; } // Ngày kết thúc

        [Required]
        public BookingStatus Status { get; set; } // Trạng thái tour
    }

    public enum BookingStatus
    {
        DaDi, // Đã đi
        DaHuy, // Đã hủy
        DangDi // Đang đi
    }
}
