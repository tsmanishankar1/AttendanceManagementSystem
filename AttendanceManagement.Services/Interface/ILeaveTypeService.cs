using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeResponse>> GetAllLeaveTypesAsync();
        Task<string> CreateLeaveTypeAsync(LeaveTypeRequest leaveTypeRequest);
        Task<string> UpdateLeaveTypeAsync(UpdateLeaveType leaveType);
    }
}
