using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationalQualificationController : ControllerBase
    {
        private readonly EducationalQualificationService _service;
        private readonly LoggingService _loggingService;

        public EducationalQualificationController(EducationalQualificationService service, LoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet("GetAllEducationalQualifications")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var details = await _service.GetAllEducationalQualifications();
                var response = new
                {
                    Success = true,
                    Message = details
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

        [HttpGet("GetEducationalQualificationById")]
        public async Task<IActionResult> GetById(int educationalQualificationId)
        {
            try
            {
                var details = await _service.GetEducationalQualificationById(educationalQualificationId);
                var response = new
                {
                    Success = true,
                    Message = details
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

        [HttpPost("AddEducationalQualification")]
        public async Task<IActionResult> Create(EducationalQualificationDto qualificationDto)
        {
            try
            {
                var createdDetail = await _service.CreateEducationalQualification(qualificationDto);
                var response = new
                {
                    Success = true,
                    Message = createdDetail
                };
                await _loggingService.AuditLog("Educational Qualification", "POST", "/EducationalQualification/AddEducationalQualification", createdDetail, qualificationDto.CreatedBy, JsonSerializer.Serialize(qualificationDto));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Educational Qualification", "POST", "/EducationalQualification/AddEducationalQualification", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, qualificationDto.CreatedBy, JsonSerializer.Serialize(qualificationDto));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateEducationalQualification")]
        public async Task<IActionResult> Update(UpdateEducationalQualification qualificationDto)
        {
            try
            {
                var updated = await _service.UpdateEducationalQualification(qualificationDto);
                var response = new
                {
                    Success = true,
                    Message = updated
                };
                await _loggingService.AuditLog("Educational Qualification", "POST", "/EducationalQualification/UpdateEducationalQualification", updated, qualificationDto.UpdatedBy, JsonSerializer.Serialize(qualificationDto));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Educational Qualification", "POST", "/EducationalQualification/UpdateEducationalQualification", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, qualificationDto.UpdatedBy, JsonSerializer.Serialize(qualificationDto));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Educational Qualification", "POST", "/EducationalQualification/UpdateEducationalQualification", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, qualificationDto.UpdatedBy, JsonSerializer.Serialize(qualificationDto));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}
