using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Text;
using Document = iTextSharp.text.Document;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;
using NETCore.MailKit.Core;
using System.Threading.Tasks;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using Font = iTextSharp.text.Font;
using iText.IO.Font;
using Microsoft.Win32.SafeHandles;
using DocumentFormat.OpenXml.Wordprocessing;
using Paragraph = iTextSharp.text.Paragraph;
using PageSize = iTextSharp.text.PageSize;

namespace AttendanceManagement.Services
{
    public class ProbationService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly EmailService _emailService;
        private readonly string _workspacePath;
        public ProbationService(AttendanceManagementSystemContext context, EmailService emailService, IWebHostEnvironment env)
        {
            _context = context;
            _emailService = emailService;
            _workspacePath = Path.Combine(env.ContentRootPath, "GeneratedLetters");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
        }

        public async Task<List<ProbationResponse>> GetAllProbationsAsync()
        {
            var allProbation = await (
                from p in _context.Probations
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                join r in _context.ProbationReports on s.StaffId equals r.EmpId into reportGroup
                from report in reportGroup.DefaultIfEmpty()
                let latestFeedback = _context.Feedbacks
                    .Where(f => f.ProbationId == p.Id && f.IsActive)
                    .OrderByDescending(f => f.Id)
                    .FirstOrDefault()
                where p.IsActive && s.IsActive == true
                select new ProbationResponse
                {
                    ProbationId = p.Id,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                    DepartmentName = d.Name,
                    ProbationStartDate = p.ProbationStartDate,
                    ProbationEndDate = latestFeedback != null && latestFeedback.ExtensionPeriod != null
                        ? latestFeedback.ExtensionPeriod.Value
                        : p.ProbationEndDate,
                    CreatedBy = p.CreatedBy,
                    ProbationReport = report
                }).ToListAsync();
            if (!allProbation.Any())
            {
                throw new MessageNotFoundException("No Probations found");
            }
            return allProbation;
        }

        public async Task<object> GetAllManagers()
        {
            var managers = await _context.StaffCreations.Where(s => s.AccessLevel == "REPORTING MANAGER" && s.IsActive == true)
                .Select(s => new
                {
                   Id = s.Id,
                   StaffId = s.StaffId,
                   Name = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                   OfficialMail = s.OfficialEmail
                }).ToListAsync();
            if(managers.Count == 0) throw new MessageNotFoundException("No Managers found");
            return managers;
        }

