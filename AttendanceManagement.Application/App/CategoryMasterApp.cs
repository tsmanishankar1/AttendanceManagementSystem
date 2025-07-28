using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class CategoryMasterApp : ICategoryMasterApp
{
    private readonly ICategoryMasterInfra _categoryMasterInfra;
    public CategoryMasterApp(ICategoryMasterInfra categoryMasterInfra)
    {
        _categoryMasterInfra = categoryMasterInfra;

    }

    public async Task<string> CreateCategoryAsync(CategoryMasterRequest request)
        => await _categoryMasterInfra.CreateCategoryAsync(request);

    public async Task<List<CategoryMasterResponse>> GetAllCategoriesAsync()
        => await _categoryMasterInfra.GetAllCategoriesAsync();

    public Task<string> UpdateCategoryAsync(UpdateCategory request)
        => _categoryMasterInfra.UpdateCategoryAsync(request);
}