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

namespace AttendanceManagement.Services
{
    public class ProbationService
    {
        private readonly AttendanceManagementSystemContext _context;

        public ProbationService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<List<ProbationResponse>> GetAllProbationsAsync()
        {
            var allProbation = await (from p in _context.Probations
                                      join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                      join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                      join o in _context.OrganizationTypes on d.Id equals o.Id
                                      where p.IsActive
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffId = p.StaffCreationId,
                                          StaffCreationId = $"{o.ShortName}{s.Id}",
                                          StaffName = s.FirstName + " " + s.LastName,
                                          DepartmentName = d.FullName,
                                          ProbationStartDate = p.ProbationStartDate,
                                          ProbationEndDate = p.ProbationEndDate,
                                          IsCompleted = p.IsCompleted,
                                          CreatedBy = p.CreatedBy
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
                                   join o in _context.OrganizationTypes on d.Id equals o.Id
                                   where p.Id == probationId && p.IsActive
                                   select new ProbationResponse
                                   {
                                       ProbationId = p.Id,
                                       StaffId = p.StaffCreationId,
                                       StaffCreationId = $"{o.ShortName}{s.Id}",
                                       StaffName = s.FirstName + " " + s.LastName,
                                       DepartmentName = d.FullName,
                                       ProbationStartDate = p.ProbationStartDate,
                                       ProbationEndDate = p.ProbationEndDate,
                                       IsCompleted = p.IsCompleted,
                                       CreatedBy = p.CreatedBy
                                   }).FirstOrDefaultAsync();

            if (probation == null)
            {
                throw new MessageNotFoundException("Probation not found");
            }

            return probation;
        }

