using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class AgtCardHolder
{
    public decimal CardNo { get; set; }

    public string ChId { get; set; } = null!;

    public string Fname { get; set; } = null!;

    public string Mname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Pin { get; set; } = null!;

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public decimal TrackCard { get; set; }

    public decimal VoidCard { get; set; }

    public decimal DisableAntiPassback { get; set; }

    public decimal MsgId { get; set; }

    public string CreatedOn { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string UpdatedOn { get; set; } = null!;

    public string UpdatedBy { get; set; } = null!;

    public string Identification { get; set; } = null!;

    public string SupervisorName { get; set; } = null!;

    public decimal Issued { get; set; }

    public decimal Pno { get; set; }

    public decimal? Bio { get; set; }

    public string? Finger1 { get; set; }

    public string? Finger2 { get; set; }

    public string Gender { get; set; } = null!;

    public string BloodGroup { get; set; } = null!;

    public decimal Nationality { get; set; }

    public decimal Qualification { get; set; }

    public string ContactAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string MailId { get; set; } = null!;

    public string LicenseNumber { get; set; } = null!;

    public DateTime Doj { get; set; }

    public DateTime Dos { get; set; }

    public decimal Designation { get; set; }

    public decimal LocofPosting { get; set; }

    public decimal Cardtype { get; set; }

    public string EmployeeStatus { get; set; } = null!;

    public string PrevWorkExp { get; set; } = null!;

    public byte[] Photo { get; set; } = null!;

    public byte[] Signature { get; set; } = null!;

    public decimal Grade { get; set; }

    public bool Deleted { get; set; }

    public bool Type { get; set; }

    public int Agency { get; set; }

    public int SubAgency { get; set; }

    public string SupervisorCno { get; set; } = null!;

    public string Pfcode { get; set; } = null!;

    public string SeviceProCardNo { get; set; } = null!;

    public string EmpNo { get; set; } = null!;

    public string NatureOfWork { get; set; } = null!;

    public string Pcc { get; set; } = null!;

    public int? FingerId1 { get; set; }

    public int? FingerId2 { get; set; }
}
