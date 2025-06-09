using AttendanceManagement.Input_Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using System.Linq;

namespace AttendanceManagement.Services
{
    public class LetterGenerationService
    {
        private readonly string _workspacePath;
        public LetterGenerationService(IWebHostEnvironment env)
        {
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\GeneratedLetters");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
        }

        private string GetLetterDirectory(string letterType)
        {
            var folderPath = Path.Combine(_workspacePath, letterType);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public string GenerateConfirmationPdf(int staffCreationId, string designation, string title, string startDate, string endDate, string fileName, string staffName, string employeeCode)
        {
            var letterDirectory = GetLetterDirectory("ConfirmationLetter");
            var filePath = Path.Combine(letterDirectory, fileName);

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var pdfDoc = new Document(PageSize.A4, 50, 50, 80, 50))
            {
                PdfWriter.GetInstance(pdfDoc, fs);
                pdfDoc.Open();

                // Fonts
                var underlineFont = new Font(Font.FontFamily.HELVETICA, 14, Font.UNDERLINE | Font.BOLD);
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var redFont = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.RED);

                // Date
                var dateLine = new Paragraph();
                dateLine.Add(new Chunk("Date: ", boldFont));
                dateLine.Add(new Chunk(DateTime.UtcNow.ToString("dd MMMM yyyy"), bodyFont));
                pdfDoc.Add(dateLine);
                pdfDoc.Add(new Paragraph(" "));

                // Name
                var nameLine = new Paragraph();
                nameLine.Add(new Chunk("Name: ", boldFont));
                nameLine.Add(new Chunk(staffName, bodyFont));
                pdfDoc.Add(nameLine);

                // Employee Code
                var codeLine = new Paragraph();
                codeLine.Add(new Chunk("Employee Code: ", boldFont));
                codeLine.Add(new Chunk(employeeCode, bodyFont));
                pdfDoc.Add(codeLine);

                // Designation
                var desigLine = new Paragraph();
                desigLine.Add(new Chunk("Designation: ", boldFont));
                desigLine.Add(new Chunk(designation, bodyFont));
                pdfDoc.Add(desigLine);
                pdfDoc.Add(new Paragraph(" "));

                // Salutation (Dear ...)
                var dearLine = new Paragraph();
                dearLine.Add(new Chunk("Dear ", boldFont));
                dearLine.Add(new Chunk($"{title} {staffName},", bodyFont));
                pdfDoc.Add(dearLine);
                pdfDoc.Add(new Paragraph(" "));

                // Subject - centered and underlined
                var subjectParagraph = new Paragraph("Sub: Service Confirmation", underlineFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                pdfDoc.Add(subjectParagraph);
                pdfDoc.Add(new Paragraph(" "));

                // Paragraph 1 with red VLead
                var para1 = new Paragraph();
                para1.Add(new Chunk("V", redFont));
                para1.Add(new Chunk($"Lead Design Services appreciates your continuous participation and involvement in the organizational growth. Based on the review of your performance for the period from {startDate} to {endDate}, we are pleased to inform and confirm your services with effect from ", bodyFont));
                para1.Add(new Chunk(DateTime.UtcNow.ToString("dd MMMM yyyy"), bodyFont));
                para1.Add(new Chunk(" in the organization.", bodyFont));
                pdfDoc.Add(para1);
                pdfDoc.Add(new Paragraph(" "));

                // Paragraph 2
                pdfDoc.Add(new Paragraph("All other terms and conditions as per your appointment order will remain unchanged.", bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                // Paragraph 3
                pdfDoc.Add(new Paragraph("Please sign and return the enclosed copy of this letter as a token of acknowledgement.", bodyFont));
                pdfDoc.Add(new Paragraph(" "));

                // Paragraph 4 with red VLead
                var para4 = new Paragraph();
                para4.Add(new Chunk("We wish you all the best in your assignments with ", bodyFont));
                para4.Add(new Chunk("V", redFont));
                para4.Add(new Chunk("Lead Design Services.", bodyFont));
                pdfDoc.Add(para4);
                pdfDoc.Add(new Paragraph(" "));

                // Signature block in one line with red V
                var signature = new Paragraph();
                signature.Add(new Chunk("For ", bodyFont));
                signature.Add(new Chunk("V", redFont));
                signature.Add(new Chunk("Lead Design Services Private Limited", bodyFont));
                pdfDoc.Add(signature);

                pdfDoc.Add(new Paragraph(" "));
                pdfDoc.Add(new Paragraph("Nirmala Thamarai", bodyFont));
                pdfDoc.Add(new Paragraph("Manager - HR", bodyFont));

                pdfDoc.Close();
            }

            return filePath;
        }

        public string GeneratePaySlip(PayslipResponse payslip, string fileName)
        {
            var letterDirectory = GetLetterDirectory("PaySlip");
            var filePath = Path.Combine(letterDirectory, fileName);

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
                File.WriteAllBytes(filePath, memoryStream.ToArray());
                return filePath;
            }
        }

        public string GeneratePaysheetPdf(PaysheetResponse paysheet, string fileName)
        {
            var letterDirectory = GetLetterDirectory("PaySheet");
            var filePath = Path.Combine(letterDirectory, fileName);
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
            File.WriteAllBytes(filePath, stream.ToArray());
            return filePath;
        }

        public string GenerateAppraisalLetterPdf(AppraisalAnnexureResponse model, string fileName)
        {
            using var stream = new MemoryStream();

            // Increase A4 width by 200 points (approx. 2.78 inches)
            float customWidth = PageSize.A4.Width;
            float customHeight = PageSize.A4.Height + 100f;
            var letterDirectory = GetLetterDirectory("AppraisalLetter");
            string filePath = "";
            if (model.IsDesignationChange)
            {
                /*                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                */
                filePath = Path.Combine(letterDirectory, fileName);
            }
            else
            {
                /*                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }
                */
                filePath = Path.Combine(letterDirectory, fileName);
            }
            // Create a custom rectangle
            Rectangle customPageSize = new Rectangle(customWidth, customHeight);

            // Apply custom page size to the document
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var doc = new Document(customPageSize, 80f, 80f, 54f, 36f))
            {
                var writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Define fonts
                var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                var fontSmallBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

                BaseFont unicodeBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font unicodeFont = new Font(unicodeBaseFont, 10, Font.NORMAL, BaseColor.BLACK);
                Font unicodeBoldFont = new Font(unicodeBaseFont, 10, Font.BOLD, BaseColor.BLACK);

                // Add title
                var title = new Paragraph("Appraisal Letter", fontTitle)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 20f
                };
                doc.Add(title);

                // Date line (left aligned)
                var datePara = new Paragraph($"{DateTime.Now:dd MMMM, yyyy}", fontBold)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingAfter = 5f
                };
                doc.Add(datePara);

