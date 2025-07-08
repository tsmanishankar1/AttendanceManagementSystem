using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class AppraisalManagementApp : IAppraisalManagementApp
    {
        private readonly IAppraisalManagementInfra _appraisalManagementInfra;

        public AppraisalManagementApp(IAppraisalManagementInfra appraisalManagementInfra)
        {
            _appraisalManagementInfra = appraisalManagementInfra;
        }

        public async Task<string> AcceptAppraisalLetter(LetterAcceptance letterAcceptance)
            => await _appraisalManagementInfra.AcceptAppraisalLetter(letterAcceptance);

        public async Task<string> AgmApproval(AgmApprovalRequest agmApprovalRequest)
            => await _appraisalManagementInfra.AgmApproval(agmApprovalRequest);

        public async Task<string> CreateKra(KraDto kraDto)
            => await _appraisalManagementInfra.CreateKra(kraDto);

        public async Task<string> CreateManagerEvaluation(ManagerEvaluationRequest managerEvaluationRequest)
            => await _appraisalManagementInfra.CreateManagerEvaluation(managerEvaluationRequest);

        public async Task<string> CreateSelfEvaluation(SelfEvaluationRequest selfEvaluationRequest)
            => await _appraisalManagementInfra.CreateSelfEvaluation(selfEvaluationRequest);

        public async Task<string> DownloadAppraisalLetter(int staffId)
            => await _appraisalManagementInfra.DownloadAppraisalLetter(staffId);

        public async Task<string> GenerateAppraisalLetter(int createdBy, int year)
            => await _appraisalManagementInfra.GenerateAppraisalLetter(createdBy, year);

        public async Task<List<LetterAcceptanceResponse>> GetAcceptedEmployees(int year)
            => await _appraisalManagementInfra.GetAcceptedEmployees(year);

        public async Task<List<AgmDetails>> GetAllAgm()
            => await _appraisalManagementInfra.GetAllAgm();

        public async Task<object> GetFinalAverageManagerScore(int createdBy, int appraisalId, int year, string? quarter)
            => await _appraisalManagementInfra.GetFinalAverageManagerScore(createdBy, appraisalId, year, quarter);

        public async Task<List<HrUploadResponse>> GetHrUploadedSheet(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetHrUploadedSheet(appraisalId, year, quarter);

        public async Task<List<KraResponse>> GetKra(int createdBy, int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetKra(createdBy, appraisalId, year, quarter);

        public async Task<List<ManagerEvaluationResponse>> GetManagerEvaluation(int createdBy, int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetManagerEvaluation(createdBy, appraisalId, year, quarter);

        public async Task<List<object>> GetNonProductionEmployees(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetNonProductionEmployees(appraisalId, year, quarter);

        public async Task<List<object>> GetProductionEmployees(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetProductionEmployees(appraisalId, year, quarter);

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeAgmApproval(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetSelectedEmployeeAgmApproval(appraisalId, year, quarter);

        public async Task<List<PerformanceReviewResponse>> GetSelectedEmployeeReview(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetSelectedEmployeeReview(appraisalId, year, quarter);

        public async Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedEmployees(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetSelectedEmployees(appraisalId, year, quarter);

        public async Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedNonProductionEmployees(int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetSelectedNonProductionEmployees(appraisalId, year, quarter);

        public async Task<List<SelfEvaluationResponse>> GetSelfEvaluation(int approverId, int appraisalId, int year, string quarter)
            => await _appraisalManagementInfra.GetSelfEvaluation(approverId, appraisalId, year, quarter);

        public async Task<string> HrUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
            => await _appraisalManagementInfra.HrUploadSheet(uploadMisSheetRequest);

        public async Task<string> MisUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
            => await _appraisalManagementInfra.MisUploadSheet(uploadMisSheetRequest);

        public async Task<string> MoveSelectedStaff(SelectedEmployeesRequest selectedEmployeesRequest)
            => await _appraisalManagementInfra.MoveSelectedStaff(selectedEmployeesRequest);

        public async Task<string> MoveSelectedStaffToMis(SelectedEmployeesRequest selectedEmployeesRequest)
            => await _appraisalManagementInfra.MoveSelectedStaffToMis(selectedEmployeesRequest);

        public async Task<string> MoveToAgmApproval(AgmApprovalTab agmApprovalRequest)
            => await _appraisalManagementInfra.MoveToAgmApproval(agmApprovalRequest);

        public async Task<(Stream PdfStream, string FileName)> ViewAppraisalLetter(int staffId)
            => await _appraisalManagementInfra.ViewAppraisalLetter(staffId);
    }
}