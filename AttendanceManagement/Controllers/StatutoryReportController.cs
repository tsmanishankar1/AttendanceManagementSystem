using AttendanceManagement.InputModels;
using AttendanceManagement.Services;
using AttendanceManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatutoryReportController : ControllerBase
    {
        private readonly IStatutoryReport _service;
        private readonly ILoggingService _loggingService;
        public StatutoryReportController(IStatutoryReport service, ILoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        [HttpPost("GenerateStatutoryReport")]
        public async Task<IActionResult> GenerateStatutoryReport(StatutoryReportRequest statutoryReportRequest)
        {
            try
            {
                var report = await _service.GenerateStatutoryReport(statutoryReportRequest);
                var response = new
                {
                    Success = true,
                    Message = report
                };
                await _loggingService.AuditLog("Statutory Report", "POST", "/api/StatutoryReport/GenerateStatutoryReport", "Report retrieved successfully", statutoryReportRequest.CreatedBy, JsonSerializer.Serialize(statutoryReportRequest));
                return Ok(response);
            }
            catch(MessageNotFoundException ex)
            {
                await _loggingService.LogError("Statutory Report", "POST", "/api/StatutoryReport/GenerateStatutoryReport", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, statutoryReportRequest.CreatedBy, JsonSerializer.Serialize(statutoryReportRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Statutory Report", "POST", "/api/StatutoryReport/GenerateStatutoryReport", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, statutoryReportRequest.CreatedBy, JsonSerializer.Serialize(statutoryReportRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }
    }
}