using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IApproveApplication
    {
        Task<string> ApproveApplicationRequisition(ApproveLeaveRequest approveLeaveRequest);
    }
}
