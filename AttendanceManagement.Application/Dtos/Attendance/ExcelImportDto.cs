using Microsoft.AspNetCore.Http;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ExcelImportDto
    {
        public int ExcelImportId { get; set; }
        public int? Year { get; set; }
        public int? Month {  get; set; }
        public string? QuarterType { get; set; }
        public int? PerformanceTypeId { get; set; }
        public int CreatedBy { get; set; }
        public IFormFile File { get; set; } = null!;
    }

    public class ExcelImportResultDto
    {
        public int TotalRecords { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
        public string? SuccessFilePath { get; set; }
        public string? ErrorFilePath { get; set; }
        public string? SuccessFileName { get; set; }
        public string? ErrorFileName { get; set; }
    }
}