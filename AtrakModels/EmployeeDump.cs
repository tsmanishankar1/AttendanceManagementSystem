using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmployeeDump
{
    public long Id { get; set; }

    public string? StaffId { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? ShortName { get; set; }

    public string? Gender { get; set; }

    public string? BloodGroup { get; set; }

    public string? MaritalStatus { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? MarriageDate { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? AadharNo { get; set; }

    public string? Panno { get; set; }

    public string? PassportNo { get; set; }

    public string? DrivingLicense { get; set; }

    public string? BankName { get; set; }

    public string? BankAcno { get; set; }

    public string? BankIfscode { get; set; }

    public string? BankBranch { get; set; }

    public string? Qualification { get; set; }

    public string? HomeAddress { get; set; }

    public string? Location { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? FatherAadharNo { get; set; }

    public string? MotherAadharNo { get; set; }

    public string? EmergencyContactPerson1 { get; set; }

    public string? EmergencyContactPerson2 { get; set; }

    public string? EmergencyContactNo1 { get; set; }

    public string? EmergencyContactNo2 { get; set; }

    public DateTime? JoiningDate { get; set; }

    public string? Company { get; set; }

    public string? OfficialLocation { get; set; }

    public string? Branch { get; set; }

    public string? Department { get; set; }

    public string? Division { get; set; }

    public string? Volume { get; set; }

    public string? Designation { get; set; }

    public string? Grade { get; set; }

    public string? Category { get; set; }

    public string? CostCentre { get; set; }

    public string? WorkStation { get; set; }

    public string? ApprovalLevel { get; set; }

    public string? Approver1 { get; set; }

    public string? Approver2 { get; set; }

    public string? AccessLevel { get; set; }

    public string? PolicyGroup { get; set; }

    public string? LeaveGroup { get; set; }

    public string? HolidayGroup { get; set; }

    public string? OfficialPhone { get; set; }

    public string? OfficialEmail { get; set; }

    public string? ExtNo { get; set; }

    public string? Uanno { get; set; }

    public string? Esino { get; set; }

    public bool? IsError { get; set; }

    public bool? IsProcessed { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public string? ExcelFileName { get; set; }
}
