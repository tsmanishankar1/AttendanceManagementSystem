using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface ILetterGeneration
    {
        string GenerateConfirmationPdf(int staffCreationId, string designation, string title, string startDate, string endDate, string fileName, string staffName, string employeeCode);
        string GeneratePaySlip(PayslipResponse payslip, string fileName);
        string GeneratePaysheetPdf(PaysheetResponse paysheet, string fileName);
        string GenerateAppraisalLetterPdf(AppraisalAnnexureResponse model, string fileName);
    }
}
