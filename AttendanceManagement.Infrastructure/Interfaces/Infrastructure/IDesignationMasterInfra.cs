using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IDesignationMasterInfra
    {
        Task<List<DesignationResponse>> GetAllDesignations();
        Task<string> AddDesignation(DesignationRequest designationRequest);
        Task<string> UpdateDesignation(UpdateDesignation designation);
    }
}
