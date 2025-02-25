namespace AttendanceManagement.Input_Models
{
    public class CertificateTrackingDto
    {
        public int StaffCreationId { get; set; }
        public bool CertificationCourseApplication { get; set; }
        public string CertificationCourse { get; set; } = null!;
        public string Institute { get; set; } = null!;
        public int ValidUpto { get; set; }
        public int YearOfPassing { get; set; }
        public string CourseAppraisal { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CertificateTrackingResponse
    {
        public int CertificateId { get; set; }
        public int StaffCreationId { get; set; }
        public bool CertificationCourseApplication { get; set; }
        public string CertificationCourse { get; set; } = null!;
        public string Institute { get; set; } = null!;
        public int ValidUpto { get; set; }
        public int YearOfPassing { get; set; }
        public string CourseAppraisal { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateCertificateTracking
    {
        public int CertificateId { get; set; }
        public int StaffCreationId { get; set; }
        public bool CertificationCourseApplication { get; set; }
        public string CertificationCourse { get; set; } = null!;
        public string Institute { get; set; } = null!;
        public int ValidUpto { get; set; }
        public int YearOfPassing { get; set; }
        public string CourseAppraisal { get; set; } = null!;
        public int UpdatedBy { get; set; }
    }

}