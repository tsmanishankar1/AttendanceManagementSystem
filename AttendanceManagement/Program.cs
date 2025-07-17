using AttendanceManagement.Infrastructure.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

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