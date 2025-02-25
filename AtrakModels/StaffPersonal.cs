using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class StaffPersonal
{
    public string StaffId { get; set; } = null!;

    public int StaffBloodGroup { get; set; }

    public int StaffMaritalStatus { get; set; }

    public string? Addr { get; set; }

    public string? Location { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? MarriageDate { get; set; }

    public string? Panno { get; set; }

    public string? PassportNo { get; set; }

    public string? DrivingLicense { get; set; }

    public string? BankName { get; set; }

    public string? BankAcno { get; set; }

    public string? BankIfsccode { get; set; }

    public string? BankBranch { get; set; }

    public string? AadharNo { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? FatherAadharNo { get; set; }

    public string? MotherAadharNo { get; set; }

    public string? EmergencyContactPerson1 { get; set; }

    public string? EmergencyContactPerson2 { get; set; }

    public string? EmergencyContactNo1 { get; set; }

    public string? EmergencyContactNo2 { get; set; }

    public string? Qualification { get; set; }

    public virtual Staff Staff { get; set; } = null!;

    public virtual BloodGroup StaffBloodGroupNavigation { get; set; } = null!;

    public virtual MaritalStatus StaffMaritalStatusNavigation { get; set; } = null!;
}
