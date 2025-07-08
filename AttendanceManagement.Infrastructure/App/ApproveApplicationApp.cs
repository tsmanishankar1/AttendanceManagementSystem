using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class ApproveApplicationApp : IApproveApplicationApp
    {
        private readonly IApproveApplicationInfra _approveApplicationInfra;
        public ApproveApplicationApp(IApproveApplicationInfra approveApplicationInfra)
        {
            _approveApplicationInfra = approveApplicationInfra;

        }

        public async Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest)
            => await _approveApplicationInfra.ApproveApplicationRequisition(approveLeaveRequest);
    }
}