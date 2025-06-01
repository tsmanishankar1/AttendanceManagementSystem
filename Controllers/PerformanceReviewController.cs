using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceReviewController : ControllerBase
    {
        private readonly PerformanceReviewService _service;
        private readonly LoggingService _loggingService;
        public PerformanceReviewController(PerformanceReviewService service, LoggingService loggingService)
        {
            _service = service;
            _loggingService = loggingService;
        }

        /*        [HttpGet("GetPerformanceReviewCycle")]
                public async Task<IActionResult> GetPerformanceReviewCycle()
                {
                    try
                    {
                        var performance = await _service.GetPerformanceReviewCycle();
                        var response = new
                        {
                            Success = true,
                            Message = performance
                        };
                        return Ok(response);
                    }
                    catch(MessageNotFoundException ex)
                    {
                        return ErrorClass.NotFoundResponse(ex.Message);
                    }
                    catch(Exception ex)
                    {
                        return ErrorClass.ErrorResponse(ex.Message);
                    }
                }

                [HttpPost("CreatetPerformanceReviewCycle")]
                public async Task<IActionResult> CreatetPerformanceReviewCycle(PerformanceReviewRequest performanceReviewRequest)
                {
                    try
                    {
                        var performance = await _service.CreatetPerformanceReviewCycle(performanceReviewRequest);
                        var response = new
                        {
                            Success = true,
                            Message = performance
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        return ErrorClass.ErrorResponse(ex.Message);
                    }
                }

                [HttpGet("UpdatePerformanceReviewCycle")]
                public async Task<IActionResult> UpdatePerformanceReviewCycle(UpdatePerformanceReview updatePerformanceReview)
                {
                    try
                    {
                        var performance = await _service.UpdatePerformanceReviewCycle(updatePerformanceReview);
                        var response = new
                        {
                            Success = true,
                            Message = performance
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

                [HttpGet("GetEligibleEmployees")]
                public async Task<IActionResult> GetEligibleEmployees()
                {
                    try
                    {
                        var performance = await _service.GetEligibleEmployees();
                        var response = new
                        {
                            Success = true,
                            Message = performance
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
        */

        [HttpPost("GenerateAppraisalLetter")]
        public async Task<IActionResult> GenerateAppraisalLetter(GenerateAppraisalLetterRequest generateAppraisalLetterRequest)
        {
            try
            {
                var appraisal = await _service.GenerateAppraisalLetter(generateAppraisalLetterRequest);
                var fileBytes = await System.IO.File.ReadAllBytesAsync(appraisal);
                var fileName = Path.GetFileName(appraisal);
                var contentType = "application/pdf";
                await _loggingService.AuditLog("Generate Appraisal Letter", "POST", "/api/PerformanceReview/GenerateAppraisalLetter", "Appraisal letter generated successfully", generateAppraisalLetterRequest.CreatedBy, JsonSerializer.Serialize(generateAppraisalLetterRequest));
                return File(fileBytes, contentType, fileName);
            }
            catch (MessageNotFoundException ex)
            {
                await _loggingService.LogError("Generate Appraisal Letter", "POST", "/api/PerformanceReview/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, generateAppraisalLetterRequest.CreatedBy, JsonSerializer.Serialize(generateAppraisalLetterRequest));
                return ErrorClass.NotFoundResponse(ex.Message);
            }
            catch (Exception ex)
            {
                await _loggingService.LogError("Generate Appraisal Letter", "POST", "/api/PerformanceReview/GenerateAppraisalLetter", ex.Message, ex.StackTrace?.ToString() ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, generateAppraisalLetterRequest.CreatedBy, JsonSerializer.Serialize(generateAppraisalLetterRequest));
                return ErrorClass.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("DownloadAppraisalLetter")]
        public async Task<IActionResult> DownloadAppraisalLetter(int staffId, int fileId)
        {
            try
            {
                var filePath = await _service.DownloadAppraisalLetter(staffId, fileId);
                if (!System.IO.File.Exists(filePath))
                {
                    return ErrorClass.NotFoundResponse("Appraisal letter not found");
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

        [HttpGet("ViewAppraisalLetter")]
        public async Task<IActionResult> ViewAppraisalLetter(int staffId, int fileId)
        {
            try
            {
                var (stream, fileName) = await _service.ViewAppraisalLetter(staffId, fileId);
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
    }
}
