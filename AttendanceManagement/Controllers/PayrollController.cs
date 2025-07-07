using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IPayrollInfra _payrollService;
        private readonly ILoggingInfra _loggingService;
        public PayrollController(IPayrollInfra payrollService, ILoggingInfra loggingService)
        {
            _payrollService = payrollService;
            _loggingService = loggingService;
        }

        [HttpPost("UploadPaySlip")]
        public async Task<IActionResult> UploadPaySlip(IFormFile file, int createdBy)
        {
            try
            {
                var result = await _payrollService.UploadPaySlip(file, createdBy);
                var response = new
                {
                    Success = true,
                    Message = result
                };
                await _loggingService.AuditLog("Payslip", "POST", "/api/Payroll/UploadPaySlip", result, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.UnsupportedMediaTypeResponse(ex.Message);
            }
            catch (IOException ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.ConflictResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("GeneratePaySlip")]
        public async Task<IActionResult> GeneratePaySlip(GeneratePaySheetRequest paySlipGenerate)
        {
            try
            {
                var payslip = await _payrollService.GeneratePaySlip(paySlipGenerate);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(payslip);
                var fileName = Path.GetFileName(payslip);
                var contentType = "application/pdf";
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

        [HttpPost("GeneratePaySheet")]
        public async Task<IActionResult> GeneratePaySheet(GeneratePaySheetRequest generatePaySheetRequest)
        {
            try
            {
                var paysheet = await _payrollService.GeneratePaySheet(generatePaySheetRequest);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(paysheet);
                var fileName = Path.GetFileName(paysheet);
                var contentType = "application/pdf";
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

        [HttpGet("ViewPayslip")]
        public async Task<IActionResult> ViewPayslip(int staffId, int month, int year)
        {
            try
            {
                var (stream, fileName) = await _payrollService.ViewPayslip(staffId, month, year);
                Response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileName}\"");
                return File(stream, "application/pdf");
            }
            catch (FileNotFoundException ex)
            {
                return ErrorClass.NotFoundResponse(ex.Message);
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

        [HttpGet("DownloadPayslip")]
        public async Task<IActionResult> DownloadPayslip(int staffId, int month, int year)
        {
            try
            {
                var filePath = await _payrollService.DownloadPayslip(staffId, month, year);
                if (!System.IO.File.Exists(filePath))
                {
                    return ErrorClass.NotFoundResponse("Payslip not found");
                }
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var file = Path.GetFileName(filePath);
                var contentType = "application/pdf";

                return File(fileBytes, contentType, file);
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

        [HttpGet("GetAllPaySlip")]
        public async Task<IActionResult> GetAllPaySlip()
        {
            try
            {
                var result = await _payrollService.GetAllPaySlip();
                var response = new
                {
                    Success = true,
                    Message = result
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

        [HttpPost("UploadSalaryStructure")]
        public async Task<IActionResult> UploadSalaryStructure(IFormFile file, int createdBy)
        {
            try
            {
                var result = await _payrollService.UploadSalaryStructure(file, createdBy);
                var response = new
                {
                    Success = true,
                    Message = result
                };
                await _loggingService.AuditLog("Salary Structure", "POST", "/api/Payroll/UploadSalaryStructure", result, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Salary Structure", "POST", "/api/Payroll/UploadSalaryStructure", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Salary Structure", "POST", "/api/Payroll/UploadSalaryStructure", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetSalaryStructure")]
        public async Task<IActionResult> GetSalaryStructure(int staffId)
        {
            try
            {
                var result = await _payrollService.GetSalaryStructure(staffId);
                var response = new
                {
                    Success = true,
                    Message = result
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

        [HttpGet("GetAllSalaryStructure")]
        public async Task<IActionResult> GetAllSalaryStructure()
        {
            try
            {
                var result = await _payrollService.GetAllSalaryStructure();
                var response = new
                {
                    Success = true,
                    Message = result
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
    }
}