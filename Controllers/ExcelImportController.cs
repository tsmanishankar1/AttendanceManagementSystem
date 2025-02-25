using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelImportController : ControllerBase
{
    private readonly ExcelImportService _excelImportService;
    private readonly AttendanceManagementSystemContext _context;

    public ExcelImportController(ExcelImportService excelImportService, AttendanceManagementSystemContext context)
    {
        _excelImportService = excelImportService;
        _context = context;
    }

    [HttpPost("ImportExcel")]
    public async Task<IActionResult> ImportExcel(ExcelImportDto excelImportDto)
    {
        try
        {
            var result = await _excelImportService.ImportExcelAsync(excelImportDto.ExcelImportId, excelImportDto.CreatedBy, excelImportDto.File);
            var response = new
            {
                Success = true,
                Message = result
            };
            AuditLog log = new AuditLog
            {
                Module = "Excel Import",
                HttpMethod = "POST",
                ApiEndpoint = "/ExcelImport/ImportExcelFiles",
                SuccessMessage = result,
                Payload = System.Text.Json.JsonSerializer.Serialize(excelImportDto),
                StaffId = excelImportDto.CreatedBy,
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
                    Module = "Excel Import",
                    HttpMethod = "POST",
                    ApiEndpoint = "/ExcelImport/ImportExcelFiles",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    InnerException = ex.InnerException?.ToString(),
                    StaffId = excelImportDto.CreatedBy,
                    Payload = System.Text.Json.JsonSerializer.Serialize(excelImportDto),
                    CreatedUtc = DateTime.UtcNow
                };

                logContext.ErrorLogs.Add(log);
                await logContext.SaveChangesAsync();
            }
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}