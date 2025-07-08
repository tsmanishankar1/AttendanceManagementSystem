using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IGradeMasterApp
    {
        Task<List<GradeMasterResponse>> GetAllGrades();
        Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest);
        Task<string> UpdateGrade(UpdateGradeMaster gradeMaster);
    }
}