                // Employee info (left aligned)
                var namePara = new Paragraph();
                namePara.Add(new Chunk("Name: ", fontBold));
                namePara.Add(new Chunk(model.EmployeeName, fontNormal));
                doc.Add(namePara);

                var codePara = new Paragraph();
                codePara.Add(new Chunk("Employee Code: ", fontBold));
                codePara.Add(new Chunk(model.EmployeeCode, fontNormal));
                doc.Add(codePara);

                var designationPara = new Paragraph();
                designationPara.Add(new Chunk("Designation: ", fontBold));
                designationPara.Add(new Chunk(model.Designation, fontNormal));
                designationPara.SpacingAfter = 15f;
                doc.Add(designationPara);

                // Salutation
                var salutation = new Paragraph($"Dear {model.Title} {model.EmployeeName},", fontNormal)
                {
                    SpacingAfter = 20f
                };
                doc.Add(salutation);

                // Main body paragraphs
                // First paragraph
                var bodyPara1 = new Paragraph();
                bodyPara1.SpacingAfter = 10f;
                bodyPara1.Add(new Chunk("We would like to congratulate you on your performance over the last one year.", fontNormal));
                doc.Add(bodyPara1);

                // Second paragraph (with increment and salary details)
                var bodyPara2 = new Paragraph();
                bodyPara2.SpacingAfter = 10f;
                if (model.IsDesignationChange)
                {
                    bodyPara2.Add(new Chunk("In recognition of your performance and contribution to the organization, we are glad to inform you that, you have been promoted as ", fontNormal));
                    bodyPara2.Add(new Chunk(model.Designation, fontBold));
                    bodyPara2.Add(new Chunk(" with an increment of ", fontNormal));
                }
                else
                {
                    bodyPara2.Add(new Chunk("In recognition of your performance and contribution to the organization, we are glad to announce an increment of ", fontNormal));
                }
                bodyPara2.Add(new Chunk($"Rs.{model.SalaryAfterAppraisal.AppraisalAmount:N0}", fontBold));
                bodyPara2.Add(new Chunk($"/- PA ({ConvertAmountToWords(model.SalaryAfterAppraisal.AppraisalAmount)} Rupees only) on your existing gross salary with effect from June 1st, {model.SalaryAfterAppraisal.AppraisalYear}. Detailed revised salary structure is enclosed in the Annexure A.", fontNormal));
                doc.Add(bodyPara2);

