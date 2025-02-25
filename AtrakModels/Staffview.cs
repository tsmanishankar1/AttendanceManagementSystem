using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Staffview
{
    public string StaffId { get; set; } = null!;

    public int? StatusId { get; set; }

    public string? StatusName { get; set; }

    public string? CardCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? ShortName { get; set; }

    public string? Gender { get; set; }

    public bool IsHidden { get; set; }

    public bool? Canteen { get; set; }

    public bool? Travel { get; set; }

    public string? DateOfJoining { get; set; }

    public string? DateOfResignation { get; set; }

    public string? OfficialPhone { get; set; }

    public bool? ShiftType { get; set; }

    public string? Esicno { get; set; }

    public string? Uanno { get; set; }

    public string? DateOfRelieving { get; set; }

    public string? OfficalFax { get; set; }

    public string? OfficalEmail { get; set; }

    public int? OfficalExtNo { get; set; }

    public string? CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyShortName { get; set; }

    public string? CompanyLegalName { get; set; }

    public string? CompanyWebSite { get; set; }

    public string? CompanyRegisterNo { get; set; }

    public string? CompanyTngsno { get; set; }

    public string? CompanyCstNo { get; set; }

    public string? CompanyTinNo { get; set; }

    public string? CompanyServiceTaxNo { get; set; }

    public string? CompanyPanNo { get; set; }

    public string? CompanyPfNo { get; set; }

    public bool? CompanyIsActive { get; set; }

    public string? BranchId { get; set; }

    public string? BranchCompanyId { get; set; }

    public string? BranchName { get; set; }

    public string? BranchShortName { get; set; }

    public string? BranchAddress { get; set; }

    public string? BranchCity { get; set; }

    public string? BranchDistrict { get; set; }

    public string? BranchState { get; set; }

    public string? BranchCountry { get; set; }

    public int? BranchPostalCode { get; set; }

    public string? BranchPhone { get; set; }

    public string? BranchFax { get; set; }

    public string? BranchEmail { get; set; }

    public bool? IsHeadOffice { get; set; }

    public string IsHeadOfficeCaption { get; set; } = null!;

    public bool? BranchIsActive { get; set; }

    public string BranchIsActiveCaption { get; set; } = null!;

    public string? DeptId { get; set; }

    public string? DeptName { get; set; }

    public string? DeptShortName { get; set; }

    public string? DeptPhone { get; set; }

    public string? DeptFax { get; set; }

    public string? DeptEmail { get; set; }

    public bool? DeptIsActive { get; set; }

    public string? DivisionId { get; set; }

    public string? DivisionName { get; set; }

    public string? DivisionShortName { get; set; }

    public bool? DivisionIsActive { get; set; }

    public string? VolumeId { get; set; }

    public string? VolumeName { get; set; }

    public string? DesignationId { get; set; }

    public string? DesignationName { get; set; }

    public string? DesignationShortName { get; set; }

    public bool? DesignationIsActive { get; set; }

    public string? GradeId { get; set; }

    public string? GradeName { get; set; }

    public string? GradeShortName { get; set; }

    public bool? GradeIsActive { get; set; }

    public string? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? CostCentreId { get; set; }

    public string? CostCentreName { get; set; }

    public string? LocationId { get; set; }

    public string? LocationName { get; set; }

    public string? WorkStationId { get; set; }

    public string? WorkStationName { get; set; }

    public int? SecurityGroupId { get; set; }

    public string? SecurityGroupName { get; set; }

    public string? LeaveGroupId { get; set; }

    public string? LeaveGroupName { get; set; }

    public bool? LeaveGroupIsActive { get; set; }

    public string? WeeklyOffId { get; set; }

    public string? WeeklyOffName { get; set; }

    public bool? WeeklyOffIsActive { get; set; }

    public int? HolidayGroupId { get; set; }

    public string? HolidayGroupName { get; set; }

    public bool? HolidayGroupIsActive { get; set; }

    public int? BloodGroup { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? EmergencyContactNo1 { get; set; }

    public string? EmergencyContactNo2 { get; set; }

    public int? MaritalStatus { get; set; }

    public string? HomeAddress { get; set; }

    public string? HomeLocation { get; set; }

    public string? HomeCity { get; set; }

    public string? HomeDistrict { get; set; }

    public string? HomeState { get; set; }

    public string? HomeCountry { get; set; }

    public string? HomePostalCode { get; set; }

    public string? HomePhone { get; set; }

    public string? PersonalEmail { get; set; }

    public string? Qualification { get; set; }

    public string? DateOfBirth { get; set; }

    public string? MarriageDate { get; set; }

    public string? PersonalPanNo { get; set; }

    public string? PassportNo { get; set; }

    public string? DrivingLicense { get; set; }

    public string? PersonalBankName { get; set; }

    public string? PersonalBankAccount { get; set; }

    public string? PersonalBankIfsccode { get; set; }

    public string? PersonalBankBranch { get; set; }

    public string? Reportingmgrid { get; set; }

    public string? Repmgrfirstname { get; set; }

    public string? Repmgrmiddlename { get; set; }

    public string? Repmgrlastname { get; set; }

    public int? Expr1 { get; set; }

    public double? WorkingPattern { get; set; }

    public string? EmployeeGroupId { get; set; }

    public string? EmployeeGroupName { get; set; }

    public string? ShiftId { get; set; }

    public string? ShiftName { get; set; }

    public string? DomainId { get; set; }

    public string? AadharNo { get; set; }

    public string? Approver2 { get; set; }

    public string? ReviewerName { get; set; }

    public int? ApproverLevel { get; set; }

    public bool? IsOteligible { get; set; }
}
