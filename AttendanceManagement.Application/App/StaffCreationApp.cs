using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class StaffCreationApp : IStaffCreationApp
    {
        private readonly IStaffCreationInfra _staffCreationInfra;

        public StaffCreationApp(IStaffCreationInfra staffCreationInfra)
        {
            _staffCreationInfra = staffCreationInfra;
        }

        public async Task<string> AddStaff(StaffCreationInputModel staffInput)
            => await _staffCreationInfra.AddStaff(staffInput);

        public async Task<string> ApprovePendingStaffs(ApprovePendingStaff approvePendingStaff)
            => await _staffCreationInfra.ApprovePendingStaffs(approvePendingStaff);

        public async Task<string> CreateDropDownDetails(DropDownDetailsRequest dropDownDetailsRequest)
            => await _staffCreationInfra.CreateDropDownDetails(dropDownDetailsRequest);

        public async Task<string> CreateDropDownMaster(DropDownRequest dropDownRequest)
            => await _staffCreationInfra.CreateDropDownMaster(dropDownRequest);

        public async Task<List<DropDownResponse>> GetAllDropDowns(int id)
            => await _staffCreationInfra.GetAllDropDowns(id);

        public async Task<StaffCreationResponse> GetByUserManagementIdAsync(int staffId)
            => await _staffCreationInfra.GetByUserManagementIdAsync(staffId);

        public async Task<List<DropDownResponse>> GetDropDownMaster()
            => await _staffCreationInfra.GetDropDownMaster();

        public async Task<IndividualStaffResponse> GetMyProfile(int staffId)
            => await _staffCreationInfra.GetMyProfile(staffId);

        public async Task<List<StaffCreationResponse>> GetPendingStaffForManagerApproval(int approverId)
            => await _staffCreationInfra.GetPendingStaffForManagerApproval(approverId);

        public async Task<List<StaffDto>> GetStaffAsync(GetStaff getStaff)
            => await _staffCreationInfra.GetStaffAsync(getStaff);

        public async Task<List<StaffCreationResponse>> GetStaffRecordsByApprovalLevelAsync(
            int currentApprover1, bool? isApprovalLevel1, bool? isApprovalLevel2)
            => await _staffCreationInfra.GetStaffRecordsByApprovalLevelAsync(currentApprover1, isApprovalLevel1, isApprovalLevel2);

        public async Task<string> UpdateApproversAsync(List<int> staffIds, int? approverId1, int? approverId2, int updatedBy)
            => await _staffCreationInfra.UpdateApproversAsync(staffIds, approverId1, approverId2, updatedBy);

        public async Task<string> UpdateDropDownDetails(DropDownDetailsUpdate dropDownDetailsRequest)
            => await _staffCreationInfra.UpdateDropDownDetails(dropDownDetailsRequest);

        public async Task<string> UpdateDropDownMaster(UpdateDropDown updateDropDown)
            => await _staffCreationInfra.UpdateDropDownMaster(updateDropDown);

        public async Task<string> UpdateMyProfile(IndividualStaffUpdate individualStaffUpdate)
            => await _staffCreationInfra.UpdateMyProfile(individualStaffUpdate);

        public async Task<string> UpdateStaffCreationAsync(UpdateStaff updatedStaff)
            => await _staffCreationInfra.UpdateStaffCreationAsync(updatedStaff);
    }
}