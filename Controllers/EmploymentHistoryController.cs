using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentHistoryController : ControllerBase
    {
        private readonly EmploymentHistoryService _employmentHistoryService;
        private readonly LoggingService _loggingService;

        public EmploymentHistoryController(EmploymentHistoryService employmentHistoryService, LoggingService loggingService)
        {
            _employmentHistoryService = employmentHistoryService;
            _loggingService = loggingService;
        }

        [HttpGet("GetAllEmploymentHistory")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var employmentHistories = await _employmentHistoryService.GetAllAsync();
                var response = new
                {
                    Success = true,
                    Message = employmentHistories
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

        [HttpPost("CreateEmploymentHistory")]
        public async Task<IActionResult> CreateAsync(EmploymentHistoryRequestModel model)
        {
            try
            {
                var newEmploymentHistory = await _employmentHistoryService.CreateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = newEmploymentHistory
                };
                await _loggingService.AuditLog("Employment History", "POST", "/api/EmploymentHistory/CreateEmploymentHistory", newEmploymentHistory, model.CreatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Employment History", "POST", "/api/EmploymentHistory/CreateEmploymentHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.CreatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(EmploymentHistoryUpdateModel model)
        {
            try
            {
                var updatedEmploymentHistory = await _employmentHistoryService.UpdateAsync(model);
                var response = new
                {
                    Success = true,
                    Message = updatedEmploymentHistory
                };
                await _loggingService.AuditLog("Employment History", "POST", "/api/EmploymentHistory/UpdateEmploymentHistory", updatedEmploymentHistory, model.UpdatedBy, JsonSerializer.Serialize(model));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Employment History", "POST", "/api/EmploymentHistory/UpdateEmploymentHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Employment History", "POST", "/api/EmploymentHistory/UpdateEmploymentHistory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, model.UpdatedBy, JsonSerializer.Serialize(model));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}
