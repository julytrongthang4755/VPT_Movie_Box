namespace VPT_Movie_Box.Models
{
        public class ETicketViewModel
        {
            public string MovieTitle { get; set; }
            public string PosterUrl { get; set; }

            public string ScreenName { get; set; }

            public string SeatRow { get; set; }
            public string SeatNumber { get; set; }

            public DateTime EndTime { get; set; }
            public decimal Price { get; set; }

            public DateTime StartTime { get; set; }

            public int BookingId { get; set; }
        }
}
