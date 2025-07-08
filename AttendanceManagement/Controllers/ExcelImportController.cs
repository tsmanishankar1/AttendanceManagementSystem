using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelImportController : ControllerBase
{
    private readonly IExcelImportApp _excelImportService;
    private readonly ILoggingApp _loggingService;

    public ExcelImportController(IExcelImportApp excelImportService, ILoggingApp loggingService)
    {
        _excelImportService = excelImportService;
        _loggingService = loggingService;
    }

    [HttpGet("DownloadExcelTemplates")]
    public async Task<IActionResult> DownloadExcelTemplates(int excelImportId, int performanceId)
    {
        try
        {
            var filePath = await _excelImportService.GetExcelTemplateFilePath(excelImportId, performanceId);
            if (!System.IO.File.Exists(filePath))
            {
                return ErrorClass.NotFoundResponse("Excel template file not found");
            }
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var fileName = Path.GetFileName(filePath);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileBytes, contentType, fileName);
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

    [HttpPost("ImportExcel")]
    public async Task<IActionResult> ImportExcel(ExcelImportDto excelImportDto)
    {
        try
        {
            var result = await _excelImportService.ImportExcelAsync(excelImportDto);
            var response = new
            {
                Success = true,
                Message = result
            };
            await _loggingService.AuditLog("Excel Import", "POST", "/api/ExcelImport/ImportExcel", result, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (ConflictException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (FormatException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.BadResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (InvalidDataException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.UnsupportedMediaTypeResponse(ex.Message);
        }
        catch (ArgumentException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.BadResponse(ex.Message);
        }
        catch (IOException ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Excel Import", "POST", "/api/ExcelImport/ImportExcel", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, excelImportDto.CreatedBy, JsonSerializer.Serialize(excelImportDto));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}