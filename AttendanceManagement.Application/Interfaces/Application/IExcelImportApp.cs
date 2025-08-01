using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IExcelImportApp
    {
        Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId);
        Task<ExcelImportResultDto> ImportExcelAsync(ExcelImportDto excelImportDto);
    }
}
