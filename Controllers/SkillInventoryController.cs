using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillInventoryController : ControllerBase
    {
        private readonly SkillInventoryService _skillInventoryService;
        private readonly LoggingService _loggingService;

        public SkillInventoryController(SkillInventoryService skillInventoryService, LoggingService loggingService)
        {
            _skillInventoryService = skillInventoryService;
            _loggingService = loggingService;
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
                await _loggingService.AuditLog("Skill Inventory", "POST", "/Skills/CreateSkill", skill, model.CreatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Skill Inventory", "POST", "/Skills/CreateSkill", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.CreatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Skill Inventory", "POST", "/Skills/CreateSkill", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.CreatedBy, JsonSerializer.Serialize(model));
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
                await _loggingService.AuditLog("Skill Inventory", "POST", "/Skills/UpdateSkill", updatedSkill, model.UpdatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Skill Inventory", "POST", "/Skills/UpdateSkill", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Skill Inventory", "POST", "/Skills/UpdateSkill", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}

