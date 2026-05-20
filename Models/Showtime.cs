using System;

namespace VPT_Movie_Box.Models
{
    public class Showtime
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        public int ScreenId { get; set; }

        public DateTime StartTime { get; set; }

        public decimal Price { get; set; }

        public Screen Screen { get; set; }

        public virtual Movie Movie { get; set; }

        public DateTime EndTime { get; set; }
    }
}