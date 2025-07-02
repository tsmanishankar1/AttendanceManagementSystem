using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILoggingService
    {
        Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int createdBy, object? payload);
        Task AuditLog(string module, string httpMethod, string apiEndpoint, string successMessage, int createdBy, object? payload);
    }
}
