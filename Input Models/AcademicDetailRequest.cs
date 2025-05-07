
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class AcademicDetailRequest
    {
        [MaxLength(255)]
        public string? Qualification { get; set; }
        [MaxLength(255)]
        public string? Specialization { get; set; }
        [MaxLength(255)]
        public string? University { get; set; }
        [MaxLength(255)]
        public string? Institution { get; set; }
        [MaxLength(100)]
        public string? MediumOfInstruction { get; set; }
        [MaxLength(100)]
        public string? CourseType { get; set; }
        public int? YearOfPassing { get; set; }
        [MaxLength(255)]
        public string? CourseOfAppraisal { get; set; }
        [MaxLength(255)]
        public string? Board { get; set; }
      
    }

    public class ListAcademicDetailRequest
    {
        public int StaffId { get; set; }
        public int CreatedBy { get; set; }
        public List<AcademicDetailRequest> AcademicDetails { get; set; } = new List<AcademicDetailRequest>();
    }

    public class AcademicDetailUpdateRequest
    {
        public int AcademicDetailId { get; set; }
        [MaxLength(255)]
        public string? Qualification { get; set; }
        [MaxLength(255)]
        public string? Specialization { get; set; }
        [MaxLength(255)]
        public string? University { get; set; }
        [MaxLength(255)]
        public string? Institution { get; set; }
        [MaxLength(100)]
        public string? MediumOfInstruction { get; set; }
        [MaxLength(100)]
        public string? CourseType { get; set; }
        public int? YearOfPassing { get; set; }
        [MaxLength(255)]
        public string? CourseOfAppraisal { get; set; }
        [MaxLength(255)]
        public string? Board { get; set; }
    }

    public class ListAcademicDetailUpdateRequest
    {
        public int UpdatedBy { get; set; }
        public List<AcademicDetailUpdateRequest> AcademicDetails { get; set; } = new List<AcademicDetailUpdateRequest>();
    }

    public class AcademicDetailResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? Institution { get; set; }
        public string? University { get; set; }
        public string? MediumOfInstruction { get; set; }
        public string? CourseType { get; set; }
        public int? YearOfPassing { get; set; }
        public string? CourseOfAppraisal { get; set; }
        public string? Board { get; set; }

    }

    public class CertificationCourseRequest
    {
        [MaxLength(255)]
        public string? CertificationCourseName { get; set; }
        public DateOnly? ValidUpto { get; set; }
        [MaxLength(255)]
        public string? CourseAppraisal { get; set; }
        [MaxLength(255)]
        public string? CertificationInstitute { get; set; }
    }

    public class ListCertificationCourseRequest
    {
        public int StaffId { get; set; }
        public int CreatedBy { get; set; }
        public List<CertificationCourseRequest> CertificationCourses { get; set; } = new List<CertificationCourseRequest>();
    }

    public class CertificationCourseUpdateRequest
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string? CertificationCourseName { get; set; }
        public DateOnly? ValidUpto { get; set; }
        [MaxLength(255)]
        public string? CourseAppraisal { get; set; }
        [MaxLength(255)]
        public string? CertificationInstitute { get; set; }
    }

    public class ListCertificationCourseUpdateRequest
    {
        public int UpdatedBy { get; set; }
        public List<CertificationCourseUpdateRequest> CertificationCourses { get; set; } = new List<CertificationCourseUpdateRequest>();
    }

    public class CertificationCourseResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string? CertificationCourseName { get; set; }
        public DateOnly? ValidUpto { get; set; }
        public string? CourseAppraisal { get; set; }
        public string? CertificationInstitute { get; set; }
    }

    public class PreviousEmploymentRequest
    {
        [MaxLength(255)]
        public string? CompanyName { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        [MaxLength(255)]
        public string? PreviousLocation { get; set; }
        [MaxLength(255)]
        public string? FunctionalArea { get; set; }
        public decimal? LastGrossSalary { get; set; }
    }

    public class ListPreviousEmploymentRequest
    {
        public int StaffId { get; set; }
        public int CreatedBy { get; set; }
        public List<PreviousEmploymentRequest> PreviousEmployments { get; set; } = new List<PreviousEmploymentRequest>();
    }


    public class PreviousEmploymentUpdateRequest
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string? CompanyName { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        [MaxLength(255)]
        public string? PreviousLocation { get; set; }
        [MaxLength(255)]
        public string? FunctionalArea { get; set; }
        public decimal? LastGrossSalary { get; set; }
    }

    public class ListPreviousEmploymentUpdateRequest
    {
        public int UpdatedBy { get; set; }
        public List<PreviousEmploymentUpdateRequest> PreviousEmployments { get; set; } =new  List<PreviousEmploymentUpdateRequest>();
    }

    public class PreviousEmploymentResponse
    {
        public int? Id { get; set; }
        public int StaffId { get; set; }
        public string? CompanyName { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? PreviousLocation { get; set; }
        public string? FunctionalArea { get; set; }
        public decimal? LastGrossSalary { get; set; }
    }
}