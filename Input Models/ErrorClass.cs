using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Input_Models
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
    }
}
