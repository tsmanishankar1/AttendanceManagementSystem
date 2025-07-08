using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class LoggingApp : ILoggingApp
    {
        private readonly ILoggingInfra _loggingInfra;
        public LoggingApp(ILoggingInfra loggingInfra)
        {
            _loggingInfra = loggingInfra;
        }

        public async Task AuditLog(string module, string httpMethod, string apiEndpoint, string successMessage, int createdBy, object? payload)
            => await _loggingInfra.AuditLog(module, httpMethod, apiEndpoint, successMessage, createdBy, payload);   

        public async Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int createdBy, object? payload)
            => await _loggingInfra.LogError(module, httpMethod, apiEndpoint, errorMessage, stackTrace, innerException, createdBy, payload);
    }
}