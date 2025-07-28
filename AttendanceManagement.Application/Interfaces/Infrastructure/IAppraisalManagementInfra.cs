using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IAppraisalManagementInfra
    {
        Task<List<object>> GetProductionEmployees(int appraisalId, int year, string? quarter, int? month);
        Task<string> MoveSelectedStaffToMis(SelectedEmployeesRequest selectedEmployeesRequest);
        Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedEmployees(int appraisalId, int year, string? quarter, int? month);
        string GetExcelTemplateFile(string fileName);
        Task<string> MisUploadSheet(UploadMisSheetRequest uploadMisSheetRequest);
        Task<List<PerformanceReviewResponse>> GetSelectedEmployeeReview(int appraisalId, int year, string? quarter, int? month);
        Task<List<AgmDetails>> GetAllAgm();
        Task<string> MoveToAgmApproval(AgmApprovalTab agmApprovalRequest);
        Task<List<PerformanceReviewResponse>> GetSelectedEmployeeAgmApproval(int appraisalId, int year, string? quarter, int? month);
        Task<string> AgmApproval(AgmApprovalRequest agmApprovalRequest);
        Task<string> GenerateAppraisalLetter(int createdBy, int year);
        Task<(Stream PdfStream, string FileName)> ViewAppraisalLetter(int staffId);
        Task<string> DownloadAppraisalLetter(int staffId);
        Task<string> AcceptAppraisalLetter(LetterAcceptance letterAcceptance);
        Task<List<LetterAcceptanceResponse>> GetAcceptedEmployees(int year);
        Task<List<object>> GetNonProductionEmployees(int appraisalId, int year, string? quarter, int? month);
        Task<string> MoveSelectedStaff(SelectedEmployeesRequest selectedEmployeesRequest);
        Task<List<SelectedEmployeesResponseSelectedRows>> GetSelectedNonProductionEmployees(int appraisalId, int year, string? quarter, int? month);
        Task<string> CreateKra(KraDto kraDto);
        Task<List<KraResponse>> GetKra(int createdBy, int appraisalId, int year, string? quarter, int? month);
        Task<string> CreateSelfEvaluation(SelfEvaluationRequest selfEvaluationRequest);
        Task<List<SelfEvaluationResponse>> GetSelfEvaluation(int approverId, int appraisalId, int year, string? quarter, int? month);
        Task<string> CreateManagerEvaluation(ManagerEvaluationRequest managerEvaluationRequest);
        Task<List<ManagerEvaluationResponse>> GetManagerEvaluation(int createdBy, int appraisalId, int year, string? quarter, int? month);
        Task<object> GetFinalAverageManagerScore(int createdBy, int appraisalId, int year, string? quarter, int? month);
        Task<string> HrUploadSheet(UploadMisSheetRequest uploadMisSheetRequest);
        Task<List<HrUploadResponse>> GetHrUploadedSheet(int appraisalId, int year, string? quarter, int? month);
        Task<(Stream PdfStream, string FileName)> ViewSelfEvaluationAttachment(int selfEvaluationId);
        Task<(Stream PdfStream, string FileName)> ViewManagerEvaluationAttachment(int managerEvaluationId);
    }
}
