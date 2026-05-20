using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models
{
    public class Cinemas
    {
        [Key]
        public int CinemaId { get; set; }

        public string CinemaName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}
