using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class EmploymentHistoryService
    {
        private readonly AttendanceManagementSystemContext _context;

        public EmploymentHistoryService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        // Get All Employment Histories
        public async Task<List<EmploymentHistoryResponseModel>> GetAllAsync()
        {
            var employmentHistory = await _context.EmploymentHistories
                .Select(eh => new EmploymentHistoryResponseModel
                {
                    EmployeeHistoryId = eh.Id,
                    StaffCreationId = eh.StaffCreationId,
                    CompanyName = eh.CompanyName,
                    JobTitle = eh.JobTitle,
                    StartDate = eh.StartDate,
                    EndDate = eh.EndDate,
                    LastDrawnSalary = eh.LastDrawnSalary,
                    JobLocation = eh.JobLocation,
                    EmploymentType = eh.EmploymentType,
                    ReasonForLeaving = eh.ReasonForLeaving,
                    ReferenceContact = eh.ReferenceContact
                })
                .ToListAsync();
            if(employmentHistory.Count == 0) throw new MessageNotFoundException("No employment history found.");
            return employmentHistory;
        }

        // Get By Id
        public async Task<EmploymentHistoryResponseModel> GetByIdAsync(int employementHistoryId)
        {
            var employmentHistory = await _context.EmploymentHistories
                .Where(eh => eh.Id == employementHistoryId && eh.IsActive)
                .Select(eh => new EmploymentHistoryResponseModel
                {
                    EmployeeHistoryId = eh.Id,
                    StaffCreationId = eh.StaffCreationId,
                    CompanyName = eh.CompanyName,
                    JobTitle = eh.JobTitle,
                    StartDate = eh.StartDate,
                    EndDate = eh.EndDate,
                    LastDrawnSalary = eh.LastDrawnSalary,
                    JobLocation = eh.JobLocation,
                    EmploymentType = eh.EmploymentType,
                    ReasonForLeaving = eh.ReasonForLeaving,
                    ReferenceContact = eh.ReferenceContact
                })
                .FirstOrDefaultAsync();
            if(employmentHistory == null) throw new MessageNotFoundException("Employment history not found.");
            return employmentHistory;
        }

        // Create Employment History
        public async Task<string> CreateAsync(EmploymentHistoryRequestModel model)
        {
            var message = "Employment history created successfully.";
            var newEmploymentHistory = new EmploymentHistory
            {

                CompanyName = model.CompanyName,
                JobTitle = model.JobTitle,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                LastDrawnSalary = model.LastDrawnSalary,
                JobLocation = model.JobLocation,
                EmploymentType = model.EmploymentType,
                ReasonForLeaving = model.ReasonForLeaving,
                ReferenceContact = model.ReferenceContact,
                StaffCreationId = model.StaffCreationId,
                IsActive = true,
                CreatedBy = model.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.EmploymentHistories.Add(newEmploymentHistory);
            await _context.SaveChangesAsync();
            return message;
        }

        // Update Employment History
        public async Task<string> UpdateAsync(EmploymentHistoryUpdateModel model)
        {
            var message = "Employment history updated successfully.";
            var employmentHistory = await _context.EmploymentHistories.FirstOrDefaultAsync(lg => lg.Id == model.EmployeeHistoryId && lg.IsActive);
            if (employmentHistory == null) throw new MessageNotFoundException("Employment history not found.");

            employmentHistory.CompanyName = model.CompanyName;
            employmentHistory.JobTitle = model.JobTitle;
            employmentHistory.StartDate = model.StartDate;
            employmentHistory.EndDate = model.EndDate;
            employmentHistory.StaffCreationId = model.StaffCreationId;
            employmentHistory.LastDrawnSalary = model.LastDrawnSalary;
            employmentHistory.JobLocation = model.JobLocation;
            employmentHistory.EmploymentType = model.EmploymentType;
            employmentHistory.ReasonForLeaving = model.ReasonForLeaving;
            employmentHistory.ReferenceContact = model.ReferenceContact;
            employmentHistory.IsActive = model.IsActive;
            employmentHistory.UpdatedBy = model.UpdatedBy;
            employmentHistory.UpdatedUtc = DateTime.UtcNow;

            _context.EmploymentHistories.Update(employmentHistory);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
