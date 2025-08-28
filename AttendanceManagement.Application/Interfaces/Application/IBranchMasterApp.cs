using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IBranchMasterApp
    {
        Task<List<BranchMasterResponse>> GetAllBranches();
        Task<string> CreateBranch(BranchMasterRequest branchMasterRequest);
        Task<string> UpdateBranch(UpdateBranch branchMasterRequest);
        string GetAppsettings();
        List<string> GetWorkspaceFile();
        Task<List<Goal>> GetGoals();
        Task<List<KraSelfReview>> KraSelfReviews();
        Task<List<KraManagerReview>> KraManagerReviews();
        Task<List<UserManagement>> GetUserManagement();
        Task<List<Probation>> GetProbations();
        Task<List<Feedback>> GetFeedbacks();
        Task<List<ProbationReport>> GetProbationReports();
        Task<List<RefreshToken>> GetRefreshToken();
        Task<List<AssignShift>> GetAssignShift();
        Task<List<AuditLog>> GetAuditLog();
        Task<List<ErrorLog>> GetErrorLog();
    }
}