using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _payrollService;
        private readonly LoggingService _loggingService;
        private readonly AttendanceManagementSystemContext _context;
        public PayrollController(PayrollService payrollService, LoggingService loggingService, AttendanceManagementSystemContext context)
        {
            _payrollService = payrollService;
            _loggingService = loggingService;
            _context = context;
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
                await _loggingService.AuditLog("Payslip", "POST", "/api/Payroll/UploadPaySlip", result, createdBy, JsonSerializer.Serialize(new { file, createdBy}));
                return Ok(response);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Payslip", "POST", "/api/Payroll/UploadPaySlip", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, createdBy, JsonSerializer.Serialize(new { file, createdBy }));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetPaySlip")]
        public async Task<IActionResult> GetPaySlip(int staffId, int month, int year)
        {
            try
            {
                var payslip = await _payrollService.GetPaySlip(staffId, month, year);
                var pdfBytes = PayslipPdfGenerator.Generate(payslip);
                string fileName = $"Payslip_{payslip.StaffCreationId}_{payslip.SalaryMonth}_{payslip.SalaryYear}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
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
                var pdfBytes = PayslipPdfGenerator.GeneratePaysheetPdf(paysheet);
                string fileName = $"Paysheet_{paysheet.StaffId}_{paysheet.Month}_{paysheet.Year}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
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