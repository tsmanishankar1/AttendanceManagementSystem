namespace AttendanceManagement.Input_Models
{
    public class CompanyMasterDto
    {
        public int CompanyMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public string LegalName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Website { get; set; } = null!;

        public string RegisterNumber { get; set; } = null!;

        public string Tngsnumber { get; set; } = null!;

        public string Cstnumber { get; set; } = null!;

        public string Tinnumber { get; set; } = null!;

        public string ServiceTaxNo { get; set; } = null!;

        public string Pannumber { get; set; } = null!;

        public string Pfnumber { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class CompanyMasterResponse
    {
        public int CompanyMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public string LegalName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Website { get; set; } = null!;

        public string RegisterNumber { get; set; } = null!;

        public string Tngsnumber { get; set; } = null!;

        public string Cstnumber { get; set; } = null!;

        public string Tinnumber { get; set; } = null!;

        public string ServiceTaxNo { get; set; } = null!;

        public string Pannumber { get; set; } = null!;

        public string Pfnumber { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }


    public class CompanyMasterRequest
    {
        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public string LegalName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Website { get; set; } = null!;

        public string RegisterNumber { get; set; } = null!;

        public string Tngsnumber { get; set; } = null!;

        public string Cstnumber { get; set; } = null!;

        public string Tinnumber { get; set; } = null!;

        public string ServiceTaxNo { get; set; } = null!;

        public string Pannumber { get; set; } = null!;

        public string Pfnumber { get; set; } = null!;   

        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
