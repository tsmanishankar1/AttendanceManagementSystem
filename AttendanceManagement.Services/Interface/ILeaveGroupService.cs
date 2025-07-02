using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILeaveGroupService
    {
        Task<List<LeaveGroupResponse>> GetAllLeaveGroups();
        Task<string> AddLeaveGroupWithTransactionsAsync(AddLeaveGroupDto addLeaveGroupDto);
        Task<string> UpdateLeaveGroup(UpdateLeaveGroup leaveGroup);
    }
}
