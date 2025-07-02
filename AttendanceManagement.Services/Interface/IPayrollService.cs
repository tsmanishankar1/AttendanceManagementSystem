/*using AttendanceManagement.InputModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IPayrollService
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
*/