using AttendanceManagement.Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Input_Models
{
    public class StoredProcedureDbContext : DbContext
    {
        public StoredProcedureDbContext()
        {
        }

        public StoredProcedureDbContext(DbContextOptions<StoredProcedureDbContext> options)
            : base(options)
        {
        }

        public DbSet<CommonPermissionResponse> CommonPermissionResponses { get; set; } = null!;
        public DbSet<StaffDto> StaffDto { get; set; } = null!;
        public DbSet<LeaveRequisitionResponse> LeaveRequisitionResponses { get; set; } = null!;
        public DbSet<LeaveTakenResponse> LeaveTakenResponses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonPermissionResponse>().HasNoKey();
        }
    }
}
