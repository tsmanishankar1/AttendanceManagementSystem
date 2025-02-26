using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Input_Models;
using Swashbuckle.AspNetCore.Annotations;
using AttendanceManagement.Input_Models.AttendanceManagement.Models;
using NSwag.Annotations;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactController : ControllerBase
    {
        private readonly EmergencyContactService _emergencyContactService;
        private readonly LoggingService _loggingService;

        public EmergencyContactController(EmergencyContactService emergencyContactService, LoggingService loggingService)
        {
            _emergencyContactService = emergencyContactService;
            _loggingService = loggingService;
        }

        [HttpGet("GetAllEmergencyContacts")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await _emergencyContactService.GetAllAsync();
                var response = new
                {
                    Success = true,
                    Message = contacts
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

        [HttpGet("GetEmergencyById")]
        public async Task<IActionResult> GetByIdAsync(int emergencyContactId)
        {
            try
            {
                var emergencyContact = await _emergencyContactService.GetByIdAsync(emergencyContactId);
                var response = new
                {
                    Success = true,
                    Message = emergencyContact
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

        [HttpPost("CreateEmergencyContact")]
        public async Task<IActionResult> Create(EmergencyContactRequestModel model)
        {
            try
            {
                var createdContact = await _emergencyContactService.CreateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = createdContact
                };
                await _loggingService.AuditLog("Emergency Contact", "POST", "/EmergencyContact/CreateEmergencyContact", createdContact, model.CreatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Emergency Contact", "POST", "/EmergencyContact/CreateEmergencyContact", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.CreatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("UpdateEmergencyContact")]
        public async Task<IActionResult> Update(EmergencyContactUpdateModel model)
        {
            try
            {
                var updatedContact = await _emergencyContactService.UpdateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = updatedContact
                };
                await _loggingService.AuditLog("Emergency Contact", "POST", "/EmergencyContact/UpdateEmergencyContact", updatedContact, model.UpdatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Emergency Contact", "POST", "/EmergencyContact/UpdateEmergencyContact", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Emergency Contact", "POST", "/EmergencyContact/UpdateEmergencyContact", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}
