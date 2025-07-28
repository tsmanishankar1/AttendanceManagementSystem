using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class LoggingInfra : ILoggingInfra
    {
        private readonly IConfiguration _configuration;
        public LoggingInfra(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int createdBy, object? payload)
        {
            using (var logContext = CreateDbContext())
            {
                var errorLog = new ErrorLog
                {
                    Module = module,
                    HttpMethod = httpMethod,
                    ApiEndpoint = apiEndpoint,
                    ErrorMessage = errorMessage,
                    StackTrace = stackTrace,
                    InnerException = innerException?.ToString(),
                    CreatedBy = createdBy,
                    Payload = payload?.ToString(),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(errorLog);
                await logContext.SaveChangesAsync();
            }
        }

        public async Task AuditLog(string module, string httpMethod, string apiEndpoint, string successMessage, int createdBy, object? payload)
        {
            using (var logContext = CreateDbContext())
            {
                var auditLog = new AuditLog
                {
                    Module = module,
                    HttpMethod = httpMethod,
                    ApiEndpoint = apiEndpoint,
                    SuccessMessage = successMessage,
                    CreatedBy = createdBy,
                    Payload = payload?.ToString(),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.AuditLogs.Add(auditLog);
                await logContext.SaveChangesAsync();
            }
        }

        private AttendanceManagementSystemContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AttendanceManagementSystemContext>();
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DBConnection"));

            return new AttendanceManagementSystemContext(optionsBuilder.Options);
        }
    }
}