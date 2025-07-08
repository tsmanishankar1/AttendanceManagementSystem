using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IApproveApplicationApp
    {
        Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest);
    }
}
