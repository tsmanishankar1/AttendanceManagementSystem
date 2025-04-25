using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProbationController : ControllerBase
{
    private readonly ProbationService _probationService;
    private readonly LoggingService _loggingService;

    public ProbationController(ProbationService probationService, LoggingService loggingService)
    {
        _probationService = probationService;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllProbations")]
    public async Task<IActionResult> GetAllProbations(int approverId)
    {
        try
        {
            var probations = await _probationService.GetAllProbationsAsync(approverId);
            var response = new
            {
                Success = true,
                Message = probations
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AssignManagerForProbationReview")]
    public async Task<IActionResult> AssignManagerForProbationReview(AssignManagerRequest assignManagerRequest)
    {
        try
        {
            var probation = await _probationService.AssignManagerForProbationReview(assignManagerRequest);
            var response = new
            {
                Success = true,
                Message = probation
            };
            await _loggingService.AuditLog("Assign Probation Manager", "POST", "/api/Probation/AssignManagerForProbationReview", probation, assignManagerRequest.CreatedBy, JsonSerializer.Serialize(assignManagerRequest));
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Assign Probation Manager", "POST", "/api/Probation/AssignManagerForProbationReview", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignManagerRequest.CreatedBy, JsonSerializer.Serialize(assignManagerRequest));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Assign Probation Manager", "POST", "/api/Probation/AssignManagerForProbationReview", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignManagerRequest.CreatedBy, JsonSerializer.Serialize(assignManagerRequest));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Assign Probation Manager", "POST", "/api/Probation/AssignManagerForProbationReview", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, assignManagerRequest.CreatedBy, JsonSerializer.Serialize(assignManagerRequest));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddProbation")]
    public async Task<IActionResult> CreateProbation(ProbationRequest probation)
    {
        try
        {
            var createdProbation = await _probationService.CreateProbationAsync(probation);
            var response = new
            {
                Success = true,
                Message = createdProbation
            };
            await _loggingService.AuditLog("Probation", "POST", "/api/Probation/AddProbation", createdProbation, probation.CreatedBy, JsonSerializer.Serialize(probation));
            return Ok(response);
        }
        catch(InvalidOperationException ex)
        {
            await _loggingService.LogError("Probation", "POST", "/api/Probation/AddProbation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, probation.CreatedBy, JsonSerializer.Serialize(probation));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Probation", "POST", "/api/Probation/AddProbation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, probation.CreatedBy, JsonSerializer.Serialize(probation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateProbation")]
    public async Task<IActionResult> UpdateProbation(UpdateProbation probation)
    {
        try
        {
            var updated = await _probationService.UpdateProbationAsync(probation);
            var response = new
            {
                Success = true,
                Message = updated
            };
            await _loggingService.AuditLog("Probation", "POST", "/api/Probation/UpdateProbation", updated, probation.UpdatedBy, JsonSerializer.Serialize(probation));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Probation", "POST", "/api/Probation/UpdateProbation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, probation.UpdatedBy, JsonSerializer.Serialize(probation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Probation", "POST", "/api/Probation/UpdateProbation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, probation.UpdatedBy, JsonSerializer.Serialize(probation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetProbationDetailsByApproverLevel")]
    public async Task<IActionResult> GetProbationDetailsByApproverLevel(int approverLevelId)
    {
        try
        {
            var result = await _probationService.GetProbationDetailsByApproverLevel(approverLevelId);
            var response = new
            {
                Success = true,
                Message = result
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
    [HttpGet("GetFeedbackByApproverLevel1")]
    public async Task<IActionResult> GetFeedbackByApproverLevel1(int approverLevel1Id)
    {
        try
        {
            var result = await _probationService.GetFeedbackDetailsByApproverLevel1(approverLevel1Id);
            var response = new
            {
                Success = true,
                Message = result
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("HrApprovalWithLetterGeneration")]
    public async Task<IActionResult> ProcessApprovalAsync(HrConfirmation hrConfirmation)
    {
        try
        {
            var pdfBase64 = await _probationService.ProcessApprovalAsync(hrConfirmation);
            var response = new
            {
                Success = true,
                Message = pdfBase64
            };
            await _loggingService.AuditLog("Letter Generation", "POST", "/api/Probation/HrApprovalWithLetterGeneration", pdfBase64, hrConfirmation.CreatedBy, JsonSerializer.Serialize(hrConfirmation));
            return Ok(response);
        }
        catch(MessageNotFoundException ex)
        {
            await _loggingService.LogError("Letter Generation", "POST", "/api/Probation/HrApprovalWithLetterGeneration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, hrConfirmation.CreatedBy, JsonSerializer.Serialize(hrConfirmation));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Letter Generation", "POST", "/api/Probation/HrApprovalWithLetterGeneration", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, hrConfirmation.CreatedBy, JsonSerializer.Serialize(hrConfirmation));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("AddFeedbackbyManager")]
    public async Task<IActionResult> AddFeedback(FeedbackRequest feedback)
    {
        try
        {
            var createdFeedback = await _probationService.AddFeedbackAsync(feedback);
            var response = new
            {
                Success = true,
                Message = createdFeedback
            };
            await _loggingService.AuditLog("Feedback Manager", "POST", "/api/Probation/AddFeedbackbyManager", createdFeedback, feedback.CreatedBy, JsonSerializer.Serialize(feedback));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Feedback Manager", "POST", "/api/Probation/AddFeedbackbyManager", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, feedback.CreatedBy, JsonSerializer.Serialize(feedback));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Feedback Manager", "POST", "/api/Probation/AddFeedbackbyManager", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, feedback.CreatedBy, JsonSerializer.Serialize(feedback));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Feedback Manager", "POST", "//api/Probation/AddFeedbackbyManager", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, feedback.CreatedBy, JsonSerializer.Serialize(feedback));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetFeebackById")]
    public async Task<IActionResult> GetFeedbackById(int feedbackId)
    {
        try
        {
            var feedback = await _probationService.GetFeedbackByIdAsync(feedbackId);
            var response = new
            {
                Success = true,
                Message = feedback
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetAllFeedbacks")]
    public async Task<IActionResult> GetAllFeedbacks()
    {
        try
        {
            var feedbacks = await _probationService.GetAllFeedbacksAsync();
            var response = new
            {
                Success = true,
                Message = feedbacks
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateFeedback")]
    public async Task<IActionResult> UpdateFeedback(UpdateFeedback updatedFeedback)
    {
        try
        {
            var feedback = await _probationService.UpdateFeedbackAsync(updatedFeedback);
            var response = new
            {
                Success = true,
                Message = feedback
            };
            await _loggingService.AuditLog("Feedback Manager", "POST", "/api/Probation/UpdateFeedback", feedback, updatedFeedback.UpdatedBy, JsonSerializer.Serialize(updatedFeedback));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Feedback Manager", "POST", "/api/Probation/UpdateFeedback", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedFeedback.UpdatedBy, JsonSerializer.Serialize(updatedFeedback));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Feedback Manager", "POST", "/api/Probation/UpdateFeedback", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedFeedback.UpdatedBy, JsonSerializer.Serialize(updatedFeedback));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("ViewConfirmationLetter")]
    public async Task<IActionResult> ViewPdfContent(int staffCreationId)
    {
        try
        {
            var content = await _probationService.GetPdfContent(staffCreationId);
            return Content(content, "text/plain");
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("DownloadConfirmationLetter")]
    public async Task<IActionResult> DownloadPdf(int staffCreationId)
    {
        try
        {
            var filePath = await _probationService.GetPdfFilePath(staffCreationId);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);

            return File(fileBytes, "application/pdf", fileName);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}
