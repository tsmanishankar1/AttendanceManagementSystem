using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IBranchMasterInfra
    {
        Task<List<BranchMasterResponse>> GetAllBranches();
        Task<string> CreateBranch(BranchMasterRequest branchMasterRequest);
        Task<string> UpdateBranch(UpdateBranch branchMasterRequest);
        string GetAppsettings();
        List<string> GetWorkspaceFile();
        Task<List<Goal>> GetGoals();
        Task<List<KraSelfReview>> KraSelfReviews();
        Task<List<KraManagerReview>> KraManagerReviews();
    }
}