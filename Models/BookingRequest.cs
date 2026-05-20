namespace VPT_Movie_Box.Models
{
    public class BookingRequest
    {
        public int ShowtimeId { get; set; }
        public List<int> SeatIds { get; set; }
    }
}
