using LabManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LabManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceBorrowingRequest> DeviceBorrowingRequests { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabBorrowingRequest> LabBorrowingRequests { get; set; }
        public DbSet<RoomBookingRequest> RoomBookingRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
    }
}
