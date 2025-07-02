using AttendanceManagement.InputModels;

namespace AttendanceManagement.Services.Interface
{
    public interface IBranchMasterService
    {
        Task<List<BranchMasterResponse>> GetAllBranches();
        Task<string> CreateBranch(BranchMasterRequest branchMasterRequest);
        Task<string> UpdateBranch(UpdateBranch branchMasterRequest);
    }
}