using LabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LabManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabBorrowingRequest> LabBorrowingRequests { get; set; }
        public DbSet<RoomBookingRequest> RoomBookingRequests { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
          
        }
    }
}
