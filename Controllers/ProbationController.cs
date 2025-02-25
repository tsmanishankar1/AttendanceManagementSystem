using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProbationController : ControllerBase
{
    private readonly ProbationService _probationService;
    private readonly AttendanceManagementSystemContext _context;

    public ProbationController(ProbationService probationService, AttendanceManagementSystemContext context)
    {
        _probationService = probationService;
        _context = context;
    }

    [HttpGet("GetAllProbations")]
    public async Task<IActionResult> GetAllProbations()
    {
        try
        {
            var probations = await _probationService.GetAllProbationsAsync();
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

    [HttpGet("GetProbationById")]
    public async Task<IActionResult> GetProbationById(int probationId)
    {
        try
        {
            var probation = await _probationService.GetProbationByIdAsync(probationId);
            var response = new
            {
                Success = true,
                Message = probation
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
            AuditLog log = new AuditLog
            {
                Module = "Probation",
                HttpMethod = "POST",
                ApiEndpoint = "/Probation/AddProbation",
                SuccessMessage = createdProbation,
                Payload = System.Text.Json.JsonSerializer.Serialize(probation),
                StaffId = probation.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Probation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Probation/AddProbation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = probation.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(probation),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }

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
            AuditLog log = new AuditLog
            {
                Module = "Probation",
                HttpMethod = "POST",
                ApiEndpoint = "/Probation/UpdateProbation",
                SuccessMessage = updated,
                Payload = System.Text.Json.JsonSerializer.Serialize(probation),
                StaffId = probation.UpdatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Probation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Probation/UpdateProbation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = probation.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(probation),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "probation",
                    HttpMethod = "POST",
                    ApiEndpoint = "/probation/Updateprobation",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = probation.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(probation),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetByApproverLevel1")]
    public async Task<IActionResult> GetByApproverLevel1(int approverLevel1Id)
    {
        try
        {
            var result = await _probationService.GetProbationDetailsByApproverLevel1(approverLevel1Id);
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
    public async Task<IActionResult> ProcessApprovalAsync(ApprovalRequest approval)
    {
        try
        {
            var pdfBase64 = await _probationService.ProcessApprovalAsync(approval);
            var response = new
            {
                Success = true,
                Message = pdfBase64
            };
            AuditLog log = new AuditLog
            {
                Module = "HrApprovalWithLetterGeneration",
                HttpMethod = "POST",
                ApiEndpoint = "/HrApprovalWithLetterGeneration/AddHrApprovalWithLetterGeneration",
                SuccessMessage = pdfBase64,
                Payload = System.Text.Json.JsonSerializer.Serialize(approval),
                StaffId = approval.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "HrApprovalWithLetterGeneration",
                    HttpMethod = "POST",
                    ApiEndpoint = "/HrApprovalWithLetterGeneration/AddHrApprovalWithLetterGeneration",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = approval.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(approval),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
            AuditLog log = new AuditLog
            {
                Module = "AddFeedbackbyManager",
                HttpMethod = "POST",
                ApiEndpoint = "/AddFeedbackbyManager/AddAddFeedbackbyManager",
                SuccessMessage = createdFeedback,
                Payload = System.Text.Json.JsonSerializer.Serialize(feedback),
                StaffId = feedback.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "FeedbackbyManager",
                    HttpMethod = "POST",
                    ApiEndpoint = "/FeedbackbyManager/AddFeedbackbyManager",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = feedback.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(feedback),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "FeedbackbyManager",
                    HttpMethod = "POST",
                    ApiEndpoint = "/FeedbackbyManager/AddFeedbackbyManager",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = feedback.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(feedback),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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

            AuditLog log = new AuditLog
            {
                Module = "UpdateFeedback",
                HttpMethod = "POST",
                ApiEndpoint = "/Feedback/UpdateFeedback",
                SuccessMessage = feedback,
                Payload = System.Text.Json.JsonSerializer.Serialize(updatedFeedback),
                StaffId = updatedFeedback.UpdatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Feedback",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Feedback/UpdateFeedback",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedFeedback.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedFeedback),
                    CreatedUtc = DateTime.UtcNow
                };
                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }

            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            using (var logContext = new AttendanceManagementSystemContext())
            {
                ErrorLog log = new ErrorLog
                {
                    Module = "Feedback",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Feedback/UpdateFeedback",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = updatedFeedback.UpdatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(updatedFeedback),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
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
