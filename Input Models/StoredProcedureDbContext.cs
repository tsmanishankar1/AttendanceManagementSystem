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
        public DbSet<AbsentListResponse> AbsentListResponses { get; set; } = null!;
        public DbSet<ContinuousAbsentListResponse> ContinuousAbsentListResponses { get; set; } = null!;
        public DbSet<CompOffAvailResponse> CompOffAvailResponses { get; set; } = null!;
        public DbSet<CompOffCreditResponse> CompOffCreditResponses { get; set; } = null!;
        public DbSet<CommonPermissionResponse> CommonPermissionResponses { get; set; } = null!;
        public DbSet<StaffDto> StaffDto { get; set; } = null!;
        public DbSet<FirstInLastOutResponse> FirstInLastOutResponses { get; set; } = null!;
        public DbSet<LeaveRequisitionResponse> LeaveRequisitionResponses { get; set; } = null!;
        public DbSet<LeaveTakenResponse> LeaveTakenResponses { get; set; } = null!;
        public DbSet<ManualPunchResponse> ManualPunchResponse { get; set; } = null!;
        public DbSet<PresentListResponse> PresentListResponses { get; set; } = null!;
        public DbSet<OnDutyRequisitionResponse> OnDutyRequisitionResponses { get; set; } = null!;
        public DbSet<RawPunchResponse> RawPunchResponses { get; set; } = null!;
        public DbSet<CurrentDaySwipeInResponse> CurrentDaySwipeInResponses { get; set; } = null!;
        public DbSet<BusinessTravelResponse> BusinessTravelResponses { get; set; } = null!;
        public DbSet<NightShiftCountResponse> NightShiftCountResponses { get; set; } = null!;
        public DbSet<DailyPerformanceResponse> DailyPerformanceResponses { get; set; } = null!;
        public DbSet<WorkFromHomeResponse> workFromHomeResponses { get; set; } = null!;
        public DbSet<AttendanceResponse> AttendanceResponses { get; set; } = null!;
        public DbSet<LeaveBalanceResponse> LeaveBalanceResponses { get; set; } = null!;
        public DbSet<WeeklyOffHolidayWorkingResponse> WeeklyOffHolidayWorkingResponses { get; set; } = null!;
        public DbSet<VaccinationReportResponse> VaccinationReportResponses { get; set; } = null!;
        public DbSet<ShiftExtensionResponse> shiftExtensionResponses { get; set; } = null!;
        public DbSet<PermissionRequisitionResponse> PermissionRequisitionResponses { get; set; } = null!;
        public DbSet<MonthlyReportResponse> MonthlyReportResponse { get; set; } = null!;
        public DbSet<AttendanceRecordDto> attendanceRecordDtos { get; set; } = null!;
        public DbSet<StatutoryReportResponse> statutoryReportResponses { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AbsentListResponse>().HasNoKey();
            modelBuilder.Entity<AttendanceResponse>().HasNoKey();
            modelBuilder.Entity<ContinuousAbsentListResponse>().HasNoKey();
            modelBuilder.Entity<CompOffAvailResponse>().HasNoKey();
            modelBuilder.Entity<CompOffCreditResponse>().HasNoKey();
            modelBuilder.Entity<CommonPermissionResponse>().HasNoKey();
            modelBuilder.Entity<FirstInLastOutResponse>().HasNoKey();
            modelBuilder.Entity<LeaveTakenResponse>().HasNoKey();
            modelBuilder.Entity<DailyPerformanceResponse>().HasNoKey();
            modelBuilder.Entity<LeaveRequisitionResponse>().HasNoKey();
            modelBuilder.Entity<CurrentDaySwipeInResponse>().HasNoKey();
            modelBuilder.Entity<NightShiftCountResponse>().HasNoKey();
            modelBuilder.Entity<RawPunchResponse>().HasNoKey();
            modelBuilder.Entity<ManualPunchResponse>().HasNoKey();
            modelBuilder.Entity<PresentListResponse>().HasNoKey();
            modelBuilder.Entity<OnDutyRequisitionResponse>().HasNoKey();
            modelBuilder.Entity<BusinessTravelResponse>().HasNoKey();
            modelBuilder.Entity<PermissionRequisitionResponse>().HasNoKey();
            modelBuilder.Entity<WorkFromHomeResponse>().HasNoKey();
            modelBuilder.Entity<LeaveBalanceResponse>().HasNoKey();
            modelBuilder.Entity<WeeklyOffHolidayWorkingResponse>().HasNoKey();
            modelBuilder.Entity<VaccinationReportResponse>().HasNoKey();
            modelBuilder.Entity<ShiftExtensionResponse>().HasNoKey();
            modelBuilder.Entity<MonthlyReportResponse>().HasNoKey();
            modelBuilder.Entity<AttendanceRecordDto>().HasNoKey();
            modelBuilder.Entity<StatutoryReportResponse>().HasNoKey();
        }
    }
}
