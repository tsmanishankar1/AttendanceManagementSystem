using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Services
{
    public class PerformanceReviewService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly string _workspacePath;
        public PerformanceReviewService(AttendanceManagementSystemContext context, IWebHostEnvironment env)
        {
            _context = context;
            _workspacePath = Path.Combine(env.ContentRootPath, "GeneratedLetters");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
        }

        /*        public async Task<List<PerformanceReviewDto>> GetPerformanceReviewCycle()
                {
                    var performances = await (from per in _context.PerformanceReviewCycles
                                              select new PerformanceReviewDto
                                              {
                                                  Id = per.Id,
                                                  From = per.From,
                                                  To = per.To,
                                                  IsActive = per.IsActive,
                                                  CreatedBy = per.CreatedBy
                                              })
                                              .ToListAsync();
                    if (performances.Count == 0) throw new MessageNotFoundException("No performance review cycles found");
                    return performances;
                }

                public async Task<string> CreatetPerformanceReviewCycle(PerformanceReviewRequest performanceReviewRequest)
                {
                    var message = "Performance review cycle created successfully";
                    var performance = new PerformanceReviewCycle
                    {
                        From = performanceReviewRequest.From,
                        To = performanceReviewRequest.To,
                        IsActive = performanceReviewRequest.IsActive,
                        CreatedBy = performanceReviewRequest.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.AddAsync(performance);
                    await _context.SaveChangesAsync();
                    return message;
                }

                public async Task<string> UpdatePerformanceReviewCycle (UpdatePerformanceReview updatePerformanceReview)
                {
                    var message = "Performance review cycle updated successfully";
                    var existingPerformanceReview = await _context.PerformanceReviewCycles.FirstOrDefaultAsync(p => p.Id == updatePerformanceReview.Id);
                    if (existingPerformanceReview == null) throw new MessageNotFoundException("Performance review cycle not found");
                    existingPerformanceReview.From = updatePerformanceReview.From;
                    existingPerformanceReview.To = updatePerformanceReview.To;
                    existingPerformanceReview.IsActive = updatePerformanceReview.IsActive;
                    existingPerformanceReview.UpdatedBy = updatePerformanceReview.UpdatedBy;
                    existingPerformanceReview.UpdatedUtc = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return message;
                }

                public async Task<List<EligibleEmployeeResponse>> GetEligibleEmployees()
                {
                    var performances = await (from per in _context.PerformanceReviewEmployees
                                              join staff in _context.StaffCreations on per.StaffId equals staff.Id
                                              join cycle in _context.PerformanceReviewCycles on per.PerformanceCycleId equals cycle.Id
                                              where per.IsActive == true && staff.IsActive == true && cycle.IsActive == true
                                              select new EligibleEmployeeResponse
                                              {
                                                  Id = per.Id,
                                                  StaffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}",
                                                  PerformanceCycle = $"{cycle.From} - {cycle.To}",
                                                  IsActive = per.IsActive,
                                                  CreatedBy = per.CreatedBy
                                              })
                                  .ToListAsync();
                    if (performances.Count == 0) throw new MessageNotFoundException("No eligile staffs found");
                    return performances;
                }

                public async Task<string> CreatetEligibleEmployees(EligibleEmployeeRequest performanceReviewRequest)
                {
                    var message = "Eligible staffs submitted successfully";
                    var performance = new PerformanceReviewEmployee
                    {
                        StaffId = performanceReviewRequest.StaffId,
                        PerformanceCycleId = performanceReviewRequest.PerformanceCycleId,
                        IsActive = performanceReviewRequest.IsActive,
                        CreatedBy = performanceReviewRequest.CreatedBy,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _context.AddAsync(performance);
                    await _context.SaveChangesAsync();
                    return message;
                }
        */
        public async Task<string> GenerateAppraisalLetter(GenerateAppraisalLetterRequest generateAppraisalLetterRequest)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.StaffId == generateAppraisalLetterRequest.StaffId && s.IsActive == true);
            if (staff == null) throw new MessageNotFoundException("Staff not found");
            var staffName = $"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}";
            var employeeCode = staff.StaffId;
            DesignationMaster? designation;
            string fileName = "";
            if (generateAppraisalLetterRequest.DesignationId.HasValue)
            {
                designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == generateAppraisalLetterRequest.DesignationId.Value && d.IsActive == true);
                if (designation == null) throw new MessageNotFoundException("Designation not found");
                staff.DesignationId = generateAppraisalLetterRequest.DesignationId.Value;
                await _context.SaveChangesAsync();
                fileName = $"Appraisal_Letter_With_DC_{staff.StaffId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            }
            else
            {
                designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == staff.DesignationId && d.IsActive == true);
                if (designation == null) throw new MessageNotFoundException("Designation not found");
                fileName = $"Appraisal_Letter_Without_DC_{staff.StaffId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            }
            var newDesignation = designation.Name;
            int currentYear = DateTime.Now.Year;

            var appraisals = await (from app in _context.AppraisalAnnexureAs
                                    where app.EmployeeCode == employeeCode && app.IsActive
                                    select app).ToListAsync();

            var previousAppraisal = appraisals
                .Where(x => x.AppraisalYear == currentYear - 1)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            var currentAppraisal = appraisals
                .Where(x => x.AppraisalYear == currentYear)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
            var title = staff.Title;
            var appraisal = new AppraisalAnnexureResponse
            {
                EmployeeName = staffName,
                EmployeeCode = employeeCode,
                Designation = newDesignation,
                Title = title,
                IsDesignationChange = generateAppraisalLetterRequest.DesignationId.HasValue,
                CurrentSalary = previousAppraisal != null ? new PreviousYearAppraisal
                {
                    Basic = previousAppraisal.Basic,
                    Hra = previousAppraisal.Hra,
                    Conveyance = previousAppraisal.Conveyance,
                    MedicalAllowance = previousAppraisal.MedicalAllowance,
                    SpecialAllowance = previousAppraisal.SpecialAllowance,
                    Gross = previousAppraisal.Basic + previousAppraisal.Hra + previousAppraisal.Conveyance + previousAppraisal.MedicalAllowance + previousAppraisal.SpecialAllowance,
                    EmployerPfContribution = previousAppraisal.EmployerPfContribution,
                    EmployerEsiContribution = previousAppraisal.EmployerEsiContribution,
                    EmployerGroupMedicalInsurance = previousAppraisal.EmployerGroupMedicalInsurance,
                    GroupPersonalAccident = previousAppraisal.GroupPersonalAccident,
                    Ctc = previousAppraisal.Basic + previousAppraisal.Hra + previousAppraisal.Conveyance + previousAppraisal.MedicalAllowance + previousAppraisal.SpecialAllowance +
                          previousAppraisal.EmployerPfContribution + previousAppraisal.EmployerEsiContribution + previousAppraisal.EmployerGroupMedicalInsurance +
                          previousAppraisal.GroupPersonalAccident,
                    EmployeePfContribution = previousAppraisal.EmployeePfContribution,
                    EmployeeEsiContribution = previousAppraisal.EmployeeEsiContribution,
                    ProfessionalTax = previousAppraisal.ProfessionalTax,
                    EmployeeGroupMedicalInsurance = previousAppraisal.EmployeeGroupMedicalInsurance,
                    NetTakeHome = (previousAppraisal.Basic + previousAppraisal.Hra + previousAppraisal.Conveyance + previousAppraisal.MedicalAllowance + previousAppraisal.SpecialAllowance) -
                                  (previousAppraisal.EmployeePfContribution + previousAppraisal.EmployeeEsiContribution + previousAppraisal.ProfessionalTax +
                                   previousAppraisal.EmployeeGroupMedicalInsurance),
                    AppraisalAmount = previousAppraisal.AppraisalAmount,
                    AppraisalYear = previousAppraisal.AppraisalYear
                } : null,
                SalaryAfterAppraisal = currentAppraisal != null ? new CurrentYearAppraisal
                {
                    Basic = currentAppraisal.Basic,
                    Hra = currentAppraisal.Hra,
                    Conveyance = currentAppraisal.Conveyance,
                    MedicalAllowance = currentAppraisal.MedicalAllowance,
                    SpecialAllowance = currentAppraisal.SpecialAllowance,
                    Gross = currentAppraisal.Basic + currentAppraisal.Hra + currentAppraisal.Conveyance + currentAppraisal.MedicalAllowance + currentAppraisal.SpecialAllowance,
                    EmployerPfContribution = currentAppraisal.EmployerPfContribution,
                    EmployerEsiContribution = currentAppraisal.EmployerEsiContribution,
                    EmployerGroupMedicalInsurance = currentAppraisal.EmployerGroupMedicalInsurance,
                    GroupPersonalAccident = currentAppraisal.GroupPersonalAccident,
                    Ctc = currentAppraisal.Basic + currentAppraisal.Hra + currentAppraisal.Conveyance + currentAppraisal.MedicalAllowance + currentAppraisal.SpecialAllowance +
                          currentAppraisal.EmployerPfContribution + currentAppraisal.EmployerEsiContribution + currentAppraisal.EmployerGroupMedicalInsurance +
                          currentAppraisal.GroupPersonalAccident,
                    EmployeePfContribution = currentAppraisal.EmployeePfContribution,
                    EmployeeEsiContribution = currentAppraisal.EmployeeEsiContribution,
                    ProfessionalTax = currentAppraisal.ProfessionalTax,
                    EmployeeGroupMedicalInsurance = currentAppraisal.EmployeeGroupMedicalInsurance,
                    NetTakeHome = (currentAppraisal.Basic + currentAppraisal.Hra + currentAppraisal.Conveyance + currentAppraisal.MedicalAllowance + currentAppraisal.SpecialAllowance) -
                                  (currentAppraisal.EmployeePfContribution + currentAppraisal.EmployeeEsiContribution + currentAppraisal.ProfessionalTax +
                                   currentAppraisal.EmployeeGroupMedicalInsurance),
                    AppraisalAmount = currentAppraisal.AppraisalAmount,
                    AppraisalYear = currentAppraisal.AppraisalYear
                } : null
            };
            if (appraisal == null) throw new MessageNotFoundException("Appraisal annexure not found");
            var file = GenerateAppraisalLetter(appraisal, fileName);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(file);
            string base64Pdf = Convert.ToBase64String(pdfBytes);
            var letterGeneration = new LetterGeneration
            {
                LetterPath = file,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staff.Id,
                CreatedBy = generateAppraisalLetterRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            await _context.LetterGenerations.AddAsync(letterGeneration);
            await _context.SaveChangesAsync();
            return file;
        }

        public static string GenerateAppraisalLetter(AppraisalAnnexureResponse model, string fileName)
        {
            using var stream = new MemoryStream();

            // Increase A4 width by 200 points (approx. 2.78 inches)
            float customWidth = PageSize.A4.Width;
            float customHeight = PageSize.A4.Height + 100f;
            string filePath = "";
            if (model.IsDesignationChange)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                filePath = Path.Combine(directoryPath, fileName);
            }
            else
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                filePath = Path.Combine(directoryPath, fileName);
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
                var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
                var fontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 11);
                var fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                var fontSmallBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);

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
                evalPara.Add(Chunk.NEWLINE);
                evalPara.Add(new Chunk("        a) Your contribution on the team's target\n        b) Cross Training\n        c) Bench Contribution", fontNormal));
                doc.Add(evalPara);

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
                        table.AddCell(new PdfPCell(new Phrase(currPA > 0 ? $"₹{currPA:N0}" : "-", font))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        table.AddCell(new PdfPCell(new Phrase(currPA > 0 ? $"₹{currPA / 12:N0}" : "-", font))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        // Revised salary columns
                        table.AddCell(new PdfPCell(new Phrase(revPA > 0 ? $"₹{revPA:N0}" : "-", font))
                        {
                            Border = borderStyle,
                            Padding = 5f,
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        table.AddCell(new PdfPCell(new Phrase(revPA > 0 ? $"₹{revPA / 12:N0}" : "-", font))
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

                table.AddCell(new PdfPCell(new Phrase($"₹{c.Gross:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{c.Gross / 12:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.Gross:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.Gross / 12:N0}", fontBold))
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

                table.AddCell(new PdfPCell(new Phrase($"₹{c.Ctc:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{c.Ctc / 12:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.Ctc:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.Ctc / 12:N0}", fontBold))
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

                table.AddCell(new PdfPCell(new Phrase($"₹{c.NetTakeHome:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{c.NetTakeHome / 12:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.NetTakeHome:N0}", fontBold))
                {
                    Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER,
                    Padding = 5f,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    BackgroundColor = new BaseColor(250, 250, 250)
                });

                table.AddCell(new PdfPCell(new Phrase($"₹{r.NetTakeHome / 12:N0}", fontBold))
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


        public async Task<string> DownloadAppraisalLetter(int staffId, int fileId)
        {
            var appraisalLetter = await _context.LetterGenerations.FirstOrDefaultAsync(x => x.StaffCreationId == staffId && x.Id == fileId && x.IsActive == true);
            if (appraisalLetter == null)
            {
                throw new MessageNotFoundException("Appraisal letter not found");
            }
            string file = appraisalLetter.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("File name is empty");
            }
            return Path.Combine(_workspacePath, file);
        }

        public async Task<(Stream PdfStream, string FileName)> ViewAppraisalLetter(int staffId, int fileId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(lg => lg.StaffCreationId == staffId && lg.Id == fileId && lg.IsActive);
            if (letterGeneration == null)
            {
                throw new FileNotFoundException("Letter generation record not found.");
            }
            var filePath = letterGeneration.LetterPath;

            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("PDF file not found on disk.");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileName = Path.GetFileName(filePath);
            return (stream, fileName);
        }
    }
}