                // Third paragraph (next appraisal info)
                var bodyPara3 = new Paragraph();
                bodyPara3.SpacingAfter = 10f;
                bodyPara3.Add(new Chunk($"Next appraisals would be held in July {model.SalaryAfterAppraisal.AppraisalYear + 1} if the company does well commercially. Parameters for the next appraisal shall be on the following basis:", fontNormal));
                doc.Add(bodyPara3);

                // Bullet points for parameters
                var bulletPara1 = new Paragraph();
                bulletPara1.SpacingAfter = 10f;
                bulletPara1.Add(new Chunk("        a) 40 points for Attendance\n        b) 40 points for Production\n        c) 20 points for Quality", fontNormal));
                doc.Add(bulletPara1);

                // Additional evaluation criteria
                var evalPara = new Paragraph();
                evalPara.SpacingAfter = 10f;
                evalPara.Add(new Chunk("In addition to the above, you will also be evaluated on the following:", fontNormal));
                doc.Add(bulletPara1);

                // Additional evaluation criteria
                var evalPara1 = new Paragraph();
                evalPara1.SpacingAfter = 10f;
                evalPara1.Add(new Chunk("        a) Your contribution on the team's target\n        b) Cross Training\n        c) Bench Contribution", fontNormal));
                doc.Add(evalPara1);

                // First closing paragraph
                var closingPara1 = new Paragraph();
                closingPara1.SpacingAfter = 10f;
                closingPara1.Add(new Chunk("Other terms and conditions as mentioned in your Appointment Letter continue to apply.", fontNormal));
                doc.Add(closingPara1);

                // Second closing paragraph
                var closingPara2 = new Paragraph();
                closingPara2.SpacingAfter = 10f;
                closingPara2.Add(new Chunk("Thanks again for putting in your best efforts and looking forward to a mutually rewarding time ahead!", fontNormal));
                doc.Add(closingPara2);

                // Third closing paragraph
                var closingPara3 = new Paragraph();
                closingPara3.SpacingAfter = 10f;
                closingPara3.Add(new Chunk("Kindly sign the copy of the letter as a token of your acceptance.", fontNormal));
                doc.Add(closingPara3);

                // Signature block
                var signaturePara = new Paragraph("Sincerely,", fontNormal);
                doc.Add(signaturePara);

                // Create a paragraph
                var companyPara = new Paragraph();
                companyPara.SpacingAfter = 30f;

                // Red font (only for "V")
                var redFont = FontFactory.GetFont(fontBold.Familyname, fontBold.Size, fontBold.Style, BaseColor.RED);

