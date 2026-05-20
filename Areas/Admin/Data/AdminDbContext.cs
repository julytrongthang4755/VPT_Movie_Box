using Microsoft.EntityFrameworkCore;
using VPT_Movie_Box.Models;

namespace VPT_Movie_Box.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options)
            : base(options) { }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<MoviesViewModel> Movies { get; set; }
    }
}