        public async Task<ProbationResponse> GetProbationByIdAsync(int probationId)
        {
            var probation = await (from p in _context.Probations
                                      join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                      join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                      join r in _context.ProbationReports on s.StaffId equals r.EmpId into reportGroup
                                      from report in reportGroup.DefaultIfEmpty()
                                      where p.IsActive
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffId = p.StaffCreationId,
                                          StaffCreationId = s.StaffId,
                                          StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                          DepartmentName = d.Name,
                                          ProbationStartDate = p.ProbationStartDate,
                                          ProbationEndDate = p.ProbationEndDate,
                                          CreatedBy = p.CreatedBy,
                                          ProbationReport = report,
                                      }).FirstOrDefaultAsync();
            if (probation == null)
            {
                throw new MessageNotFoundException("Probation not found");
            }
            return probation;
        }

        public async Task<string> AssignManagerForProbationReview(AssignManagerRequest assignManagerRequest)
        {
            var message = "Manager assigned successfully";
            var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == assignManagerRequest.ProbationId && p.IsActive);
            if (probation == null) throw new MessageNotFoundException("Probation not found");
            var probationer = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true);
            if (probationer == null) throw new MessageNotFoundException("Probationer not found");
            var manager = await _context.StaffCreations.FirstOrDefaultAsync(m => m.Id == assignManagerRequest.ManagerId && m.IsActive == true);
            if (manager == null) throw new MessageNotFoundException("Manager not found");
            if (manager.OfficialEmail == null) throw new MessageNotFoundException("Manager email not found");
            if (probation.ManagerId != null) throw new ConflictException("Manager already assigned");
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.ProbationId == probation.Id && f.IsActive);
            var effectiveEndDate = feedback?.ExtensionPeriod ?? probation.ProbationEndDate;
            probation.ManagerId = assignManagerRequest.ManagerId;
            probation.AssignedBy = assignManagerRequest.CreatedBy;
            probation.AssignedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await _emailService.AssignManager(manager.OfficialEmail, manager.Id, $"{manager.FirstName}{(string.IsNullOrWhiteSpace(manager.LastName) ? "" : " " + manager.LastName)}",
                $"{probationer.FirstName}{(string.IsNullOrWhiteSpace(probationer.LastName) ? "" : " " + probationer.LastName)}", probation.ProbationStartDate, effectiveEndDate,
                assignManagerRequest.CreatedBy);
            return message;
        }

        public async Task<string> CreateProbationAsync(ProbationRequest probationRequest)
        {
            var message = "Probation created successfully.";
            var staff = await _context.StaffCreations.AnyAsync(p => p.Id == probationRequest.StaffId && p.IsActive == true);
            if (!staff) throw new MessageNotFoundException("Staff not found");
            var pro = await _context.Probations.AnyAsync(p => p.StaffCreationId == probationRequest.StaffId);
            if (pro) throw new ConflictException("Probation Details Already Exists");
            var probation = new Probation
            {
                StaffCreationId = probationRequest.StaffId,
                ProbationStartDate = probationRequest.ProbationStartDate,
                ProbationEndDate = probationRequest.ProbationEndDate,
                IsCompleted = false,
                IsActive = true,
                CreatedBy = probationRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.Probations.AddAsync(probation);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateProbationAsync(UpdateProbation probation)
        {
            var message = "Probation updated successfully.";
            var staff = await _context.StaffCreations.AnyAsync(p => p.Id == probation.StaffId && p.IsActive == true);
            if (!staff) throw new MessageNotFoundException("Staff not found");
            var existingProbation = await _context.Probations.FirstOrDefaultAsync(b => b.Id == probation.ProbationId && b.IsActive);
            if (existingProbation == null)
            {
                throw new MessageNotFoundException("Probation not found");
            }
            existingProbation.StaffCreationId = probation.StaffId;
            existingProbation.ProbationStartDate = probation.ProbationStartDate;
            existingProbation.ProbationEndDate = probation.ProbationEndDate;
            existingProbation.UpdatedBy = probation.UpdatedBy;
            existingProbation.UpdatedUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel(int approverLevelId)
        {
            var matchingProbations = await (from p in _context.Probations
                                      join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                      join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                      join r in _context.ProbationReports on s.StaffId equals r.EmpId into reportGroup
                                      from report in reportGroup.DefaultIfEmpty()
                                      where p.IsActive && s.IsActive == true && (s.ApprovalLevel1 == approverLevelId || s.ApprovalLevel2 == approverLevelId || s.AccessLevel == "SUPER ADMIN")
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffId = p.StaffCreationId,
                                          StaffCreationId = s.StaffId,
                                          StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                          DepartmentName = d.Name,
                                          ProbationStartDate = p.ProbationStartDate,
                                          ProbationEndDate = p.ProbationEndDate,
                                          CreatedBy = p.CreatedBy,
                                          ProbationReport = report
                                      }).ToListAsync();
            if (!matchingProbations.Any())
            {
                throw new MessageNotFoundException("No Probations found");
            }
            return matchingProbations;
        }

        public async Task<List<FeedbackResponse>> GetFeedbackDetailsByApproverLevel1(int approverId)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN";

            var feedbackWithJoins = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.ProbationId equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                where s.IsActive == true && f.IsActive && p.IsActive && (isSuperAdmin || s.ApprovalLevel1 == approverId || s.ApprovalLevel2 == approverId)
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = p.Id,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                    FeedbackText = f.FeedbackText,
                    ProbationExtensionPeriod = f.ExtensionPeriod,
                    IsApproved = f.IsApproved,
                    CreatedBy = f.CreatedBy
                }).ToListAsync();
            if (!feedbackWithJoins.Any())
            {
                throw new MessageNotFoundException("Feedback not found");
            }
            return feedbackWithJoins;
        }

        public async Task<string> AddFeedbackAsync(FeedbackRequest feedbackRequest)
        {
            var message = "";
            var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == feedbackRequest.ProbationId && p.IsActive);
            if (probation == null) throw new MessageNotFoundException("Probation not found");
            var probationer = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true);
            if (probationer == null) throw new MessageNotFoundException("Probationer not found");
            var approver = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == feedbackRequest.CreatedBy && s.IsActive == true);
            if (approver == null) throw new MessageNotFoundException("Approver not found");
            var feedbacks = await _context.Feedbacks.Where(f => f.ProbationId == probation.Id && f.IsActive).OrderByDescending(f => f.Id).FirstOrDefaultAsync();
            if (feedbackRequest.IsApproved)
            {
                message = "Probation approved successfully.";
                if (feedbacks != null)
                {
                    if (feedbacks.IsApproved == true) throw new ConflictException("Probation already approved");
                    feedbacks.IsActive = false;
                    feedbacks.UpdatedBy = feedbackRequest.CreatedBy;
                    feedbacks.UpdatedUtc = DateTime.UtcNow;
                }
                var feedback = new Feedback
                {
                    ProbationId = feedbackRequest.ProbationId,
                    FeedbackText = feedbackRequest.FeedbackText,
                    IsApproved = feedbackRequest.IsApproved,
                    IsActive = true,
                    CreatedBy = feedbackRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                await _context.Feedbacks.AddAsync(feedback);
                string approvedTime = feedback.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                await _emailService.SendProbationConfirmationNotificationToHrAsync
                (approver.Id, $"{probationer.FirstName}{(string.IsNullOrWhiteSpace(probationer.LastName) ? "" : " " + probationer.LastName)}", probation.ProbationStartDate,
                probation.ProbationEndDate, null, feedbackRequest.IsApproved, $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}",
                approvedTime, feedbackRequest.CreatedBy);
            }
            else
            {
                message = "Probation period has been extended successfully.";
                if (feedbacks != null)
                {
                    if (feedbacks.IsApproved == true) throw new ConflictException("Probation already approved");
                    feedbacks.IsActive = false;
                    feedbacks.UpdatedBy = feedbackRequest.CreatedBy;
                    feedbacks.UpdatedUtc = DateTime.UtcNow;
                }
                var effectiveEndDate = feedbacks?.ExtensionPeriod ?? probation.ProbationEndDate;
                var feedback = new Feedback
                {
                    ProbationId = feedbackRequest.ProbationId,
                    FeedbackText = feedbackRequest.FeedbackText,
                    IsApproved = feedbackRequest.IsApproved,
                    ExtensionPeriod = feedbackRequest.ExtensionPeriod,
                    IsActive = true,
                    CreatedBy = feedbackRequest.CreatedBy,
                    CreatedUtc = DateTime.UtcNow
                };
                await _context.Feedbacks.AddAsync(feedback);
                string approvedTime = feedback.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                await _emailService.SendProbationConfirmationNotificationToHrAsync
                (approver.Id, $"{probationer.FirstName}{(string.IsNullOrWhiteSpace(probationer.LastName) ? "" : " " + probationer.LastName)}",
                probation.ProbationStartDate, effectiveEndDate, feedbackRequest.ExtensionPeriod, feedbackRequest.IsApproved,
                $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}", approvedTime, feedbackRequest.CreatedBy);
            }
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<FeedbackResponse> GetFeedbackByIdAsync(int feedbackId)
        {
            var feedbackWithJoins = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.Id equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                where f.Id == feedbackId && f.IsActive
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = f.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                    IsApproved = f.IsApproved,
                    CreatedBy = f.CreatedBy
                }).FirstOrDefaultAsync();
            if (feedbackWithJoins == null)
            {
                throw new MessageNotFoundException("Feedback not found");
            }
            return feedbackWithJoins;
        }

        public async Task<List<FeedbackResponse>> GetAllFeedbacksAsync()
        {
            var feedbackList = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.Id equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                where f.IsActive && p.IsActive && s.IsActive == true
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = f.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                    IsApproved = f.IsApproved,
                    CreatedBy = f.CreatedBy
                }).ToListAsync();
            if (feedbackList.Count == 0)
            {
                throw new MessageNotFoundException("No Feedbacks found");
            }
            return feedbackList;
        }

        public async Task<string> UpdateFeedbackAsync(UpdateFeedback updatedFeedback)
        {
            var message = "Feedback updated successfully";
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.Id == updatedFeedback.FeedbackId && f.IsActive);
            if (feedback == null || !feedback.IsActive) throw new MessageNotFoundException("Feedback not found");
            feedback.Id = updatedFeedback.ProbationId;
            feedback.FeedbackText = updatedFeedback.FeedbackText;
            feedback.UpdatedBy = updatedFeedback.UpdatedBy;
            feedback.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> ProcessApprovalAsync(HrConfirmation hrConfirmation)
        {
            var staff = await _context.StaffCreations.Where(s => s.Id == hrConfirmation.CreatedBy && s.IsActive == true)
                .Select(s => $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}").FirstOrDefaultAsync();
            string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == hrConfirmation.ProbationId && p.IsActive);
            if (probation == null) throw new MessageNotFoundException("Probation not found");
            var feedback = await _context.Feedbacks.Where(f => f.ProbationId == hrConfirmation.ProbationId && f.IsActive).OrderByDescending(f => f.Id).FirstOrDefaultAsync();
            if (feedback == null) throw new MessageNotFoundException("Manager feedback not found");
            if (probation.IsCompleted == true) throw new ConflictException("Probation process has been already completed");
            probation.IsCompleted = hrConfirmation.IsCompleted;
            probation.IsActive = false;
            probation.UpdatedBy = hrConfirmation.CreatedBy;
            probation.UpdatedUtc = DateTime.UtcNow;
            feedback.IsActive = false;
            await _context.SaveChangesAsync();
            var notification = new ApprovalNotification
            {
                StaffId = probation.StaffCreationId,
                Message =  $"Your Probation period has been completed. Approved by - {staff} on {approvedDateTime}",
                IsActive = true,
                CreatedBy = hrConfirmation.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            await _context.ApprovalNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            probation.ApprovalNotificationId = notification.Id;
            await _context.SaveChangesAsync();

            var staffCreationId = probation.StaffCreationId;
            var staffId = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive == true);
            if (staffId == null) throw new MessageNotFoundException("Staff not found");
            var designation = await _context.DesignationMasters.FirstOrDefaultAsync(d => d.Id == staffId.DesignationId && d.IsActive);
            if (designation == null) throw new MessageNotFoundException("Designation not found");
            var startDate = probation.ProbationStartDate.ToString("dd MMMM yyyy");
            var endDate = probation.ProbationEndDate.ToString("dd MMMM yyyy");
            if(feedback.ExtensionPeriod != null)
            {
                endDate = feedback.ExtensionPeriod.Value.ToString("dd MMMM yyyy");
            }
            var fileName = $"Confirmation_Letter_{staffId.StaffId}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            var pdfPath = GeneratePdf(staffCreationId, designation.Name, staffId.Title, startDate, endDate, fileName);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            string base64Pdf = Convert.ToBase64String(pdfBytes);
            var letterGeneration = new LetterGeneration
            {
                LetterPath = pdfPath,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staffCreationId,
                FileName = fileName,
                CreatedBy = hrConfirmation.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            await _context.LetterGenerations.AddAsync(letterGeneration);
            await _context.SaveChangesAsync();
            return pdfPath;
        }

        private string GeneratePdf(int staffCreationId, string designation, string title, string startDate, string endDate, string fileName)
        {
            var staff = _context.StaffCreations.FirstOrDefault(s => s.Id == staffCreationId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException($"Staff with ID {staffCreationId} not found.");
            }
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, fileName);

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
                nameLine.Add(new Chunk($"{staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)}", bodyFont));
                pdfDoc.Add(nameLine);

                // Employee Code
                var codeLine = new Paragraph();
                codeLine.Add(new Chunk("Employee Code: ", boldFont));
                codeLine.Add(new Chunk(staff.StaffId, bodyFont));
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
                dearLine.Add(new Chunk($"{title} {staff.FirstName}{(string.IsNullOrWhiteSpace(staff.LastName) ? "" : " " + staff.LastName)},", bodyFont));
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

        public async Task<List<GeneratedLetterResponse>> GetGeneratedLetters(int staffId)
        {
            var letterGenerations = await (from letter in _context.LetterGenerations
                                           join staff in _context.StaffCreations on letter.StaffCreationId equals staff.Id
                                           where letter.StaffCreationId == staffId && letter.IsActive
                                           select new GeneratedLetterResponse
                                           {
                                               Id = letter.Id,
                                               StaffId = letter.StaffCreationId,
                                               StaffCreationId = staff.StaffId,
                                               FileName = letter.FileName,
                                               LetterPath = letter.LetterPath,
                                               LetterContent = letter.LetterContent
                                           }).ToListAsync();
            if (letterGenerations.Count == 0)
            {
                throw new MessageNotFoundException("No PDF files found");
            }
            return letterGenerations;
        }

        public async Task<string> GetPdfFilePath(int staffCreationId, int fileId)
        {
            var probation = await _context.LetterGenerations.FirstOrDefaultAsync(x => x.StaffCreationId == staffCreationId && x.Id == fileId && x.IsActive == true);
            if (probation == null)
            {
                throw new MessageNotFoundException("Confirmation letter not found");
            }
            string file = probation.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("File name is empty");
            }
            return Path.Combine(_workspacePath, file);
        }

        public async Task<string> GetPdfContent(int staffCreationId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);
            if (letterGeneration == null)
            {
                throw new MessageNotFoundException("Letter generation record not found for the provided StaffCreationId");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new MessageNotFoundException("Generated PDF file not found.");
            }
            using (var pdfReader = new iText.Kernel.Pdf.PdfReader(filePath))
            using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(pdfReader))
            {
                var textContent = new StringWriter();
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    var pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                    textContent.WriteLine(pageContent);
                }
                return textContent.ToString();
            }
        }

        public async Task<(byte[] fileBytes, string fileName, string contentType)> DownloadPdf(int staffCreationId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);
            if (letterGeneration == null)
            {
                throw new MessageNotFoundException("Letter generation record not found for the provided StaffCreationId.");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new MessageNotFoundException("Generated PDF file not found.");
            }
            var fileBytes = File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            const string contentType = "application/pdf";
            return (fileBytes, fileName, contentType);
        }
    }
}