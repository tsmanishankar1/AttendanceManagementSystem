namespace AttendanceManagement.Input_Models
{
    public class CompanyMasterDto
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
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}