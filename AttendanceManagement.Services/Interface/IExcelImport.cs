using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IExcelImport
    {
        Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId);
        Task<string> ImportExcelAsync(ExcelImportDto excelImportDto);
    }
}
