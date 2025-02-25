using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillInventoryController : ControllerBase
    {
        private readonly SkillInventoryService _skillInventoryService;
        private readonly AttendanceManagementSystemContext _context;

        public SkillInventoryController(SkillInventoryService skillInventoryService, AttendanceManagementSystemContext context)
        {
            _skillInventoryService = skillInventoryService;
            _context = context;
        }

        [HttpGet("GetAllSkills")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var skills = await _skillInventoryService.GetAllAsync();
                var response = new
                {
                    Success = true,
                    Message = skills
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

        [HttpGet("GetSkillById")]
        public async Task<IActionResult> GetByIdAsync(int skillId)
        {
            try
            {
                var skill = await _skillInventoryService.GetByIdAsync(skillId);
                var response = new
                {
                    Success = true,
                    Message = skill
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

        [HttpPost("CreateSkill")]
        public async Task<IActionResult> CreateAsync(SkillInventoryRequestModel model)
        {
            try
            {
                var skill = await _skillInventoryService.CreateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = skill
                };
                AuditLog log = new AuditLog
                {
                    Module = "CreateSkill",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Skills/CreateSkill",
                    SuccessMessage = skill,
                    Payload = System.Text.Json.JsonSerializer.Serialize(model),
                    StaffId = model.CreatedBy,
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
                        Module = "CreateSkill",
                        HttpMethod = "POST",
                        ApiEndpoint = "/Skills/CreateSkill",
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
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                using (var logContext = new AttendanceManagementSystemContext())
                {
                    ErrorLog log = new ErrorLog
                    {
                        Module = "CreateSkill",
                        HttpMethod = "POST",
                        ApiEndpoint = "/Skills/CreateSkill",
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

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(SkillInventoryUpdateModel model)
        {
            try
            {
                var updatedSkill = await _skillInventoryService.UpdateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = updatedSkill
                };
                AuditLog log = new AuditLog
                {
                    Module = "UpdateSkill",
                    HttpMethod = "POST",
                    ApiEndpoint = "/Shift/UpdateSkill",
                    SuccessMessage = updatedSkill,
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
                        Module = "UpdateSkill",
                        HttpMethod = "POST",
                        ApiEndpoint = "/Skill/UpdateSkill",
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
                        Module = "UpdateSkill",
                        HttpMethod = "POST",
                        ApiEndpoint = "/Skill/UpdateSkill",
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

