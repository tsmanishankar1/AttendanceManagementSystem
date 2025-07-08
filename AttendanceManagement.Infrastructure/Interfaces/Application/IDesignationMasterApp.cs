using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IDesignationMasterApp
    {
        Task<List<DesignationResponse>> GetAllDesignations();
        Task<string> AddDesignation(DesignationRequest designationRequest);
        Task<string> UpdateDesignation(UpdateDesignation designation);
    }
}
