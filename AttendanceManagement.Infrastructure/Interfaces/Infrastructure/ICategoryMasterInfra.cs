using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ICategoryMasterInfra
    {
        Task<List<CategoryMasterResponse>> GetAllCategoriesAsync();
        Task<string> CreateCategoryAsync(CategoryMasterRequest request);
        Task<string> UpdateCategoryAsync(UpdateCategory request);
    }
}
