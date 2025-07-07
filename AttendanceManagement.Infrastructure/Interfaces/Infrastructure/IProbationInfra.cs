using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IProbationInfra
    {
        Task<List<ProbationResponse>> GetAllProbationsAsync();
        Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel(int approverLevelId);
        Task<List<ProbationReportResponse>> GetProbationReportsByApproverLevel(int approverLevel, int year);
        Task<ProbationResponse> GetProbationByIdAsync(int probationId);
        Task<object> GetAllManagers();
        Task<string> AssignManagerForProbationReview(AssignManagerRequest assignManagerRequest);
        Task<string> CreateProbationAsync(ProbationRequest probationRequest);
        Task<string> UpdateProbationAsync(UpdateProbation probation);
        Task<string> AddFeedbackAsync(FeedbackRequest feedbackRequest);
        Task<string> UpdateFeedbackAsync(UpdateFeedback updatedFeedback);
        Task<FeedbackResponse> GetFeedbackByIdAsync(int feedbackId);
        Task<List<FeedbackResponse>> GetAllFeedbacksAsync();
        Task<List<FeedbackResponse>> GetFeedbackDetailsByApproverLevel1(int approverId);
        Task<string> ProcessApprovalAsync(HrConfirmation hrConfirmation);
        Task<List<GeneratedLetterResponse>> GetGeneratedLetters(int staffId);
        Task<string> GetPdfFilePath(int staffCreationId);
        Task<(Stream PdfStream, string FileName)> GetPdfContent(int staffCreationId);
        Task<string> DownloadPdf(int staffCreationId);
    }
}
