using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Models;

namespace ReservationSystem.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Hotel> Hotels { get; set; } 
        public DbSet<Room> Rooms { get; set; }  
        public DbSet<RoomInventory> RoomInventories { get; set; }  
        public DbSet<RoomTypePrice> RoomTypePrices { get; set; }  
        public DbSet<Guest> Guests { get; set; }  
    }
}
