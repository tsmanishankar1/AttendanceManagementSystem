using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class BranchMasterRequest
    {
        public int CompanyMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(100)]
        public string Address { get; set; } = null!;
        [MaxLength(50)]
        public string City { get; set; } = null!;
        [MaxLength(100)]
        public string District { get; set; } = null!;
        [MaxLength(50)]
        public string State { get; set; } = null!;
        [MaxLength(50)]
        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }
        [MaxLength(100)]
        public string? Fax { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public bool IsHeadOffice { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class BranchMasterResponse
    {
        public int BranchMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public int CompanyMasterId { get; set; }
        public string CompanyMasterName { get; set; } = null!;
        public bool IsHeadOffice { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateBranch
    {
        public int BranchMasterId { get; set; }
        public int CompanyMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(100)]
        public string Address { get; set; } = null!;
        [MaxLength(50)]
        public string City { get; set; } = null!;
        [MaxLength(100)]
        public string District { get; set; } = null!;
        [MaxLength(50)]
        public string State { get; set; } = null!;
        [MaxLength(50)]
        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public long PhoneNumber { get; set; }
        [MaxLength(100)]
        public string? Fax { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; } 
        public bool IsHeadOffice { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}