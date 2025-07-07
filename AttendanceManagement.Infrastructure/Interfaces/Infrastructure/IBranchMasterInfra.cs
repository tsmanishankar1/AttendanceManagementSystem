using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IBranchMasterInfra
    {
        Task<List<BranchMasterResponse>> GetAllBranches();
        Task<string> CreateBranch(BranchMasterRequest branchMasterRequest);
        Task<string> UpdateBranch(UpdateBranch branchMasterRequest);
        string GetAppsettings();
        List<string> GetWorkspaceFile();
    }
}