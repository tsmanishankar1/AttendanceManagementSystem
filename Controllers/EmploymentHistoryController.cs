using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploymentHistoryController : ControllerBase
    {
        private readonly EmploymentHistoryService _employmentHistoryService;
        private readonly AttendanceManagementSystemContext _context;

        public EmploymentHistoryController(EmploymentHistoryService employmentHistoryService, AttendanceManagementSystemContext context)
        {
            _employmentHistoryService = employmentHistoryService;
            _context = context;
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

        [HttpGet("GetEmploymentHistoryById")]
        public async Task<IActionResult> GetByIdAsync(int employeeHistoryId)
        {
            try
            {
                var employmentHistory = await _employmentHistoryService.GetByIdAsync(employeeHistoryId);
                var response = new
                {
                    Success = true,
                    Message = employmentHistory
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
                AuditLog log = new AuditLog
                {
                    Module = "Employment History",
                    HttpMethod = "POST",
                    ApiEndpoint = "/EmploymentHistory/CreateEmploymentHistory",
                    SuccessMessage = newEmploymentHistory,
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
                        Module = "Employment History",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmploymentHistory/CreateEmploymentHistory",
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
                AuditLog log = new AuditLog
                {
                    Module = "Employment History",
                    HttpMethod = "GET",
                    ApiEndpoint = "/EmploymentHistory/UpdateEmploymentHistory",
                    SuccessMessage = updatedEmploymentHistory,
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
                        Module = "Employment History",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmploymentHistory/UpdateEmploymentHistory",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = model.UpdatedBy,
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
                        Module = "Employment History",
                        HttpMethod = "POST",
                        ApiEndpoint = "/EmploymentHistory/UpdateEmploymentHistory",
                        ErrorMessage = ex.Message,
                        StackTrace = ex.StackTrace,
                        InnerException = ex.InnerException?.ToString(),
                        StaffId = model.UpdatedBy,
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
