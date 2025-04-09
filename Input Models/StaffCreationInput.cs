
namespace AttendanceManagement.Input_Models
{
    public class StaffCreationInputModel
    {
        public string CardCode { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ShortName { get; set; }

        public string Gender { get; set; } = null!;

        public bool Hide { get; set; }

        public string BloodGroup { get; set; } = null!;

        public IFormFile? ProfilePhoto { get; set; }
        public IFormFile? AadharCardFilePath { get; set; }
        public IFormFile? PanCardFilePath { get; set; }
        public IFormFile? DrivingLicenseFilePath { get; set; }
        public string MaritalStatus { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public DateOnly? MarriageDate { get; set; }

        public long PersonalPhone { get; set; }
        public long? OfficialPhone { get; set; }
        public DateOnly JoiningDate { get; set; }

        public bool Confirmed { get; set; }
        public DateOnly? ConfirmationDate { get; set; }
        public int BranchId { get; set; }

        public int DepartmentId { get; set; }

        public int DivisionId { get; set; }

        public string Volume { get; set; } = null!;

        public int DesignationId { get; set; }

        public int GradeId { get; set; }

        public int CategoryId { get; set; }

        public int CostCenterId { get; set; }

        public int WorkStationId { get; set; }

        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }

        public bool OtEligible { get; set; }
        public string ApprovalLevel { get; set; } = null!;
        public int ApprovalLevel1 { get; set; }

        public int? ApprovalLevel2 { get; set; }

        public string AccessLevel { get; set; } = null!;

        public string PolicyGroup { get; set; } = null!;

        public string WorkingDayPattern { get; set; } = null!;

        public decimal Tenure { get; set; }

        public string? UanNumber { get; set; }

        public string? EsiNumber { get; set; }

        public bool IsMobileAppEligible { get; set; }

        public string GeoStatus { get; set; } = null!;

        public int CreatedBy { get; set; }

        public string? MiddleName { get; set; }

        public string PersonalLocation { get; set; } = null!;

        public string PersonalEmail { get; set; } = null!;
        public string? OfficialEmail { get; set; }

        public int LeaveGroupId { get; set; }

        public int CompanyMasterId { get; set; }

        public int LocationMasterId { get; set; }

        public int HolidayCalendarId { get; set; }

        public int StatusId { get; set; }

        public long? AadharNo { get; set; }

        public string? PanNo { get; set; }

        public string? PassportNo { get; set; }

        public string? DrivingLicense { get; set; }

        public string? BankName { get; set; }

        public long? BankAccountNo { get; set; }

        public string? BankIfscCode { get; set; }

        public string? BankBranch { get; set; }


        public string HomeAddress { get; set; } = null!;

        public string FatherName { get; set; } = null!;
        public string MotherName { get; set; } = null!;
        public long? FatherAadharNo { get; set; }

        public long? MotherAadharNo { get; set; }

        public string EmergencyContactPerson1 { get; set; } = null!;

        public string? EmergencyContactPerson2 { get; set; }

        public long EmergencyContactNo1 { get; set; }

        public long? EmergencyContactNo2 { get; set; }
        public int OrganizationTypeId { get; set; }
        public string WorkingStatus { get; set; } = null!;
       
    }
    public class StaffCreationResponse
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;

