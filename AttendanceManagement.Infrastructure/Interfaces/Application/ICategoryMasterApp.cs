using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface ICategoryMasterApp
    {
        Task<List<CategoryMasterResponse>> GetAllCategoriesAsync();
        Task<string> CreateCategoryAsync(CategoryMasterRequest request);
        Task<string> UpdateCategoryAsync(UpdateCategory request);
    }
}
