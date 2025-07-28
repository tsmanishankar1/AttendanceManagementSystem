using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IGradeMasterInfra
    {
        Task<List<GradeMasterResponse>> GetAllGrades();
        Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest);
        Task<string> UpdateGrade(UpdateGradeMaster gradeMaster);
    }
}
