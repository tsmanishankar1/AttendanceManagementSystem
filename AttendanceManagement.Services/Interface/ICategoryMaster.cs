using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ICategoryMaster
    {
        Task<List<CategoryMasterResponse>> GetAllCategoriesAsync();
        Task<string> CreateCategoryAsync(CategoryMasterRequest request);
        Task<string> UpdateCategoryAsync(UpdateCategory request);
    }
}
