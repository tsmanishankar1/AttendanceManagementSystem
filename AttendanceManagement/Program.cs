using AttendanceManagement.Application.App;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Infrastructure.Infra;
using AttendanceManagement.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var configuration = ConfigureWebApiAppSettings();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VLead Attendance System API",
        Version = "v1",
        Description = "API for managing attendance, leave requests and more in the Attendance Management System.",
    });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SwaggerDocs", "SwaggerAPIDocumentation.xml");
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "*", "http://servicedesk.vleadservices.com:84", "http://172.16.10.79")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddDbContext<AttendanceManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddDbContext<AtrakContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ATRAKConnection"),
        sqlOptions => sqlOptions.CommandTimeout(180)
    ));
builder.Services.AddDbContext<StoredProcedureDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<ILoginApp, LoginApp>();
builder.Services.AddScoped<IBranchMasterApp, BranchMasterApp>();
builder.Services.AddScoped<ICategoryMasterApp, CategoryMasterApp>();
builder.Services.AddScoped<ICompanyMasterApp, CompanyMasterApp>();
builder.Services.AddScoped<ICostCentreApp, CostCentreMasterApp>();
builder.Services.AddScoped<IDepartmentMasterApp, DepartmentMasterApp>();
builder.Services.AddScoped<IDesignationMasterApp, DesignationMasterApp>();
builder.Services.AddScoped<IDivisionMasterApp, DivisionMasterApp>();
builder.Services.AddScoped<IExcelImportApp, ExcelImportApp>();
builder.Services.AddScoped<IGradeMasterApp, GradeMasterApp>();
builder.Services.AddScoped<ILocationApp, LocationApp>();
builder.Services.AddScoped<IStaffCreationApp, StaffCreationApp>();
builder.Services.AddScoped<ISubFunctionMasterApp, SubFunctionMasterApp>();
builder.Services.AddScoped<IUserManagementApp, UserManagementApp>();
builder.Services.AddScoped<IWorkstationMasterApp, WorkStationMasterApp>();
builder.Services.AddScoped<IZoneMasterApp, ZoneMasterApp>();
builder.Services.AddScoped<ILeaveTypeApp, LeaveTypeApp>();
builder.Services.AddScoped<IPrefixAndSuffixApp, PrefixAndSuffixApp>();
builder.Services.AddScoped<ILeaveGroupApp, LeaveGroupApp>();
builder.Services.AddScoped<ILeaveGroupConfigurationApp, LeaveGroupConfigurationApp>();
builder.Services.AddScoped<IShiftApp, ShiftApp>();
builder.Services.AddScoped<IWeeklyOffApp, WeeklyOffApp>();
builder.Services.AddScoped<IHolidayApp, HolidayApp>();
builder.Services.AddScoped<IApplicationApp, ApplicationApp>();
builder.Services.AddScoped<IToolsApp, ToolsApp>();
builder.Services.AddScoped<IDashboardApp, DashboardApp>();
builder.Services.AddScoped<IProbationApp, ProbationApp>();
builder.Services.AddScoped<ILoggingApp, LoggingApp>();
builder.Services.AddScoped<IDailyReportApp, DailyReportsApp>();
builder.Services.AddScoped<IPayrollApp, PayrollApp>();
builder.Services.AddScoped<IAttendanceApp, AttendanceApp>();
builder.Services.AddScoped<IEmailApp, EmailApp>();
builder.Services.AddScoped<IStaffTransactionApp, StaffTransactionApp>();
builder.Services.AddScoped<IApproveApplicationApp, ApproveApplicationApp>();
builder.Services.AddScoped<IStatutoryReportApp, StatutoryReportApp>();
builder.Services.AddScoped<IPerformanceReviewApp, PerformanceReviewApp>();
builder.Services.AddScoped<IAppraisalManagementApp, AppraisalManagementApp>();
builder.Services.AddScoped<ILetterGenerationApp, LetterGenerationApp>();


