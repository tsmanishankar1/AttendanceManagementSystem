using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class ToolsApp : IToolsApp
    {
        private readonly IToolsInfra _toolsInfra;

        public ToolsApp(IToolsInfra toolsInfra)
        {
            _toolsInfra = toolsInfra;
        }

        public async Task<string> AddLeaveCreditDebitForMultipleStaffAsync(LeaveCreditDebitRequest leaveCreditDebitRequest)
            => await _toolsInfra.AddLeaveCreditDebitForMultipleStaffAsync(leaveCreditDebitRequest);

        public async Task<string> AddReaderConfigurationAsync(ReaderConfigurationRequest request)
            => await _toolsInfra.AddReaderConfigurationAsync(request);

        public async Task<string> CreateAssignLeaveType(CreateAssignLeaveTypeDTO dto)
            => await _toolsInfra.CreateAssignLeaveType(dto);

        public async Task<string> CreateAttendanceStatusColorAsync(AttendanceStatusColorDto dto)
            => await _toolsInfra.CreateAttendanceStatusColorAsync(dto);

        public async Task<List<AssignLeaveTypeDTO>> GetAllAssignLeaveTypes()
            => await _toolsInfra.GetAllAssignLeaveTypes();

        public async Task<List<AttendanceStatusColorResponse>> GetAttendanceStatusColor()
            => await _toolsInfra.GetAttendanceStatusColor();

        public async Task<List<ReaderConfigurationResponse>> GetReaderConfigurationsAsync()
            => await _toolsInfra.GetReaderConfigurationsAsync();

        public async Task<List<StaffInfoDto>> GetStaffInfoByOrganizationTypeAsync(int organizationTypeId)
            => await _toolsInfra.GetStaffInfoByOrganizationTypeAsync(organizationTypeId);

        public async Task<List<StaffLeaveDto>> GetStaffInfoByStaffId(List<int> staffIds)
            => await _toolsInfra.GetStaffInfoByStaffId(staffIds);

        public async Task<string> UpdateAssignLeaveType(UpdateAssignLeaveTypeDTO dto)
            => await _toolsInfra.UpdateAssignLeaveType(dto);

        public async Task<string> UpdateAttendanceStatusAsync(UpdateAttendanceStatusRequest request)
            => await _toolsInfra.UpdateAttendanceStatusAsync(request);

        public async Task<string> UpdateAttendanceStatusColor(UpdateAttendanceStatusColor updateAttendanceStatusColor)
            => await _toolsInfra.UpdateAttendanceStatusColor(updateAttendanceStatusColor);
    }
}