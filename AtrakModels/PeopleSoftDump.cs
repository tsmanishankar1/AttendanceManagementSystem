using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PeopleSoftDump
{
    public int Id { get; set; }

    public string? EmpCode { get; set; }

    public string? PsoftEmpId { get; set; }

    public string? NamePrefix { get; set; }

    public string? Name { get; set; }

    public string? FatherName { get; set; }

    public string? Gender { get; set; }

    public string? DateOfBirth { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string? City { get; set; }

    public string? Postal { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? EmployeeStatus { get; set; }

    public string? JoiningDate { get; set; }

    public string? Designation { get; set; }

    public string? Grade { get; set; }

    public string? DeptId { get; set; }

    public string? Department { get; set; }

    public string? CompId { get; set; }

    public string? Company { get; set; }

    public string? Location { get; set; }

    public string? LocationDesc { get; set; }

    public string? Plant { get; set; }

    public string? DomainId { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public string? SupervisorName1 { get; set; }

    public string? SupervisorEmail1 { get; set; }

    public string? Flag { get; set; }

    public string? SupervisorName2 { get; set; }

    public string? SupervisorEmail2 { get; set; }

    public string? TotalLeave { get; set; }

    public string? LeaveTaken { get; set; }

    public string? LeaveBalance { get; set; }

    public string? NoofWorkingDays { get; set; }

    public string? NoOfWorkedDays { get; set; }

    public string? Month { get; set; }

    public string? SanctionLeave { get; set; }

    public string? Lop { get; set; }

    public string? Lta { get; set; }

    public string? Absent { get; set; }

    public string? Ltastatus { get; set; }

    public string? Moff { get; set; }

    public string? MoffStatus { get; set; }

    public string? Flag2 { get; set; }

    public string? Dummy2 { get; set; }

    public string? Dummy3 { get; set; }

    public string? Rhstatus { get; set; }

    public string? Dummy5 { get; set; }

    public string? Dummy6 { get; set; }

    public string? Dummy7 { get; set; }

    public string? WorkWeekPattern { get; set; }

    public string? BusinessArea { get; set; }

    public string? CostCentre { get; set; }

    public string? Team { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool IsProcessed { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public bool IsSentToSmax { get; set; }

    public DateTime? SentToSmaxon { get; set; }

    public string? ImportFileName { get; set; }

    public string? DataLineFromFile { get; set; }

    public string? DataOrigin { get; set; }
}
