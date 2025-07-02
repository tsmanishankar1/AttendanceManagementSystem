using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IToolsService
    {
        Task<List<StaffInfoDto>> GetStaffInfoByOrganizationTypeAsync(int organizationTypeId);
        Task<List<StaffLeaveDto>> GetStaffInfoByStaffId(List<int> staffIds);
        Task<List<AssignLeaveTypeDTO>> GetAllAssignLeaveTypes();
        Task<string> CreateAssignLeaveType(CreateAssignLeaveTypeDTO dto);
        Task<string> UpdateAssignLeaveType(UpdateAssignLeaveTypeDTO dto);
        Task<string> AddLeaveCreditDebitForMultipleStaffAsync(LeaveCreditDebitRequest leaveCreditDebitRequest);
        Task<string> AddReaderConfigurationAsync(ReaderConfigurationRequest request);
        Task<List<ReaderConfigurationResponse>> GetReaderConfigurationsAsync();
        Task<string> UpdateAttendanceStatusAsync(UpdateAttendanceStatusRequest request);
        Task<string> CreateAttendanceStatusColorAsync(AttendanceStatusColorDto dto);
        Task<List<AttendanceStatusColorResponse>> GetAttendanceStatusColor();
        Task<string> UpdateAttendanceStatusColor(UpdateAttendanceStatusColor updateAttendanceStatusColor);
    }
}
