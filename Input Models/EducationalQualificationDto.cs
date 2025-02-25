namespace AttendanceManagement.Input_Models
{
    public class EducationalQualificationDto
    {
        public int StaffCreationId { get; set; }

        public string Qualification { get; set; } = null!;

        public string Specilization { get; set; } = null!;

        public string University { get; set; } = null!;

        public string Institute { get; set; } = null!;

        public string MediumOfInstruction { get; set; } = null!;

        public string CourseType { get; set; } = null!;

        public int YearOfPassing { get; set; }

        public string CourseAppraisal { get; set; } = null!;

        public int? Score { get; set; }

        public int? OutOf { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }
    public class EducationalQualificationResponse
    {
        public int EducationalQualificationId { get; set; }

        public int StaffCreationId { get; set; }

        public string Qualification { get; set; } = null!;

        public string Specilization { get; set; } = null!;

        public string University { get; set; } = null!;

        public string Institute { get; set; } = null!;

        public string MediumOfInstruction { get; set; } = null!;

        public string CourseType { get; set; } = null!;

        public int YearOfPassing { get; set; }

        public string CourseAppraisal { get; set; } = null!;

        public int? Score { get; set; }

        public int? OutOf { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }
    public class UpdateEducationalQualification
    {
        public int EducationalQualificationId { get; set; }

        public int StaffCreationId { get; set; }

        public string Qualification { get; set; } = null!;

        public string Specilization { get; set; } = null!;

        public string University { get; set; } = null!;

        public string Institute { get; set; } = null!;

        public string MediumOfInstruction { get; set; } = null!;

        public string CourseType { get; set; } = null!;

        public int YearOfPassing { get; set; }

        public string CourseAppraisal { get; set; } = null!;

        public int? Score { get; set; }

        public int? OutOf { get; set; }

        public int UpdatedBy { get; set; }
    }
}
