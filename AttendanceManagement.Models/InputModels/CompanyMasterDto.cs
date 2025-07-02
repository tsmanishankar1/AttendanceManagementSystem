using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class CompanyMasterDto
    {
        public int CompanyMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(50)]
        public string LegalName { get; set; } = null!;
        [MaxLength(255)]
        public string? Address { get; set; }
        [MaxLength(255)]
        public string? Website { get; set; }
        [MaxLength(50)]
        public string? RegisterNumber { get; set; }
        [MaxLength(50)]
        public string? Tngsnumber { get; set; }
        [MaxLength(50)]
        public string? Cstnumber { get; set; }
        [MaxLength(50)]
        public string? Tinnumber { get; set; }
        [MaxLength(50)]
        public string? ServiceTaxNo { get; set; }
        [MaxLength(50)]
        public string? Pannumber { get; set; }
        [MaxLength(50)]
        public string? Pfnumber { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class CompanyMasterResponse
    {
        public int CompanyMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string LegalName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Website { get; set; }
        public string? RegisterNumber { get; set; } 
        public string? Tngsnumber { get; set; }
        public string? Cstnumber { get; set; }
        public string? Tinnumber { get; set; }
        public string? ServiceTaxNo { get; set; }
        public string? Pannumber { get; set; }
        public string? Pfnumber { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class CompanyMasterRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(50)]
        public string LegalName { get; set; } = null!;
        [MaxLength(255)]
        public string? Address { get; set; }
        [MaxLength(255)]
        public string? Website { get; set; }
        [MaxLength(50)]
        public string? RegisterNumber { get; set; }
        [MaxLength(50)]
        public string? Tngsnumber { get; set; }
        [MaxLength(50)]
        public string? Cstnumber { get; set; }
        [MaxLength(50)]
        public string? Tinnumber { get; set; }
        [MaxLength(50)]
        public string? ServiceTaxNo { get; set; }
        [MaxLength(50)]
        public string? Pannumber { get; set; }
        [MaxLength(50)]
        public string? Pfnumber { get; set; }  
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}