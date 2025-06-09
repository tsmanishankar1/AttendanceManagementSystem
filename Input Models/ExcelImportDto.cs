namespace AttendanceManagement.Input_Models
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
}