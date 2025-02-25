using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class EducationalQualificationService
    {
        private readonly AttendanceManagementSystemContext _context;

        public EducationalQualificationService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EducationalQualificationResponse>> GetAllEducationalQualifications()
        {
            var qualifications = await _context.EducationalQualifications
                .Select(q => new EducationalQualificationResponse
                {
                    EducationalQualificationId = q.Id,
                    StaffCreationId = q.StaffCreationId,
                    Qualification = q.Qualification,
                    Specilization = q.Specilization,
                    University = q.University,
                    Institute = q.Institute,
                    MediumOfInstruction = q.MediumOfInstruction,
                    CourseType = q.CourseType,
                    YearOfPassing = q.YearOfPassing,
                    CourseAppraisal = q.CourseAppraisal,
                    Score = q.Score,
                    OutOf = q.OutOf,
                    IsActive = q.IsActive,
                    CreatedBy = q.CreatedBy
                })
                .ToListAsync();

            if (qualifications.Count == 0)
            {
                throw new MessageNotFoundException("No educational qualifications found");
            }
            return qualifications;
        }

        public async Task<EducationalQualificationResponse> GetEducationalQualificationById(int educationalQualificationId)
        {
            var qualification = await _context.EducationalQualifications
                .Where(q => q.Id == educationalQualificationId)
                .Select(q => new EducationalQualificationResponse
                {
                    EducationalQualificationId = q.Id,
                    StaffCreationId = q.StaffCreationId,
                    Qualification = q.Qualification,
                    Specilization = q.Specilization,
                    University = q.University,
                    Institute = q.Institute,
                    MediumOfInstruction = q.MediumOfInstruction,
                    CourseType = q.CourseType,
                    YearOfPassing = q.YearOfPassing,
                    CourseAppraisal = q.CourseAppraisal,
                    Score = q.Score,
                    OutOf = q.OutOf,
                    IsActive = q.IsActive,
                    CreatedBy = q.CreatedBy
                })
                .FirstOrDefaultAsync();

            if (qualification == null)
            {
                throw new MessageNotFoundException("Educational qualification not found");
            }
            return qualification;
        }

        public async Task<string> CreateEducationalQualification(EducationalQualificationDto qualificationDto)
        {
            var message = "Educational qualification added successfully.";
            if (qualificationDto == null)
            {
                throw new ArgumentNullException(nameof(qualificationDto), "Educational qualification details cannot be null.");
            }

            var qualification = new EducationalQualification
            {
                StaffCreationId = qualificationDto.StaffCreationId,
                Qualification = qualificationDto.Qualification,
                Specilization = qualificationDto.Specilization,
                University = qualificationDto.University,
                Institute = qualificationDto.Institute,
                MediumOfInstruction = qualificationDto.MediumOfInstruction,
                CourseType = qualificationDto.CourseType,
                YearOfPassing = qualificationDto.YearOfPassing,
                CourseAppraisal = qualificationDto.CourseAppraisal,
                Score = qualificationDto.Score,
                OutOf = qualificationDto.OutOf,
                IsActive = qualificationDto.IsActive,
                CreatedBy = qualificationDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.EducationalQualifications.Add(qualification);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<string> UpdateEducationalQualification(UpdateEducationalQualification qualificationDto)
        {
            var message = "Educational qualification updated successfully.";
            if (qualificationDto == null)
            {
                throw new ArgumentNullException(nameof(qualificationDto), "Educational qualification details cannot be null.");
            }

            var existingQualification = await _context.EducationalQualifications.FirstOrDefaultAsync(q => q.Id == qualificationDto.EducationalQualificationId);
            if (existingQualification == null)
            {
                throw new MessageNotFoundException("Educational qualification not found");
            }

            existingQualification.StaffCreationId = qualificationDto.StaffCreationId;
            existingQualification.Qualification = qualificationDto.Qualification;
            existingQualification.Specilization = qualificationDto.Specilization;
            existingQualification.University = qualificationDto.University;
            existingQualification.Institute = qualificationDto.Institute;
            existingQualification.MediumOfInstruction = qualificationDto.MediumOfInstruction;
            existingQualification.CourseType = qualificationDto.CourseType;
            existingQualification.YearOfPassing = qualificationDto.YearOfPassing;
            existingQualification.CourseAppraisal = qualificationDto.CourseAppraisal;
            existingQualification.Score = qualificationDto.Score;
            existingQualification.OutOf = qualificationDto.OutOf;
            existingQualification.UpdatedBy = qualificationDto.UpdatedBy;
            existingQualification.UpdatedUtc = DateTime.UtcNow;

            _context.Update(existingQualification);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}