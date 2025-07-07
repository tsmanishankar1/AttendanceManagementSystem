using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Infrastructure
{
    public interface ILetterGenerationInfra
    {
        string GenerateConfirmationPdf(int staffCreationId, string designation, string title, string startDate, string endDate, string fileName, string staffName, string employeeCode);
        string GeneratePaySlip(PayslipResponse payslip, string fileName);
        string GeneratePaysheetPdf(PaysheetResponse paysheet, string fileName);
        string GenerateAppraisalLetterPdf(AppraisalAnnexureResponse model, string fileName);
    }
}
