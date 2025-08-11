using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Infrastructure.Data;
using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class ProbationInfra : IProbationInfra
    {
        private readonly AttendanceManagementSystemContext _context;
        private readonly IEmailInfra _emailService;
        private readonly string _workspacePath;
        private readonly ILetterGenerationInfra _letterGenerationService;
        public ProbationInfra(AttendanceManagementSystemContext context, IEmailInfra emailService, IWebHostEnvironment env, ILetterGenerationInfra letterGenerationService)
        {
            _context = context;
            _emailService = emailService;
            _workspacePath = Path.Combine(env.ContentRootPath, "wwwroot\\GeneratedLetters\\ConfirmationLetter");
            if (!Directory.Exists(_workspacePath))
            {
                Directory.CreateDirectory(_workspacePath);
            }
            _letterGenerationService = letterGenerationService;
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
                where p.IsActive && s.IsActive == true && (p.ProbationEndDate <= DateOnly.FromDateTime(DateTime.UtcNow) || latestFeedback.ExtensionPeriod <= DateOnly.FromDateTime(DateTime.UtcNow))
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
                    IsAssigned = p.IsAssigned,
                    ManagerId = p.ManagerId,
                    IsApproved = latestFeedback.IsApproved,
                    FeedBackText = latestFeedback.FeedbackText,
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
            var approvalLevel1Ids = await _context.StaffCreations
                .Where(s => s.IsActive == true)
                .Select(s => s.ApprovalLevel1)
                .Distinct()
                .ToListAsync();
            var managers = await _context.StaffCreations
                .Where(m => approvalLevel1Ids.Contains(m.Id) && m.IsActive == true)
                .Select(m => new
                {
                    Id = m.Id,
                    StaffId = m.StaffId,
                    Name = $"{m.FirstName}{(string.IsNullOrWhiteSpace(m.LastName) ? "" : " " + m.LastName)}",
                    OfficialMail = m.OfficialEmail
                })
                .ToListAsync();
            if (managers.Count == 0) throw new MessageNotFoundException("No Managers found");

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
            //if (probation.ManagerId != null) throw new ConflictException("Manager already assigned");
            var feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.ProbationId == probation.Id && f.IsActive);
            var effectiveEndDate = feedback?.ExtensionPeriod ?? probation.ProbationEndDate;
            probation.IsAssigned = true;
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
            if (pro) throw new ConflictException("Probation already exists");
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

        public async Task<List<ProbationReportResponse>> GetProbationReportsByApproverLevel(int approverLevel, int year, int month)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverLevel && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";
            var matchingProbations = await (from p in _context.Probations
                                            join f in _context.Feedbacks on p.Id equals f.ProbationId into feedbackGroup
                                            from feedback in feedbackGroup.DefaultIfEmpty()
                                            join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                            join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                            join r in _context.ProbationReports on s.StaffId equals r.EmpId into reportGroup
                                            from report in reportGroup.DefaultIfEmpty()
                                            let latestFeedback = _context.Feedbacks
                                            .Where(f => f.ProbationId == p.Id && f.IsActive)
                                            .OrderByDescending(f => f.Id)
                                            .FirstOrDefault()
                                            where s.IsActive == true && d.IsActive && report.ProductivityYear == year && report.Month == month && (isSuperAdmin || p.ManagerId == approverLevel) && (feedback.IsApproved == null || feedback.IsApproved == false)
                                            select new ProbationReportResponse
                                            {
                                                EmpId = s.StaffId,
                                                Name = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                                Department = d.Name,
                                                ProdScore = report.ProdScore,
                                                ProdPercentage = report.ProdPercentage,
                                                ProdGrade = report.ProdGrade,
                                                QualityScore = report.QualityScore,
                                                QualityPercentage = report.QualityPercentage,
                                                NoOfAbsent = report.NoOfAbsent,
                                                AttendanceScore = report.AttendanceScore,
                                                AttendancePercentage = report.AttendancePercentage,
                                                AttendanceGrade = report.AttendanceGrade,
                                                FinalTotal = report.FinalTotal,
                                                TotalScore = report.TotalScore,
                                                FinalScorePercentage = report.FinalScorePercentage,
                                                FinalGrade = report.FinalGrade,
                                                ProductionAchievedPercentageJan = report.ProductionAchievedPercentageJan,
                                                ProductionAchievedPercentageFeb = report.ProductionAchievedPercentageFeb,
                                                ProductionAchievedPercentageMar = report.ProductionAchievedPercentageMar,
                                                ProductionAchievedPercentageApr = report.ProductionAchievedPercentageApr,
                                                ProductionAchievedPercentageMay = report.ProductionAchievedPercentageMay,
                                                ProductionAchievedPercentageJun = report.ProductionAchievedPercentageJun,
                                                ProductionAchievedPercentageJul = report.ProductionAchievedPercentageJul,
                                                ProductionAchievedPercentageAug = report.ProductionAchievedPercentageAug,
                                                ProductionAchievedPercentageSep = report.ProductionAchievedPercentageSep,
                                                ProductionAchievedPercentageOct = report.ProductionAchievedPercentageOct,
                                                ProductionAchievedPercentageNov = report.ProductionAchievedPercentageNov,
                                                ProductionAchievedPercentageDec = report.ProductionAchievedPercentageDec,
                                                ProductivityYear = report.ProductivityYear
                                            }).ToListAsync();
            if (!matchingProbations.Any())
            {
                throw new MessageNotFoundException("No Probation reports found");
            }
            return matchingProbations;
        }
        public async Task<List<ProbationResponse>> GetProbationDetailsByApproverLevel(int approverLevelId, int year, int month)
        {
            var approver = await _context.StaffCreations
                .Where(x => x.Id == approverLevelId && x.IsActive == true)
                .Select(x => x.AccessLevel)
                .FirstOrDefaultAsync();
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";

            var matchingProbations = await (from p in _context.Probations
                                      join s in _context.StaffCreations on p.StaffCreationId equals s.Id
                                      join d in _context.DepartmentMasters on s.DepartmentId equals d.Id
                                      join r in _context.ProbationReports on s.StaffId equals r.EmpId into reportGroup
                                      from report in reportGroup.DefaultIfEmpty()
                                      let latestFeedback = _context.Feedbacks
                                      .Where(f => f.ProbationId == p.Id && f.IsActive)
                                      .OrderByDescending(f => f.Id)
                                      .FirstOrDefault()
                                      where p.IsActive && s.IsActive == true && d.IsActive && ((p.ProbationEndDate.Month == month && p.ProbationEndDate.Year == year) || latestFeedback.ExtensionPeriod.HasValue && latestFeedback.ExtensionPeriod.Value.Month == month && latestFeedback.ExtensionPeriod.Value.Year == year) && (isSuperAdmin || p.ManagerId == approverLevelId) && (p.ProbationEndDate <= DateOnly.FromDateTime(DateTime.UtcNow) || latestFeedback.ExtensionPeriod <= DateOnly.FromDateTime(DateTime.UtcNow))
                                      select new ProbationResponse
                                      {
                                          ProbationId = p.Id,
                                          StaffId = p.StaffCreationId,
                                          StaffCreationId = s.StaffId,
                                          StaffName = $"{s.FirstName}{(string.IsNullOrWhiteSpace(s.LastName) ? "" : " " + s.LastName)}",
                                          DepartmentName = d.Name,
                                          ProbationStartDate = p.ProbationStartDate,
                                          ProbationEndDate = p.ProbationEndDate,
                                          FeedBackText = latestFeedback.FeedbackText,
                                          IsApproved = latestFeedback.IsApproved,
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
            bool isSuperAdmin = approver == "SUPER ADMIN" || approver == "HR ADMIN";

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
/*                await _emailService.SendProbationConfirmationNotificationToStaffAsync
                (approver.Id, $"{probationer.FirstName}{(string.IsNullOrWhiteSpace(probationer.LastName) ? "" : " " + probationer.LastName)}", probation.ProbationStartDate,
                probation.ProbationEndDate, null, feedbackRequest.IsApproved, $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}",
                approvedTime, feedbackRequest.CreatedBy);
*/            }
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
/*                await _emailService.SendProbationConfirmationNotificationToStaffAsync
                (approver.Id, $"{probationer.FirstName}{(string.IsNullOrWhiteSpace(probationer.LastName) ? "" : " " + probationer.LastName)}", probation.ProbationStartDate,
                probation.ProbationEndDate, null, feedbackRequest.IsApproved, $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}",
                approvedTime, feedbackRequest.CreatedBy);
*/            }
            await _context.SaveChangesAsync();
            var approvedDateTime = DateTime.UtcNow.ToLocalTime().ToString("dd-MMM-yyyy 'at' HH:mm:ss");
            var approverName = $"{approver.FirstName}{(string.IsNullOrWhiteSpace(approver.LastName) ? "" : " " + approver.LastName)}";

            var notificationMessage = feedbackRequest.IsApproved
                ? $"Your probation has been approved by {approverName} on {approvedDateTime}."
                : $"Your probation period has been extended by {approverName} on {approvedDateTime}. Extended date: {feedbackRequest.ExtensionPeriod}.";

            var notification = new ApprovalNotification
            {
                StaffId = probation.StaffCreationId,
                Message = notificationMessage,
                IsActive = true,
                CreatedBy = feedbackRequest.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            await _context.ApprovalNotifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            probation.ApprovalNotificationId = notification.Id;
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
            var staffs = _context.StaffCreations.FirstOrDefault(s => s.Id == staffCreationId && s.IsActive == true);
            if (staffs == null)
            {
                throw new MessageNotFoundException($"Staff with ID {staffCreationId} not found.");
            }
            var name = $"{staffs.FirstName}{(string.IsNullOrWhiteSpace(staffs.LastName) ? "" : " " + staffs.LastName)}";
            var pdfPath = _letterGenerationService.GenerateConfirmationPdf(staffCreationId, designation.Name, staffId.Title, startDate, endDate, fileName, name, staffs.StaffId);
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
            //await _emailService.ProbationProcessCompleted(staffCreationId, name, hrConfirmation.CreatedBy);
            return pdfPath;
        }

        public async Task<List<GeneratedLetterResponse>> GetGeneratedLetters(int staffId)
        {
            var letterGenerations = await (from letter in _context.LetterGenerations
                                           join staff in _context.StaffCreations on letter.StaffCreationId equals staff.Id
                                           where letter.StaffCreationId == staffId && letter.CreatedUtc.Year == DateTime.UtcNow.Year && letter.IsActive
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
                throw new MessageNotFoundException("Letter found");
            }
            return letterGenerations;
        }

        public async Task<string> GetPdfFilePath(int staffCreationId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Confirmation_Letter_{staff.StaffId}_";
            var letterGeneration = await _context.LetterGenerations
                .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                .OrderByDescending(x => x.CreatedUtc)
                .FirstOrDefaultAsync();
            if (letterGeneration == null)
            {
                throw new MessageNotFoundException("Letter not found");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new MessageNotFoundException("Letter not found");
            }
            string file = letterGeneration.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("Letter not found");
            }
            return Path.Combine(_workspacePath, file);
        }

        public async Task<(Stream PdfStream, string FileName)> GetPdfContent(int staffCreationId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Confirmation_Letter_{staff.StaffId}_";
            var letterGeneration = await _context.LetterGenerations
                 .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                 .OrderByDescending(x => x.CreatedUtc)
                 .FirstOrDefaultAsync();

            if (letterGeneration == null)
            {
                throw new FileNotFoundException("Letter not found");
            }

            var filePath = letterGeneration.LetterPath;
            if (string.IsNullOrWhiteSpace(filePath) || !System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("Letter not found");
            }

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = Path.GetFileName(filePath);
            return (stream, fileName);
        }

        public async Task<string> DownloadPdf(int staffCreationId)
        {
            var staff = await _context.StaffCreations.FirstOrDefaultAsync(s => s.Id == staffCreationId && s.IsActive == true);
            if (staff == null)
            {
                throw new MessageNotFoundException("Staff not found");
            }
            string filePrefix = $"Confirmation_Letter_{staff.StaffId}_";
            var letterGeneration = await _context.LetterGenerations
                .Where(x => x.FileName.StartsWith(filePrefix) && x.IsActive)
                .OrderByDescending(x => x.CreatedUtc)
                .FirstOrDefaultAsync();
            if (letterGeneration == null)
            {
                throw new MessageNotFoundException("Letter not found");
            }
            var filePath = letterGeneration.LetterPath;
            if (!File.Exists(filePath))
            {
                throw new MessageNotFoundException("Letter not found");
            }
            string file = letterGeneration.FileName ?? string.Empty;
            if (string.IsNullOrWhiteSpace(file))
            {
                throw new MessageNotFoundException("Letter not found");
            }
            return Path.Combine(_workspacePath, file);
        }
    }
}