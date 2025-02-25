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
            var allProbation = await (from probation in _context.Probations
                                      where probation.IsActive
                                      select new ProbationResponse
                                      {
                                          ProbationId = probation.Id,
                                          StaffCreationId = probation.StaffCreationId,
                                          ProbationStartDate = probation.ProbationStartDate,
                                          ProbationEndDate = probation.ProbationEndDate,
                                          IsCompleted = probation.IsCompleted,
                                          CreatedBy = probation.CreatedBy
                                      }).ToListAsync();
            if (allProbation.Count == 0)
            {
                throw new MessageNotFoundException("No Probations found");
            }
            return allProbation;
        }

        public async Task<ProbationResponse> GetProbationByIdAsync(int probationId)
        {
            var allProbation = await (from probation in _context.Probations
                                      where probation.Id == probationId && probation.IsActive
                                      select new ProbationResponse
                                      {
                                          ProbationId = probation.Id,
                                          StaffCreationId = probation.StaffCreationId,
                                          ProbationStartDate = probation.ProbationStartDate,
                                          ProbationEndDate = probation.ProbationEndDate,
                                          IsCompleted = probation.IsCompleted,
                                          CreatedBy = probation.CreatedBy
                                      }).FirstOrDefaultAsync();
            if (allProbation == null)
            {
                throw new MessageNotFoundException("Probation not found");
            }
            return allProbation;
        }

        public async Task<string> CreateProbationAsync(ProbationRequest probationRequest)
        {
            var message = "Probation created successfully.";
            var pro = await _context.Probations.FirstOrDefaultAsync(p => p.StaffCreationId == probationRequest.StaffCreationId && p.IsActive);
            if (pro != null)
            {
                throw new Exception("Probation Details ALready Exists");
            }
            var probation = new Probation
            {
                StaffCreationId = probationRequest.StaffCreationId,
                ProbationStartDate = probationRequest.ProbationStartDate,
                ProbationEndDate = probationRequest.ProbationEndDate,
                IsCompleted = probationRequest.IsCompleted,
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
            existingProbation.StaffCreationId = probation.StaffCreationId;
            existingProbation.ProbationStartDate = probation.ProbationStartDate;
            existingProbation.ProbationEndDate = probation.ProbationEndDate;
            existingProbation.IsCompleted = probation.IsCompleted;
            existingProbation.UpdatedBy = probation.UpdatedBy;
            existingProbation.UpdatedUtc = DateTime.UtcNow;
            _context.Entry(existingProbation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel1(int approverLevel1Id)
        {
            var matchingProbations = await (from p in _context.Probations
                                      join s in _context.StaffCreations
                                      on p.StaffCreationId equals s.Id
                                      where s.ApprovalLevel1 == approverLevel1Id &&
                                            p.IsCompleted == true && p.StaffCreationId == s.Id &&
                                            p.IsActive
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffCreationId = p.StaffCreationId,
                                          ProbationStartDate = p.ProbationStartDate,
                                          ProbationEndDate = p.ProbationEndDate,
                                          IsCompleted = p.IsCompleted,
                                          CreatedBy = p.CreatedBy
                                      }).ToListAsync();
            if (matchingProbations.Count == 0)
            {
                throw new MessageNotFoundException("No Probations found");
            }
            return matchingProbations;
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

        // Get Feedback by ID with Joins
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
                    StaffCreationId = p.StaffCreationId,
                    StaffCreationName = s.FirstName + " " + s.LastName,
                    CreatedBy = f.CreatedBy
                }).FirstOrDefaultAsync();
            if (feedbackWithJoins == null)
            {
                throw new MessageNotFoundException("Feedback not found");
            }
            return feedbackWithJoins;
        }

        // Get All Feedbacks with Joins
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
                    StaffCreationId = p.StaffCreationId,
                    StaffCreationName = s.FirstName + " " + s.LastName,
                    CreatedBy = f.CreatedBy
                }).ToListAsync();

            if (feedbackList.Count == 0)
            {
                throw new MessageNotFoundException("No Feedbacks found");
            }
            return feedbackList;
        }
        // Update Feedback
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
            // Validate the Approval input
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
            // Add the Approval entry to the database
            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            // Get related Feedback and Probation details
            var feedback = await (from feedBack in _context.Feedbacks
                                  join probation in _context.Probations
                                  on feedBack.Id equals probation.Id
                                  where feedBack.Id == approval.FeedbackId && feedBack.IsActive
                                  select new FeedbackResponse
                                  {
                                      FeedbackId = feedBack.Id,
                                      ProbationId = feedBack.Id,
                                      FeedbackText = feedBack.FeedbackText,
                                      StaffCreationId = probation.StaffCreationId
                                  })
                .FirstOrDefaultAsync();
            if (feedback == null)
            {
                throw new Exception("Feedback not found.");
            }
            var pro = await _context.Probations.FirstOrDefaultAsync(f => f.Id == feedback.ProbationId && f.IsActive);
            if (pro == null)
            {
                throw new Exception("Probation not found.");
            }
            var staffCreationId = feedback.StaffCreationId;

            // Generate the PDF and get its path
            var pdfPath = GeneratePdf(staffCreationId);

            // Convert the PDF file to base64
            byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfPath);
            string base64Pdf = Convert.ToBase64String(pdfBytes);

            // Save the LetterGeneration record
            var letterGeneration = new LetterGeneration
            {
                LetterPath = pdfPath,
                LetterContent = Convert.FromBase64String(base64Pdf), // Store the base64 content as byte array
                StaffCreationId = staffCreationId,
                CreatedBy = approval.CreatedBy,
                UpdatedBy = approval.UpdatedBy,
                CreatedUtc = DateTime.UtcNow,
                IsActive = true,
            };

            _context.LetterGenerations.Add(letterGeneration);
            await _context.SaveChangesAsync();

            return pdfPath; // Return the path or base64 string if needed
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

            using (var pdfReader = new iText.Kernel.Pdf.PdfReader(filePath)) // Fully qualify PdfReader
            using (var pdfDoc = new iText.Kernel.Pdf.PdfDocument(pdfReader)) // Fully qualify PdfDocument
            {
                var textContent = new StringWriter();
                for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
                {
                    var pageContent = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                    textContent.WriteLine(pageContent);
                }

                return textContent.ToString(); // Return plain text
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
