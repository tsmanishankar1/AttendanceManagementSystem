using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace AttendanceManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyDetailsController : ControllerBase
    {
        private readonly FamilyDetailsService _service;
        private readonly LoggingService _loggingService;

        public FamilyDetailsController(FamilyDetailsService service, LoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpGet("GetAllFamilyDetails")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var details = await _service.GetAllFamilyDetails();
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

        [HttpPost("AddFamilyDetails")]
        public async Task<IActionResult> Create(FamilyDetailsDTO familyDetailsDTO)
        {
            try
            {
                var createdDetail = await _service.CreateFamilyDetail(familyDetailsDTO);
                var response = new
                {
                    Success = true,
                    Message = createdDetail
                };
                await _loggingService.AuditLog("Family Details", "POST", "/api/FamilyDetails/AddFamilyDetails", createdDetail, familyDetailsDTO.CreatedBy, JsonSerializer.Serialize(familyDetailsDTO));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Family Details", "POST", "/api/FamilyDetails/AddFamilyDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, familyDetailsDTO.CreatedBy, JsonSerializer.Serialize(familyDetailsDTO));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateFamilyDetails")]
        public async Task<IActionResult> Update(UpdateFamilyDetails familyDetailsDTO)
        {
            try
            {
                var updated = await _service.UpdateFamilyDetail(familyDetailsDTO);
                var response = new
                {
                    Success = true,
                    Message = updated
                };
                await _loggingService.AuditLog("Family Details", "POST", "/api/FamilyDetails/UpdateFamilyDetails", updated, familyDetailsDTO.UpdatedBy, JsonSerializer.Serialize(familyDetailsDTO));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Family Details", "POST", "/api/FamilyDetails/UpdateFamilyDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, familyDetailsDTO.UpdatedBy, JsonSerializer.Serialize(familyDetailsDTO));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Family Details", "POST", "/api/FamilyDetails/UpdateFamilyDetails", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, familyDetailsDTO.UpdatedBy, JsonSerializer.Serialize(familyDetailsDTO));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}