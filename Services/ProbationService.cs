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

namespace AttendanceManagement.Services
{
    public class ProbationService
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly EmailService _emailService;

        public ProbationService(AttendanceManagementSystemContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        public async Task<List<ProbationResponse>> GetAllProbationsAsync(int approverId)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverId)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN";
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
                where p.IsActive && s.IsActive == true && (isSuperAdmin || s.ApprovalLevel1 == approverId || s.ApprovalLevel2 == approverId)
                select new ProbationResponse
                {
                    ProbationId = p.Id,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = s.FirstName + " " + s.LastName,
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
                                          StaffName = s.FirstName + " " + s.LastName,
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
            if (probation.ManagerId != null) throw new InvalidOperationException("Manager already assigned");
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.ProbationId == probation.Id && f.IsActive);
            var effectiveEndDate = feedback?.ExtensionPeriod ?? probation.ProbationEndDate;
            probation.ManagerId = assignManagerRequest.ManagerId;
            probation.AssignedBy = assignManagerRequest.CreatedBy;
            probation.AssignedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await _emailService.AssignManager(manager.OfficialEmail, manager.Id, $"{manager.FirstName} {manager.LastName}", $"{probationer.FirstName} {probationer.LastName}", probation.ProbationStartDate, effectiveEndDate, assignManagerRequest.CreatedBy);
            return message;
        }

        public async Task<string> CreateProbationAsync(ProbationRequest probationRequest)
        {
            var message = "Probation created successfully.";
            var pro = await _context.Probations.FirstOrDefaultAsync(p => p.StaffCreationId == probationRequest.StaffId && p.IsActive);
            if (pro != null)
            {
                throw new Exception("Probation Details Already Exists");
            }
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
            _context.Probations.Add(probation);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateProbationAsync(UpdateProbation probation)
        {
            var message = "Probation updated successfully.";
            var existingProbation = _context.Probations.FirstOrDefault(b => b.Id == probation.ProbationId && b.IsActive);
            if (existingProbation == null)
            {
                throw new MessageNotFoundException("Probation not found");
            }
            existingProbation.StaffCreationId = probation.StaffId;
            existingProbation.ProbationStartDate = probation.ProbationStartDate;
            existingProbation.ProbationEndDate = probation.ProbationEndDate;
            existingProbation.UpdatedBy = probation.UpdatedBy;
            existingProbation.UpdatedUtc = DateTime.UtcNow;
            _context.Entry(existingProbation).State = EntityState.Modified;
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
                                      where p.IsActive && (s.ApprovalLevel1 == approverLevelId || s.ApprovalLevel2 == approverLevelId || s.AccessLevel == "SUPER ADMIN")
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffId = p.StaffCreationId,
                                          StaffCreationId = s.StaffId,
                                          StaffName = s.FirstName + " " + s.LastName,
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
                .Where(x => x.Id == approverId)
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
                    StaffName = s.FirstName + " " + s.LastName,
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
            if (feedbackRequest.IsApproved)
            {
                message = "Probation approved successfully.";
                var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == feedbackRequest.ProbationId && p.IsActive);
                if (probation == null) throw new MessageNotFoundException("Probation not found");
                var probationer = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true);
                if (probationer == null) throw new MessageNotFoundException("Probationer not found");
                var approver = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == feedbackRequest.CreatedBy && s.IsActive == true);
                var feedbacks = await _context.Feedbacks.Where(f => f.ProbationId == probation.Id && f.IsActive).OrderByDescending(f => f.Id).FirstOrDefaultAsync();
                if (feedbacks != null)
                {
                    if (feedbacks.IsApproved == true) throw new InvalidOperationException("Probation already approved");
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
                _context.Feedbacks.Add(feedback);
                string approvedTime = feedback.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                await _emailService.SendProbationConfirmationNotificationToHrAsync(approver.Id, $"{probationer.FirstName} {probationer.LastName}", probation.ProbationStartDate, probation.ProbationEndDate, null, feedbackRequest.IsApproved, $"{approver.FirstName} {approver.LastName}", approvedTime, feedbackRequest.CreatedBy);
            }
            else
            {
                message = "Probation period has been extended successfully.";
                var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == feedbackRequest.ProbationId && p.IsActive);
                if (probation == null) throw new MessageNotFoundException("Probation not found");
                var probationer = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == probation.StaffCreationId && s.IsActive == true);
                if (probationer == null) throw new MessageNotFoundException("Probationer not found");
                var approver = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == feedbackRequest.CreatedBy && s.IsActive == true);
                var feedbacks = await _context.Feedbacks.Where(f => f.ProbationId == probation.Id && f.IsActive).OrderByDescending(f => f.Id).FirstOrDefaultAsync();
                if (feedbacks != null)
                {
                    if (feedbacks.IsApproved == true) throw new InvalidOperationException("Probation already approved");
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
                _context.Feedbacks.Add(feedback);
                string approvedTime = feedback.CreatedUtc.Value.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
                await _emailService.SendProbationConfirmationNotificationToHrAsync(approver.Id, $"{probationer.FirstName} {probationer.LastName}", probation.ProbationStartDate, effectiveEndDate, feedbackRequest.ExtensionPeriod, feedbackRequest.IsApproved, $"{approver.FirstName} {approver.LastName}", approvedTime, feedbackRequest.CreatedBy);
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
                    StaffName = s.FirstName + " " + s.LastName,
                    IsApproved = f.IsApproved,
                    CreatedBy = f.CreatedBy
                }).FirstOrDefaultAsync();
            if (feedbackWithJoins == null)
            {
                throw new MessageNotFoundException("Feedback not found");
            }
            return feedbackWithJoins;
        }

        public async Task<IEnumerable<FeedbackResponse>> GetAllFeedbacksAsync()
        {
            var feedbackList = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.Id equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                where f.IsActive
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = f.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = s.StaffId,
                    StaffName = s.FirstName + " " + s.LastName,
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
            var message = "Feedback updated successfully.";
            var feedback = _context.Feedbacks.FirstOrDefault(f => f.Id == updatedFeedback.FeedbackId && f.IsActive);
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
            var staff = _context.StaffCreations.Where(s => s.Id == hrConfirmation.CreatedBy && s.IsActive == true).Select(s => $"{s.FirstName}{s.LastName}").FirstOrDefault();
            string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == hrConfirmation.ProbationId && p.IsActive);
            if (probation == null) throw new MessageNotFoundException("Probation not found");
            var feedback = await _context.Feedbacks.Where(f => f.ProbationId == hrConfirmation.ProbationId && f.IsActive).OrderByDescending(f => f.Id).FirstOrDefaultAsync();
            if (feedback == null) throw new MessageNotFoundException("Manager feedback not found");
            if (probation.IsCompleted == true) throw new InvalidOperationException("Probation process has been already completed");
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
            _context.ApprovalNotifications.Add(notification);
            await _context.SaveChangesAsync();
            probation.ApprovalNotificationId = notification.Id;
            await _context.SaveChangesAsync();

            var staffCreationId = probation.StaffCreationId;
            var pdfPath = GeneratePdf(staffCreationId);
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            string base64Pdf = Convert.ToBase64String(pdfBytes);
            var letterGeneration = new LetterGeneration
            {
                LetterPath = pdfPath,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staffCreationId,
                CreatedBy = hrConfirmation.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            _context.LetterGenerations.Add(letterGeneration);
            await _context.SaveChangesAsync();
            return pdfPath;
        }

        private string GeneratePdf(int staffCreationId)
        {
            var staff = _context.StaffCreations.Find(staffCreationId);
            if (staff == null)
            {
                throw new Exception($"Staff with ID {staffCreationId} not found.");
            }
            var fileName = $"Letter_{staff.FirstName} {staff.LastName}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "GeneratedLetters");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, fileName);
            using (var pdfDoc = new Document(PageSize.A4))
            {
                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph($"Employee Confirmation Letter"));
                pdfDoc.Add(new Paragraph($"Name: {staff.FirstName} {staff.LastName}"));
                pdfDoc.Add(new Paragraph($"Congratulations! Your employment has been confirmed."));
                pdfDoc.Add(new Paragraph($"Effective Date: {DateTime.UtcNow.ToShortDateString()}"));
                pdfDoc.Close();
            }
            return filePath;
        }

        public async Task<string> GetPdfFilePath(int staffCreationId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);
            if (letterGeneration == null)
            {
                throw new Exception("Letter generation record not found.");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new Exception("PDF file not found.");
            }
            return filePath;
        }

        public async Task<string> GetPdfContent(int staffCreationId)
        {
            var letterGeneration = await _context.LetterGenerations.FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);
            if (letterGeneration == null)
            {
                throw new Exception("Letter generation record not found for the provided StaffCreationId.");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new Exception("Generated PDF file not found.");
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
                throw new Exception("Letter generation record not found for the provided StaffCreationId.");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new Exception("Generated PDF file not found.");
            }
            var fileBytes = File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            const string contentType = "application/pdf";
            return (fileBytes, fileName, contentType);
        }
    }
}