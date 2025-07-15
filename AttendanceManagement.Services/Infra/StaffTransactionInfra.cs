using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Infrastructure;
using AttendanceManagement.Domain.Entities.Attendance;
using AttendanceManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Infrastructure.Infra
{
    public class StaffTransactionInfra : IStaffTransactionInfra
    {
        private readonly AttendanceManagementSystemContext _context;

        public StaffTransactionInfra(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<AcademicDetailResponse>> GetByStaffIdAsync(int staffId)
        {
            var academicDetails = await _context.AcademicDetails
                .Where(a => a.IsActive == true && a.Id == staffId)
                .ToListAsync();
            if (!academicDetails.Any()) return new List<AcademicDetailResponse>();
            var result = academicDetails.Select(academicDetail => new AcademicDetailResponse
            {
                Id = academicDetail.Id,
                StaffId = academicDetail.StaffId,
                Qualification = academicDetail.Qualification,
                Specialization= academicDetail.Specialization,
                Institution = academicDetail.Institute,
                University = academicDetail.University,
                MediumOfInstruction = academicDetail.MediumOfInstruction,
                CourseType = academicDetail.CourseType,
                YearOfPassing = academicDetail.YearOfPassing,
                CourseOfAppraisal = academicDetail.CourseOfAppraisal,
                Board = academicDetail.Board
            }).ToList();
            if (result.Count == 0) throw new MessageNotFoundException("No academic details found");
            return result;
        }

        private async Task StaffFoundMethod(string staffId)
        {
            var staff = await _context.StaffCreations.AnyAsync(p => p.StaffId == staffId && p.IsActive == true);
            if (!staff) throw new MessageNotFoundException("Staff not found");
        }

        public async Task<string> CreateAsync(ListAcademicDetailRequest academicDetailRequests)
        {
            await StaffFoundMethod(academicDetailRequests.StaffId);
            foreach (var request in academicDetailRequests.AcademicDetails)
            {
                var academicDetail = new AcademicDetail
                {
                    Qualification = request.Qualification,
                    Specialization = request.Specialization,
                    University = request.University,
                    Institute = request.Institution,
                    MediumOfInstruction = request.MediumOfInstruction,
                    CourseType = request.CourseType,
                    YearOfPassing = request.YearOfPassing,
                    CourseOfAppraisal = request.CourseOfAppraisal,
                    Board = request.Board,
                    StaffId = academicDetailRequests.StaffId,
                    CreatedBy = academicDetailRequests.CreatedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsActive = true
                };
                await _context.AcademicDetails.AddRangeAsync(academicDetail);
                await _context.SaveChangesAsync();
            }
            return "Academic records added successfully.";
        }

        public async Task<string> UpdateAsync(ListAcademicDetailUpdateRequest academicDetailsRequests)
        {
            var academicDetailIds = academicDetailsRequests.AcademicDetails.Select(a => a.AcademicDetailId).ToList();
            var existingRecords = _context.AcademicDetails.Where(b => academicDetailIds.Contains(b.Id)).ToList();
            foreach (var academicDetail in academicDetailsRequests.AcademicDetails)
            {
                var existing = existingRecords.FirstOrDefault(b => b.Id == academicDetail.AcademicDetailId);
                if (existing != null)
                {
                    existing.Qualification = academicDetail.Qualification;
                    existing.Specialization = academicDetail.Specialization;
                    existing.University = academicDetail.University;
                    existing.Institute = academicDetail.Institution;
                    existing.MediumOfInstruction = academicDetail.MediumOfInstruction;
                    existing.CourseType = academicDetail.CourseType;
                    existing.YearOfPassing = academicDetail.YearOfPassing;
                    existing.CourseOfAppraisal = academicDetail.CourseOfAppraisal;
                    existing.Board = academicDetail.Board;
                    existing.UpdatedBy = academicDetailsRequests.UpdatedBy;
                    existing.UpdatedUtc = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return $"{existingRecords.Count} academic records updated successfully.";
        }

        public async Task<string> DeleteAcademicDetailAsync(int academicDetailId, int deletedBy)
        {
            var message = " Details Deleted Successfully;";
            var academicDetail = await _context.AcademicDetails.FirstOrDefaultAsync(a => a.Id == academicDetailId && a.IsActive == true);
            if (academicDetail == null) throw new MessageNotFoundException("No academic details found");
            academicDetail.IsActive = false;
            academicDetail.UpdatedBy = deletedBy;
            academicDetail.UpdatedUtc = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<CertificationCourseResponse>> GetByCerticateStaffIdAsync(int staffId)
        {
            var certificationCourses = await _context.CertificationCourses
                .Where(c => c.IsActive == true && c.Id == staffId)
                .ToListAsync();
            if (!certificationCourses.Any()) return new List<CertificationCourseResponse>();
            var result = certificationCourses.Select(certificationCourse => new CertificationCourseResponse
            {
                Id = certificationCourse.Id,
                StaffId = certificationCourse.StaffId,
                CertificationCourseName = certificationCourse.Course,
                ValidUpto = certificationCourse.ValidUpto,
                CourseAppraisal = certificationCourse.CourseAppraisal,
                CertificationInstitute = certificationCourse.CertificationInstitute,
            }).ToList();
            if (result.Count == 0) throw new MessageNotFoundException("No certificate course found");
            return result;
        }

        public async Task<string> CreateAsync(ListCertificationCourseRequest certificationCourseRequests)
        {
            await StaffFoundMethod(certificationCourseRequests.StaffId);
            foreach (var request in certificationCourseRequests.CertificationCourses)
            {
                var certificationCourse = new CertificationCourse
                {
                    StaffId = certificationCourseRequests.StaffId,
                    Course = request.CertificationCourseName,
                    ValidUpto = request.ValidUpto,
                    CourseAppraisal = request.CourseAppraisal,
                    CertificationInstitute = request.CertificationInstitute,
                    CreatedBy = certificationCourseRequests.CreatedBy, 
                    CreatedUtc = DateTime.UtcNow,
                    IsActive = true
                };
                await _context.CertificationCourses.AddAsync(certificationCourse);
                await _context.SaveChangesAsync();
            }
            return "Certification courses added successfully.";
        }

        public async Task<string> UpdateAsync(ListCertificationCourseUpdateRequest certificationCourseRequests)
        {
            var ids = certificationCourseRequests.CertificationCourses.Select(c => c.Id).ToList();
            var existingCourses = await _context.CertificationCourses.Where(c => ids.Contains(c.Id)).ToListAsync();
            foreach (var courseRequest in certificationCourseRequests.CertificationCourses)
            {
                var existing = existingCourses.FirstOrDefault(c => c.Id == courseRequest.Id);
                if (existing != null)
                {
                    existing.Course = courseRequest.CertificationCourseName;
                    existing.ValidUpto = courseRequest.ValidUpto;
                    existing.CourseAppraisal = courseRequest.CourseAppraisal;
                    existing.CertificationInstitute = courseRequest.CertificationInstitute;
                    existing.UpdatedBy = certificationCourseRequests.UpdatedBy;
                    existing.UpdatedUtc = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return $"{existingCourses.Count} certification courses updated successfully.";
        }

        public async Task<string> DeleteCertificationCourseAsync(int certificationCourseId, int deletedBy)
        {
            var message = "Certification course deleted successfully";
            var certificationCourse = await _context.CertificationCourses.FirstOrDefaultAsync(c => c.Id == certificationCourseId && c.IsActive == true);
            if (certificationCourse == null) throw new MessageNotFoundException("Certification course not found");
            certificationCourse.IsActive = false;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<PreviousEmploymentResponse>> GetWorkhistoryByStaffIdAsync(int staffId)
        {
            var previousEmployments = await _context.PreviousEmployments
                .Where(pe => pe.Id == staffId && pe.IsActive)
                .ToListAsync();
            if (!previousEmployments.Any()) return new List<PreviousEmploymentResponse>();
            return previousEmployments.Select(previousEmployment => new PreviousEmploymentResponse
            {
                Id = previousEmployment.Id,
                StaffId = previousEmployment.StaffId,
                CompanyName = previousEmployment.CompanyName,
                FromDate = previousEmployment.FromDate,
                ToDate = previousEmployment.ToDate,
                PreviousLocation = previousEmployment.PreviousLocation,
                FunctionalArea = previousEmployment.FunctionalArea,
                LastGrossSalary = previousEmployment.LastGrossSalary
            }).ToList();
        } 

        public async Task<string> CreateAsync(ListPreviousEmploymentRequest previousEmploymentRequests)
        {
            await StaffFoundMethod(previousEmploymentRequests.StaffId);
            foreach (var request in previousEmploymentRequests.PreviousEmployments)
            {
                var previousEmployment = new PreviousEmployment
                {
                    StaffId = previousEmploymentRequests.StaffId,
                    CompanyName = request.CompanyName,
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    PreviousLocation = request.PreviousLocation,
                    FunctionalArea = request.FunctionalArea,
                    LastGrossSalary = request.LastGrossSalary,
                    CreatedBy = previousEmploymentRequests.CreatedBy,
                    CreatedUtc = DateTime.UtcNow,
                    IsActive = true
                };
                await _context.PreviousEmployments.AddAsync(previousEmployment);
                await _context.SaveChangesAsync();
            }
            return "Previous employment records added successfully.";
        }

        public async Task<string> UpdateAsync(ListPreviousEmploymentUpdateRequest previousEmploymentUpdateRequest)
        {
            var ids = previousEmploymentUpdateRequest.PreviousEmployments.Select(r => r.Id).ToList();
            var existingRecords = await _context.PreviousEmployments
                .Where(pe => ids.Contains(pe.Id))
                .ToListAsync();
            if (!existingRecords.Any()) throw new MessageNotFoundException("No matching records found for update.");
            foreach (var request in previousEmploymentUpdateRequest.PreviousEmployments)
            {
                var existing = existingRecords.FirstOrDefault(pe => pe.Id == request.Id);
                if (existing != null)
                {
                    existing.CompanyName = request.CompanyName;
                    existing.FromDate = request.FromDate;
                    existing.ToDate = request.ToDate;
                    existing.PreviousLocation = request.PreviousLocation;
                    existing.FunctionalArea = request.FunctionalArea;
                    existing.LastGrossSalary = request.LastGrossSalary;
                    existing.UpdatedBy = previousEmploymentUpdateRequest.UpdatedBy; 
                    existing.UpdatedUtc = DateTime.UtcNow;
                }
            }
            await _context.SaveChangesAsync();
            return $"{existingRecords.Count} previous employment records updated successfully.";
        }

        public async Task<string> DeleteAsync(int previousEmploymentId, int deletedBy)
        {
            var message = "Previous employment record deleted successfully";
            var previousEmployment = await _context.PreviousEmployments.FirstOrDefaultAsync(pe => pe.Id == previousEmploymentId && pe.IsActive == true);
            if (previousEmployment == null) throw new MessageNotFoundException("Previous employment record not found");
            previousEmployment.IsActive = false;
            await _context.SaveChangesAsync();
            return message;
        }
    }
}