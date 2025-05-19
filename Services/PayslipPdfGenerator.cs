using AttendanceManagement.Input_Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class PayslipPdfGenerator
    {
        public static byte[] Generate(PayslipResponse payslip)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 36, 36, 54, 36);
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                var italicFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10);
                var underlineFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.UNDERLINE);

                // Title
                var title = new Paragraph($"Pay Slip - {payslip.SalaryMonth} {payslip.SalaryYear}", boldFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                document.Add(title);

                // Staff Info
                Paragraph staffNamePara = new Paragraph();
                staffNamePara.Add(new Chunk("Staff Name: ", boldFont));
                staffNamePara.Add(new Chunk(payslip.StaffName, normalFont));
                document.Add(staffNamePara);

                // Staff ID
                Paragraph staffIdPara = new Paragraph();
                staffIdPara.Add(new Chunk("Staff ID: ", boldFont));
                staffIdPara.Add(new Chunk(payslip.StaffCreationId, normalFont));
                document.Add(staffIdPara);
                document.Add(new Paragraph(" ")); // Spacer

                // Earnings
                document.Add(new Paragraph("Earnings:", underlineFont));
                document.Add(new Paragraph(" ", normalFont)); // Adds a line space
                var earningsTable = new PdfPTable(2) { WidthPercentage = 100 };
                earningsTable.AddCell(new PdfPCell(new Phrase("Component", boldFont)));
                earningsTable.AddCell(new PdfPCell(new Phrase("Amount", boldFont)));
                earningsTable.AddCell("Basic");
                earningsTable.AddCell($"₹{payslip.Basic:F2}");
                earningsTable.AddCell("HRA");
                earningsTable.AddCell($"₹{payslip.Hra.GetValueOrDefault():F2}");
                earningsTable.AddCell("DA");
                earningsTable.AddCell($"₹{payslip.Da.GetValueOrDefault():F2}");
                earningsTable.AddCell("Other Allowance");
                earningsTable.AddCell($"₹{payslip.OtherAllowance.GetValueOrDefault():F2}");
                earningsTable.AddCell("Special Allowance");
                earningsTable.AddCell($"₹{payslip.SpecialAllowance.GetValueOrDefault():F2}");
                earningsTable.AddCell("Conveyance");
                earningsTable.AddCell($"₹{payslip.Conveyance.GetValueOrDefault():F2}");
                document.Add(earningsTable);

                document.Add(new Paragraph(" ")); // Spacer

                // Deductions
                document.Add(new Paragraph("Deductions:", underlineFont));
                document.Add(new Paragraph(" ", normalFont)); // Adds a line space
                var deductionsTable = new PdfPTable(2) { WidthPercentage = 100 };
                earningsTable.AddCell(new PdfPCell(new Phrase("Component", boldFont)));
                earningsTable.AddCell(new PdfPCell(new Phrase("Amount", boldFont)));
                deductionsTable.AddCell("PF");
                deductionsTable.AddCell($"₹{payslip.Pf:F2}");
                deductionsTable.AddCell("ESIC");
                deductionsTable.AddCell($"₹{payslip.Esic.GetValueOrDefault():F2}");
                deductionsTable.AddCell("TDS");
                deductionsTable.AddCell($"₹{payslip.Tds.GetValueOrDefault():F2}");
                deductionsTable.AddCell("PT");
                deductionsTable.AddCell($"₹{payslip.Pt.GetValueOrDefault():F2}");
                document.Add(deductionsTable);

                document.Add(new Paragraph(" ")); // Spacer

                // Other Info
                document.Add(new Paragraph("Other Details:", underlineFont));
                document.Add(new Paragraph(" ", normalFont)); // Adds a line space
                var infoTable = new PdfPTable(2) { WidthPercentage = 100 };
                infoTable.AddCell("OT Hours");
                infoTable.AddCell($"{payslip.Othours.GetValueOrDefault()}");
                infoTable.AddCell("OT/Hour");
                infoTable.AddCell($"₹{payslip.OtperHour.GetValueOrDefault():F2}");
                infoTable.AddCell("Total OT");
                infoTable.AddCell($"₹{payslip.TotalOt.GetValueOrDefault():F2}");
                infoTable.AddCell("Absent Days");
                infoTable.AddCell($"{payslip.AbsentDays.GetValueOrDefault()}");
                infoTable.AddCell("LOP/Day");
                infoTable.AddCell($"₹{payslip.LopperDay.GetValueOrDefault():F2}");
                infoTable.AddCell("Total LOP");
                infoTable.AddCell($"₹{payslip.TotalLop.GetValueOrDefault():F2}");
                infoTable.AddCell("LWOP Days");
                infoTable.AddCell($"{payslip.Lwopdays.GetValueOrDefault()}");
                document.Add(infoTable);

                document.Add(new Paragraph(" ")); // Spacer

                // Payslip Components
                if (payslip.PaySlipComponents?.Any() == true)
                {
                    document.Add(new Paragraph("Payslip Components:", underlineFont));
                    document.Add(new Paragraph(" ", normalFont)); // Adds a line space
                    var compTable = new PdfPTable(4) { WidthPercentage = 100 };
                    compTable.AddCell(new PdfPCell(new Phrase("Type", boldFont)));
                    compTable.AddCell(new PdfPCell(new Phrase("Name", boldFont)));
                    compTable.AddCell(new PdfPCell(new Phrase("Amount", boldFont)));
                    compTable.AddCell(new PdfPCell(new Phrase("Remarks", boldFont)));

                    foreach (var component in payslip.PaySlipComponents)
                    {
                        compTable.AddCell(component.ComponentType ?? "-");
                        compTable.AddCell(component.ComponentName ?? "-");
                        compTable.AddCell($"₹{component.Amount:F2}");
                        compTable.AddCell(component.Remarks ?? "-");
                    }

                    document.Add(compTable);
                }

                // Footer
                var footer = new Paragraph($"\nGenerated on: {System.DateTime.Now:dd-MMM-yyyy HH:mm}", italicFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                };
                document.Add(footer);

                document.Close();
                return memoryStream.ToArray();
            }
        }

        public static byte[] GeneratePaysheetPdf(PaysheetResponse paysheet)
        {
            using var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 36, 36, 54, 36);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var italicFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 10);
            var underlineFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.UNDERLINE);

            // Title
            var title = new Paragraph("Paysheet", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 8f
            };
            document.Add(title);

            // Add underline under title
            var titleLine = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -2);
            document.Add(new Chunk(titleLine));

            document.Add(new Paragraph(" ")); // spacer after underline

            // Personal Details Table
            document.Add(new Paragraph("Personal Details:", underlineFont) { SpacingAfter = 8f });
            var personalTable = new PdfPTable(2) { WidthPercentage = 100 };
            personalTable.SetWidths(new float[] { 60, 40 });
            void AddPersonalCell(string label, string amount)
            {
                personalTable.AddCell(new PdfPCell(new Phrase(label, labelFont)) { PaddingBottom = 4 });
                personalTable.AddCell(new PdfPCell(new Phrase(amount, valueFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 4 });
            }

            AddPersonalCell("Staff Id", paysheet.StaffId);
            AddPersonalCell("Employee Name", paysheet.EmployeeName);
            AddPersonalCell("Group Name", paysheet.GroupName);
            AddPersonalCell("Display Name In Reports", paysheet.DisplayNameInReports);
            AddPersonalCell("Month", paysheet.Month);
            AddPersonalCell("Year", paysheet.Year.ToString());
            AddPersonalCell("Date Of Joining", paysheet.DateOfJoining.ToString("dd-MM-yyyy"));
            AddPersonalCell("Employee Number", paysheet.EmployeeNumber.ToString());
            AddPersonalCell("Designation", paysheet.Designation);
            AddPersonalCell("Department", paysheet.Department);
            AddPersonalCell("Location", paysheet.Location);
            AddPersonalCell("Gender", paysheet.Gender);
            AddPersonalCell("Date Of Birth", paysheet.DateOfBirth.ToString("dd-MM-yyyy"));
            AddPersonalCell("Father Or Mother Name", paysheet.FatherOrMotherName);
            AddPersonalCell("Spouse Name", paysheet.SpouseName ?? "-");
            AddPersonalCell("Address", paysheet.Address);
            AddPersonalCell("Email", paysheet.Email);
            AddPersonalCell("Phone No", paysheet.PhoneNo.ToString());
            AddPersonalCell("Bank Name", paysheet.BankName);
            AddPersonalCell("Account No", paysheet.AccountNo.ToString());
            AddPersonalCell("IFSC Code", paysheet.IfscCode);
            AddPersonalCell("PF Account No", paysheet.PfAccountNo);
            AddPersonalCell("UAN", paysheet.Uan.ToString());
            AddPersonalCell("PAN", paysheet.Pan);
            AddPersonalCell("Aadhaar No", paysheet.AadhaarNo.ToString());
            AddPersonalCell("ESI No", paysheet.EsiNo.ToString());
            AddPersonalCell("Salary Effective From", paysheet.SalaryEffectiveFrom.ToString("dd-MM-yyyy"));

            document.Add(personalTable);

            document.Add(new Paragraph(" ")); // spacer before next section


            // Earnings Section
            document.Add(new Paragraph("Earnings:", underlineFont) { SpacingAfter = 8f });

            var earningsTable = new PdfPTable(2) { WidthPercentage = 100 };
            earningsTable.SetWidths(new float[] { 60, 40 });

            void AddEarningCell(string label, string amount)
            {
                earningsTable.AddCell(new PdfPCell(new Phrase(label, labelFont)) { PaddingBottom = 4 });
                earningsTable.AddCell(new PdfPCell(new Phrase(amount, valueFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 4 });
            }

            AddEarningCell("Basic Actual", $"₹{paysheet.BasicActual:F2}");
            AddEarningCell("HRA Actual", $"₹{paysheet.HraActual:F2}");
            AddEarningCell("Conve Actual", $"₹{paysheet.ConveActual:F2}");
            AddEarningCell("Med Allow Actual", $"₹{paysheet.MedAllowActual:F2}");
            AddEarningCell("Spl Allow Actual", $"₹{paysheet.SplAllowActual:F2}");

            AddEarningCell("Basic Earned", $"₹{paysheet.BasicEarned:F2}");
            AddEarningCell("Basic Arradj", $"₹{paysheet.BasicArradj:F2}");
            AddEarningCell("HRA Earned", $"₹{paysheet.HraEarned:F2}");
            AddEarningCell("HRA Arradj", $"₹{paysheet.HraArradj:F2}");
            AddEarningCell("Conve Earned", $"₹{paysheet.ConveEarned:F2}");
            AddEarningCell("Conve Arradj", $"₹{paysheet.ConveArradj:F2}");
            AddEarningCell("Med Allow Earned", $"₹{paysheet.MedAllowEarned:F2}");
            AddEarningCell("Med Allow Arradj", $"₹{paysheet.MedAllowArradj:F2}");
            AddEarningCell("Spl Allow Earned", $"₹{paysheet.SplAllowEarned:F2}");
            AddEarningCell("Spl Allow Arradj", $"₹{paysheet.SplAllowArradj:F2}");

            AddEarningCell("Other All", $"₹{paysheet.OtherAll:F2}");
            AddEarningCell("Gross Earn", $"₹{paysheet.GrossEarn:F2}");

            document.Add(earningsTable);

            document.Add(new Paragraph(" ")); // Spacer

            // Deductions Section
            document.Add(new Paragraph("Deductions:", underlineFont) { SpacingAfter = 8f });

            var deductionsTable = new PdfPTable(2) { WidthPercentage = 100 };
            deductionsTable.SetWidths(new float[] { 60, 40 });

            void AddDeductionCell(string label, string amount)
            {
                deductionsTable.AddCell(new PdfPCell(new Phrase(label, labelFont)) { PaddingBottom = 4 });
                deductionsTable.AddCell(new PdfPCell(new Phrase(amount, valueFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 4 });
            }

            AddDeductionCell("PF", $"₹{paysheet.Pf:F2}");
            AddDeductionCell("ESI", $"₹{paysheet.Esi:F2}");
            AddDeductionCell("LWF", $"₹{paysheet.Lwf:F2}");
            AddDeductionCell("PT", $"₹{paysheet.Pt:F2}");
            AddDeductionCell("IT", $"₹{paysheet.It:F2}");
            AddDeductionCell("Med Claim", $"₹{paysheet.MedClaim:F2}");
            AddDeductionCell("Other Ded", $"₹{paysheet.OtherDed:F2}");
            AddDeductionCell("Gross Ded", $"₹{paysheet.GrossDed:F2}");

            document.Add(deductionsTable);

            document.Add(new Paragraph(" ")); // Spacer

            // Other Details Section
            document.Add(new Paragraph("Other Details:", underlineFont) { SpacingAfter = 8f });

            var otherDetailsTable = new PdfPTable(2) { WidthPercentage = 100 };
            otherDetailsTable.SetWidths(new float[] { 60, 40 });

            void AddOtherDetailCell(string label, string value)
            {
                otherDetailsTable.AddCell(new PdfPCell(new Phrase(label, labelFont)) { PaddingBottom = 4 });
                otherDetailsTable.AddCell(new PdfPCell(new Phrase(value, valueFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 4 });
            }

            AddOtherDetailCell("LOP Days", paysheet.LopDays.ToString("0.##"));
            AddOtherDetailCell("Std Days", paysheet.StdDays.ToString("0.##"));
            AddOtherDetailCell("Wrk Days", paysheet.WrkDays.ToString("0.##"));

            AddOtherDetailCell("PF Admin", paysheet.PfAdmin);

            AddOtherDetailCell("Net Pay", $"₹{paysheet.NetPay:F2}");

            document.Add(otherDetailsTable);

            // Footer
            var footer = new Paragraph($"\nGenerated on: {System.DateTime.Now:dd-MMM-yyyy HH:mm}", italicFont)
            {
                Alignment = Element.ALIGN_RIGHT
            };
            document.Add(footer);

            document.Close();

            return stream.ToArray();
        }
    }
}