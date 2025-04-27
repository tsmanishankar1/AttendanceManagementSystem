using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryMasterController : ControllerBase
{
    private readonly CategoryMasterService _service;
    private readonly LoggingService _loggingService;

    public CategoryMasterController(CategoryMasterService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _service.GetAllCategoriesAsync();
            var response = new
            {
                Success = true,
                Message = categories
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

    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory(CategoryMasterRequest category)
    {
        try
        {
            var createdCategory = await _service.CreateCategoryAsync(category);
            var response = new
            {
                Success = true,
                Message = createdCategory
            };
            await _loggingService.AuditLog("Category Master", "POST", "/api/CategoryMaster/CreateCategory", createdCategory, category.CreatedBy, JsonSerializer.Serialize(category));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Category Master", "POST", "/api/CategoryMaster/CreateCategory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, category.CreatedBy, JsonSerializer.Serialize(category));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateCategory")]
    public async Task<IActionResult> UpdateCategory(UpdateCategory updatedCategory)
    {
        try
        {
            var success = await _service.UpdateCategoryAsync(updatedCategory);
            var response = new
            {
                Success = true,
                Message = success
            };
            await _loggingService.AuditLog("Category Master", "POST", "/api/CategoryMaster/UpdateCategory", success, updatedCategory.UpdatedBy, JsonSerializer.Serialize(updatedCategory));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Category Master", "POST", "/api/CategoryMaster/UpdateCategory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedCategory.UpdatedBy, JsonSerializer.Serialize(updatedCategory));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Category Master", "POST", "/api/CategoryMaster/UpdateCategory", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedCategory.UpdatedBy, JsonSerializer.Serialize(updatedCategory));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}