using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IStaffCreationApp
    {
        Task<string> AddStaff(StaffCreationInputModel staffInput);
        Task<string> UpdateStaffCreationAsync(UpdateStaff updatedStaff);
        Task<string> UpdateApproversAsync(List<int> staffIds, int? approverId1, int? approverId2, int updatedBy);
        Task<StaffCreationResponse> GetByUserManagementIdAsync(int staffId);
        Task<List<StaffDto>> GetStaffAsync(GetStaff getStaff);
        Task<string> UpdateMyProfile(IndividualStaffUpdate individualStaffUpdate);
        Task<IndividualStaffResponse> GetMyProfile(int staffId);
        Task<List<StaffCreationResponse>> GetStaffRecordsByApprovalLevelAsync(int currentApprover1, bool? isApprovalLevel1, bool? isApprovalLevel2);
        Task<List<StaffCreationResponse>> GetPendingStaffForManagerApproval(int approverId);
        Task<string> ApprovePendingStaffs(ApprovePendingStaff approvePendingStaff);
        Task<string> CreateDropDownMaster(DropDownRequest dropDownRequest);
        Task<List<DropDownResponse>> GetDropDownMaster();
        Task<string> UpdateDropDownMaster(UpdateDropDown updateDropDown);
        Task<string> CreateDropDownDetails(DropDownDetailsRequest dropDownDetailsRequest);
        Task<List<DropDownResponse>> GetAllDropDowns(int id);
        Task<string> UpdateDropDownDetails(DropDownDetailsUpdate dropDownDetailsRequest);
    }
}