        public async Task<string> CreateProbationAsync(ProbationRequest probationRequest)
        {
            var message = "Probation created successfully.";
            var pro = await _context.Probations.FirstOrDefaultAsync(p => p.StaffCreationId == probationRequest.StaffId && p.IsActive);
            if (pro != null)
            {
                throw new Exception("Probation Details ALready Exists");
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
        public async Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel1(int approverLevel1Id)
        {
            var matchingProbations = await (from p in _context.Probations
                                            join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                            join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                            join o in _context.OrganizationTypes on d.Id equals o.Id
                                            where s.ApprovalLevel1 == approverLevel1Id &&
                                                  p.StaffCreationId == s.Id &&
                                                  p.IsActive
                                            select new ProbationResponse
                                            {
                                                ProbationId = p.Id,
                                                StaffId = p.StaffCreationId,
                                                StaffCreationId = $"{o.ShortName}{s.Id}",
                                                StaffName = s.FirstName + " " + s.LastName,
                                                DepartmentName = d.FullName,
                                                ProbationStartDate = p.ProbationStartDate,
                                                ProbationEndDate = p.ProbationEndDate,
                                                IsCompleted = p.IsCompleted,
                                                CreatedBy = p.CreatedBy
                                            }).ToListAsync();

            if (!matchingProbations.Any())
            {
                throw new MessageNotFoundException("No Probations found");
            }

            return matchingProbations;
        }
        public async Task<List<FeedbackResponse>> GetFeedbackDetailsByApproverLevel1(int approverLevel1Id)
        {
            var feedbackWithJoins = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.ProbationId equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                join o in _context.OrganizationTypes on s.OrganizationTypeId equals o.Id
                where s.ApprovalLevel1 == approverLevel1Id && f.IsActive
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = p.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = $"{o.ShortName}{s.Id}",
                    StaffCreationName = s.FirstName + " " + s.LastName,
                    IsCompleted = p.IsCompleted,
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
            var message = "Feedback added successfully.";
            var feedback = new Feedback
            {
                ProbationId = feedbackRequest.ProbationId,
                FeedbackText = feedbackRequest.FeedbackText,
                CreatedBy = feedbackRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<FeedbackResponse> GetFeedbackByIdAsync(int feedbackId)
        {
            var feedbackWithJoins = await (
                from f in _context.Feedbacks
                join p in _context.Probations on f.Id equals p.Id
                join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                join o in _context.OrganizationTypes on s.OrganizationTypeId equals o.Id
                where f.Id == feedbackId && f.IsActive
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = f.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = $"{o.ShortName}{s.Id}",
                    StaffCreationName = s.FirstName + " " + s.LastName,
                    IsCompleted = p.IsCompleted,
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
                join o in _context.OrganizationTypes on s.OrganizationTypeId equals o.Id
                where f.IsActive
                select new FeedbackResponse
                {
                    FeedbackId = f.Id,
                    ProbationId = f.Id,
                    FeedbackText = f.FeedbackText,
                    StaffId = p.StaffCreationId,
                    StaffCreationId = $"{o.ShortName}{s.Id}",
                    StaffCreationName = s.FirstName + " " + s.LastName,
                    IsCompleted = p.IsCompleted,
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
            if (feedback == null || !feedback.IsActive)
                throw new MessageNotFoundException("Feedback not found");

            feedback.Id = updatedFeedback.ProbationId;
            feedback.FeedbackText = updatedFeedback.FeedbackText;
            feedback.UpdatedBy = updatedFeedback.UpdatedBy;
            feedback.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<string> ProcessApprovalAsync(ApprovalRequest approvalRequest)
        {
            var staff = _context.StaffCreations.Where(s => s.Id == approvalRequest.CreatedBy && s.IsActive == true).Select(s => $"{s.FirstName}{s.LastName}").FirstOrDefault();
            string approvedDateTime = DateTime.Now.ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            if (approvalRequest.IsApproved != true)
            {
                throw new Exception("Approval is not marked as approved.");
            }

            var approval = new Approval
            {
                FeedbackId = approvalRequest.FeedbackId,
                IsApproved = approvalRequest.IsApproved,
                ApprovalComment = approvalRequest.ApprovalComment,
                CreatedBy = approvalRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true
            };

            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            var feedback = await (from feedBack in _context.Feedbacks
                                  join probations in _context.Probations
                                  on feedBack.ProbationId equals probations.Id
                                  join s in _context.StaffCreations
                                  on probations.StaffCreationId equals s.Id
                                  join o in _context.OrganizationTypes
                                  on s.OrganizationTypeId equals o.Id
                                  where feedBack.Id == approval.FeedbackId && feedBack.IsActive
                                  select new FeedbackResponse
                                  {
                                      FeedbackId = feedBack.Id,
                                      ProbationId = probations.Id,
                                      FeedbackText = feedBack.FeedbackText,
                                      StaffId = probations.StaffCreationId,
                                      StaffCreationId = $"{o.ShortName}{s.Id}",
                                  })
                       .FirstOrDefaultAsync();

            if (feedback == null)
            {
                throw new MessageNotFoundException("Feedback not found");
            }

            var probation = await _context.Probations.FirstOrDefaultAsync(p => p.Id == feedback.ProbationId && p.IsActive);
            if (probation == null)
            {
                throw new Exception("Probation not found.");
            }
            probation.IsCompleted = true;
            probation.IsActive = false;
            _context.Probations.Update(probation);
            await _context.SaveChangesAsync();

            var notification = new ApprovalNotification
            {
                StaffId = probation.StaffCreationId,
                Message =  $"Your Probation request has been approved. Approved by - {staff} on {approvedDateTime}",
                IsActive = true,
                CreatedBy = approvalRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.ApprovalNotifications.Add(notification);
            await _context.SaveChangesAsync();

            probation.ApprovalNotificationId = notification.Id;
            await _context.SaveChangesAsync();

            var staffCreationId = feedback.StaffId;

            var pdfPath = GeneratePdf(staffCreationId);

            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            string base64Pdf = Convert.ToBase64String(pdfBytes);

            var letterGeneration = new LetterGeneration
            {
                LetterPath = pdfPath,
                LetterContent = Convert.FromBase64String(base64Pdf),
                StaffCreationId = staffCreationId,
                CreatedBy = approval.CreatedBy,
                UpdatedBy = approval.UpdatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true,
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
            var letterGeneration = await _context.LetterGenerations
                .FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);

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
            var letterGeneration = await _context.LetterGenerations
                .FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);

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
            var letterGeneration = await _context.LetterGenerations
                .FirstOrDefaultAsync(lg => lg.StaffCreationId == staffCreationId && lg.IsActive);

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
