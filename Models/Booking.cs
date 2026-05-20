using System;
using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int ShowtimeId { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public virtual Showtime Showtime { get; set; }
        public ICollection<BookingSeats> BookingSeats { get; set; }
    }
}