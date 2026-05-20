using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models // Thay bằng tên Project của bạn
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? ShowtimeId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? Status { get; set; }
    }
}