using System;
using System.ComponentModel.DataAnnotations;

namespace VPT_Movie_Box.Models
{
    public class MoviesViewModel
    {
        [Key]
        public int MovieId { get; set; } // Khóa chính không được null (đúng)

        public string? Title { get; set; }
        public string? Description { get; set; }

        // LỖI THƯỜNG Ở ĐÂY: Nếu trong DB cột Duration hoặc ReleaseDate bị NULL 
        // mà bạn không để dấu '?' thì EF sẽ báo lỗi ngay khi .ToListAsync()
        public int? Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public string? Genre { get; set; }
        public string? Language { get; set; }
        public string? AgeRating { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public string? Status { get; set; }
    }
}
