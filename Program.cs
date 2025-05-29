using AttendanceManagement;
using AttendanceManagement.AtrakModels;
using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var configuration = ConfigureWebApiAppSettings();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddConfiguration(configuration);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
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
            policy.WithOrigins("http://localhost:3000", "*", "http://servicedesk.vleadservices.com:84")
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

builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<BranchMasterService>();
builder.Services.AddScoped<CategoryMasterService>();
builder.Services.AddScoped<CompanyMasterService>();
builder.Services.AddScoped<CostCentreMasterService>();
builder.Services.AddScoped<DepartmentMasterService>();
builder.Services.AddScoped<DesignationMasterService>();
builder.Services.AddScoped<DivisionMasterService>();
builder.Services.AddScoped<ExcelImportService>();
builder.Services.AddScoped<GradeMasterService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<StaffCreationService>();
builder.Services.AddScoped<SubFunctionMasterService>();
builder.Services.AddScoped<UserManagementService>();
builder.Services.AddScoped<WorkstationMasterService>();
builder.Services.AddScoped<ZoneMasterService>();
builder.Services.AddScoped<LeaveTypeService>();
builder.Services.AddScoped<PrefixAndSuffixService>();
builder.Services.AddScoped<LeaveGroupService>();
builder.Services.AddScoped<LeaveGroupConfigurationService>();
builder.Services.AddScoped<ShiftService>();
builder.Services.AddScoped<WeeklyOffService>();
builder.Services.AddScoped<HolidayService>();
builder.Services.AddScoped<ApplicationService>();
builder.Services.AddScoped<ToolsService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ProbationService>();
builder.Services.AddScoped<LoggingService>();
builder.Services.AddScoped<DailyReportsService>();
builder.Services.AddScoped<PayrollService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<StaffTransactionService>();
builder.Services.AddScoped<ApproveApplicationService>();
builder.Services.AddScoped<StatutoryReportService>();
builder.Services.AddScoped<PerformanceReviewService>();
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
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VLead Attendance System V1");
        c.DocumentTitle = "VLead Attendance System API";
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
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
    var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

    return configurationBuilder.Build();
}