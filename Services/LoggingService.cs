using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Text.Json;

namespace AttendanceManagement.Services
{
    public class LoggingService
    {
        private readonly IConfiguration _configuration;
        public LoggingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int staffId, object? payload)
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
                    StaffId = staffId,
                    Payload = payload?.ToString(),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(errorLog);
                await logContext.SaveChangesAsync();
            }
        }

        public async Task AuditLog(string module, string httpMethod, string apiEndpoint, string successMessage, int staffId, object? payload)
        {
            using (var logContext = CreateDbContext())
            {
                var auditLog = new AuditLog
                {
                    Module = module,
                    HttpMethod = httpMethod,
                    ApiEndpoint = apiEndpoint,
                    SuccessMessage = successMessage,
                    StaffId = staffId,
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