                // Add "V" in red
                companyPara.Add(new Chunk("V", redFont));

                // Add the rest in bold (normal black)
                companyPara.Add(new Chunk("Lead Design Services Private Limited", fontNormal));

                // Add to document
                doc.Add(companyPara);

                // HR Signature block
                var hrSignature = new Paragraph();
                hrSignature.SpacingBefore = 20f;
                hrSignature.SpacingAfter = 5f;
                hrSignature.Add(new Chunk("Nirmala Thamarai\n", fontNormal));
                hrSignature.Add(new Chunk("Manager - HR", fontNormal));
                doc.Add(hrSignature);

                // Thicker red line (e.g., 2.5pt thick, 30% page width, aligned left)
                var redLine = new iTextSharp.text.pdf.draw.LineSeparator(2.5f, 100f, BaseColor.RED, Element.ALIGN_LEFT, 0);
                doc.Add(new Chunk(redLine));

                // Start new page for Annexure
                doc.NewPage();

                doc.Add(new Paragraph("Annexure A", fontTitle) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f });

                // Employee info for annexure
                var namePara1 = new Paragraph();
                namePara1.Add(new Chunk("Employee Name: ", fontBold));
                namePara1.Add(new Chunk(model.EmployeeName, fontNormal));
                doc.Add(namePara1);

                var codePara1 = new Paragraph();
                codePara1.Add(new Chunk("Employee Code: ", fontBold));
                codePara1.Add(new Chunk(model.EmployeeCode, fontNormal));
                doc.Add(codePara1);

                doc.Add(new Paragraph("\n"));

