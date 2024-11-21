using System.Collections.Generic;

namespace WEBSAIGONGLISTEN.Models
{
    public class ProductDisplayViewModel
    {
        public Product Product { get; set; }
        public List<TourDay>? TourDays { get; set; }

        public int UserID { get; set; }
    }
}