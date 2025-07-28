using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using System.IO;

namespace AttendanceManagement.Application.App
{
    public class ProbationApp : IProbationApp
    {
        private readonly IProbationInfra _probationInfra;
        public ProbationApp(IProbationInfra probationInfra)
        {
            _probationInfra = probationInfra;
        }

        public async Task<string> AddFeedbackAsync(FeedbackRequest feedbackRequest)
            => await _probationInfra.AddFeedbackAsync(feedbackRequest);

        public async Task<string> AssignManagerForProbationReview(AssignManagerRequest assignManagerRequest)
            => await _probationInfra.AssignManagerForProbationReview(assignManagerRequest);

        public async Task<string> CreateProbationAsync(ProbationRequest probationRequest)
            => await _probationInfra.CreateProbationAsync(probationRequest);

        public async Task<string> DownloadPdf(int staffCreationId)
            => await _probationInfra.DownloadPdf(staffCreationId);

        public async Task<List<FeedbackResponse>> GetAllFeedbacksAsync()
            => await _probationInfra.GetAllFeedbacksAsync();

        public async Task<object> GetAllManagers()
            => await _probationInfra.GetAllManagers();

        public async Task<List<ProbationResponse>> GetAllProbationsAsync()
            => await _probationInfra.GetAllProbationsAsync();

        public async Task<FeedbackResponse> GetFeedbackByIdAsync(int feedbackId)
            => await _probationInfra.GetFeedbackByIdAsync(feedbackId);

        public async Task<List<FeedbackResponse>> GetFeedbackDetailsByApproverLevel1(int approverId)
            => await _probationInfra.GetFeedbackDetailsByApproverLevel1(approverId);

        public async Task<List<GeneratedLetterResponse>> GetGeneratedLetters(int staffId)
            => await _probationInfra.GetGeneratedLetters(staffId);

        public async Task<(Stream PdfStream, string FileName)> GetPdfContent(int staffCreationId)
            => await _probationInfra.GetPdfContent(staffCreationId);

        public async Task<string> GetPdfFilePath(int staffCreationId)
            => await _probationInfra.GetPdfFilePath(staffCreationId);

        public async Task<ProbationResponse> GetProbationByIdAsync(int probationId)
            => await _probationInfra.GetProbationByIdAsync(probationId);

        public async Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel(int approverLevelId, int year, int month)
            => await _probationInfra.GetProbationDetailsByApproverLevel(approverLevelId, year, month);

        public async Task<List<ProbationReportResponse>> GetProbationReportsByApproverLevel(int approverLevel, int year, int month)
            => await _probationInfra.GetProbationReportsByApproverLevel(approverLevel, year, month);

        public async Task<string> ProcessApprovalAsync(HrConfirmation hrConfirmation)
            => await _probationInfra.ProcessApprovalAsync(hrConfirmation);

        public async Task<string> UpdateFeedbackAsync(UpdateFeedback updatedFeedback)
            => await _probationInfra.UpdateFeedbackAsync(updatedFeedback);

        public async Task<string> UpdateProbationAsync(UpdateProbation probation)
            => await _probationInfra.UpdateProbationAsync(probation);
    }
}