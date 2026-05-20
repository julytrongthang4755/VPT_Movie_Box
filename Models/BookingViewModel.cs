namespace VPT_Movie_Box.Models
{
    public class BookingViewModel
    {
        public List<Showtime> Showtimes { get; set; }

        public int SelectedShowtimeId { get; set; }

        public string MovieTitle { get; set; }
        public string PosterUrl { get; set; }

        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }

        public string ScreenName { get; set; }
        public string SeatRow { get; set; }
        public int SeatNumber { get; set; }

        public DateTime EndTime { get; set; }

        public int BookingId { get; set; }
    }
}
