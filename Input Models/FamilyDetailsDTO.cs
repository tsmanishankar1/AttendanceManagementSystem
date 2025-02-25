namespace AttendanceManagement.Input_Models
{
    public class FamilyDetailsDTO
    {
        public string MemberName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public decimal? IncomePerAnnum { get; set; }
        public string? Occupation { get; set; }
        public bool NomineeForPF { get; set; }
        public decimal? PFSharePercentage { get; set; }
        public bool NomineeForGratuity { get; set; }
        public decimal? GratuitySharePercentage { get; set; }
        public int StaffCreationId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
    public class FamilyDetailsResponse
    {
        public int FamilyDetailsId { get; set; }
        public string MemberName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public decimal? IncomePerAnnum { get; set; }
        public string? Occupation { get; set; }
        public bool NomineeForPF { get; set; }
        public decimal? PFSharePercentage { get; set; }
        public bool NomineeForGratuity { get; set; }
        public decimal? GratuitySharePercentage { get; set; }
        public int StaffCreationId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
    public class UpdateFamilyDetails
    {
        public int FamilyDetailsId { get; set; }
        public string MemberName { get; set; } = null!;
        public string Relationship { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public decimal? IncomePerAnnum { get; set; }
        public string? Occupation { get; set; }
        public bool NomineeForPF { get; set; }
        public decimal? PFSharePercentage { get; set; }
        public bool NomineeForGratuity { get; set; }
        public decimal? GratuitySharePercentage { get; set; }
        public int StaffCreationId { get; set; }
        public int UpdatedBy { get; set; }
    }
}
