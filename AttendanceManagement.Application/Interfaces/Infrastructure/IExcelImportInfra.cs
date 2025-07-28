using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IExcelImportInfra
    {
        Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId);
        Task<string> ImportExcelAsync(ExcelImportDto excelImportDto);
    }
}
