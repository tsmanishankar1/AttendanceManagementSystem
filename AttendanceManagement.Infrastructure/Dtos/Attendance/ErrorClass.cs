using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ErrorClass
    {
        public static IActionResult ErrorResponse(string message)
        {
            return new ObjectResult(new
            {
                Success = false,
                Message = message
            })
            {
                StatusCode = 500
            };
        }

        public static IActionResult NotFoundResponse(string message)
        {
            return new NotFoundObjectResult(new
            {
                Success = false,
                Message = message
            });
        }

        public static IActionResult ConflictResponse(string message)
        {
            return new ConflictObjectResult(new
            {
                Success = false,
                Message = message
            });
        }

        public static IActionResult UnsupportedMediaTypeResponse(string message)
        {
            return new ObjectResult(new
            {
                Success = false,
                Message = message
            })
            {
                StatusCode = 415
            };
        }

        public static IActionResult BadResponse(string message)
        {
            return new BadRequestObjectResult(new
            {
                Success = false,
                Message = message
            })
            {
                StatusCode = 400
            };
        }
    }
}