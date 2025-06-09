using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using DocumentFormat.OpenXml.Wordprocessing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffTransactionController : ControllerBase
    {
        private readonly StaffTransactionService _academicDetailService;
        private readonly LoggingService _loggingService;

        public StaffTransactionController(StaffTransactionService academicDetailService, LoggingService loggingService)
        {
            _academicDetailService = academicDetailService;
            _loggingService = loggingService;
        }

        [HttpGet("GetAcademicDetailsbyStaffId")]
        public async Task<IActionResult> GetByStaffId(int staffId)
        {
            try
            {
                var academicDetail = await _academicDetailService.GetByStaffIdAsync(staffId);
                var response = new
                {
                    Success = true,
                    Message = academicDetail
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

        [HttpPost("CreateAcademicDetails")]
        public async Task<IActionResult> Create(ListAcademicDetailRequest academicDetailRequests)
        {
            try
            {
                var createdMessage = await _academicDetailService.CreateAsync(academicDetailRequests);
                var response = new
                {
                    Success = true,
                    Message = createdMessage
                };
                await _loggingService.AuditLog("Academic Detail", "POST", "/api/StaffTransaction/CreateAcademicDetails", createdMessage,academicDetailRequests.CreatedBy,JsonSerializer.Serialize(academicDetailRequests));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Academic Detail", "POST", "/api/StaffTransaction/CreateAcademicDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, academicDetailRequests.CreatedBy, JsonSerializer.Serialize(academicDetailRequests));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateAcademicDetails")]
        public async Task<IActionResult> Update(ListAcademicDetailUpdateRequest academicDetailsRequests)
        {
            try
            {
                var updatedMessage = await _academicDetailService.UpdateAsync(academicDetailsRequests);

                var response = new
                {
                    Success = true,
                    Message = updatedMessage
                };
                await _loggingService.AuditLog("Academic Detail", "POST", "/api/StaffTransaction/UpdateAcademicDetails", updatedMessage, academicDetailsRequests.UpdatedBy, JsonSerializer.Serialize(academicDetailsRequests));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Academic Detail", "POST", "/api/StaffTransaction/UpdateAcademicDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, academicDetailsRequests.UpdatedBy, JsonSerializer.Serialize(academicDetailsRequests));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Academic Detail", "POST", "/api/StaffTransaction/UpdateAcademicDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, academicDetailsRequests.UpdatedBy, JsonSerializer.Serialize(academicDetailsRequests));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("DeleteAcademicdetails")]
        public async Task<IActionResult> DeleteAcademicDetailAsync(int academicDetailId, int deletedBy)
        {
            try
            {
                var isDeleted = await _academicDetailService.DeleteAcademicDetailAsync(academicDetailId, deletedBy);
                var response = new
                {
                    Success = true,
                    Message = isDeleted
                };
                await _loggingService.AuditLog("Academic Detail", "POST", "/api/StaffTransaction/DeleteAcademicdetails", isDeleted, deletedBy, JsonSerializer.Serialize(new { academicDetailId, deletedBy }));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Academic Detail", "POST", "/api/StaffTransaction/DeleteAcademicdetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { academicDetailId, deletedBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Academic Detail", "POST", "/api/StaffTransaction/DeleteAcademicdetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { academicDetailId, deletedBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetCertificationDetailsByStaffId")]
        public async Task<IActionResult> GetById(int staffId)
        {
            try
            {
                var certification = await _academicDetailService.GetByCerticateStaffIdAsync(staffId);
                var response = new
                {
                    Success = true,
                    Message = certification
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

        [HttpPost("CreateCertificationDetails")]
        public async Task<IActionResult> Create(ListCertificationCourseRequest certificationCourseRequests)
        {
            try
            {
                var createdMessage = await _academicDetailService.CreateAsync(certificationCourseRequests);
                var response = new
                {
                    Success = true,
                    Message = createdMessage
                };
                await _loggingService.AuditLog( "Certification Detail", "POST","/api/StaffTransaction/CreateCertificationDetails", createdMessage, certificationCourseRequests.CreatedBy, JsonSerializer.Serialize(certificationCourseRequests));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Certification Detail", "POST", "/api/StaffTransaction/CreateCertificationDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty,  certificationCourseRequests.CreatedBy,  JsonSerializer.Serialize(certificationCourseRequests));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateCertificationDetails")]
        public async Task<IActionResult> Update(ListCertificationCourseUpdateRequest certificationCourses)
        {
            try
            {
                var updatedMessage = await _academicDetailService.UpdateAsync(certificationCourses);
                var response = new
                {
                    Success = true,
                    Message = updatedMessage
                };
                await _loggingService.AuditLog("Certification Detail", "POST", "/api/StaffTransaction/UpdateCertificationDetails", updatedMessage,certificationCourses.UpdatedBy, JsonSerializer.Serialize(certificationCourses));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Certification Detail", "POST", "/api/StaffTransaction/UpdateCertificationDetails", ex.Message, ex.StackTrace ?? string.Empty,  ex.InnerException?.ToString() ?? string.Empty,  certificationCourses.UpdatedBy,    JsonSerializer.Serialize(certificationCourses));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError( "Certification Detail", "POST", "/api/StaffTransaction/UpdateCertificationDetails", ex.Message, ex.StackTrace ?? string.Empty,ex.InnerException?.ToString() ?? string.Empty, certificationCourses.UpdatedBy, JsonSerializer.Serialize(certificationCourses));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("DeleteCertificationDetails")]
        public async Task<IActionResult> Delete(int certificationCourseId, int deletedBy)
        {
            try
            {
                var deleted = await _academicDetailService.DeleteCertificationCourseAsync(certificationCourseId, deletedBy);
                var response = new
                {
                    Success = true,
                    Message = deleted
                };
                await _loggingService.AuditLog("Certification Detail", "POST", "/api/StaffTransaction/DeleteCertificationDetails", deleted, deletedBy, JsonSerializer.Serialize(new {certificationCourseId,deletedBy}));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Certification Detail", "POST", "/api/StaffTransaction/DeleteCertificationDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { certificationCourseId, deletedBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Certification Detail", "POST", "/api/StaffTransaction/DeleteCertificationDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { certificationCourseId, deletedBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    
        [HttpGet("GetWorkHistorybyStaffId")]
        public async Task<IActionResult> GetWorkhistoryByStaffId(int staffId)
        {
            try
            { 
                var result = await _academicDetailService.GetWorkhistoryByStaffIdAsync(staffId);
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

        [HttpPost("CreateWorkHistory")]
        public async Task<IActionResult> Create(ListPreviousEmploymentRequest previousEmploymentRequests)
        {
            try
            {
                var message = await _academicDetailService.CreateAsync(previousEmploymentRequests);
                var response = new
                {
                    Success = true,
                    Message = message
                };
                await _loggingService.AuditLog("WorkHistory Detail", "POST", "/api/StaffTransaction/CreateWorkHistory", message, previousEmploymentRequests.CreatedBy,  JsonSerializer.Serialize(previousEmploymentRequests));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError( "WorkHistory Detail", "POST"," /api/StaffTransaction/CreateWorkHistory", ex.Message, ex.StackTrace ?? string.Empty,ex.InnerException?.ToString() ?? string.Empty,previousEmploymentRequests.CreatedBy,JsonSerializer.Serialize(previousEmploymentRequests));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateWorkHistory")]
        public async Task<IActionResult> Update(ListPreviousEmploymentUpdateRequest previousEmploymentUpdateRequest)
        {
            try
            {
                var message = await _academicDetailService.UpdateAsync(previousEmploymentUpdateRequest);
                var response = new
                {
                    Success = true,
                    Message = message
                };
                await _loggingService.AuditLog("WorkHistory Detail", "POST", "/api/StaffTransaction/UpdateWorkHistory", message,previousEmploymentUpdateRequest.UpdatedBy,JsonSerializer.Serialize(previousEmploymentUpdateRequest));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("WorkHistory Detail", "POST", "/api/StaffTransaction/UpdateWorkHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty,previousEmploymentUpdateRequest.UpdatedBy,JsonSerializer.Serialize(previousEmploymentUpdateRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("WorkHistory Detail", "POST", "/api/StaffTransaction/UpdateWorkHistory", ex.Message, ex.StackTrace ?? string.Empty,ex.InnerException?.ToString() ?? string.Empty,previousEmploymentUpdateRequest.UpdatedBy,JsonSerializer.Serialize(previousEmploymentUpdateRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("DeleteWorkHistory")]
        public async Task<IActionResult> DeleteWorkhistory(int id, int deletedBy)
        {
            try
            { 
                var message = await _academicDetailService.DeleteAsync(id, deletedBy);
                var response = new
                {
                    Success = true,
                    Message = message
                };
                await _loggingService.AuditLog("Delete Work History", "POST", "/api/StaffTransaction/DeleteWorkHistory", message, deletedBy, JsonSerializer.Serialize(new { id, deletedBy }));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Delete Work History", "POST", "/api/StaffTransaction/DeleteWorkHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { id, deletedBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Delete Work History", "POST", "/api/StaffTransaction/DeleteWorkHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, deletedBy, JsonSerializer.Serialize(new { id, deletedBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}