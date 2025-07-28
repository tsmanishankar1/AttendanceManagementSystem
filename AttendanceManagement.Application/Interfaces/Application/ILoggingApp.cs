namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ILoggingApp
    {
        Task LogError(string module, string httpMethod, string apiEndpoint, string errorMessage, string stackTrace, string innerException, int createdBy, object? payload);
        Task AuditLog(string module, string httpMethod, string apiEndpoint, string successMessage, int createdBy, object? payload);
    }
}
