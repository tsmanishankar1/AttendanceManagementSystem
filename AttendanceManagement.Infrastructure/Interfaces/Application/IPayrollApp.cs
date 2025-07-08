using AttendanceManagement.Application.Dtos.Attendance;
using Microsoft.AspNetCore.Http;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IPayrollApp
    {
        Task<string> UploadPaySlip(IFormFile file, int createdBy);
        Task<string> GeneratePaySlip(GeneratePaySheetRequest paySlipGenerate);
        Task<(Stream PdfStream, string FileName)> ViewPayslip(int staffId, int month, int year);
        Task<string> DownloadPayslip(int staffId, int month, int year);
        Task<string> GeneratePaySheet(GeneratePaySheetRequest generatePaySheetRequest);
        Task<List<PayslipResponse>> GetAllPaySlip();
        Task<string> UploadSalaryStructure(IFormFile file, int createdBy);
        Task<SalaryStructureResponse> GetSalaryStructure(int staffId);
        Task<List<SalaryStructureResponse>> GetAllSalaryStructure();
    }
}
