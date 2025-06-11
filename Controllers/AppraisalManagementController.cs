using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tsp;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppraisalManagementController : ControllerBase
    {
        private readonly AppraisalManagementService _service;
        private readonly LoggingService _loggingService;
        public AppraisalManagementController(AppraisalManagementService service, LoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet("GetProductionEmployees")]
        public async Task<IActionResult> GetProductionEmployees(int appraisalId)
        {
            try
            {
                var appraisalDetails = await _service.GetProductionEmployees(appraisalId);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                return Ok(response);
            }
            catch(MessageNotFoundException ex)
            {
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("MoveSelectedStaffToMis")]
        public async Task<IActionResult> MoveSelectedStaffToMis(SelectedEmployeesRequest selectedEmployeesRequest)
        {
            try
            {
                var appraisalDetails = await _service.MoveSelectedStaffToMis(selectedEmployeesRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaffToMis", appraisalDetails, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaffToMis", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaffToMis", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaffToMis", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelectedEmployees")]
        public async Task<IActionResult> GetSelectedEmployees(int appraisalId)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployees(appraisalId);
                var response = new
                {
                    Success = true,
                    Message = fileBytes
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

        [HttpPost("MisUploadSheet")]
        public async Task<IActionResult> MisUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
        {
            try
            {
                var appraisalDetails = await _service.MisUploadSheet(uploadMisSheetRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", appraisalDetails, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelectedEmployeeReview")]
        public async Task<IActionResult> GetSelectedEmployeeReview(int appraisalId)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployeeReview(appraisalId);
                var response = new
                {
                    Success = true,
                    Message = fileBytes
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

        [HttpGet("GetAllAgm")]
        public async Task<IActionResult> GetAllAgm()
        {
            try
            {
                var fileBytes = await _service.GetAllAgm();
                var response = new
                {
                    Success = true,
                    Message = fileBytes
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

        [HttpPost("MoveToAgmApproval")]
        public async Task<IActionResult> MoveToAgmApproval(AgmApprovalTab agmApprovalRequest)
        {
            try
            {
                var appraisalDetails = await _service.MoveToAgmApproval(agmApprovalRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/MoveToAgmApproval", appraisalDetails, agmApprovalRequest.CreatedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveToAgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.CreatedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveToAgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.CreatedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveToAgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.CreatedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelectedEmployeeAgmApproval")]
        public async Task<IActionResult> GetSelectedEmployeeAgmApproval(int appraisalId)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployeeAgmApproval(appraisalId);
                var response = new
                {
                    Success = true,
                    Message = fileBytes
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

        [HttpPost("AgmApproval")]
        public async Task<IActionResult> AgmApproval(AgmApprovalRequest agmApprovalRequest)
        {
            try
            {
                var fileBytes = await _service.AgmApproval(agmApprovalRequest);
                var response = new
                {
                    Success = true,
                    Message = fileBytes
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/AgmApproval", fileBytes, agmApprovalRequest.ApprovedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.ApprovedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.ApprovedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GenerateAppraisalLetter")]
        public async Task<IActionResult> GenerateAppraisalLetter(int createdBy)
        {
            try
            {
                var appraisal = await _service.GenerateAppraisalLetter(createdBy);
                var response = new
                {
                    Success = true,
                    Message = appraisal
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/GenerateAppraisalLetter", appraisal, createdBy, JsonSerializer.Serialize(createdBy));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(createdBy));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(createdBy));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("ViewAppraisalLetter")]
        public async Task<IActionResult> ViewAppraisalLetter(int staffId, int fileId)
        {
            try
            {
                var (stream, fileName) = await _service.ViewAppraisalLetter(staffId, fileId);
                Response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileName}\"");
                return File(stream, "application/pdf");
            }
            catch (FileNotFoundException ex)
            {
                return ErrorClass.NotFoundResponse(ex.Message);
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

        [HttpGet("DownloadAppraisalLetter")]
        public async Task<IActionResult> DownloadAppraisalLetter(int staffId, int fileId)
        {
            try
            {
                var filePath = await _service.DownloadAppraisalLetter(staffId, fileId);
                if (!System.IO.File.Exists(filePath))
                {
                    return ErrorClass.NotFoundResponse("Appraisal letter not found");
                }
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var file = Path.GetFileName(filePath);
                var contentType = "application/pdf";

                return File(fileBytes, contentType, file);
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

        [HttpPost("AcceptAppraisalLetter")]
        public async Task<IActionResult> AcceptAppraisalLetter(LetterAcceptance letterAcceptance)
        {
            try
            {
                var fileBytes = await _service.AcceptAppraisalLetter(letterAcceptance);
                var response = new
                {
                    Success = true,
                    Message = fileBytes
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/AcceptAppraisalLetter", fileBytes, letterAcceptance.AcceptedBy, JsonSerializer.Serialize(letterAcceptance));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AcceptAppraisalLetter", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, letterAcceptance.AcceptedBy, JsonSerializer.Serialize(letterAcceptance));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AcceptAppraisalLetter", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, letterAcceptance.AcceptedBy, JsonSerializer.Serialize(letterAcceptance));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AcceptAppraisalLetter", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, letterAcceptance.AcceptedBy, JsonSerializer.Serialize(letterAcceptance));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetAcceptedEmployees")]
        public async Task<IActionResult> GetAcceptedEmployees()
        {
            try
            {
                var appraisalDetails = await _service.GetAcceptedEmployees();
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
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
    }
}
