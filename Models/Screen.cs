namespace VPT_Movie_Box.Models
{
    public class Screen
    {
        public int ScreenId { get; set; }

        public string ScreenName { get; set; }

        public int TotalSeats { get; set; }

        public List<Showtime> Showtimes { get; set; }

    }
}