using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Application.App;

public class BranchMasterApp : IBranchMasterApp
{
    private readonly IBranchMasterInfra _branchMasterInfra;

    public BranchMasterApp(IBranchMasterInfra branchMasterInfra)
    {
        _branchMasterInfra = branchMasterInfra;
    }

    public async Task<string> CreateBranch(BranchMasterRequest branchMasterRequest)
        => await _branchMasterInfra.CreateBranch(branchMasterRequest);

    public async Task<List<BranchMasterResponse>> GetAllBranches()
        => await _branchMasterInfra.GetAllBranches();

    public string GetAppsettings()
        => _branchMasterInfra.GetAppsettings();

    public async Task<List<Goal>> GetGoals()
        => await _branchMasterInfra.GetGoals();

    public List<string> GetWorkspaceFile()
        => _branchMasterInfra.GetWorkspaceFile();

    public async Task<List<KraManagerReview>> KraManagerReviews()
        => await _branchMasterInfra.KraManagerReviews();

    public async Task<List<KraSelfReview>> KraSelfReviews()
        => await _branchMasterInfra.KraSelfReviews();

    public async Task<string> UpdateBranch(UpdateBranch branchMasterRequest)
        => await _branchMasterInfra.UpdateBranch(branchMasterRequest);

    public async Task<List<UserManagement>> GetUserManagement()
        => await _branchMasterInfra.GetUserManagement();

    public async Task<List<Probation>> GetProbations()
    => await _branchMasterInfra.GetProbations();

    public async Task<List<Feedback>> GetFeedbacks()
        => await _branchMasterInfra.GetFeedbacks();

    public async Task<List<ProbationReport>> GetProbationReports()
        => await _branchMasterInfra.GetProbationReports();
}