builder.Services.AddScoped<ILoginInfra, LoginInfra>();
builder.Services.AddScoped<IBranchMasterInfra, BranchMasterInfra>();
builder.Services.AddScoped<ICategoryMasterInfra, CategoryMasterInfra>();
builder.Services.AddScoped<ICompanyMasterInfra, CompanyMasterInfra>();
builder.Services.AddScoped<ICostCentreInfra, CostCentreMasterInfra>();
builder.Services.AddScoped<IDepartmentMasterInfra, DepartmentMasterInfra>();
builder.Services.AddScoped<IDesignationMasterInfra, DesignationMasterInfra>();
builder.Services.AddScoped<IDivisionMasterInfra, DivisionMasterInfra>();
builder.Services.AddScoped<IExcelImportInfra, ExcelImportInfra>();
builder.Services.AddScoped<IGradeMasterInfra, GradeMasterInfra>();
builder.Services.AddScoped<ILocationInfra, LocationInfra>();
builder.Services.AddScoped<IStaffCreationInfra, StaffCreationInfra>();
builder.Services.AddScoped<ISubFunctionMasterInfra, SubFunctionMasterService>();
builder.Services.AddScoped<IUserManagementInfra, UserManagementInfra>();
builder.Services.AddScoped<IPasswordHasher<UserManagement>, PasswordHasher<UserManagement>>();
builder.Services.AddScoped<IWorkstationMasterInfra, WorkStationMasterInfra>();
builder.Services.AddScoped<IZoneMasterInfra, ZoneMasterInfra>();
builder.Services.AddScoped<ILeaveTypeInfra, LeaveTypeInfra>();
builder.Services.AddScoped<IPrefixAndSuffixInfra, PrefixAndSuffixInfra>();
builder.Services.AddScoped<ILeaveGroupInfra, LeaveGroupInfra>();
builder.Services.AddScoped<ILeaveGroupConfigurationInfra, LeaveGroupConfigurationInfra>();
builder.Services.AddScoped<IShiftInfra, ShiftInfra>();
builder.Services.AddScoped<IWeeklyOffInfra, WeeklyOffInfra>();
builder.Services.AddScoped<IHolidayInfra, HolidayInfra>();
builder.Services.AddScoped<IApplicationInfra, ApplicationInfra>();
builder.Services.AddScoped<IToolsInfra, ToolsInfra>();
builder.Services.AddScoped<IDashboardInfra, DashboardInfra>();
builder.Services.AddScoped<IProbationInfra, ProbationInfra>();
builder.Services.AddScoped<ILoggingInfra, LoggingInfra>();
builder.Services.AddScoped<IDailyReportInfra, DailyReportsInfra>();
builder.Services.AddScoped<IPayrollInfra, PayrollInfra>();
builder.Services.AddScoped<IAttendanceInfra, AttendanceInfra>();
builder.Services.AddScoped<IEmailInfra, EmailInfra>();
builder.Services.AddScoped<IStaffTransactionInfra, StaffTransactionInfra>();
builder.Services.AddScoped<IApproveApplicationInfra, ApproveApplicationInfra>();
builder.Services.AddScoped<IStatutoryReportInfra, StatutoryReportInfra>();
builder.Services.AddScoped<IPerformanceReviewInfra, PerformanceReviewInfra>();
builder.Services.AddScoped<IAppraisalManagementInfra, AppraisalManagementInfra>();
builder.Services.AddScoped<ILetterGenerationInfra, LetterGenerationInfra>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddHostedService<ProbationConfirmationInfra>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VLead Attendance System V1");
        c.DocumentTitle = "VLead Attendance System API";
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VLead Attendance System V1");
        c.DocumentTitle = "VLead Attendance System API";
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles();

app.MapControllers();

app.Run();
static IConfigurationRoot ConfigureWebApiAppSettings()
{
    var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    return configurationBuilder.Build();
}