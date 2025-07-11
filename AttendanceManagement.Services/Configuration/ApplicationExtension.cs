using AttendanceManagement.Application.App;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Infrastructure.Infra;
using AttendanceManagement.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceManagement.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ILoginApp, LoginApp>();
            services.AddScoped<IBranchMasterApp, BranchMasterApp>();
            services.AddScoped<ICategoryMasterApp, CategoryMasterApp>();
            services.AddScoped<ICompanyMasterApp, CompanyMasterApp>();
            services.AddScoped<ICostCentreApp, CostCentreMasterApp>();
            services.AddScoped<IDepartmentMasterApp, DepartmentMasterApp>();
            services.AddScoped<IDesignationMasterApp, DesignationMasterApp>();
            services.AddScoped<IDivisionMasterApp, DivisionMasterApp>();
            services.AddScoped<IExcelImportApp, ExcelImportApp>();
            services.AddScoped<IGradeMasterApp, GradeMasterApp>();
            services.AddScoped<ILocationApp, LocationApp>();
            services.AddScoped<IStaffCreationApp, StaffCreationApp>();
            services.AddScoped<ISubFunctionMasterApp, SubFunctionMasterApp>();
            services.AddScoped<IUserManagementApp, UserManagementApp>();
            services.AddScoped<IWorkstationMasterApp, WorkStationMasterApp>();
            services.AddScoped<IZoneMasterApp, ZoneMasterApp>();
            services.AddScoped<ILeaveTypeApp, LeaveTypeApp>();
            services.AddScoped<IPrefixAndSuffixApp, PrefixAndSuffixApp>();
            services.AddScoped<ILeaveGroupApp, LeaveGroupApp>();
            services.AddScoped<ILeaveGroupConfigurationApp, LeaveGroupConfigurationApp>();
            services.AddScoped<IShiftApp, ShiftApp>();
            services.AddScoped<IWeeklyOffApp, WeeklyOffApp>();
            services.AddScoped<IHolidayApp, HolidayApp>();
            services.AddScoped<IApplicationApp, ApplicationApp>();
            services.AddScoped<IToolsApp, ToolsApp>();
            services.AddScoped<IDashboardApp, DashboardApp>();
            services.AddScoped<IProbationApp, ProbationApp>();
            services.AddScoped<ILoggingApp, LoggingApp>();
            services.AddScoped<IDailyReportApp, DailyReportsApp>();
            services.AddScoped<IPayrollApp, PayrollApp>();
            services.AddScoped<IAttendanceApp, AttendanceApp>();
            services.AddScoped<IEmailApp, EmailApp>();
            services.AddScoped<IStaffTransactionApp, StaffTransactionApp>();
            services.AddScoped<IApproveApplicationApp, ApproveApplicationApp>();
            services.AddScoped<IStatutoryReportApp, StatutoryReportApp>();
            services.AddScoped<IPerformanceReviewApp, PerformanceReviewApp>();
            services.AddScoped<IAppraisalManagementApp, AppraisalManagementApp>();
            services.AddScoped<ILetterGenerationApp, LetterGenerationApp>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILoginInfra, LoginInfra>();
            services.AddScoped<IBranchMasterInfra, BranchMasterInfra>();
            services.AddScoped<ICategoryMasterInfra, CategoryMasterInfra>();
            services.AddScoped<ICompanyMasterInfra, CompanyMasterInfra>();
            services.AddScoped<ICostCentreInfra, CostCentreMasterInfra>();
            services.AddScoped<IDepartmentMasterInfra, DepartmentMasterInfra>();
            services.AddScoped<IDesignationMasterInfra, DesignationMasterInfra>();
            services.AddScoped<IDivisionMasterInfra, DivisionMasterInfra>();
            services.AddScoped<IExcelImportInfra, ExcelImportInfra>();
            services.AddScoped<IGradeMasterInfra, GradeMasterInfra>();
            services.AddScoped<ILocationInfra, LocationInfra>();
            services.AddScoped<IStaffCreationInfra, StaffCreationInfra>();
            services.AddScoped<ISubFunctionMasterInfra, SubFunctionMasterService>();
            services.AddScoped<IUserManagementInfra, UserManagementInfra>();
            services.AddScoped<IPasswordHasher<UserManagement>, PasswordHasher<UserManagement>>();
            services.AddScoped<IWorkstationMasterInfra, WorkStationMasterInfra>();
            services.AddScoped<IZoneMasterInfra, ZoneMasterInfra>();
            services.AddScoped<ILeaveTypeInfra, LeaveTypeInfra>();
            services.AddScoped<IPrefixAndSuffixInfra, PrefixAndSuffixInfra>();
            services.AddScoped<ILeaveGroupInfra, LeaveGroupInfra>();
            services.AddScoped<ILeaveGroupConfigurationInfra, LeaveGroupConfigurationInfra>();
            services.AddScoped<IShiftInfra, ShiftInfra>();
            services.AddScoped<IWeeklyOffInfra, WeeklyOffInfra>();
            services.AddScoped<IHolidayInfra, HolidayInfra>();
            services.AddScoped<IApplicationInfra, ApplicationInfra>();
            services.AddScoped<IToolsInfra, ToolsInfra>();
            services.AddScoped<IDashboardInfra, DashboardInfra>();
            services.AddScoped<IProbationInfra, ProbationInfra>();
            services.AddScoped<ILoggingInfra, LoggingInfra>();
            services.AddScoped<IDailyReportInfra, DailyReportsInfra>();
            services.AddScoped<IPayrollInfra, PayrollInfra>();
            services.AddScoped<IAttendanceInfra, AttendanceInfra>();
            services.AddScoped<IEmailInfra, EmailInfra>();
            services.AddScoped<IStaffTransactionInfra, StaffTransactionInfra>();
            services.AddScoped<IApproveApplicationInfra, ApproveApplicationInfra>();
            services.AddScoped<IStatutoryReportInfra, StatutoryReportInfra>();
            services.AddScoped<IPerformanceReviewInfra, PerformanceReviewInfra>();
            services.AddScoped<IAppraisalManagementInfra, AppraisalManagementInfra>();
            services.AddScoped<ILetterGenerationInfra, LetterGenerationInfra>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddHostedService<ProbationConfirmationInfra>();
            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AttendanceManagementSystemContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

            services.AddDbContext<AtrakContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ATRAKConnection"),
                    sqlOptions => sqlOptions.CommandTimeout(180)
                ));

            services.AddDbContext<StoredProcedureDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

            return services;
        }    }
}