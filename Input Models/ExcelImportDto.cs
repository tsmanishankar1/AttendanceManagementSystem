namespace AttendanceManagement.Input_Models
{
    public class ExcelImportDto
    {
        public int ExcelImportId { get; set; }
        public int CreatedBy { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
