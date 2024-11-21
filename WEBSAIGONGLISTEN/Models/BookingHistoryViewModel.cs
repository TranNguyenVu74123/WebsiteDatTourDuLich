namespace WEBSAIGONGLISTEN.Models
{
    public class BookingHistoryViewModel
    {
        public int BookingId { get; set; }
        public string TourName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }
    }
}