                // Define table with proper borders
                var table = new PdfPTable(5) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 4f, 2f, 2f, 2f, 2f });

                // Optional: Add empty cell for Description column to keep alignment
                table.AddCell(new PdfPCell(new Phrase("", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                // Salary Components header centered across columns 2-5
                var salaryHeader = new PdfPCell(new Phrase("Salary Components", fontBold))
                {
                    Colspan = 5,
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240),
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingLeft = 80f
                };
                table.AddCell(salaryHeader);

                // Add Description header
                table.AddCell(new PdfPCell(new Phrase("Description", fontBold))
                {
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240),
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                // Add Current Salary header
                var currentHeader = new PdfPCell(new Phrase("Current Salary", fontBold))
                {
                    Colspan = 2,
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                table.AddCell(currentHeader);

                // Add Appraisal Salary header
                var appraisalHeader = new PdfPCell(new Phrase($"Salary after Appraisal, June {model.SalaryAfterAppraisal.AppraisalYear}", fontBold))
                {
                    Colspan = 2,
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                table.AddCell(appraisalHeader);

                // Add sub-headers row
                table.AddCell(new PdfPCell(new Phrase("", fontBold))
                {
                    Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                table.AddCell(new PdfPCell(new Phrase("Per Annum", fontBold))
                {
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                table.AddCell(new PdfPCell(new Phrase("Per Month", fontBold))
                {
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                table.AddCell(new PdfPCell(new Phrase("Per Annum", fontBold))
                {
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                table.AddCell(new PdfPCell(new Phrase("Per Month", fontBold))
                {
                    Border = Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 6f,
                    BackgroundColor = new BaseColor(240, 240, 240)
                });

                // MODIFIED AddRow function with proper formatting for section headers and right border
                void AddRow(string label, decimal currPA, decimal revPA, bool isBold = false, bool isSectionHeader = false, bool isFirstRow = false)
                {
                    var font = isBold ? fontBold : fontNormal;

                    if (isSectionHeader)
                    {
                        // Section headers: LEFT ALIGNED with all borders including right border
                        var cell = new PdfPCell(new Phrase(label, font))
                        {
                            Colspan = 5,
                            Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                            Padding = 6f,
                            PaddingLeft = 8f,
                            BackgroundColor = new BaseColor(220, 220, 220),
                            HorizontalAlignment = Element.ALIGN_LEFT
                        };
                        table.AddCell(cell);
                    }
                    else
                    {
                        // Determine border style - add top border for first row
                        int borderStyle = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                        if (isFirstRow)
                        {
                            borderStyle |= Rectangle.TOP_BORDER;
                        }

                        // Description column - LEFT ALIGNED with right border and proper word wrapping
                        var descCell = new PdfPCell(new Phrase(label, font))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            PaddingLeft = 8f,
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            NoWrap = label == "Employee ESI Contribution" // Force single line for ESI Contribution
                        };
                        table.AddCell(descCell);

                        // Current salary columns
                        table.AddCell(new PdfPCell(new Phrase(currPA > 0 ? $"₹ {currPA:N0}" : "-", unicodeFont))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        table.AddCell(new PdfPCell(new Phrase(currPA > 0 ? $"₹ {currPA / 12:N0}" : "-", unicodeFont))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        // Revised salary columns
                        table.AddCell(new PdfPCell(new Phrase(revPA > 0 ? $"₹ {revPA:N0}" : "-", unicodeFont))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        table.AddCell(new PdfPCell(new Phrase(revPA > 0 ? $"₹ {revPA / 12:N0}" : "-", unicodeFont))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });
                    }
                }

                // Add data rows with proper null checking
                var c = model.CurrentSalary;
                var r = model.SalaryAfterAppraisal;

                // Verify that the model data is not null
                if (c == null || r == null)
                {
                    throw new ArgumentException("CurrentSalary or SalaryAfterAppraisal data is missing");
                }

                // Gross (A) section - Note the isFirstRow: true for Basic
                AddRow("Basic", c.Basic, r.Basic, isFirstRow: true);
                AddRow("HRA", c.Hra, r.Hra);
                AddRow("Conveyance", c.Conveyance, r.Conveyance);
                AddRow("Medical Allowance", c.MedicalAllowance, r.MedicalAllowance);
                AddRow("Special Allowance", c.SpecialAllowance, r.SpecialAllowance);

                // Add bottom border to last row before total
                var grossRow = new PdfPCell(new Phrase("Gross (A)", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    PaddingLeft = 8f,
                    BackgroundColor = new BaseColor(250, 250, 250)
                };
                table.AddCell(grossRow);

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.Gross:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.Gross / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.Gross:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.Gross / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                // Benefits (B) section - with right border
                AddRow("Benefits (B)", 0, 0, true, true);
                AddRow("Employer PF Contribution", c.EmployerPfContribution, r.EmployerPfContribution, isFirstRow: true);
                AddRow("Employer ESI Contribution", c.EmployerEsiContribution, r.EmployerEsiContribution);

                // Benefits (C) section - with right border
                AddRow("Benefits (C)", 0, 0, true, true);
                AddRow("Group Medical Insurance", c.EmployerGroupMedicalInsurance, r.EmployerGroupMedicalInsurance, isFirstRow: true);
                AddRow("Group Personal Accident", c.GroupPersonalAccident, r.GroupPersonalAccident);

                // CTC Total row
                var ctcRow = new PdfPCell(new Phrase("CTC (A+B+C)", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    PaddingLeft = 8f,
                    BackgroundColor = new BaseColor(250, 250, 250)
                };
                table.AddCell(ctcRow);

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.Ctc:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.Ctc / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.Ctc:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.Ctc / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                // Deduction (D) section - with right border
                AddRow("Deduction (D)", 0, 0, true, true);
                AddRow("Employee PF Contribution", c.EmployeePfContribution, r.EmployeePfContribution, isFirstRow: true);
                AddRow("Employee ESI Contribution", c.EmployeeEsiContribution, r.EmployeeEsiContribution);
                AddRow("Professional Tax", c.ProfessionalTax, r.ProfessionalTax);
                AddRow("Group Medical Insurance", c.EmployeeGroupMedicalInsurance, r.EmployeeGroupMedicalInsurance);

                // Net Take Home Total row
                var netRow = new PdfPCell(new Phrase("Net Take Home (A-D)", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    PaddingLeft = 8f,
                    BackgroundColor = new BaseColor(250, 250, 250)
                };
                table.AddCell(netRow);

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.NetTakeHome:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {c.NetTakeHome / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.NetTakeHome:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹ {r.NetTakeHome / 12:N0}", unicodeBoldFont))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                // Add the statutory deductions note as the last row of the table
                var phrase = new Phrase();
                phrase.Add(new Chunk("D", fontSmallBold));
                phrase.Add(new Chunk(" - Statutory Deductions applicable from time to time", fontSmall));

                var statutoryRow = new PdfPCell(phrase)
                {
                    Colspan = 5,
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 8f,
                    PaddingLeft = 10f,
                    BackgroundColor = BaseColor.WHITE,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(statutoryRow);

                // Add table to document
                doc.Add(table);
                //doc.Add(new Paragraph("\nD - Statutory Deductions applicable from time to time", fontSmall));
                var notePara = new Paragraph();
                notePara.SpacingBefore = 10f;
                notePara.SpacingAfter = 10f;

                // Define red font for "VL"
                var redFont2 = FontFactory.GetFont(fontSmall.Familyname, fontSmall.Size, fontSmall.Style, BaseColor.RED);

                // Define normal font (black) for the rest
                var normalFont = fontSmall;

                // Construct the note with "VL" in red
                notePara.Add(new Chunk("Note: All the salary components associated with your employment in ", normalFont));
                notePara.Add(new Chunk("VL", redFont2));
                notePara.Add(new Chunk("ead like Special Value Payment (SVP), any Variable Component, Bonus, Commission, Verbal or Written promises or representations, Mobile/Telephone reimbursements, other allowances, other benefits mentioned in your previous Appointment letters, Appraisal letters stands cancelled.", normalFont));

                // Add to document
                doc.Add(notePara);
                // Signature block
                var signaturePara1 = new Paragraph("Sincerely,", fontNormal);
                doc.Add(signaturePara1);

                // Create a paragraph
                var companyPara1 = new Paragraph();
                companyPara1.SpacingAfter = 30f;

                // Red font (only for "V")
                var redFont1 = FontFactory.GetFont(fontBold.Familyname, fontBold.Size, fontBold.Style, BaseColor.RED);

                // Add "V" in red
                companyPara1.Add(new Chunk("V", redFont1));

                // Add the rest in bold (normal black)
                companyPara1.Add(new Chunk("Lead Design Services Private Limited", fontNormal));

                // Add to document
                doc.Add(companyPara1);

                // HR Signature block
                var hrSignature1 = new Paragraph();
                hrSignature1.SpacingBefore = 20f;
                hrSignature1.SpacingAfter = 5f;
                hrSignature1.Add(new Chunk("Nirmala Thamarai\n", fontNormal));
                hrSignature1.Add(new Chunk("Manager - HR", fontNormal));
                doc.Add(hrSignature1);

                // Thicker red line (e.g., 2.5pt thick, 30% page width, aligned left)
                var redLine1 = new iTextSharp.text.pdf.draw.LineSeparator(2.5f, 100f, BaseColor.RED, Element.ALIGN_LEFT, 0);
                doc.Add(new Chunk(redLine1));

                doc.Close();
            }
            return filePath;
        }

        private static string ConvertAmountToWords(decimal amount)
        {
            if (amount == 0)
                return "Zero";

            long number = (long)Math.Floor(amount);
            return ConvertNumberToWords(number);
        }

        private static string ConvertNumberToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + ConvertNumberToWords(Math.Abs(number));

            string words = "";

            if (number / 10000000 > 0)
            {
                words += ConvertNumberToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if (number / 100000 > 0)
            {
                words += ConvertNumberToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if (number / 1000 > 0)
            {
                words += ConvertNumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += ConvertNumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                           "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if (number % 10 > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words.Trim();
        }
    }
}