        public string CardCode { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ShortName { get; set; }

        public string Gender { get; set; } = null!;

        public bool Hide { get; set; }

        public string BloodGroup { get; set; } = null!;

        public string? ProfilePhoto { get; set; }
        public string? AadharFilePath { get; set; }
        public string? PanCardFilePath { get; set; }
        public string? DrivingLicenseFilePath { get; set; }

        public int MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public DateOnly? MarriageDate { get; set; }

        public long PersonalPhone { get; set; }
        public long? OfficialPhone { get; set; }

        public DateOnly JoiningDate { get; set; }

        public bool Confirmed { get; set; }
        public DateOnly? ConfirmationDate { get; set; }
        public int BranchId { get; set; }
        public string Branch { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string Department { get; set; } = null!;
        public int DivisionId { get; set; }
        public string Division { get; set; } = null!;
        public int VolumeId { get; set; }
        public string Volume { get; set; } = null!;
        public int DesignationId { get; set; }
        public string Designation { get; set; } = null!;
        public int GradeId { get; set; }
        public string Grade { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Category { get; set; } = null!;
        public int CostCenterId { get; set; }
        public string CostCenter { get; set; } = null!;
        public int WorkStationId { get; set; }
        public string WorkStation { get; set; } = null!;

        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public bool OtEligible { get; set; }
        public string ApprovalLevel { get; set; } = null!;
        public int? ApprovalLevelId1 { get; set; }
        public string ApprovalLevel1 { get; set; } = null!;
        public int? ApprovalLevelId2 { get; set; }
        public string? ApprovalLevel2 { get; set; }
        public int AccessLevelId { get; set; }
        public string AccessLevel { get; set; } = null!;
        public int PolicyGroupId { get; set; }
        public string PolicyGroup { get; set; } = null!;
        public int WorkingDayPatternId { get; set; }
        public string WorkingDayPattern { get; set; } = null!;

        public decimal Tenure { get; set; }

        public string? UanNumber { get; set; }

        public string? EsiNumber { get; set; }

        public bool IsMobileAppEligible { get; set; }
        public int GeoStatusId { get; set; }
        public string GeoStatus { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string PersonalLocation { get; set; } = null!;

        public string? OfficialEmail { get; set; }
        public string PersonalEmail { get; set; } = null!;
        public int LeaveGroupId { get; set; }
        public string LeaveGroup { get; set; } = null!;
        public int CompanyMasterId { get; set; }
        public string Company { get; set; } = null!;
        public int LocationMasterId { get; set; }
        public string Location { get; set; } = null!;
        public int HolidayCalendarId { get; set; }
        public string HolidayCalendar { get; set; } = null!;
        public int StatusId { get; set; }
        public string Status { get; set; } = null!;

        public long? AadharNo { get; set; }

        public string? PanNo { get; set; }

        public string? PassportNo { get; set; }

        public string? DrivingLicense { get; set; }

        public string? BankName { get; set; }

        public long? BankAccountNo { get; set; }

        public string? BankIfscCode { get; set; }

        public string? BankBranch { get; set; }

     
        public string HomeAddress { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string? EmergencyContactPerson1 { get; set; }

        public string? EmergencyContactPerson2 { get; set; }

        public long EmergencyContactNo1 { get; set; }

        public long? EmergencyContactNo2 { get; set; }

        public string MotherName { get; set; } = null!;

        public long? FatherAadharNo { get; set; }

        public long? MotherAadharNo { get; set; }
        public int CreatedBy { get; set; }
        public int OrganizationTypeId { get; set; }
        public string OrganizationTypeName { get; set; } = null!;
        public int WorkingStatusId { get; set; }
        public string WorkingStatus { get; set; } = null!;
        public DateOnly? ResignationDate { get; set; }
        public DateOnly? RelievingDate { get; set; }
       

    }
    public class IndividualStaffUpdate
    {
        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ShortName { get; set; }

        public string Gender { get; set; } = null!;

        public IFormFile? ProfilePhoto { get; set; }
        public IFormFile? AadharFilePath { get; set; }
        public IFormFile? PanCardFilePath { get; set; }
        public IFormFile? DrivingLicenseFilePath { get; set; }
        public string MaritalStatus { get; set; } = null!;

        public long PersonalPhone { get; set; }
        public long? OfficialPhone { get; set; }

        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public string? MiddleName { get; set; }

        public string PersonalLocation { get; set; } = null!;

        public string? OfficialEmail { get; set; }
        public string PersonalEmail { get; set; } = null!;
        public string? BankName { get; set; }

        public long? BankAccountNo { get; set; }

        public string? BankIfscCode { get; set; }

        public string? BankBranch { get; set; }

     

        public string HomeAddress { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string EmergencyContactPerson1 { get; set; } = null!;

        public string? EmergencyContactPerson2 { get; set; }

        public long EmergencyContactNo1 { get; set; }

        public long? EmergencyContactNo2 { get; set; }

        public string MotherName { get; set; } = null!;

        public int UpdatedBy { get; set; }
       
    }

    public class IndividualStaffResponse
    {
        public string StaffCreationId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ShortName { get; set; }

        public string Gender { get; set; } = null!;

        public string BloodGroup { get; set; } = null!;

        public string? ProfilePhoto { get; set; }
        public string? AadharFilePath { get; set; }
        public string? PanCardFilePath { get; set; }
        public string? DrivingLicenseFilePath { get; set; }
        public int MaritalStatusId { get; set; }
        public string MaritalStatus { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public DateOnly? MarriageDate { get; set; }

        public long PersonalPhone { get; set; }
        public long? OfficialPhone { get; set; }

        public DateOnly JoiningDate { get; set; }

        public bool Confirmed { get; set; }
        public DateOnly? ConfirmationDate { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; } = null!;
        public int DivisionId { get; set; }
        public string Division { get; set; } = null!;
        public int DesignationId { get; set; }
        public string Designation { get; set; } = null!;
        public int GradeId { get; set; }
        public string Grade { get; set; } = null!;
        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public int? ApprovalLevelId1 { get; set; }
        public string ApprovalLevel1 { get; set; } = null!;
        public int? ApprovalLevelId2 { get; set; }
        public string? ApprovalLevel2 { get; set; }

        public decimal Tenure { get; set; }

        public string? UanNumber { get; set; }

        public string? EsiNumber { get; set; }

        public string? MiddleName { get; set; }

        public string PersonalLocation { get; set; } = null!;

        public string? OfficialEmail { get; set; }
        public string PersonalEmail { get; set; } = null!;
        public long? AadharNo { get; set; }

        public string? PanNo { get; set; }

        public string? PassportNo { get; set; }

        public string? DrivingLicense { get; set; }

        public string? BankName { get; set; }

        public long? BankAccountNo { get; set; }

        public string? BankIfscCode { get; set; }

        public string? BankBranch { get; set; }


        public string HomeAddress { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string? EmergencyContactPerson1 { get; set; }

        public string? EmergencyContactPerson2 { get; set; }

        public long EmergencyContactNo1 { get; set; }

        public long? EmergencyContactNo2 { get; set; }

        public string MotherName { get; set; } = null!;

        public long? FatherAadharNo { get; set; }

        public long? MotherAadharNo { get; set; }
        public int CreatedBy { get; set; }
        public int OrganizationTypeId { get; set; }
        public string OrganizationTypeName { get; set; } = null!;
        public int WorkingStatusId { get; set; }
        public string WorkingStatus { get; set; } = null!;
      
    }
    public class StaffCreationClass
    {
        public int StaffCreationId { get; set; }
    }

    public class NewJoinee
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string JoiningDate { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }

    public class JoiningAnniversary
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string JoiningDate { get; set; } = null!;
        public string JoiningAnniversaryYear { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }

    public class ThreeYearsOfService
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }

    public class GetStaff
    {
        public int? ApproverId { get; set; }
        public string? CompanyName { get; set; }
        public string? CategoryName { get; set; }
        public string? CostCentreName { get; set; }
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
       public string? DivisionName { get; set; }
        public string? DesignationName { get; set; }
        public string? StaffName { get; set; }
        public string? LocationName { get; set; }
        public string? GradeName { get; set; }
        public string? OrganizationTypeName { get; set; }
        public string? ShiftName { get; set; }
        public string? Status { get; set; }
        public string? LoginUserName { get; set; }
        public bool? IncludeTerminated { get; set; }
    }
    public class UpdateStaff
    {
        public int StaffCreationId { get; set; }

        public string CardCode { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public string Title { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ShortName { get; set; }

        public string Gender { get; set; } = null!;

        public bool Hide { get; set; }

        public string BloodGroup { get; set; } = null!;

        public IFormFile? ProfilePhoto { get; set; }

        public string MaritalStatus { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public DateOnly? MarriageDate { get; set; }

        public long PersonalPhone { get; set; }
        public long? OfficialPhone { get; set; }
        public DateOnly JoiningDate { get; set; }

        public bool Confirmed { get; set; }
        public DateOnly? ConfirmationDate { get; set; }
        public int BranchId { get; set; }

        public int DepartmentId { get; set; }

        public int DivisionId { get; set; }

        public string Volume { get; set; } = null!;

        public int DesignationId { get; set; }

        public int GradeId { get; set; }

        public int CategoryId { get; set; }

        public int CostCenterId { get; set; }

        public int WorkStationId { get; set; }

        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Country { get; set; } = null!;
        public int PostalCode { get; set; }
        public bool OtEligible { get; set; }
        public string ApprovalLevel { get; set; } = null!;
        public int ApprovalLevel1 { get; set; }

        public int? ApprovalLevel2 { get; set; }

        public string AccessLevel { get; set; } = null!;

        public string PolicyGroup { get; set; } = null!;

        public string WorkingDayPattern { get; set; } = null!;

        public decimal Tenure { get; set; }

        public string? UanNumber { get; set; }

        public string? EsiNumber { get; set; }

        public bool IsMobileAppEligible { get; set; }

        public string GeoStatus { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string PersonalLocation { get; set; } = null!;

        public string PersonalEmail { get; set; } = null!;
        public string? OfficialEmail { get; set; }

        public int LeaveGroupId { get; set; }

        public int CompanyMasterId { get; set; }

        public int LocationMasterId { get; set; }

        public int HolidayCalendarId { get; set; }

        public int StatusId { get; set; }

        public long? AadharNo { get; set; }

        public string? PanNo { get; set; }

        public string? PassportNo { get; set; }

        public string? DrivingLicense { get; set; }

        public string? BankName { get; set; }

        public long BankAccountNo { get; set; }

        public string? BankIfscCode { get; set; }

        public string? BankBranch { get; set; }

     
        public string HomeAddress { get; set; } = null!;

        public string FatherName { get; set; } = null!;

        public string EmergencyContactPerson1 { get; set; } = null!;

        public string? EmergencyContactPerson2 { get; set; }

        public long EmergencyContactNo1 { get; set; }

        public long? EmergencyContactNo2 { get; set; }

        public string MotherName { get; set; } = null!;

        public long? FatherAadharNo { get; set; }

        public long? MotherAadharNo { get; set; }

        public int UpdatedBy { get; set; }
        public int OrganizationTypeId { get; set; }
        public string WorkingStatus { get; set; } = null!;
        public DateOnly? ResignationDate { get; set; }
        public DateOnly? RelievingDate { get; set; }
        public IFormFile? AadharCardFilePath { get; set; }
        public IFormFile? PanCardFilePath { get; set; }
        public IFormFile? DrivingLicenseFilePath { get; set; }
      
    }

    public class OrganizationTypeResponse
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; } = null!;

        public string ShortName { get; set; } = null!;
    }

    public class StatusResponse
    {
        public int Id { get; set; }

        public string StatusName { get; set; } = null!;
    }

    public class DropDownRequest
    {
        public string Name { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class DropDownResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
    public class UpdateDropDown
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int UpdatedBy { get; set; }
    }

    public class DropDownDetailsRequest
    {
        public int DropDownMasterId { get; set; }
        public string Name { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class DropDownDetailsUpdate
    {
        public int DropDownMasterId { get; set; }
        public int DropDownDetailId { get; set; }
        public string Name { get; set; } = null!;
        public int UpdatedBy { get; set; }
    }
    public class ApproverUpdateRequest
    {
        public List<int> StaffIds { get; set; } = new();
        public int? ApproverId1 { get; set; }
        public int? ApproverId2 { get; set; }
        public int UpdatedBy { get; set; }
    }
}
