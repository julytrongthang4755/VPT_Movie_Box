using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models
{
    public class BookingSeats
    {
        [Key]
        public int BookingSeatId { get; set; }

        public int BookingId { get; set; }

        public int SeatId { get; set; }

        public Bookings Booking { get; set; }

        public Seats Seat { get; set; }
    }
}