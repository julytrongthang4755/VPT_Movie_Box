using Microsoft.EntityFrameworkCore;
using VPT_Movie_Box.Models;

namespace VPT_Movie_Box.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Screen> Screens { get; set; }

        public DbSet<Showtime> Showtimes { get; set; }

        public DbSet<Seats> Seats { get; set; }

        public DbSet<Bookings> Bookings { get; set; }

        public DbSet<BookingSeats> BookingSeats { get; set; }

        public DbSet<Contact> Contacts { get; set; }
    }
}