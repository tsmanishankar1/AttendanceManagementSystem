using AttendanceManagement.Input_Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelImportController : ControllerBase
{
    private readonly ExcelImportService _excelImportService;
    private readonly LoggingService _loggingService;

    public ExcelImportController(ExcelImportService excelImportService, LoggingService loggingService)
    {
        _excelImportService = excelImportService;
        _loggingService = loggingService;
    }

    [HttpGet("DownloadExcelTemplates")]
    public async Task<IActionResult> DownloadExcelTemplates(int excelImportId)
    {
        try
        {
            var template = await _excelImportService.DownloadExcelTemplates(excelImportId);
            return File(template.file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", template.ExcelName);
        }
        catch(MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
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
            await _loggingService.AuditLog("Excel Import", "POST", "/ExcelImport/ImportExcelFiles", result, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/ExcelImport/ImportExcelFiles", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}