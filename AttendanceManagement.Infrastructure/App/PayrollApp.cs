using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace AttendanceManagement.Application.App
{
    public class PayrollApp : IPayrollApp
    {
        private readonly IPayrollInfra _payrollInfra;

        public PayrollApp(IPayrollInfra payrollInfra)
        {
            _payrollInfra = payrollInfra;
        }

        public async Task<string> DownloadPayslip(int staffId, int month, int year)
            => await _payrollInfra.DownloadPayslip(staffId, month, year);

        public async Task<string> GeneratePaySheet(GeneratePaySheetRequest generatePaySheetRequest)
            => await _payrollInfra.GeneratePaySheet(generatePaySheetRequest);

        public async Task<string> GeneratePaySlip(GeneratePaySheetRequest paySlipGenerate)
            => await _payrollInfra.GeneratePaySlip(paySlipGenerate);

        public async Task<List<PayslipResponse>> GetAllPaySlip()
            => await _payrollInfra.GetAllPaySlip();

        public async Task<List<SalaryStructureResponse>> GetAllSalaryStructure()
            => await _payrollInfra.GetAllSalaryStructure();

        public async Task<SalaryStructureResponse> GetSalaryStructure(int staffId)
            => await _payrollInfra.GetSalaryStructure(staffId);

        public async Task<string> UploadPaySlip(IFormFile file, int createdBy)
            => await _payrollInfra.UploadPaySlip(file, createdBy);

        public async Task<string> UploadSalaryStructure(IFormFile file, int createdBy)
            => await _payrollInfra.UploadSalaryStructure(file, createdBy);

        public async Task<(Stream PdfStream, string FileName)> ViewPayslip(int staffId, int month, int year)
            => await _payrollInfra.ViewPayslip(staffId, month, year);
    }
}