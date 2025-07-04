using AttendanceManagement.AtrakModels;
using AttendanceManagement.InputModels;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Core;
using EmailService = AttendanceManagement.Services.EmailService;
using IEmailService = AttendanceManagement.Services.Interface.IEmailService;

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("ATRAKConnection")));
builder.Services.AddDbContext<StoredProcedureDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IBranchMasterService, BranchMasterService>();
builder.Services.AddScoped<ICategoryMaster, CategoryMasterService>();
builder.Services.AddScoped<ICompanyMaster, CompanyMasterService>();
builder.Services.AddScoped<ICostCentre, CostCentreMasterService>();
builder.Services.AddScoped<IDepartmentMasterService, DepartmentMasterService>();
builder.Services.AddScoped<IDesignationMasterService, DesignationMasterService>();
builder.Services.AddScoped<IDivisionMasterService, DivisionMasterService>();
builder.Services.AddScoped<IExcelImport, ExcelImportService>();
builder.Services.AddScoped<IGradeMasterService, GradeMasterService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IStaffCreationService, StaffCreationService>();
builder.Services.AddScoped<ISubFunctionMasterService, SubFunctionMasterService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IPasswordHasher<UserManagement>, PasswordHasher<UserManagement>>();
builder.Services.AddScoped<IWorkstationMasterService, WorkstationMasterService>();
builder.Services.AddScoped<IZoneMasterService, ZoneMasterService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<IPrefixAndSuffixService, PrefixAndSuffixService>();
builder.Services.AddScoped<ILeaveGroupService, LeaveGroupService>();
builder.Services.AddScoped<ILeaveGroupConfigurationService, LeaveGroupConfigurationService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IWeeklyOffService, WeeklyOffService>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IToolsService, ToolsService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProbationService, ProbationService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<IDailyReport, DailyReportsService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStaffTransactionService, StaffTransactionService>();
builder.Services.AddScoped<IApproveApplication, ApproveApplicationService>();
builder.Services.AddScoped<IStatutoryReport, StatutoryReportService>();
builder.Services.AddScoped<IPerformanceReview, PerformanceReviewService>();
builder.Services.AddScoped<IAppraisalManagement, AppraisalManagementService>();
builder.Services.AddScoped<ILetterGeneration, LetterGenerationService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddHostedService<ProbationConfirmationService>();
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