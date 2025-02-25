using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Staff
{
    public string Id { get; set; } = null!;

    public int StaffStatusId { get; set; }

    public string? PeopleSoftCode { get; set; }

    public int SalutationId { get; set; }

    public string? CardCode { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? ShortName { get; set; }

    public string? Gender { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsSentToSmax { get; set; }

    public bool IsHidden { get; set; }

    public bool IsAttached { get; set; }

    public virtual ICollection<AbsenceApproval> AbsenceApprovals { get; set; } = new List<AbsenceApproval>();

    public virtual ICollection<AssignDepartment> AssignDepartments { get; set; } = new List<AssignDepartment>();

    public virtual ICollection<AttendanceStatusChange> AttendanceStatusChanges { get; set; } = new List<AttendanceStatusChange>();

    public virtual ICollection<EmployeeGroupTxn> EmployeeGroupTxns { get; set; } = new List<EmployeeGroupTxn>();

    public virtual ICollection<EmployeeLeaveAccount> EmployeeLeaveAccounts { get; set; } = new List<EmployeeLeaveAccount>();

    public virtual ICollection<GroupAssociation> GroupAssociations { get; set; } = new List<GroupAssociation>();

    public virtual ICollection<LaterOff> LaterOffs { get; set; } = new List<LaterOff>();

    public virtual ICollection<LeaveApplication> LeaveApplicationCreatedByNavigations { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<LeaveApplication> LeaveApplicationModifiedByNavigations { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<LeaveApplication> LeaveApplicationStaffs { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<MaintenanceOff> MaintenanceOffs { get; set; } = new List<MaintenanceOff>();

    public virtual ICollection<ManualPunch> ManualPunches { get; set; } = new List<ManualPunch>();

    public virtual ICollection<Otapplication> Otapplications { get; set; } = new List<Otapplication>();

    public virtual Salutation Salutation { get; set; } = null!;

    public virtual ICollection<StaffEducation> StaffEducations { get; set; } = new List<StaffEducation>();

    public virtual ICollection<StaffFamily> StaffFamilies { get; set; } = new List<StaffFamily>();

    public virtual StaffOfficial? StaffOfficial { get; set; }

    public virtual StaffPersonal? StaffPersonal { get; set; }

    public virtual StaffStatus StaffStatus { get; set; } = null!;

    public virtual ICollection<SwipeDatum> SwipeData { get; set; } = new List<SwipeDatum>();

    public virtual ICollection<TeamHierarchy> TeamHierarchyReportingManagers { get; set; } = new List<TeamHierarchy>();

    public virtual ICollection<TeamHierarchy> TeamHierarchyStaffs { get; set; } = new List<TeamHierarchy>();

    public virtual VaccinatedDetail? VaccinatedDetail { get; set; }

    public virtual ICollection<VaccinationDetailsDump> VaccinationDetailsDumps { get; set; } = new List<VaccinationDetailsDump>();

    public virtual ICollection<ViewApproval> ViewApprovals { get; set; } = new List<ViewApproval>();
}
