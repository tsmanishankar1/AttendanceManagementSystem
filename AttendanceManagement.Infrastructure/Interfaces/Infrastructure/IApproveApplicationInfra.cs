using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IApproveApplicationInfra
    {
        Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest);
    }
}
