using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceReviewController : ControllerBase
    {
        private readonly IPerformanceReviewApp _service;
        public PerformanceReviewController(IPerformanceReviewApp service)
        {
            _service = service;
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

        [HttpGet("GetMonthlyPerformance")]
        public async Task<IActionResult> GetMonthlyPerformance(int year, int month)
        {
            try
            {
                var performance = await _service.GetMonthlyPerformance(year, month);
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

        [HttpGet("GetQuarterlyPerformance")]
        public async Task<IActionResult> GetQuarterlyPerformance(int year, string quarterType)
        {
            try
            {
                var performance = await _service.GetQuarterlyPerformance(year, quarterType);
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

        [HttpGet("GetYearlyPerformance")]
        public async Task<IActionResult> GetYearlyPerformance(int year)
        {
            try
            {
                var performance = await _service.GetYearlyPerformance(year);
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
    }
}
