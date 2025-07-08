using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class LetterGenerationApp : ILetterGenerationApp
    {
        private readonly ILetterGenerationInfra _letterGenerationInfra;
        public LetterGenerationApp(ILetterGenerationInfra letterGenerationInfra)
        {
            _letterGenerationInfra = letterGenerationInfra;
        }

        public string GenerateAppraisalLetterPdf(AppraisalAnnexureResponse model, string fileName)
            => _letterGenerationInfra.GenerateAppraisalLetterPdf(model, fileName);

        public string GenerateConfirmationPdf(int staffCreationId, string designation, string title, string startDate, string endDate, string fileName, string staffName, string employeeCode)
            => _letterGenerationInfra.GenerateConfirmationPdf(staffCreationId, designation, title, startDate, endDate, fileName, staffName, employeeCode);

        public string GeneratePaysheetPdf(PaysheetResponse paysheet, string fileName)
            => _letterGenerationInfra.GeneratePaysheetPdf(paysheet, fileName);

        public string GeneratePaySlip(PayslipResponse payslip, string fileName)
            => _letterGenerationInfra.GeneratePaySlip(payslip, fileName);
    }
}