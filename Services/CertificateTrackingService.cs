using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class CertificateTrackingService
    {
        private readonly AttendanceManagementSystemContext _context;

        public CertificateTrackingService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<CertificateTrackingResponse>> GetAllCertificates()
        {
            var certifications = await _context.CertificateTrackings
                .Select(c => new CertificateTrackingResponse
                {
                    CertificateId = c.Id,
                    StaffCreationId = c.StaffCreationId,
                    CertificationCourseApplication = c.CertificationCourseApplication,
                    CertificationCourse = c.CertificationCourse,
                    Institute = c.Institute,
                    ValidUpto = c.ValidUpto,
                    YearOfPassing = c.YearOfPassing,
                    CourseAppraisal = c.CourseAppraisal,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy
                })
                .ToListAsync();
            if(certifications.Count == 0)
            {
                throw new MessageNotFoundException("No certificate details found");
            }
            return certifications;
        }

        public async Task<CertificateTrackingResponse?> GetCertificateById(int certificateId)
        {
            var certificate = await _context.CertificateTrackings
                .Where(c => c.Id == certificateId)
                .Select(c => new CertificateTrackingResponse
                {
                    CertificateId = c.Id,
                    StaffCreationId = c.StaffCreationId,
                    CertificationCourseApplication = c.CertificationCourseApplication,
                    CertificationCourse = c.CertificationCourse,
                    Institute = c.Institute,
                    ValidUpto = c.ValidUpto,
                    YearOfPassing = c.YearOfPassing,
                    CourseAppraisal = c.CourseAppraisal,
                    IsActive = c.IsActive,
                    CreatedBy = c.CreatedBy
                })
                .FirstOrDefaultAsync();
            if (certificate == null)
            {
                throw new MessageNotFoundException("Certificate details not found");
            }
            return certificate;
        }

        public async Task<string> CreateCertificate(CertificateTrackingDto dto)
        {
            var message = "certificate Added Successfully";
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Certificate details cannot be null.");
            }
            var certificate = new CertificateTracking
            {
                StaffCreationId = dto.StaffCreationId,
                CertificationCourseApplication = dto.CertificationCourseApplication,
                CertificationCourse = dto.CertificationCourse,
                Institute = dto.Institute,
                ValidUpto = dto.ValidUpto,
                YearOfPassing = dto.YearOfPassing,
                CourseAppraisal = dto.CourseAppraisal,
                IsActive = dto.IsActive,
                CreatedBy = dto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };

            _context.CertificateTrackings.Add(certificate);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateCertificate(UpdateCertificateTracking dto)
        {
            var message = "certificate Updated Successfully";
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Certificate details cannot be null.");
            }

            var certificate = await _context.CertificateTrackings.FirstOrDefaultAsync(f => f.Id == dto.CertificateId);
            if (certificate == null)
            {
                throw new MessageNotFoundException("Family detail not found");
            }

            certificate.StaffCreationId = dto.StaffCreationId;
            certificate.CertificationCourseApplication = dto.CertificationCourseApplication;
            certificate.CertificationCourse = dto.CertificationCourse;
            certificate.Institute = dto.Institute;
            certificate.ValidUpto = dto.ValidUpto;
            certificate.YearOfPassing = dto.YearOfPassing;
            certificate.CourseAppraisal = dto.CourseAppraisal;
            certificate.UpdatedBy = dto.UpdatedBy;
            certificate.UpdatedUtc = DateTime.UtcNow;
            _context.Update(certificate);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
