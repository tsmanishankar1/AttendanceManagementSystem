using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;
public class ExcelImportApp : IExcelImportApp
{
    private readonly IExcelImportInfra _excelImportInfra;
    public ExcelImportApp(IExcelImportInfra excelImportInfra)
    {
        _excelImportInfra = excelImportInfra;

    }

    public async Task<string> GetExcelTemplateFilePath(int excelImportId, int performanceId)
        => await _excelImportInfra.GetExcelTemplateFilePath(excelImportId, performanceId);

    public async Task<string> ImportExcelAsync(ExcelImportDto excelImportDto)
        => await _excelImportInfra.ImportExcelAsync(excelImportDto);
}