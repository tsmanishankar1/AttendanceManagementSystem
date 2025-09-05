using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface IExcelImportInfra
    {
        Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId);
        Task<ExcelImportResultDto> ImportExcelAsync(ExcelImportDto excelImportDto);
        Task<(byte[] FileBytes, string FileName)> GetLatestSuccessFileAsync();
        Task<(byte[] FileBytes, string FileName)> GetLatestErrorFileAsync();
    }
}
