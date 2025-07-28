using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ISubFunctionMasterInfra
    {
        Task<List<SubFunctionResponse>> GetAllSubFunctionsAsync();
        Task<string> CreateSubFunctionAsync(SubFunctionRequest subFunctionMaster);
        Task<string> UpdateSubFunctionAsync(UpdateSubFunction subFunctionMaster);
    }
}
