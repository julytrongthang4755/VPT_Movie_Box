namespace VPT_Movie_Box.Models
{
    public class BookingHistoryViewModel
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public string MovieTitle { get; set; }
        public string ScreenName { get; set; }

        public DateTime StartTime { get; set; }
        public string Seats { get; set; }

        public string SRow { get; set; }

    }
}
