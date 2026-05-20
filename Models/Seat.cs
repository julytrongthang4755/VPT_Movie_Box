using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models
{
    public class Seats
    {
        [Key]
        public int SeatId { get; set; }

        public int ScreenId { get; set; }

        public char SeatRow { get; set; }

        public int SeatNumber { get; set; }

        public string SeatType { get; set; }
    }
}