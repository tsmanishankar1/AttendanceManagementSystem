using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using System.Text.Json;

namespace AttendanceManagement.Services
{
    public class LoggingService
    {
        public LoggingService()
        {
        }

        public async Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int staffId, object? payload)
        {
            using (var logContext = new AttendanceManagementSystemContext())
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
            using (var logContext = new AttendanceManagementSystemContext())
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
    }
}
