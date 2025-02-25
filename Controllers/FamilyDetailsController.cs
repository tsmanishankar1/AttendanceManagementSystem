using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace AttendanceManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyDetailsController : ControllerBase
    {
        private readonly FamilyDetailsService _service;
        private readonly AttendanceManagementSystemContext _context;

        public FamilyDetailsController(FamilyDetailsService service, AttendanceManagementSystemContext context)
        {
            _service = service;
            _context = context;
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

        [HttpGet("GetFamilyDetailById")]
        public async Task<IActionResult> GetById(int familyDetailsId)
        {
            try
            {
                var details = await _service.GetFamilyDetailById(familyDetailsId);
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
                AuditLog log = new AuditLog
                {
                    Module = "Family Details",
                    HttpMethod = "POST",
                    ApiEndpoint = "/FamilyDetails/CreateFamilyDetails",
                    SuccessMessage = createdDetail,
                    Payload = System.Text.Json.JsonSerializer.Serialize(familyDetailsDTO),
                    StaffId = familyDetailsDTO.CreatedBy,
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
                        Module = "Family Details",
                        HttpMethod = "POST",
                        ApiEndpoint = "/FamilyDetails/CreateFamilyDetails",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = familyDetailsDTO.CreatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(familyDetailsDTO),
                        CreatedUtc = DateTime.UtcNow
                    };

                    logContext.ErrorLogs.Add(log);
                    await logContext.SaveChangesAsync();
                }
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
                AuditLog log = new AuditLog
                {
                    Module = "Family Details",
                    HttpMethod = "POST",
                    ApiEndpoint = "/FamilyDetails/UpdateFamilyDetails",
                    SuccessMessage = updated,
                    Payload = System.Text.Json.JsonSerializer.Serialize(familyDetailsDTO),
                    StaffId = familyDetailsDTO.UpdatedBy,
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
                        Module = "Family Details",
                        HttpMethod = "POST",
                        ApiEndpoint = "/FamilyDetails/UpdateFamilyDetails",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = familyDetailsDTO.UpdatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(familyDetailsDTO),
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
                        Module = "Family Details",
                        HttpMethod = "POST",
                        ApiEndpoint = "/FamilyDetails/UpdateFamilyDetails",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = familyDetailsDTO.UpdatedBy,
                        Payload = System.Text.Json.JsonSerializer.Serialize(familyDetailsDTO),
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
