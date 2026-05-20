using Microsoft.AspNetCore.Mvc;

namespace VPT_Movie_Box.Models
{
    public class ProfileViewModel
    {
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? SocialLink1 { get; set; } // Link Facebook
        public string? SocialLink2 { get; set; } // Link Twitter/X
        public string? SocialLink3 { get; set; }
    }
}
