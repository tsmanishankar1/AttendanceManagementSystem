using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ISubFunctionMasterApp
    {
        Task<List<SubFunctionResponse>> GetAllSubFunctionsAsync();
        Task<string> CreateSubFunctionAsync(SubFunctionRequest subFunctionMaster);
        Task<string> UpdateSubFunctionAsync(UpdateSubFunction subFunctionMaster);
    }
}
