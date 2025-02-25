using Microsoft.AspNetCore.Mvc;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using AttendanceManagement.Input_Models;
using Swashbuckle.AspNetCore.Annotations;
using AttendanceManagement.Input_Models.AttendanceManagement.Models;
using NSwag.Annotations;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactController : ControllerBase
    {
        private readonly EmergencyContactService _emergencyContactService;
        private readonly AttendanceManagementSystemContext _context;

        public EmergencyContactController(EmergencyContactService emergencyContactService, AttendanceManagementSystemContext context)
        {
            _emergencyContactService = emergencyContactService;
            _context = context;
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
                AuditLog log = new AuditLog
                {
                    Module = "Emergency Contact",
                    HttpMethod = "POST",
                    ApiEndpoint = "/EmergencyContact/CreateEmergencyContact",
                    SuccessMessage = createdContact,
                    Payload = System.Text.Json.JsonSerializer.Serialize(model),
                    StaffId = model.CreatedBy,
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
                        Module = "Emergency Contact",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmergencyContact/CreateEmergencyContact",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = model.CreatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(model),
                        CreatedUtc = DateTime.UtcNow
                    };

                    logContext.ErrorLogs.Add(log);
                    await logContext.SaveChangesAsync();
                }
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
                AuditLog log = new AuditLog
                {
                    Module = "Emergency Contact",
                    HttpMethod = "POST",
                    ApiEndpoint = "/EmergencyContact/UpdateEmergencyContact",
                    SuccessMessage = updatedContact,
                    Payload = System.Text.Json.JsonSerializer.Serialize(model),
                    StaffId = model.UpdatedBy,
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
                        Module = "Emergency Contact",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmergencyContact/UpdateEmergencyContact",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = model.UpdatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(model),
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
                        Module = "Emergency Contact",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmergencyContact/UpdateEmergencyContact",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = model.UpdatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(model),
                        CreatedUtc = DateTime.UtcNow
                    };

                    logContext.ErrorLogs.Add(log);
                    await logContext.SaveChangesAsync();
                }
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}
