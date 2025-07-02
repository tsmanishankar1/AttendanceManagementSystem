using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IProbationService
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
        Task<string> GetPdfFilePath(int staffCreationId, int fileId);
        Task<string> GetPdfContent(int staffCreationId);
        Task<(byte[] fileBytes, string fileName, string contentType)> DownloadPdf(int staffCreationId);
    }
}
