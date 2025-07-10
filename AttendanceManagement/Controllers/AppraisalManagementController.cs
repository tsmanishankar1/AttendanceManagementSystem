using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppraisalManagementController : ControllerBase
    {
        private readonly IAppraisalManagementApp _service;
        private readonly ILoggingApp _loggingService;
        public AppraisalManagementController(IAppraisalManagementApp service, ILoggingApp loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet("GetProductionEmployees")]
        public async Task<IActionResult> GetProductionEmployees(int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetProductionEmployees(appraisalId, year, quarter);
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
        public async Task<IActionResult> GetSelectedEmployees(int appraisalId, int year, string quarter)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployees(appraisalId, year, quarter);
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

        [HttpGet("GetExcelTemplates")]
        public async Task<IActionResult> GetExcelTemplates(string name)
        {
            try
            {
                var filePath = _service.GetExcelTemplateFile(name);
                if (!System.IO.File.Exists(filePath))
                {
                    return ErrorClass.NotFoundResponse("Excel template file not found");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(fileBytes, contentType, fileName);
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
            catch (ArgumentException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.BadResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (ConflictException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MisUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelectedEmployeeReview")]
        public async Task<IActionResult> GetSelectedEmployeeReview(int appraisalId, int year, string quarter)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployeeReview(appraisalId, year, quarter);
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
        public async Task<IActionResult> GetSelectedEmployeeAgmApproval(int appraisalId, int year, string quarter)
        {
            try
            {
                var fileBytes = await _service.GetSelectedEmployeeAgmApproval(appraisalId, year, quarter);
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
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.ApprovedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/AgmApproval", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, agmApprovalRequest.ApprovedBy, JsonSerializer.Serialize(agmApprovalRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GenerateAppraisalLetter")]
        public async Task<IActionResult> GenerateAppraisalLetter(int createdBy, int year)
        {
            try
            {
                var appraisal = await _service.GenerateAppraisalLetter(createdBy, year);
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
            catch (ConflictException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(createdBy));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(createdBy));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("ViewAppraisalLetter")]
        public async Task<IActionResult> ViewAppraisalLetter(int staffId)
        {
            try
            {
                var (stream, fileName) = await _service.ViewAppraisalLetter(staffId);
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
        public async Task<IActionResult> DownloadAppraisalLetter(int staffId)
        {
            try
            {
                var filePath = await _service.DownloadAppraisalLetter(staffId);
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
        public async Task<IActionResult> GetAcceptedEmployees(int year)
        {
            try
            {
                var appraisalDetails = await _service.GetAcceptedEmployees(year);
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

        [HttpGet("GetNonProductionEmployees")]
        public async Task<IActionResult> GetNonProductionEmployees(int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetNonProductionEmployees(appraisalId, year, quarter);
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

        [HttpPost("MoveSelectedStaff")]
        public async Task<IActionResult> MoveSelectedStaff(SelectedEmployeesRequest selectedEmployeesRequest)
        {
            try
            {
                var appraisalDetails = await _service.MoveSelectedStaff(selectedEmployeesRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaff", appraisalDetails, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/MoveSelectedStaff", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selectedEmployeesRequest.CreatedBy, JsonSerializer.Serialize(selectedEmployeesRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelectedNonProductionEmployees")]
        public async Task<IActionResult> GetSelectedNonProductionEmployees(int appraisalId, int year, string quarter)
        {
            try
            {
                var fileBytes = await _service.GetSelectedNonProductionEmployees(appraisalId, year, quarter);
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

        [HttpPost("CreateKra")]
        public async Task<IActionResult> CreateKra(KraDto kraDto)
        {
            try
            {
                var appraisalDetails = await _service.CreateKra(kraDto);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/CreateKra", appraisalDetails, kraDto.CreatedBy, JsonSerializer.Serialize(kraDto));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateKra", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, kraDto.CreatedBy, JsonSerializer.Serialize(kraDto));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateKra", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, kraDto.CreatedBy, JsonSerializer.Serialize(kraDto));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetKra")]
        public async Task<IActionResult> GetKra(int createdBy, int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetKra(createdBy, appraisalId, year, quarter);
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

        [HttpPost("CreateSelfEvaluation")]
        public async Task<IActionResult> CreateSelfEvaluation([FromForm] SelfEvaluationRequest selfEvaluationRequest)
        {
            try
            {
                var appraisalDetails = await _service.CreateSelfEvaluation(selfEvaluationRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                }; 
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/CreateSelfEvaluation", appraisalDetails, selfEvaluationRequest.CreatedBy, JsonSerializer.Serialize(selfEvaluationRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateSelfEvaluation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selfEvaluationRequest.CreatedBy, JsonSerializer.Serialize(selfEvaluationRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateSelfEvaluation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, selfEvaluationRequest.CreatedBy, JsonSerializer.Serialize(selfEvaluationRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSelfEvaluation")]
        public async Task<IActionResult> GetSelfEvaluation(int createdBy, int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetSelfEvaluation(createdBy, appraisalId, year, quarter);
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

        [HttpPost("CreateManagerEvaluation")]
        public async Task<IActionResult> CreateManagerEvaluation([FromForm] ManagerEvaluationRequest managerEvaluationRequest)
        {
            try
            {
                var appraisalDetails = await _service.CreateManagerEvaluation(managerEvaluationRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/CreateManagerEvaluation", appraisalDetails, managerEvaluationRequest.CreatedBy, JsonSerializer.Serialize(managerEvaluationRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateManagerEvaluation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, managerEvaluationRequest.CreatedBy, JsonSerializer.Serialize(managerEvaluationRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/CreateManagerEvaluation", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, managerEvaluationRequest.CreatedBy, JsonSerializer.Serialize(managerEvaluationRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetManagerEvaluation")]
        public async Task<IActionResult> GetManagerEvaluation(int createdBy, int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetManagerEvaluation(createdBy, appraisalId, year, quarter);
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

        [HttpGet("GetFinalAverageManagerScore")]
        public async Task<object> GetFinalAverageManagerScore(int createdBy, int appraisalId, int year, string? quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetFinalAverageManagerScore(createdBy, appraisalId, year, quarter);
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

        [HttpPost("HrUploadSheet")]
        public async Task<IActionResult> HrUploadSheet(UploadMisSheetRequest uploadMisSheetRequest)
        {
            try
            {
                var appraisalDetails = await _service.HrUploadSheet(uploadMisSheetRequest);
                var response = new
                {
                    Success = true,
                    Message = appraisalDetails
                };
                await _loggingService.AuditLog("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", appraisalDetails, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (ArgumentException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.UnsupportedMediaTypeResponse(ex.Message);
            }
            catch (ConflictException ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Appraisal Management", "POST", "/api/AppraisalManagement/HrUploadSheet", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, uploadMisSheetRequest.CreatedBy, JsonSerializer.Serialize(uploadMisSheetRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetHrUploadedSheet")]
        public async Task<IActionResult> GetHrUploadedSheet(int appraisalId, int year, string quarter)
        {
            try
            {
                var appraisalDetails = await _service.GetHrUploadedSheet(appraisalId, year, quarter);
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
