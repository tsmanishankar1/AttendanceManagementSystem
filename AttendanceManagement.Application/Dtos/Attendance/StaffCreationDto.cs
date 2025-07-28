using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class StaffCreationDto
    {
        public int StaffCreationId { get; set; }
        public string? CardCode { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ShortName { get; set; }
        public int? Status { get; set; }
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Approvalevel1 { get; set; }
        public string? Approvalevel2 { get; set; }
        public DateOnly? Dob { get; set; }
        public DateOnly? MarriageDate { get; set; }
        public long? PhoneNumber { get; set; }
        public DateOnly? JoiningDate { get; set; }
        public bool? Confirmed { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? AccessLevel { get; set; }
        public string? MiddleName { get; set; }
        public long? OfficialPhone { get; set; }
        public string? OfficialLocation { get; set; }
        public string? PolicyGroup { get; set; }
        public string? WorkingDayPattern { get; set; }
        public string? Tenure { get; set; }
        public string? Uannumber { get; set; }
        public string? EsiNumber { get; set; }
        public bool? IsMobileAppEligible { get; set; }
        public string? GeoStatus { get; set; }
        public string? District { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public bool? Oteligible { get; set; }
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DivisionName { get; set; }
        public string? VolumeName { get; set; }
        public string? DesignationName { get; set; }
        public string? GradeName { get; set; }
        public string? CategoryName { get; set; }
        public string? CostCenterName { get; set; }
        public string? WorkStationName { get; set; }
        public string? LeaveGroupName { get; set; }
        public string? CompanyMasterName { get; set; }
        public string? HolidayCalendarName { get; set; }
        public string? LocationMasterName { get; set; }
        public int CreatedBy { get; set; }
    }

    public class StaffBirthDayDto
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string BirthDate { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }

    public class StaffAnniversaryDto
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string MarriageDate { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }

    public class EventTypeResponse
    {
        public int EventTypeId { get; set; }
        [MaxLength(50)]
        public string EventTypeName { get; set; } = null!;
    }

    public class StaffInfoDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }
}