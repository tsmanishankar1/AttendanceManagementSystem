using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.AtrakModels;

public partial class AtrakContext : DbContext
{
    public AtrakContext()
    {
    }

    public AtrakContext(DbContextOptions<AtrakContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbsenceApproval> AbsenceApprovals { get; set; }

    public virtual DbSet<AdditionalField> AdditionalFields { get; set; }

    public virtual DbSet<AdditionalFieldValue> AdditionalFieldValues { get; set; }

    public virtual DbSet<Allowedleaf> Allowedleaves { get; set; }

    public virtual DbSet<ApplicationApproval> ApplicationApprovals { get; set; }

    public virtual DbSet<ApprovalOwnerDetail> ApprovalOwnerDetails { get; set; }

    public virtual DbSet<ApprovalStatus> ApprovalStatuses { get; set; }

    public virtual DbSet<ApprovalsView> ApprovalsViews { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AssignDepartment> AssignDepartments { get; set; }

    public virtual DbSet<Association> Associations { get; set; }

    public virtual DbSet<AtrakUserDetail> AtrakUserDetails { get; set; }

    public virtual DbSet<AttStatusUpdate02022023> AttStatusUpdate02022023s { get; set; }

    public virtual DbSet<AttachDetachLog> AttachDetachLogs { get; set; }

    public virtual DbSet<AttendanceControlTable> AttendanceControlTables { get; set; }

    public virtual DbSet<AttendanceDatum> AttendanceData { get; set; }

    public virtual DbSet<AttendanceDetail> AttendanceDetails { get; set; }

    public virtual DbSet<AttendanceReader> AttendanceReaders { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<AttendanceStatusChange> AttendanceStatusChanges { get; set; }

    public virtual DbSet<AugBreakHoursExceed> AugBreakHoursExceeds { get; set; }

    public virtual DbSet<BackDateProcessingAttendanceDatum> BackDateProcessingAttendanceData { get; set; }

    public virtual DbSet<BenchReportingManager> BenchReportingManagers { get; set; }

    public virtual DbSet<BloodGroup> BloodGroups { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<BranchTemp> BranchTemps { get; set; }

    public virtual DbSet<Btapproval> Btapprovals { get; set; }

    public virtual DbSet<BulkLeaveCreditDebitDump> BulkLeaveCreditDebitDumps { get; set; }

    public virtual DbSet<CardBlock> CardBlocks { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChangeAuditLog> ChangeAuditLogs { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompensatoryWorking> CompensatoryWorkings { get; set; }

    public virtual DbSet<CostCentre> CostCentres { get; set; }

    public virtual DbSet<CostCentreImportDump> CostCentreImportDumps { get; set; }

    public virtual DbSet<DashBoardSwipe> DashBoardSwipes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentImportDump> DepartmentImportDumps { get; set; }

    public virtual DbSet<DepartmentTemp> DepartmentTemps { get; set; }

    public virtual DbSet<DepartmentWiseLeaveRequisitionLimit> DepartmentWiseLeaveRequisitionLimits { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<DesignationImportDump> DesignationImportDumps { get; set; }

    public virtual DbSet<DesignationTemp> DesignationTemps { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<DivisionImportDump> DivisionImportDumps { get; set; }

    public virtual DbSet<DocumentUpload> DocumentUploads { get; set; }

    public virtual DbSet<Door> Doors { get; set; }

    public virtual DbSet<Ela2024Backup> Ela2024Backups { get; set; }

    public virtual DbSet<ElaBackup2023> ElaBackup2023s { get; set; }

    public virtual DbSet<Elac2024Backup> Elac2024Backups { get; set; }

    public virtual DbSet<ElacApprovedAbsenceDate> ElacApprovedAbsenceDates { get; set; }

    public virtual DbSet<EmailSendLog> EmailSendLogs { get; set; }

    public virtual DbSet<EmailSetting> EmailSettings { get; set; }

    public virtual DbSet<EmployeeDeletionDump> EmployeeDeletionDumps { get; set; }

    public virtual DbSet<EmployeeDump> EmployeeDumps { get; set; }

    public virtual DbSet<EmployeeGroup> EmployeeGroups { get; set; }

    public virtual DbSet<EmployeeGroupShiftPatternTxn> EmployeeGroupShiftPatternTxns { get; set; }

    public virtual DbSet<EmployeeGroupTxn> EmployeeGroupTxns { get; set; }

    public virtual DbSet<EmployeeLeaveAccount> EmployeeLeaveAccounts { get; set; }

    public virtual DbSet<EmployeeLeaveAccountCalendar> EmployeeLeaveAccountCalendars { get; set; }

    public virtual DbSet<EmployeePhoto> EmployeePhotos { get; set; }

    public virtual DbSet<EmployeeShiftPlan> EmployeeShiftPlans { get; set; }

    public virtual DbSet<Employeeimport> Employeeimports { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<ExcelImport> ExcelImports { get; set; }

    public virtual DbSet<FinancialYear> FinancialYears { get; set; }

    public virtual DbSet<FkEntry> FkEntries { get; set; }

    public virtual DbSet<ForTemporaryShiftChangeGrid> ForTemporaryShiftChangeGrids { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<GradeTemp> GradeTemps { get; set; }

    public virtual DbSet<GroupAssociation> GroupAssociations { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<HolidayFixedDay> HolidayFixedDays { get; set; }

    public virtual DbSet<HolidayGroup> HolidayGroups { get; set; }

    public virtual DbSet<HolidayGroupTxn> HolidayGroupTxns { get; set; }

    public virtual DbSet<HolidayWorking> HolidayWorkings { get; set; }

    public virtual DbSet<HolidayWorkingApproval> HolidayWorkingApprovals { get; set; }

    public virtual DbSet<HolidayZone> HolidayZones { get; set; }

    public virtual DbSet<HolidayZoneTxn> HolidayZoneTxns { get; set; }

    public virtual DbSet<InStaff> InStaffs { get; set; }

    public virtual DbSet<LateComingForAttendance> LateComingForAttendances { get; set; }

    public virtual DbSet<LaterOff> LaterOffs { get; set; }

    public virtual DbSet<LaterOffDate> LaterOffDates { get; set; }

    public virtual DbSet<LeaveApplication> LeaveApplications { get; set; }

    public virtual DbSet<LeaveApplicationDetail> LeaveApplicationDetails { get; set; }

    public virtual DbSet<LeaveApplicationDump> LeaveApplicationDumps { get; set; }

    public virtual DbSet<LeaveApplicationHistory> LeaveApplicationHistories { get; set; }

    public virtual DbSet<LeaveCreditDebitReason> LeaveCreditDebitReasons { get; set; }

    public virtual DbSet<LeaveDebit> LeaveDebits { get; set; }

    public virtual DbSet<LeaveDuration> LeaveDurations { get; set; }

    public virtual DbSet<LeaveGroup> LeaveGroups { get; set; }

    public virtual DbSet<LeaveGroupTxn> LeaveGroupTxns { get; set; }

    public virtual DbSet<LeaveReason> LeaveReasons { get; set; }

    public virtual DbSet<LeaveThresholdConfiguration> LeaveThresholdConfigurations { get; set; }

    public virtual DbSet<LeaveTransactionType> LeaveTransactionTypes { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Leavebalance> Leavebalances { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LogItem> LogItems { get; set; }

    public virtual DbSet<LopattUpdate030223> LopattUpdate030223s { get; set; }

    public virtual DbSet<MaintenanceOff> MaintenanceOffs { get; set; }

    public virtual DbSet<ManualPunch> ManualPunches { get; set; }

    public virtual DbSet<ManualPunchDump> ManualPunchDumps { get; set; }

    public virtual DbSet<ManualShiftChangeGrid> ManualShiftChangeGrids { get; set; }

    public virtual DbSet<ManualShiftConfiguration> ManualShiftConfigurations { get; set; }

    public virtual DbSet<MarchManualStatus> MarchManualStatuses { get; set; }

    public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

    public virtual DbSet<MenuType> MenuTypes { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<MoffYear> MoffYears { get; set; }

    public virtual DbSet<OnDutyDump> OnDutyDumps { get; set; }

    public virtual DbSet<OneRuleValue> OneRuleValues { get; set; }

    public virtual DbSet<Otapplication> Otapplications { get; set; }

    public virtual DbSet<OtapplicationEntry> OtapplicationEntries { get; set; }

    public virtual DbSet<OtbookingDump> OtbookingDumps { get; set; }

    public virtual DbSet<PeopleSoftDump> PeopleSoftDumps { get; set; }

    public virtual DbSet<PermissionApp> PermissionApps { get; set; }

    public virtual DbSet<PermissionDump> PermissionDumps { get; set; }

    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public virtual DbSet<PoCheck010324> PoCheck010324s { get; set; }

    public virtual DbSet<PoCheck01062024> PoCheck01062024s { get; set; }

    public virtual DbSet<PoCheck01102024> PoCheck01102024s { get; set; }

    public virtual DbSet<PoCheck01122024> PoCheck01122024s { get; set; }

    public virtual DbSet<PoCheck020324> PoCheck020324s { get; set; }

    public virtual DbSet<PoCheck020424> PoCheck020424s { get; set; }

    public virtual DbSet<PoCheck02052024> PoCheck02052024s { get; set; }

    public virtual DbSet<PoCheck02072024> PoCheck02072024s { get; set; }

    public virtual DbSet<PoCheck02082024> PoCheck02082024s { get; set; }

    public virtual DbSet<PoCheck02092024> PoCheck02092024s { get; set; }

    public virtual DbSet<PoCheck02112024> PoCheck02112024s { get; set; }

    public virtual DbSet<PoCheck02122024> PoCheck02122024s { get; set; }

    public virtual DbSet<PoCheck03012025> PoCheck03012025s { get; set; }

    public virtual DbSet<PoCheck03072024> PoCheck03072024s { get; set; }

    public virtual DbSet<PoCheck03102024> PoCheck03102024s { get; set; }

    public virtual DbSet<PoCheck040324> PoCheck040324s { get; set; }

    public virtual DbSet<PoCheck04052024> PoCheck04052024s { get; set; }

    public virtual DbSet<PoCheck04062024> PoCheck04062024s { get; set; }

    public virtual DbSet<PoCheck04092024> PoCheck04092024s { get; set; }

    public virtual DbSet<PoCheck04112024> PoCheck04112024s { get; set; }

    public virtual DbSet<PoCheck05022025> PoCheck05022025s { get; set; }

    public virtual DbSet<PoCheck05082024> PoCheck05082024s { get; set; }

    public virtual DbSet<PoCheck060424> PoCheck060424s { get; set; }

    public virtual DbSet<PoCheck06082024> PoCheck06082024s { get; set; }

    public virtual DbSet<PoCheck10052024> PoCheck10052024s { get; set; }

    public virtual DbSet<PoCheck10082024> PoCheck10082024s { get; set; }

    public virtual DbSet<PoCheck14082024> PoCheck14082024s { get; set; }

    public virtual DbSet<PoCheck14092024> PoCheck14092024s { get; set; }

    public virtual DbSet<PoCheck230324> PoCheck230324s { get; set; }

    public virtual DbSet<PoCheck230424> PoCheck230424s { get; set; }

    public virtual DbSet<PoCheck23052024> PoCheck23052024s { get; set; }

    public virtual DbSet<PoCheck270424> PoCheck270424s { get; set; }

    public virtual DbSet<PoCheck28112024> PoCheck28112024s { get; set; }

    public virtual DbSet<PoCheck290224> PoCheck290224s { get; set; }

    public virtual DbSet<PoCheck290324> PoCheck290324s { get; set; }

    public virtual DbSet<PoCheck300123> PoCheck300123s { get; set; }

    public virtual DbSet<PoCheck300324> PoCheck300324s { get; set; }

    public virtual DbSet<PoCheck300424> PoCheck300424s { get; set; }

    public virtual DbSet<PoCheck30082024> PoCheck30082024s { get; set; }

    public virtual DbSet<PoCheck31072024> PoCheck31072024s { get; set; }

    public virtual DbSet<PolicyDocUpload> PolicyDocUploads { get; set; }

    public virtual DbSet<PrattStatusUpdate030223> PrattStatusUpdate030223s { get; set; }

    public virtual DbSet<PrefixSuffixSetting> PrefixSuffixSettings { get; set; }

    public virtual DbSet<ProductiveHdmarking> ProductiveHdmarkings { get; set; }

    public virtual DbSet<RawPunchDetail> RawPunchDetails { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<RelationType> RelationTypes { get; set; }

    public virtual DbSet<ReportToBeSent> ReportToBeSents { get; set; }

    public virtual DbSet<ReportToBeSentTxn> ReportToBeSentTxns { get; set; }

    public virtual DbSet<Reportingtree> Reportingtrees { get; set; }

    public virtual DbSet<ReportsByEmail> ReportsByEmails { get; set; }

    public virtual DbSet<RequestApplication> RequestApplications { get; set; }

    public virtual DbSet<RestrictedHoliday> RestrictedHolidays { get; set; }

    public virtual DbSet<RolesAndResponsibility> RolesAndResponsibilities { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<RuleGroup> RuleGroups { get; set; }

    public virtual DbSet<RuleGroupTxn> RuleGroupTxns { get; set; }

    public virtual DbSet<Salutation> Salutations { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<Screen1> Screens1 { get; set; }

    public virtual DbSet<ScreenTxn> ScreenTxns { get; set; }

    public virtual DbSet<SecurityGroup> SecurityGroups { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<ShiftChangeApplication> ShiftChangeApplications { get; set; }

    public virtual DbSet<ShiftChangeApproval> ShiftChangeApprovals { get; set; }

    public virtual DbSet<ShiftExtensionAndDoubleShift> ShiftExtensionAndDoubleShifts { get; set; }

    public virtual DbSet<ShiftExtensionApproval> ShiftExtensionApprovals { get; set; }

    public virtual DbSet<ShiftExtensionDump> ShiftExtensionDumps { get; set; }

    public virtual DbSet<ShiftExtensionHistory> ShiftExtensionHistories { get; set; }

    public virtual DbSet<ShiftParentTxn> ShiftParentTxns { get; set; }

    public virtual DbSet<ShiftPattern> ShiftPatterns { get; set; }

    public virtual DbSet<ShiftPatternTxn> ShiftPatternTxns { get; set; }

    public virtual DbSet<ShiftPlan> ShiftPlans { get; set; }

    public virtual DbSet<ShiftPostingPattern> ShiftPostingPatterns { get; set; }

    public virtual DbSet<ShiftRelay> ShiftRelays { get; set; }

    public virtual DbSet<ShiftsImportDatum> ShiftsImportData { get; set; }

    public virtual DbSet<ShiftsImportDump> ShiftsImportDumps { get; set; }

    public virtual DbSet<Shiftwisemail> Shiftwisemails { get; set; }

    public virtual DbSet<SmaxTransaction> SmaxTransactions { get; set; }

    public virtual DbSet<Smaxadvmaster> Smaxadvmasters { get; set; }

    public virtual DbSet<Smaxreader> Smaxreaders { get; set; }

    public virtual DbSet<SmxCardholder> SmxCardholders { get; set; }

    public virtual DbSet<SmxReader> SmxReaders { get; set; }

    public virtual DbSet<SmxSwipedatum> SmxSwipedata { get; set; }

    public virtual DbSet<SqlExecute> SqlExecutes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffEditRequest> StaffEditRequests { get; set; }

    public virtual DbSet<StaffEducation> StaffEducations { get; set; }

    public virtual DbSet<StaffFamily> StaffFamilies { get; set; }

    public virtual DbSet<StaffOfficial> StaffOfficials { get; set; }

    public virtual DbSet<StaffPersonal> StaffPersonals { get; set; }

    public virtual DbSet<StaffStatus> StaffStatuses { get; set; }

    public virtual DbSet<Staffview> Staffviews { get; set; }

    public virtual DbSet<SubordinateTree> SubordinateTrees { get; set; }

    public virtual DbSet<SwipeDatum> SwipeData { get; set; }

    public virtual DbSet<TeamHierarchy> TeamHierarchies { get; set; }

    public virtual DbSet<UploadControlTable> UploadControlTables { get; set; }

    public virtual DbSet<VaccinatedDetail> VaccinatedDetails { get; set; }

    public virtual DbSet<VaccinationDetailsDump> VaccinationDetailsDumps { get; set; }

    public virtual DbSet<ViewApproval> ViewApprovals { get; set; }

    public virtual DbSet<VisitorPassApprovalHierarchy> VisitorPassApprovalHierarchies { get; set; }

    public virtual DbSet<VleadAugBreakExceedPr> VleadAugBreakExceedPrs { get; set; }

    public virtual DbSet<VleadDec23AttStatusChange> VleadDec23AttStatusChanges { get; set; }

    public virtual DbSet<VleadPermissionUpdate010223> VleadPermissionUpdate010223s { get; set; }

    public virtual DbSet<Volume> Volumes { get; set; }

    public virtual DbSet<VwAbsentList> VwAbsentLists { get; set; }

    public virtual DbSet<VwActiveHoliday> VwActiveHolidays { get; set; }

    public virtual DbSet<VwApprovedAbsenceDate> VwApprovedAbsenceDates { get; set; }

    public virtual DbSet<VwAttendanceDetail> VwAttendanceDetails { get; set; }

    public virtual DbSet<VwAttendanceDetailsNew> VwAttendanceDetailsNews { get; set; }

    public virtual DbSet<VwAttendanceList> VwAttendanceLists { get; set; }

    public virtual DbSet<VwBtapproval> VwBtapprovals { get; set; }

    public virtual DbSet<VwCoffApproval> VwCoffApprovals { get; set; }

    public virtual DbSet<VwCoffAvailApproval> VwCoffAvailApprovals { get; set; }

    public virtual DbSet<VwCoffAvailedHistory> VwCoffAvailedHistories { get; set; }

    public virtual DbSet<VwEmployeegroup> VwEmployeegroups { get; set; }

    public virtual DbSet<VwEmployeegroupview> VwEmployeegroupviews { get; set; }

    public virtual DbSet<VwHolidayCalendarView> VwHolidayCalendarViews { get; set; }

    public virtual DbSet<VwHolidayFixedDayView> VwHolidayFixedDayViews { get; set; }

    public virtual DbSet<VwHolidayView> VwHolidayViews { get; set; }

    public virtual DbSet<VwHolidayWorkingList> VwHolidayWorkingLists { get; set; }

    public virtual DbSet<VwLaterOffApproval> VwLaterOffApprovals { get; set; }

    public virtual DbSet<VwLeaveApplicationApproval> VwLeaveApplicationApprovals { get; set; }

    public virtual DbSet<VwLeaveApplicationList> VwLeaveApplicationLists { get; set; }

    public virtual DbSet<VwMaintenanceOffApproval> VwMaintenanceOffApprovals { get; set; }

    public virtual DbSet<VwManualPunchApproval> VwManualPunchApprovals { get; set; }

    public virtual DbSet<VwManualPunchList> VwManualPunchLists { get; set; }

    public virtual DbSet<VwOdBtWfhApproval> VwOdBtWfhApprovals { get; set; }

    public virtual DbSet<VwOdapproval> VwOdapprovals { get; set; }

    public virtual DbSet<VwOdrequestList> VwOdrequestLists { get; set; }

    public virtual DbSet<VwOtapproval> VwOtapprovals { get; set; }

    public virtual DbSet<VwOtrequestList> VwOtrequestLists { get; set; }

    public virtual DbSet<VwPermanantShiftChangeList> VwPermanantShiftChangeLists { get; set; }

    public virtual DbSet<VwPermissionApproval> VwPermissionApprovals { get; set; }

    public virtual DbSet<VwPresentList> VwPresentLists { get; set; }

    public virtual DbSet<VwRhapproval> VwRhapprovals { get; set; }

    public virtual DbSet<VwShiftChangeApproval> VwShiftChangeApprovals { get; set; }

    public virtual DbSet<VwShiftChangeList> VwShiftChangeLists { get; set; }

    public virtual DbSet<VwShiftRelay> VwShiftRelays { get; set; }

    public virtual DbSet<VwShiftsandleaf> VwShiftsandleaves { get; set; }

    public virtual DbSet<VwSmaxtransaction> VwSmaxtransactions { get; set; }

    public virtual DbSet<VwWfhapproval> VwWfhapprovals { get; set; }

    public virtual DbSet<VwZoneWiseHolidayCalendar> VwZoneWiseHolidayCalendars { get; set; }

    public virtual DbSet<Vwabsencedate> Vwabsencedates { get; set; }

    public virtual DbSet<WeeklyOff> WeeklyOffs { get; set; }

    public virtual DbSet<Wfhapproval> Wfhapprovals { get; set; }

    public virtual DbSet<WorkingDayPattern> WorkingDayPatterns { get; set; }

    public virtual DbSet<Workstation> Workstations { get; set; }

    public virtual DbSet<WorkstationAllocation> WorkstationAllocations { get; set; }

    public virtual DbSet<Zone> Zones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KTP-RLT-KIT025;Initial Catalog=ATRAK;Persist Security info=False;User ID=sa;Password=Password@1;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;Connect Timeout=600;Command Timeout=600;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbsenceApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AbsenceApproval");

            entity.ToTable("AbsenceApproval");

            entity.HasIndex(e => e.AbsenceId, "IX_AbsenceId");

            entity.HasIndex(e => e.ApprovalStatusId, "IX_ApprovalStatusId");

            entity.HasIndex(e => e.ApprovedById, "IX_ApprovedById");

            entity.Property(e => e.AbsenceId).HasMaxLength(10);
            entity.Property(e => e.ApprovedById).HasMaxLength(50);
            entity.Property(e => e.ApprovedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Comment).HasMaxLength(200);

            entity.HasOne(d => d.Absence).WithMany(p => p.AbsenceApprovals)
                .HasForeignKey(d => d.AbsenceId)
                .HasConstraintName("FK_dbo.AbsenceApproval_dbo.LeaveApplication_AbsenceId");

            entity.HasOne(d => d.ApprovalStatus).WithMany(p => p.AbsenceApprovals)
                .HasForeignKey(d => d.ApprovalStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AbsenceApproval_dbo.ApprovalStatus_ApprovalStatusId");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.AbsenceApprovals)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK_dbo.AbsenceApproval_dbo.Staff_ApprovedById");
        });

        modelBuilder.Entity<AdditionalField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AdditionalField");

            entity.ToTable("AdditionalField");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AdditionalFieldValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AdditionalFieldValue");

            entity.ToTable("AdditionalFieldValue");

            entity.HasIndex(e => e.AddfId, "IX_AddfId");

            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Addf).WithMany(p => p.AdditionalFieldValues)
                .HasForeignKey(d => d.AddfId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AdditionalFieldValue_dbo.AdditionalField_AddfId");
        });

        modelBuilder.Entity<Allowedleaf>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ALLOWEDLEAVES");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LeaveGroupId).HasMaxLength(10);
            entity.Property(e => e.LeaveGroupName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.LeaveTypeName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeShortName).HasMaxLength(5);
            entity.Property(e => e.Leavebalance)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("LEAVEBALANCE");
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<ApplicationApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ApplicationApproval");

            entity.ToTable("ApplicationApproval");

            entity.HasIndex(e => e.ApprovalStatusId, "IX_ApprovalStatusId");

            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2On).HasColumnType("smalldatetime");
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2statusId).HasDefaultValue(1);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovedBy).HasMaxLength(50);
            entity.Property(e => e.ApprovedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ForwardCounter).HasDefaultValue(1);
            entity.Property(e => e.ParentId).HasMaxLength(20);
            entity.Property(e => e.ParentType).HasMaxLength(5);

            entity.HasOne(d => d.ApprovalStatus).WithMany(p => p.ApplicationApprovals)
                .HasForeignKey(d => d.ApprovalStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.ApplicationApproval_dbo.ApprovalStatus_ApprovalStatusId");
        });

        modelBuilder.Entity<ApprovalOwnerDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApprovalOwner2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ApprovalStatus");

            entity.ToTable("ApprovalStatus");

            entity.Property(e => e.Name).HasMaxLength(60);
        });

        modelBuilder.Entity<ApprovalsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ApprovalsView");

            entity.Property(e => e.ApplicationDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.Cancelled)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.LeaveApplicationId)
                .HasMaxLength(10)
                .HasColumnName("leaveApplicationId");
            entity.Property(e => e.LeaveDurationName).HasMaxLength(50);
            entity.Property(e => e.LeaveEndDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.LeaveReason).HasMaxLength(200);
            entity.Property(e => e.LeaveStartDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Permission)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StaffName).HasMaxLength(4000);
            entity.Property(e => e.StaffStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetRoles");

            entity.Property(e => e.Id).HasMaxLength(128);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUsers");

            entity.Property(e => e.Id).HasMaxLength(128);
            entity.Property(e => e.Discriminator).HasMaxLength(128);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_RoleId");
                        j.HasIndex(new[] { "UserId" }, "IX_UserId");
                        j.IndexerProperty<string>("UserId").HasMaxLength(128);
                        j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserClaims");

            entity.HasIndex(e => e.UserId, "IX_User_Id");

            entity.Property(e => e.UserId)
                .HasMaxLength(128)
                .HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.ProviderKey }).HasName("PK_dbo.AspNetUserLogins");

            entity.HasIndex(e => e.UserId, "IX_UserId");

            entity.Property(e => e.UserId).HasMaxLength(128);
            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
        });

        modelBuilder.Entity<AssignDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AssignDepartment");

            entity.ToTable("AssignDepartment");

            entity.HasIndex(e => e.NewDepartmentId, "IX_NewDepartmentId");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EffectFromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.EffectToDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.NewDepartmentId).HasMaxLength(10);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.NewDepartment).WithMany(p => p.AssignDepartments)
                .HasForeignKey(d => d.NewDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AssignDepartment_dbo.Department_NewDepartmentId");

            entity.HasOne(d => d.Staff).WithMany(p => p.AssignDepartments)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AssignDepartment_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<Association>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Association");

            entity.ToTable("Association");

            entity.Property(e => e.Combination).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Gender).HasMaxLength(5);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.WorkingDayPattern).HasMaxLength(5);
        });

        modelBuilder.Entity<AtrakUserDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AtrakUserDetails");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<AttStatusUpdate02022023>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AttStatusUpdate_02022023");

            entity.Property(e => e.AttDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AttStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AttachDetachLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AttachDetachLog");

            entity.ToTable("AttachDetachLog");

            entity.Property(e => e.ReportingManager).HasMaxLength(10);
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StateChangedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AttendanceControlTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AttendanceControlTable");

            entity.ToTable("AttendanceControlTable");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AttendanceDatum>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => new { e.StaffId, e.ShiftInDate }, "IDX_ATTENDANCEDATA_STAFFID_SHIFTINDATE").IsClustered();

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.AccountedEarlyComingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedEarlyGoingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedLateComingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedLateGoingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedOttime)
                .HasColumnType("datetime")
                .HasColumnName("AccountedOTTime");
            entity.Property(e => e.ActualEarlyComingTime).HasColumnType("datetime");
            entity.Property(e => e.ActualEarlyGoingTime).HasColumnType("datetime");
            entity.Property(e => e.ActualInDate).HasColumnType("datetime");
            entity.Property(e => e.ActualInTime).HasColumnType("datetime");
            entity.Property(e => e.ActualLateComingTime).HasColumnType("datetime");
            entity.Property(e => e.ActualLateGoingTime).HasColumnType("datetime");
            entity.Property(e => e.ActualOttime)
                .HasColumnType("datetime")
                .HasColumnName("ActualOTTime");
            entity.Property(e => e.ActualOutDate).HasColumnType("datetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("datetime");
            entity.Property(e => e.ActualShiftId).HasMaxLength(10);
            entity.Property(e => e.ActualShiftIn)
                .HasColumnType("smalldatetime")
                .HasColumnName("ActualShiftIN");
            entity.Property(e => e.ActualShiftOut)
                .HasColumnType("smalldatetime")
                .HasColumnName("ActualShiftOUT");
            entity.Property(e => e.ActualShiftShortName).HasMaxLength(5);
            entity.Property(e => e.ActualWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.ActualWorkingHours).HasColumnType("smalldatetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(20);
            entity.Property(e => e.AutoShiftBasedSwipeInTime).HasColumnType("datetime");
            entity.Property(e => e.AutoShiftBasedSwipeOutTime).HasColumnType("datetime");
            entity.Property(e => e.AutoShiftBasedWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.BreakHours).HasColumnType("datetime");
            entity.Property(e => e.BreakInTime).HasColumnType("datetime");
            entity.Property(e => e.BreakOutTime).HasColumnType("datetime");
            entity.Property(e => e.Comments)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId)
                .HasMaxLength(10)
                .HasComputedColumnSql("(CONVERT([nvarchar](10),[dbo].[GetAssignDepartmentId]([StaffId],[ShiftInDate])))", false);
            entity.Property(e => e.EarlyComing).HasColumnType("datetime");
            entity.Property(e => e.EarlyGoing).HasColumnType("datetime");
            entity.Property(e => e.ExpectedWorkingHours).HasColumnType("datetime");
            entity.Property(e => e.ExtraBreakTime).HasColumnType("datetime");
            entity.Property(e => e.ExtraHoursApprovedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Fhaccount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("FHAccount");
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IflexiShiftTime).HasColumnName("IFlexiShiftTime");
            entity.Property(e => e.InReaderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsOt).HasColumnName("IsOT");
            entity.Property(e => e.IsOtvalid).HasColumnName("IsOTValid");
            entity.Property(e => e.LateComing).HasColumnType("datetime");
            entity.Property(e => e.LateGoing).HasColumnType("datetime");
            entity.Property(e => e.NetWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.Othours)
                .HasColumnType("datetime")
                .HasColumnName("OTHours");
            entity.Property(e => e.OutReaderName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OverRideOt).HasColumnName("OverRideOT");
            entity.Property(e => e.PostShiftOthours)
                .HasColumnType("datetime")
                .HasColumnName("PostShiftOTHours");
            entity.Property(e => e.PreShiftOthours)
                .HasColumnType("datetime")
                .HasColumnName("PreShiftOTHours");
            entity.Property(e => e.Shaccount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SHAccount");
            entity.Property(e => e.ShiftInDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("datetime");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany()
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.AttendanceData_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<AttendanceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AttendanceDetails");

            entity.Property(e => e.ActualInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(5);
            entity.Property(e => e.BranchName).HasMaxLength(50);
            entity.Property(e => e.BreakHours).HasColumnType("smalldatetime");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DateOfJoining)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DateOfResignation)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.DesignationName).HasMaxLength(50);
            entity.Property(e => e.DivisionName).HasMaxLength(50);
            entity.Property(e => e.EarlyComing).HasColumnType("smalldatetime");
            entity.Property(e => e.EarlyGoing).HasColumnType("smalldatetime");
            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ExpectedWorkingHours).HasColumnType("smalldatetime");
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(5)
                .HasColumnName("FHStatus");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GraceEarlyBy)
                .HasColumnType("smalldatetime")
                .HasColumnName("GraceEarlyBY");
            entity.Property(e => e.GraceLateBy).HasColumnType("smalldatetime");
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LateComing).HasColumnType("smalldatetime");
            entity.Property(e => e.LateGoing).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.NetWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.Othours)
                .HasColumnType("smalldatetime")
                .HasColumnName("OTHours");
            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(1);
            entity.Property(e => e.ShiftInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(5)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AttendanceReader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AttendanceReaders");

            entity.Property(e => e.IpAddress).HasMaxLength(15);
        });

        modelBuilder.Entity<AttendanceStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AttendanceStatus");

            entity.Property(e => e.ColorCode).HasMaxLength(100);
            entity.Property(e => e.ConsiderAsPresent).HasDefaultValue(true);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FhcolorCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FHColorCode");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StatusShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AttendanceStatusChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AttendanceStatusChange");

            entity.ToTable("AttendanceStatusChange");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Duration).HasMaxLength(12);
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.AttendanceStatusChanges)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AttendanceStatusChange_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<AugBreakHoursExceed>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Aug_BreakHoursExceed");

            entity.Property(e => e.AttendanceStatus).HasMaxLength(20);
            entity.Property(e => e.Fhaccount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("FHAccount");
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .HasColumnName("FHStatus");
            entity.Property(e => e.PermissionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Shaccount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("SHAccount");
            entity.Property(e => e.ShiftInDate).HasColumnType("datetime");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<BackDateProcessingAttendanceDatum>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ActualInDate).HasColumnType("datetime");
            entity.Property(e => e.ActualInTime).HasColumnType("datetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("datetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("datetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.ApplicationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApplicationType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceStatus).HasMaxLength(10);
            entity.Property(e => e.ExpectedWorkingHours).HasColumnType("datetime");
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(5)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ProcessedOn).HasColumnType("datetime");
            entity.Property(e => e.ShiftInDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("datetime");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(5)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<BenchReportingManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.BenchReportingManager");

            entity.ToTable("BenchReportingManager");

            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.BloodGroup");

            entity.ToTable("BloodGroup");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Branch");

            entity.ToTable("Branch");

            entity.HasIndex(e => e.CompanyId, "IX_CompanyID");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CompanyId)
                .HasMaxLength(10)
                .HasColumnName("CompanyID");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fax).HasMaxLength(15);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.ShortName).HasMaxLength(5);
            entity.Property(e => e.State).HasMaxLength(50);

            entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_dbo.Branch_dbo.Company_CompanyID");
        });

        modelBuilder.Entity<BranchTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BRANCH_TEMP");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Unitname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("UNITNAME");
        });

        modelBuilder.Entity<Btapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BTApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationId).HasMaxLength(20);
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Od)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("OD");
            entity.Property(e => e.Odduration)
                .HasMaxLength(20)
                .HasColumnName("ODDuration");
            entity.Property(e => e.OdfromDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODFromDate");
            entity.Property(e => e.OdfromTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODFromTime");
            entity.Property(e => e.Odreason)
                .HasMaxLength(200)
                .HasColumnName("ODReason");
            entity.Property(e => e.OdtoDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODToDate");
            entity.Property(e => e.OdtoTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODToTime");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<BulkLeaveCreditDebitDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BulkLeaveCreditDebitDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.LeaveCount)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(5, 2)");
            entity.Property(e => e.LeaveCreditDebitReason)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LeaveType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Month)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CardBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CardBlock");

            entity.ToTable("CardBlock");

            entity.Property(e => e.CardBlockedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CardOpenedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.DateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Category");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<ChangeAuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ChangeAuditLog");

            entity.ToTable("ChangeAuditLog");

            entity.Property(e => e.ActionType).HasMaxLength(6);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.PrimaryKeyValue).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(50);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Company");

            entity.ToTable("Company");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Cstno)
                .HasMaxLength(20)
                .HasColumnName("CSTNo");
            entity.Property(e => e.LegalName).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Panno)
                .HasMaxLength(20)
                .HasColumnName("PANNo");
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.Pfno)
                .HasMaxLength(20)
                .HasColumnName("PFNo");
            entity.Property(e => e.RegisterNo).HasMaxLength(20);
            entity.Property(e => e.ServiceTaxNo).HasMaxLength(20);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.Tinno)
                .HasMaxLength(20)
                .HasColumnName("TINNo");
            entity.Property(e => e.Tngsno)
                .HasMaxLength(20)
                .HasColumnName("TNGSNo");
            entity.Property(e => e.Website).HasMaxLength(50);
        });

        modelBuilder.Entity<CompensatoryWorking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CompensatoryWorking");

            entity.ToTable("CompensatoryWorking");

            entity.Property(e => e.CompensatoryWorkingDate).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Reason).HasMaxLength(500);
        });

        modelBuilder.Entity<CostCentre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CostCentre");

            entity.ToTable("CostCentre");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<CostCentreImportDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CostCentreImportDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(128);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(100);
        });

        modelBuilder.Entity<DashBoardSwipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.DashBoardSwipes");

            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.IpAddress).HasMaxLength(30);
            entity.Property(e => e.Lattitude).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionTime).HasColumnType("smalldatetime");
            entity.Property(e => e.TransactionType).HasMaxLength(5);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Department");

            entity.ToTable("Department");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fax).HasMaxLength(15);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<DepartmentImportDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DepartmentImportDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(128);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(100);
        });

        modelBuilder.Entity<DepartmentTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DEPARTMENT_TEMP");

            entity.Property(e => e.Deptname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DEPTNAME");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Shortname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SHORTNAME");
        });

        modelBuilder.Entity<DepartmentWiseLeaveRequisitionLimit>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DepartmentWiseLeaveRequisitionLimit");

            entity.Property(e => e.DepartmentId)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Designation");

            entity.ToTable("Designation");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.ShortName).HasMaxLength(10);
        });

        modelBuilder.Entity<DesignationImportDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DesignationImportDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(128);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(100);
        });

        modelBuilder.Entity<DesignationTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DESIGNATION_TEMP");

            entity.Property(e => e.Designationname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESIGNATIONNAME");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Shortname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SHORTNAME");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Division");

            entity.ToTable("Division");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<DivisionImportDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DivisionImportDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(128);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ShortName).HasMaxLength(100);
        });

        modelBuilder.Entity<DocumentUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.DocumentUpload");

            entity.ToTable("DocumentUpload");

            entity.HasIndex(e => e.ParentId, "IX_ParentId");

            entity.Property(e => e.CreatedOn)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ParentId).HasMaxLength(20);

            entity.HasOne(d => d.Parent).WithMany(p => p.DocumentUploads)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_dbo.DocumentUpload_dbo.RequestApplication_ParentId");
        });

        modelBuilder.Entity<Door>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Door");

            entity.ToTable("Door");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MofifiedBy).HasMaxLength(10);
        });

        modelBuilder.Entity<Ela2024Backup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ELA_2024_Backup");

            entity.Property(e => e.ExtensionPeriod).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinancialYearEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.FinancialYearStart).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LeaveCount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Narration).HasMaxLength(100);
            entity.Property(e => e.RefId).HasMaxLength(20);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TransctionBy).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ElaBackup2023>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ELA_Backup_2023");

            entity.Property(e => e.ExtensionPeriod).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinancialYearEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.FinancialYearStart).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LeaveCount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Narration).HasMaxLength(100);
            entity.Property(e => e.RefId).HasMaxLength(20);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TransctionBy).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Elac2024Backup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ELAC_2024_Backup");

            entity.Property(e => e.ElaId).HasColumnName("ELA_Id");
            entity.Property(e => e.FyendYear).HasColumnName("FYEndYear");
            entity.Property(e => e.FystartYear).HasColumnName("FYStartYear");
            entity.Property(e => e.LeaveCount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RefId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ElacApprovedAbsenceDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ELAC_ApprovedAbsenceDates");

            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveApplicationReason).HasMaxLength(200);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<EmailSendLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmailSendLog");

            entity.ToTable("EmailSendLog");

            entity.Property(e => e.Bcc)
                .HasMaxLength(1000)
                .HasColumnName("BCC");
            entity.Property(e => e.Cc)
                .HasMaxLength(1000)
                .HasColumnName("CC");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EmailSubject).HasMaxLength(200);
            entity.Property(e => e.ErrorDescription).HasMaxLength(1000);
            entity.Property(e => e.FilePathName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.FileType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.From).HasMaxLength(100);
            entity.Property(e => e.IncludesAttachment).HasDefaultValue(false);
            entity.Property(e => e.SentOn).HasColumnType("smalldatetime");
            entity.Property(e => e.To).HasMaxLength(100);
        });

        modelBuilder.Entity<EmailSetting>(entity =>
        {
            entity.HasKey(e => e.OutgoingServer).HasName("PK_dbo.EmailSettings");

            entity.Property(e => e.OutgoingServer).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EnableSsl).HasColumnName("EnableSSL");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<EmployeeDeletionDump>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeDeletionDump");

            entity.ToTable("EmployeeDeletionDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.RelievingDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ResignationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EmployeeDump");

            entity.Property(e => e.AadharNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.AccessLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalLevel)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Approver1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Approver2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankAcno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BankACno");
            entity.Property(e => e.BankBranch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankIfscode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BankIFSCode");
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CostCentre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.District)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Division)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DrivingLicense)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo1)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactNo2)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactPerson1)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactPerson2)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Esino)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ESINo");
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ExtNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherAadharNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Grade)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HolidayGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HomeAddress)
                .HasMaxLength(400)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LeaveGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MarriageDate).HasColumnType("datetime");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MotherAadharNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficialEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficialLocation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficialPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Panno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PANNo");
            entity.Property(e => e.PassportNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PolicyGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Qualification)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Uanno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UANNo");
            entity.Property(e => e.Volume)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WorkStation)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeeGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeGroup");

            entity.ToTable("EmployeeGroup");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<EmployeeGroupShiftPatternTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeGroupShiftPatternTxn");

            entity.ToTable("EmployeeGroupShiftPatternTxn");

            entity.HasIndex(e => e.EmployeeGroupId, "IX_EmployeeGroupId");

            entity.HasIndex(e => e.ShiftPatternId, "IX_ShiftPatternId");

            entity.Property(e => e.EmployeeGroupId).HasMaxLength(10);

            entity.HasOne(d => d.EmployeeGroup).WithMany(p => p.EmployeeGroupShiftPatternTxns)
                .HasForeignKey(d => d.EmployeeGroupId)
                .HasConstraintName("FK_dbo.EmployeeGroupShiftPatternTxn_dbo.EmployeeGroup_EmployeeGroupId");

            entity.HasOne(d => d.ShiftPattern).WithMany(p => p.EmployeeGroupShiftPatternTxns)
                .HasForeignKey(d => d.ShiftPatternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.EmployeeGroupShiftPatternTxn_dbo.ShiftPattern_ShiftPatternId");
        });

        modelBuilder.Entity<EmployeeGroupTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeGroupTxn");

            entity.ToTable("EmployeeGroupTxn");

            entity.HasIndex(e => e.EmployeeGroupId, "IX_EmployeeGroupId");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.EmployeeGroupId).HasMaxLength(10);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.EmployeeGroup).WithMany(p => p.EmployeeGroupTxns)
                .HasForeignKey(d => d.EmployeeGroupId)
                .HasConstraintName("FK_dbo.EmployeeGroupTxn_dbo.EmployeeGroup_EmployeeGroupId");

            entity.HasOne(d => d.Staff).WithMany(p => p.EmployeeGroupTxns)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.EmployeeGroupTxn_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<EmployeeLeaveAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeLeaveAccount");

            entity.ToTable("EmployeeLeaveAccount");

            entity.HasIndex(e => e.LeaveCreditDebitReasonId, "IX_LeaveCreditDebitReasonId");

            entity.HasIndex(e => e.LeaveTypeId, "IX_LeaveTypeId");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.HasIndex(e => e.TransactionFlag, "IX_TransactionFlag");

            entity.Property(e => e.ExtensionPeriod).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FinancialYearEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.FinancialYearStart).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveCount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Narration).HasMaxLength(100);
            entity.Property(e => e.RefId).HasMaxLength(20);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TransctionBy).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.LeaveCreditDebitReason).WithMany(p => p.EmployeeLeaveAccounts)
                .HasForeignKey(d => d.LeaveCreditDebitReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.EmployeeLeaveAccount_dbo.LeaveCreditDebitReason_LeaveCreditDebitReasonId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.EmployeeLeaveAccounts)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_dbo.EmployeeLeaveAccount_dbo.LeaveType_LeaveTypeId");

            entity.HasOne(d => d.Staff).WithMany(p => p.EmployeeLeaveAccounts)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.EmployeeLeaveAccount_dbo.Staff_StaffId");

            entity.HasOne(d => d.TransactionFlagNavigation).WithMany(p => p.EmployeeLeaveAccounts)
                .HasForeignKey(d => d.TransactionFlag)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.EmployeeLeaveAccount_dbo.LeaveTransactionType_TransactionFlag");
        });

        modelBuilder.Entity<EmployeeLeaveAccountCalendar>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EmployeeLeaveAccountCalendar");

            entity.Property(e => e.ElaId).HasColumnName("ELA_Id");
            entity.Property(e => e.FyendYear).HasColumnName("FYEndYear");
            entity.Property(e => e.FystartYear).HasColumnName("FYStartYear");
            entity.Property(e => e.LeaveCount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RefId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeePhoto>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_dbo.EmployeePhoto");

            entity.ToTable("EmployeePhoto");

            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<EmployeeShiftPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.EmployeeShiftPlan");

            entity.ToTable("EmployeeShiftPlan");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InUse).HasDefaultValue(true);
            entity.Property(e => e.LastUpdatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Reason)
                .HasMaxLength(150)
                .HasDefaultValue("");
            entity.Property(e => e.ShiftId).HasMaxLength(6);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.WeeklyOffId).HasMaxLength(10);
        });

        modelBuilder.Entity<Employeeimport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("EMPLOYEEIMPORT");

            entity.Property(e => e.Bloodgroup)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("bloodgroup");
            entity.Property(e => e.Canteenflag)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("canteenflag");
            entity.Property(e => e.Cardno)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cardno");
            entity.Property(e => e.Chid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("chid");
            entity.Property(e => e.Contactaddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contactaddress");
            entity.Property(e => e.Department)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.Designation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.DnName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DN_NAME");
            entity.Property(e => e.Dob)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dob");
            entity.Property(e => e.Doj)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DOJ");
            entity.Property(e => e.DpName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dp_name");
            entity.Property(e => e.Employeestatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("employeestatus");
            entity.Property(e => e.Fname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("fname");
            entity.Property(e => e.GdName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("GD_NAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Grade)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("GRADE");
            entity.Property(e => e.Isprocessed).HasColumnName("ISPROCESSED");
            entity.Property(e => e.Lname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("lname");
            entity.Property(e => e.Mailid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mailid");
            entity.Property(e => e.Mname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mname");
            entity.Property(e => e.Mobilenumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("mobilenumber");
            entity.Property(e => e.PDisp)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("p_disp");
            entity.Property(e => e.PEsino)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("p_esino");
            entity.Property(e => e.PPfno)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("p_pfno");
            entity.Property(e => e.Pannumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("pannumber");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("phonenumber");
            entity.Property(e => e.Shortname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("shortname");
            entity.Property(e => e.Temporaryaddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("temporaryaddress");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.Unit)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("unit");
            entity.Property(e => e.Unitname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("UNITNAME");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ErrorLog");

            entity.ToTable("ErrorLog");

            entity.Property(e => e.AppName).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(4000);
            entity.Property(e => e.FunctionName).HasMaxLength(50);
            entity.Property(e => e.ModuleName).HasMaxLength(50);
            entity.Property(e => e.StaffId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TxnDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ExcelImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ExcelImport");

            entity.ToTable("ExcelImport");

            entity.Property(e => e.BussinessArea).HasMaxLength(255);
            entity.Property(e => e.Company).HasMaxLength(255);
            entity.Property(e => e.CostCenter).HasMaxLength(255);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DataOrigin).HasMaxLength(255);
            entity.Property(e => e.DateOfBirth).HasMaxLength(255);
            entity.Property(e => e.DateOfJoining).HasMaxLength(255);
            entity.Property(e => e.Department).HasMaxLength(255);
            entity.Property(e => e.Designation).HasMaxLength(255);
            entity.Property(e => e.EmpNo).HasMaxLength(255);
            entity.Property(e => e.FatherName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.ImportFileName).HasMaxLength(255);
            entity.Property(e => e.ImportLine).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Team).HasMaxLength(255);
            entity.Property(e => e.WorkWeekPattern).HasMaxLength(255);
        });

        modelBuilder.Entity<FinancialYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.FinancialYear");

            entity.ToTable("FinancialYear");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.From).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.To).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FkEntry>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FK_ENTRIES");

            entity.Property(e => e.Deferrability).HasColumnName("DEFERRABILITY");
            entity.Property(e => e.DeleteRule).HasColumnName("DELETE_RULE");
            entity.Property(e => e.FkName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FK_NAME");
            entity.Property(e => e.FkcolumnName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FKCOLUMN_NAME");
            entity.Property(e => e.FktableName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FKTABLE_NAME");
            entity.Property(e => e.FktableOwner)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FKTABLE_OWNER");
            entity.Property(e => e.FktableQualifier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FKTABLE_QUALIFIER");
            entity.Property(e => e.KeySeq).HasColumnName("KEY_SEQ");
            entity.Property(e => e.PkName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PK_NAME");
            entity.Property(e => e.PkcolumnName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PKCOLUMN_NAME");
            entity.Property(e => e.PktableName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PKTABLE_NAME");
            entity.Property(e => e.PktableOwner)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PKTABLE_OWNER");
            entity.Property(e => e.PktableQualifier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PKTABLE_QUALIFIER");
            entity.Property(e => e.ProcessId).HasDefaultValue((byte)0);
            entity.Property(e => e.UpdateRule).HasColumnName("UPDATE_RULE");
        });

        modelBuilder.Entity<ForTemporaryShiftChangeGrid>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ForTemporaryShiftChangeGrid");

            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.ShortName).HasMaxLength(5);
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Grade");

            entity.ToTable("Grade");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<GradeTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GRADE_TEMP");

            entity.Property(e => e.Gradename)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("GRADENAME");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Shortname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SHORTNAME");
        });

        modelBuilder.Entity<GroupAssociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.GroupAssociation");

            entity.ToTable("GroupAssociation");

            entity.HasIndex(e => e.EmployeeId, "IX_EmployeeId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.EmployeeId).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.GroupAssociations)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_dbo.GroupAssociation_dbo.Staff_EmployeeId");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Holiday");

            entity.ToTable("Holiday");

            entity.HasIndex(e => e.LeaveTypeId, "IX_LeaveTypeId");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.LeaveType).WithMany(p => p.Holidays)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_dbo.Holiday_dbo.LeaveType_LeaveTypeId");
        });

        modelBuilder.Entity<HolidayFixedDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayFixedDay");

            entity.ToTable("HolidayFixedDay");

            entity.HasIndex(e => e.HolidayId, "IX_HolidayId");

            entity.Property(e => e.HolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayDateTo).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Holiday).WithMany(p => p.HolidayFixedDays)
                .HasForeignKey(d => d.HolidayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.HolidayFixedDay_dbo.Holiday_HolidayId");
        });

        modelBuilder.Entity<HolidayGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayGroup");

            entity.ToTable("HolidayGroup");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<HolidayGroupTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayGroupTxn");

            entity.ToTable("HolidayGroupTxn");

            entity.HasIndex(e => e.HolidayGroupId, "IX_HolidayGroupId");

            entity.HasIndex(e => e.HolidayId, "IX_HolidayId");

            entity.Property(e => e.HolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayDateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayGroupId).HasMaxLength(10);

            entity.HasOne(d => d.HolidayGroup).WithMany(p => p.HolidayGroupTxns)
                .HasForeignKey(d => d.HolidayGroupId)
                .HasConstraintName("FK_dbo.HolidayGroupTxn_dbo.HolidayGroup_HolidayGroupId");

            entity.HasOne(d => d.Holiday).WithMany(p => p.HolidayGroupTxns)
                .HasForeignKey(d => d.HolidayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.HolidayGroupTxn_dbo.Holiday_HolidayId");
        });

        modelBuilder.Entity<HolidayWorking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayWorking");

            entity.ToTable("HolidayWorking");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftId).HasMaxLength(10);
            entity.Property(e => e.ShiftInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(20);
            entity.Property(e => e.TxnDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<HolidayWorkingApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HolidayWorkingApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.HolidayWorkingId).HasMaxLength(20);
            entity.Property(e => e.HolidayWorkingReason).HasMaxLength(200);
            entity.Property(e => e.InTime)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OutTime)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HolidayZone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayZone");

            entity.ToTable("HolidayZone");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<HolidayZoneTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HolidayZoneTxn");

            entity.ToTable("HolidayZoneTxn");

            entity.HasIndex(e => e.HolidayId, "IX_HolidayId");

            entity.HasIndex(e => e.HolidayZoneId, "IX_HolidayZoneId");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Holiday).WithMany(p => p.HolidayZoneTxns)
                .HasForeignKey(d => d.HolidayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.HolidayZoneTxn_dbo.Holiday_HolidayId");

            entity.HasOne(d => d.HolidayZone).WithMany(p => p.HolidayZoneTxns)
                .HasForeignKey(d => d.HolidayZoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.HolidayZoneTxn_dbo.HolidayZone_HolidayZoneId");
        });

        modelBuilder.Entity<InStaff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__InStaffs__96D4AB17029FB26B");

            entity.Property(e => e.StaffId)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LateComingForAttendance>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LateComingForAttendance");

            entity.Property(e => e.EarlyHours).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LateHours).HasColumnType("smalldatetime");
            entity.Property(e => e.MonthCount)
                .HasDefaultValue(0.0m)
                .HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ShiftIn).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOut).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.SwipeIn).HasColumnType("smalldatetime");
            entity.Property(e => e.SwipeOut).HasColumnType("smalldatetime");
            entity.Property(e => e.TxnDate).HasColumnType("smalldatetime");
            entity.Property(e => e.WeekCount)
                .HasDefaultValue(0.0m)
                .HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<LaterOff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LaterOff");

            entity.ToTable("LaterOff");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.LaterOffAvailDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LaterOffReqDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.LaterOffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.LaterOff_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<LaterOffDate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LaterOffDate");

            entity.ToTable("LaterOffDate");

            entity.Property(e => e.ActionDate).HasColumnType("smalldatetime");
            entity.Property(e => e.CompanyId).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<LeaveApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveApplication");

            entity.ToTable("LeaveApplication");

            entity.HasIndex(e => e.CreatedBy, "IX_CreatedBy");

            entity.HasIndex(e => e.DurationId, "IX_DurationId");

            entity.HasIndex(e => e.LeaveTypeId, "IX_LeaveTypeId");

            entity.HasIndex(e => e.ModifiedBy, "IX_ModifiedBy");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveEndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveReason).HasMaxLength(200);
            entity.Property(e => e.LeaveStartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveApplicationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_dbo.LeaveApplication_dbo.Staff_CreatedBy");

            entity.HasOne(d => d.Duration).WithMany(p => p.LeaveApplications)
                .HasForeignKey(d => d.DurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.LeaveApplication_dbo.LeaveDuration_DurationId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveApplications)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_dbo.LeaveApplication_dbo.LeaveType_LeaveTypeId");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.LeaveApplicationModifiedByNavigations)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_dbo.LeaveApplication_dbo.Staff_ModifiedBy");

            entity.HasOne(d => d.Staff).WithMany(p => p.LeaveApplicationStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.LeaveApplication_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<LeaveApplicationDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApplicationId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LeaveApplicationDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LeaveApplicationDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.EndDuration)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.LeaveType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.StartDuration)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LeaveApplicationHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LeaveApplicationHistory");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(4000)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ApprovalStatusById).HasMaxLength(10);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.Approvalstatusbyname)
                .HasMaxLength(4000)
                .HasColumnName("APPROVALSTATUSBYName");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.DurationName).HasMaxLength(50);
            entity.Property(e => e.LeaveApplicationId).HasMaxLength(10);
            entity.Property(e => e.LeaveEndDate)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.LeaveReason).HasMaxLength(200);
            entity.Property(e => e.LeaveStartDate)
                .HasMaxLength(36)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Leavename)
                .HasMaxLength(50)
                .HasColumnName("LEAVENAME");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<LeaveCreditDebitReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveCreditDebitReason");

            entity.ToTable("LeaveCreditDebitReason");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<LeaveDebit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveDebits");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Dcflag)
                .HasMaxLength(1)
                .HasColumnName("DCFlag");
            entity.Property(e => e.LeaveType).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<LeaveDuration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveDuration");

            entity.ToTable("LeaveDuration");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LeaveGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveGroup");

            entity.ToTable("LeaveGroup");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LeaveGroupTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveGroupTxn");

            entity.ToTable("LeaveGroupTxn");

            entity.HasIndex(e => e.LeaveGroupId, "IX_LeaveGroupId");

            entity.HasIndex(e => e.LeaveTypeId, "IX_LeaveTypeId");

            entity.Property(e => e.CalcToWorkingDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ConsiderPh).HasColumnName("ConsiderPH");
            entity.Property(e => e.ConsiderWo).HasColumnName("ConsiderWO");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CreditDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditFreq).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EncashmentLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Lcafor).HasColumnName("LCAFor");
            entity.Property(e => e.LeaveGroupId).HasMaxLength(10);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.MaxAccDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaxAccYears).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaxDaysPerReq).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinDaysPerReq).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");

            entity.HasOne(d => d.LeaveGroup).WithMany(p => p.LeaveGroupTxns)
                .HasForeignKey(d => d.LeaveGroupId)
                .HasConstraintName("FK_dbo.LeaveGroupTxn_dbo.LeaveGroup_LeaveGroupId");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveGroupTxns)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_dbo.LeaveGroupTxn_dbo.LeaveType_LeaveTypeId");
        });

        modelBuilder.Entity<LeaveReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveReason");

            entity.ToTable("LeaveReason");
        });

        modelBuilder.Entity<LeaveThresholdConfiguration>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK_dbo.LeaveThresholdConfiguration");

            entity.ToTable("LeaveThresholdConfiguration");

            entity.Property(e => e.DepartmentId).HasMaxLength(128);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Threshold).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<LeaveTransactionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveTransactionType");

            entity.ToTable("LeaveTransactionType");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LeaveType");

            entity.ToTable("LeaveType");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<Leavebalance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LEAVEBALANCE");

            entity.Property(e => e.AvailableBalance)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveGroupId).HasMaxLength(10);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Leavebalance1)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("LEAVEBALANCE");
            entity.Property(e => e.StaffId).HasMaxLength(20);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Location");

            entity.ToTable("Location");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode).HasMaxLength(10);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<LogItem>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_dbo.LogItem");

            entity.ToTable("LogItem");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Message).HasMaxLength(500);
        });

        modelBuilder.Entity<LopattUpdate030223>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOPAtt_Update_030223");

            entity.Property(e => e.AttDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AttStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MaintenanceOff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MaintenanceOff");

            entity.ToTable("MaintenanceOff");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.DateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MoffYear).HasColumnName("MOffYear");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.MaintenanceOffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.MaintenanceOff_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<ManualPunch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ManualPunch");

            entity.ToTable("ManualPunch");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.InDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.OutDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.ManualPunches)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.ManualPunch_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<ManualPunchDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ManualPunchDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.InDateTime).HasColumnType("datetime");
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.OutDateTime).HasColumnType("datetime");
            entity.Property(e => e.PunchType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ManualShiftChangeGrid>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ManualShiftChangeGrid");

            entity.ToTable("ManualShiftChangeGrid");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TxnDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ManualShiftConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ManualShiftConfiguration");

            entity.ToTable("ManualShiftConfiguration");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Date).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftId).HasMaxLength(10);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<MarchManualStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("March_ManualStatus");

            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<MaritalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MaritalStatus");

            entity.ToTable("MaritalStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MenuType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MenuType");

            entity.ToTable("MenuType");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<MoffYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MOffYear");

            entity.ToTable("MOffYear");

            entity.Property(e => e.MoffEndDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("MOffEndDate");
            entity.Property(e => e.MoffStartDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("MOffStartDate");
        });

        modelBuilder.Entity<OnDutyDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OnDutyDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.From).HasColumnType("datetime");
            entity.Property(e => e.FromDuration)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Odtype)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("ODType");
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.To).HasColumnType("datetime");
            entity.Property(e => e.ToDuration)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OneRuleValue>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OneRuleValue");

            entity.Property(e => e.CategoryId).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Defaultvalue)
                .HasMaxLength(50)
                .HasColumnName("defaultvalue");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Rulegroupid).HasColumnName("rulegroupid");
            entity.Property(e => e.Ruleid).HasColumnName("ruleid");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName("value");
        });

        modelBuilder.Entity<Otapplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.OTApplication");

            entity.ToTable("OTApplication");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.InTime)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Otdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("OTDate");
            entity.Property(e => e.Otduration)
                .HasColumnType("smalldatetime")
                .HasColumnName("OTDuration");
            entity.Property(e => e.Otreason)
                .HasMaxLength(200)
                .HasColumnName("OTReason");
            entity.Property(e => e.Ottime)
                .HasMaxLength(20)
                .HasColumnName("OTTime");
            entity.Property(e => e.OutTime)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.Otapplications)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.OTApplication_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<OtapplicationEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.OTApplicationEntry");

            entity.ToTable("OTApplicationEntry");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<OtbookingDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OTBookingDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Otdate)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("OTDate");
            entity.Property(e => e.Ottype)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("OTType");
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<PeopleSoftDump>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PeopleSoftDump");

            entity.ToTable("PeopleSoftDump");

            entity.Property(e => e.Absent).HasMaxLength(255);
            entity.Property(e => e.Address1).HasMaxLength(255);
            entity.Property(e => e.Address2).HasMaxLength(255);
            entity.Property(e => e.Address3).HasMaxLength(255);
            entity.Property(e => e.BusinessArea).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CompId).HasMaxLength(255);
            entity.Property(e => e.Company).HasMaxLength(255);
            entity.Property(e => e.CostCentre).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DataOrigin).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasMaxLength(255);
            entity.Property(e => e.Department).HasMaxLength(255);
            entity.Property(e => e.DeptId).HasMaxLength(255);
            entity.Property(e => e.Designation).HasMaxLength(255);
            entity.Property(e => e.DomainId)
                .HasMaxLength(255)
                .HasColumnName("DomainID");
            entity.Property(e => e.Dummy2).HasMaxLength(255);
            entity.Property(e => e.Dummy3).HasMaxLength(255);
            entity.Property(e => e.Dummy5).HasMaxLength(255);
            entity.Property(e => e.Dummy6).HasMaxLength(255);
            entity.Property(e => e.Dummy7).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmpCode).HasMaxLength(255);
            entity.Property(e => e.EmployeeStatus).HasMaxLength(255);
            entity.Property(e => e.FatherName).HasMaxLength(255);
            entity.Property(e => e.Flag).HasMaxLength(255);
            entity.Property(e => e.Flag2).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.ImportFileName).HasMaxLength(50);
            entity.Property(e => e.IsSentToSmax).HasColumnName("IsSentToSMAX");
            entity.Property(e => e.JoiningDate).HasMaxLength(255);
            entity.Property(e => e.LeaveBalance).HasMaxLength(255);
            entity.Property(e => e.LeaveTaken).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.LocationDesc).HasMaxLength(255);
            entity.Property(e => e.Lop)
                .HasMaxLength(255)
                .HasColumnName("LOP");
            entity.Property(e => e.Lta)
                .HasMaxLength(255)
                .HasColumnName("LTA");
            entity.Property(e => e.Ltastatus)
                .HasMaxLength(255)
                .HasColumnName("LTAStatus");
            entity.Property(e => e.Mobile).HasMaxLength(255);
            entity.Property(e => e.Moff).HasMaxLength(255);
            entity.Property(e => e.MoffStatus)
                .HasMaxLength(255)
                .HasColumnName("MOffStatus");
            entity.Property(e => e.Month).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NamePrefix).HasMaxLength(255);
            entity.Property(e => e.NoOfWorkedDays).HasMaxLength(255);
            entity.Property(e => e.NoofWorkingDays).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Plant).HasMaxLength(255);
            entity.Property(e => e.Postal).HasMaxLength(255);
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.PsoftEmpId)
                .HasMaxLength(255)
                .HasColumnName("PSoftEmpId");
            entity.Property(e => e.Rhstatus)
                .HasMaxLength(255)
                .HasColumnName("RHStatus");
            entity.Property(e => e.SanctionLeave).HasMaxLength(255);
            entity.Property(e => e.SentToSmaxon)
                .HasColumnType("smalldatetime")
                .HasColumnName("SentToSMAXOn");
            entity.Property(e => e.State).HasMaxLength(255);
            entity.Property(e => e.SupervisorEmail1).HasMaxLength(255);
            entity.Property(e => e.SupervisorEmail2).HasMaxLength(255);
            entity.Property(e => e.SupervisorName1).HasMaxLength(255);
            entity.Property(e => e.SupervisorName2).HasMaxLength(255);
            entity.Property(e => e.Team).HasMaxLength(255);
            entity.Property(e => e.TotalLeave).HasMaxLength(255);
            entity.Property(e => e.WorkWeekPattern).HasMaxLength(255);
        });

        modelBuilder.Entity<PermissionApp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PermissionApp");

            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PermissionDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PermissionDump");

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasColumnType("datetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.PermissionDate).HasColumnType("datetime");
            entity.Property(e => e.PermissionType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PermissionType");

            entity.ToTable("PermissionType");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PoCheck010324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_010324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck01062024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_01062024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck01102024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_01102024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck01122024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_01122024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck020324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_020324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck020424>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_020424");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02052024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02052024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02072024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02072024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02092024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02092024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02112024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02112024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck02122024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_02122024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck03012025>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_03012025");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck03072024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_03072024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck03102024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_03102024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck040324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_040324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck04052024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_04052024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck04062024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_04062024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck04092024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_04092024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck04112024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_04112024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck05022025>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_05022025");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck05082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_05082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck060424>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_060424");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck06082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_06082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck10052024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_10052024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck10082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_10082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck14082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_14082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck14092024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_14092024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck230324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_230324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck230424>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_230424");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck23052024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_23052024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck270424>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_270424");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck28112024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_28112024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck290224>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_290224");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck290324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_290324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck300123>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_300123");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck300324>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_300324");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck300424>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_300424");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck30082024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_30082024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PoCheck31072024>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PO_Check_31072024");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ExpAttStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PolicyDocUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PolicyDocUpload");

            entity.ToTable("PolicyDocUpload");

            entity.Property(e => e.CancelledOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.IsCancelled).HasColumnName("isCancelled");
        });

        modelBuilder.Entity<PrattStatusUpdate030223>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PRAtt_StatusUpdate_030223");

            entity.Property(e => e.AttDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AttStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PrefixSuffixSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PrefixSuffixSetting");

            entity.ToTable("PrefixSuffixSetting");

            entity.HasIndex(e => e.LeaveTypeId, "IX_LeaveTypeId");

            entity.HasIndex(e => e.PrefixLeaveTypeId, "IX_PrefixLeaveTypeId");

            entity.HasIndex(e => e.SuffixLeaveTypeId, "IX_SuffixLeaveTypeId");

            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.PrefixLeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.SuffixLeaveTypeId).HasMaxLength(10);

            entity.HasOne(d => d.LeaveType).WithMany(p => p.PrefixSuffixSettingLeaveTypes)
                .HasForeignKey(d => d.LeaveTypeId)
                .HasConstraintName("FK_dbo.PrefixSuffixSetting_dbo.LeaveType_LeaveTypeId");

            entity.HasOne(d => d.PrefixLeaveType).WithMany(p => p.PrefixSuffixSettingPrefixLeaveTypes)
                .HasForeignKey(d => d.PrefixLeaveTypeId)
                .HasConstraintName("FK_dbo.PrefixSuffixSetting_dbo.LeaveType_PrefixLeaveTypeId");

            entity.HasOne(d => d.SuffixLeaveType).WithMany(p => p.PrefixSuffixSettingSuffixLeaveTypes)
                .HasForeignKey(d => d.SuffixLeaveTypeId)
                .HasConstraintName("FK_dbo.PrefixSuffixSetting_dbo.LeaveType_SuffixLeaveTypeId");
        });

        modelBuilder.Entity<ProductiveHdmarking>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ProductiveHDMarking");

            entity.Property(e => e.AttDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AttStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RawPunchDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RawPunchDetails");

            entity.Property(e => e.DeReadertype)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DE_READERTYPE");
            entity.Property(e => e.TrChid)
                .HasMaxLength(16)
                .HasColumnName("TR_CHID");
            entity.Property(e => e.TrDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("TR_DATE");
            entity.Property(e => e.TrTime)
                .HasColumnType("smalldatetime")
                .HasColumnName("TR_TIME");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Reader");

            entity.ToTable("Reader");

            entity.HasIndex(e => e.DoorId, "IX_DoorId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.DoorId).HasMaxLength(10);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MofifiedBy).HasMaxLength(20);

            entity.HasOne(d => d.Door).WithMany(p => p.Readers)
                .HasForeignKey(d => d.DoorId)
                .HasConstraintName("FK_dbo.Reader_dbo.Door_DoorId");
        });

        modelBuilder.Entity<RelationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RelationType");

            entity.ToTable("RelationType");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ReportToBeSent>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_dbo.ReportToBeSent");

            entity.ToTable("ReportToBeSent");

            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ReportToBeSentTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ReportToBeSentTxn");

            entity.ToTable("ReportToBeSentTxn");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Duration).HasMaxLength(5);
            entity.Property(e => e.LastRunTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.NextRunTime).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(20);
        });

        modelBuilder.Entity<Reportingtree>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("REPORTINGTREE");

            entity.Property(e => e.Repmgrfirstname)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AI")
                .HasColumnName("REPMGRFIRSTNAME");
            entity.Property(e => e.Repmgrlastname)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AI")
                .HasColumnName("REPMGRLASTNAME");
            entity.Property(e => e.Repmgrmiddlename)
                .HasMaxLength(50)
                .UseCollation("Latin1_General_CI_AI")
                .HasColumnName("REPMGRMIDDLENAME");
            entity.Property(e => e.Reporteeid)
                .HasMaxLength(20)
                .UseCollation("Latin1_General_CI_AI")
                .HasColumnName("REPORTEEID");
            entity.Property(e => e.Reportingmgrid)
                .HasMaxLength(10)
                .UseCollation("Latin1_General_CI_AI")
                .HasColumnName("REPORTINGMGRID");
        });

        modelBuilder.Entity<ReportsByEmail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ReportsByEmail");

            entity.ToTable("ReportsByEmail");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FunctionName).HasMaxLength(50);
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ParameterList).HasMaxLength(1000);
            entity.Property(e => e.ReportDescription).HasMaxLength(50);
            entity.Property(e => e.ReportPara1).HasMaxLength(1000);
            entity.Property(e => e.ReportPara3).HasMaxLength(1000);
            entity.Property(e => e.ReportSubject).HasMaxLength(50);
        });

        modelBuilder.Entity<RequestApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RequestApplication");

            entity.ToTable("RequestApplication");

            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.CancelledBy).HasMaxLength(50);
            entity.Property(e => e.CancelledDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.DurationOfHoursExtension)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ExpiryDate)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.HoursAfterShift).HasColumnType("smalldatetime");
            entity.Property(e => e.HoursBeforeShift).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.NewShiftId).HasMaxLength(10);
            entity.Property(e => e.Odduration)
                .HasMaxLength(20)
                .HasColumnName("ODDuration");
            entity.Property(e => e.Otrange)
                .HasMaxLength(20)
                .HasColumnName("OTRange");
            entity.Property(e => e.Ottype)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("OTType");
            entity.Property(e => e.PermissionType).HasMaxLength(20);
            entity.Property(e => e.PunchType).HasMaxLength(5);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.RequestApplicationType)
                .HasMaxLength(5)
                .HasDefaultValue("");
            entity.Property(e => e.Rhid).HasColumnName("RHId");
            entity.Property(e => e.ShiftExtensionType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.TotalDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalHours).HasColumnType("smalldatetime");
            entity.Property(e => e.WorkedDate)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<RestrictedHoliday>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RestrictedHolidays");

            entity.HasIndex(e => e.CompanyId, "IX_CompanyId");

            entity.HasIndex(e => e.LeaveId, "IX_LeaveId");

            entity.Property(e => e.CompanyId).HasMaxLength(10);
            entity.Property(e => e.ImportDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveId).HasMaxLength(10);
            entity.Property(e => e.Rhdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("RHDate");
            entity.Property(e => e.Rhyear).HasColumnName("RHYear");

            entity.HasOne(d => d.Company).WithMany(p => p.RestrictedHolidays)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RestrictedHolidays_dbo.Company_CompanyId");

            entity.HasOne(d => d.Leave).WithMany(p => p.RestrictedHolidays)
                .HasForeignKey(d => d.LeaveId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RestrictedHolidays_dbo.LeaveType_LeaveId");
        });

        modelBuilder.Entity<RolesAndResponsibility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RolesAndResponsibilities");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Rule");

            entity.ToTable("Rule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datatype)
                .HasMaxLength(50)
                .HasColumnName("datatype");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Ruletype)
                .HasMaxLength(50)
                .HasColumnName("ruletype");
        });

        modelBuilder.Entity<RuleGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RuleGroup");

            entity.ToTable("RuleGroup");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<RuleGroupTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.RuleGroupTxn");

            entity.ToTable("RuleGroupTxn");

            entity.HasIndex(e => e.CategoryId, "IX_CategoryId");

            entity.HasIndex(e => e.Rulegroupid, "IX_rulegroupid");

            entity.HasIndex(e => e.Ruleid, "IX_ruleid");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Defaultvalue)
                .HasMaxLength(50)
                .HasColumnName("defaultvalue");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Rulegroupid).HasColumnName("rulegroupid");
            entity.Property(e => e.Ruleid).HasColumnName("ruleid");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .HasColumnName("value");

            entity.HasOne(d => d.Rulegroup).WithMany(p => p.RuleGroupTxns)
                .HasForeignKey(d => d.Rulegroupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RuleGroupTxn_dbo.RuleGroup_rulegroupid");

            entity.HasOne(d => d.Rule).WithMany(p => p.RuleGroupTxns)
                .HasForeignKey(d => d.Ruleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.RuleGroupTxn_dbo.Rule_ruleid");
        });

        modelBuilder.Entity<Salutation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Salutation");

            entity.ToTable("Salutation");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Screen");

            entity.ToTable("Screen");

            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.ScreenName).HasMaxLength(50);
        });

        modelBuilder.Entity<Screen1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Screens");

            entity.ToTable("Screens");

            entity.Property(e => e.ActionName).HasMaxLength(101);
            entity.Property(e => e.ControllerName).HasMaxLength(51);
            entity.Property(e => e.CreatedBy).HasMaxLength(30);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.MenuIcon).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(30);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.RankingId).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ScreenName).HasMaxLength(101);
        });

        modelBuilder.Entity<ScreenTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ScreenTxn");

            entity.ToTable("ScreenTxn");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.LocationId).HasMaxLength(10);

            entity.HasOne(d => d.Screen).WithMany(p => p.ScreenTxns)
                .HasForeignKey(d => d.ScreenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.ScreenTxn_dbo.Screens_ScreenId");
        });

        modelBuilder.Entity<SecurityGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SecurityGroup");

            entity.ToTable("SecurityGroup");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Settings");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DefaultValue).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Parameter).HasMaxLength(50);
            entity.Property(e => e.Value).HasMaxLength(50);
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Shifts");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.BreakEndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.BreakStartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.GraceEarlyBy)
                .HasColumnType("smalldatetime")
                .HasColumnName("GraceEarlyBY");
            entity.Property(e => e.GraceLateBy).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayGroupId).HasMaxLength(10);
            entity.Property(e => e.LocationId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MinDayHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinWeekHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(5);
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShiftChangeApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftChangeApplication");

            entity.ToTable("ShiftChangeApplication");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValue(new DateTime(1900, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.NewShiftId).HasMaxLength(10);
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShiftChangeApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ShiftChangeApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationId).HasMaxLength(20);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2On)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval2statusName).HasMaxLength(60);
            entity.Property(e => e.Approved1On)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.EndDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.NewShiftName).HasMaxLength(50);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StaffName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.TotalHours)
                .HasMaxLength(8)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShiftExtensionAndDoubleShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftExtensionAndDoubleShift");

            entity.ToTable("ShiftExtensionAndDoubleShift");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DurationOfHoursExtension).HasMaxLength(30);
            entity.Property(e => e.Shift1).HasMaxLength(50);
            entity.Property(e => e.Shift2).HasMaxLength(50);
            entity.Property(e => e.Shift3).HasMaxLength(50);
            entity.Property(e => e.ShiftExtensionType).HasMaxLength(30);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TxnDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShiftExtensionApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ShiftExtensionApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ExtensionDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.HoursAfterShift)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.HoursBeforeShift)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.ShiftExtensionId).HasMaxLength(20);
            entity.Property(e => e.ShiftExtensionReason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<ShiftExtensionDump>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ShiftExtensionDump");

            entity.Property(e => e.AfterShiftHours).HasColumnType("datetime");
            entity.Property(e => e.BeforeShiftHours).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ExtensionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ShiftExtensionDate).HasColumnType("datetime");
            entity.Property(e => e.StaffId)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShiftExtensionHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ShiftExtensionHistory");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatus1).HasMaxLength(60);
            entity.Property(e => e.ApprovalStatus2).HasMaxLength(60);
            entity.Property(e => e.CancelledBy).HasMaxLength(50);
            entity.Property(e => e.CancelledDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.DurationOfHoursExtension)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.HoursAfterShift)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.HoursBeforeShift)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StaffName).HasMaxLength(50);
            entity.Property(e => e.TxnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShiftParentTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftParentTxn");

            entity.ToTable("ShiftParentTxn");

            entity.Property(e => e.BranchId).HasMaxLength(10);
            entity.Property(e => e.CategoryId).HasMaxLength(10);
            entity.Property(e => e.CompanyId).HasMaxLength(10);
            entity.Property(e => e.CostCentreId).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.DepartmentId).HasMaxLength(10);
            entity.Property(e => e.DesignationId).HasMaxLength(10);
            entity.Property(e => e.DivisionId).HasMaxLength(10);
            entity.Property(e => e.GradeId).HasMaxLength(10);
            entity.Property(e => e.LocationId).HasMaxLength(10);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(1);
            entity.Property(e => e.ShiftId).HasMaxLength(10);
            entity.Property(e => e.VolumeId).HasMaxLength(10);
        });

        modelBuilder.Entity<ShiftPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftPattern");

            entity.ToTable("ShiftPattern");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UpdatedUntil).HasColumnType("smalldatetime");
            entity.Property(e => e.WodayOffSet).HasColumnName("WODayOffSet");
            entity.Property(e => e.WolastUpdatedDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("WOLastUpdatedDate");
            entity.Property(e => e.WostartDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("WOStartDate");
        });

        modelBuilder.Entity<ShiftPatternTxn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftPatternTxn");

            entity.ToTable("ShiftPatternTxn");

            entity.HasIndex(e => e.PatternId, "IX_PatternId");

            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(1);

            entity.HasOne(d => d.Pattern).WithMany(p => p.ShiftPatternTxns)
                .HasForeignKey(d => d.PatternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.ShiftPatternTxn_dbo.ShiftPattern_PatternId");
        });

        modelBuilder.Entity<ShiftPlan>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ShiftPlan");

            entity.Property(e => e.ExpectedWorkingHours).HasColumnType("datetime");
            entity.Property(e => e.ShiftId)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ShiftIn).HasColumnType("datetime");
            entity.Property(e => e.ShiftInDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftOut).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShiftPostingPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftPostingPattern");

            entity.ToTable("ShiftPostingPattern");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Friday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Monday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Saturday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Sunday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Thursday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Tuesday)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Wednesday)
                .HasMaxLength(6)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShiftRelay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftRelay");

            entity.ToTable("ShiftRelay");

            entity.Property(e => e.FromTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ToTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ShiftsImportDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftsImportData");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftFromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftId).HasMaxLength(10);
            entity.Property(e => e.ShiftToDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<ShiftsImportDump>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ShiftsImportDump");

            entity.ToTable("ShiftsImportDump");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExcelFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsError).HasDefaultValue(false);
            entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Shift).HasMaxLength(100);
            entity.Property(e => e.ShiftFromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftToDate).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<Shiftwisemail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SHIFTWIS__3214EC27F75F031A");

            entity.ToTable("SHIFTWISEMAIL");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.AbsentCountLastUpdateddate).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PresentCountLastUpdateddate).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<SmaxTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SMaxTransaction");

            entity.HasIndex(e => new { e.TrChId, e.TrTime }, "IDX_SMAXTXN_STAFFID_TR_TIME");

            entity.HasIndex(e => new { e.TrDate, e.TrTime, e.TrChId, e.TrIpaddress, e.TrTtype }, "NonClusteredIndex-20160522-114948").IsUnique();

            entity.Property(e => e.DeName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.DeReadertype)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DE_READERTYPE");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Isprocessed).HasColumnName("isprocessed");
            entity.Property(e => e.SmaxId).HasColumnName("SMAX_Id");
            entity.Property(e => e.TrCardNumber)
                .HasMaxLength(20)
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrChId)
                .HasMaxLength(20)
                .HasColumnName("Tr_ChId");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrId).HasColumnName("TR_ID");
            entity.Property(e => e.TrIpaddress)
                .HasMaxLength(15)
                .HasColumnName("Tr_IPAddress");
            entity.Property(e => e.TrLnId).HasColumnName("Tr_LnId");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrNodeId).HasColumnName("Tr_NodeId");
            entity.Property(e => e.TrOpName)
                .HasMaxLength(50)
                .HasColumnName("Tr_OpName");
            entity.Property(e => e.TrReason).HasColumnName("Tr_Reason");
            entity.Property(e => e.TrSourceCreatedOn)
                .HasColumnType("datetime")
                .HasColumnName("Tr_SourceCreatedOn");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTrackCard).HasColumnName("Tr_TrackCard");
            entity.Property(e => e.TrTtype).HasColumnName("Tr_TType");
            entity.Property(e => e.TrUnit).HasColumnName("Tr_Unit");
        });

        modelBuilder.Entity<Smaxadvmaster>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("smaxadvmaster");

            entity.Property(e => e.ChId)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("designation");
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.Trade)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Smaxreader>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("SMAXREADERS");

            entity.Property(e => e.DeIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DE_IPADDRESS");
            entity.Property(e => e.DeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.DeReadertype)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("DE_READERTYPE");
        });

        modelBuilder.Entity<SmxCardholder>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("smx_cardholder");

            entity.Property(e => e.ChCardNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
        });

        modelBuilder.Entity<SmxReader>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SMX_READER");

            entity.Property(e => e.DeCreated)
                .HasColumnType("datetime")
                .HasColumnName("DE_CREATED");
            entity.Property(e => e.DeDotl).HasColumnName("DE_DOTL");
            entity.Property(e => e.DeDotz)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("DE_DOTZ");
            entity.Property(e => e.DeFirealarm).HasColumnName("DE_FIREALARM");
            entity.Property(e => e.DeFirmware)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_FIRMWARE");
            entity.Property(e => e.DeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DE_ID");
            entity.Property(e => e.DeIntzid).HasColumnName("DE_INTZID");
            entity.Property(e => e.DeIp1Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_IP1_NAME");
            entity.Property(e => e.DeIp1Nonc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_IP1_NONC");
            entity.Property(e => e.DeIp2Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_IP2_NAME");
            entity.Property(e => e.DeIp2Nonc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_IP2_NONC");
            entity.Property(e => e.DeIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DE_IPADDRESS");
            entity.Property(e => e.DeLnId).HasColumnName("DE_LN_ID");
            entity.Property(e => e.DeMemory)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_MEMORY");
            entity.Property(e => e.DeMessage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_MESSAGE");
            entity.Property(e => e.DeModel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_MODEL");
            entity.Property(e => e.DeModified)
                .HasColumnType("datetime")
                .HasColumnName("DE_MODIFIED");
            entity.Property(e => e.DeModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_MODIFIEDBY");
            entity.Property(e => e.DeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.DeNodeid).HasColumnName("DE_NODEID");
            entity.Property(e => e.DeOperational)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_OPERATIONAL");
            entity.Property(e => e.DeReadermode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_READERMODE");
            entity.Property(e => e.DeReadertype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_READERTYPE");
            entity.Property(e => e.DeRelaytime).HasColumnName("DE_RELAYTIME");
            entity.Property(e => e.DeRemarks)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DE_Remarks");
        });

        modelBuilder.Entity<SmxSwipedatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SMX_SWIPEDATA");

            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrIpAddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tr_IpAddress");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrNodeId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_NodeId");
            entity.Property(e => e.TrReason)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Reason");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTrackCard)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TrackCard");
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<SqlExecute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SqlExecute");

            entity.ToTable("SqlExecute");

            entity.Property(e => e.CancelledBy).HasMaxLength(20);
            entity.Property(e => e.CancelledOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.ExecuteDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ExecutedBy).HasMaxLength(20);
            entity.Property(e => e.ExecutedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ParentId).HasMaxLength(10);
            entity.Property(e => e.QueryType).HasMaxLength(10);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Staff");

            entity.HasIndex(e => e.SalutationId, "IX_SalutationId");

            entity.HasIndex(e => e.StaffStatusId, "IX_StaffStatusId");

            entity.Property(e => e.Id).HasMaxLength(50);
            entity.Property(e => e.CardCode).HasMaxLength(8);
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(1);
            entity.Property(e => e.IsSentToSmax).HasColumnName("IsSentToSMax");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.PeopleSoftCode)
                .HasMaxLength(10)
                .HasDefaultValue("-");
            entity.Property(e => e.SalutationId).HasDefaultValue(1);
            entity.Property(e => e.ShortName).HasMaxLength(50);

            entity.HasOne(d => d.Salutation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.SalutationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Staff_dbo.Salutation_SalutationId");

            entity.HasOne(d => d.StaffStatus).WithMany(p => p.Staff)
                .HasForeignKey(d => d.StaffStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.Staff_dbo.StaffStatus_StaffStatusId");
        });

        modelBuilder.Entity<StaffEditRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.StaffEditRequest");

            entity.ToTable("StaffEditRequest");

            entity.Property(e => e.Createdon).HasColumnType("smalldatetime");
            entity.Property(e => e.RequestId).HasMaxLength(10);
            entity.Property(e => e.UserId).HasMaxLength(20);
        });

        modelBuilder.Entity<StaffEducation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.StaffEducation");

            entity.ToTable("StaffEducation");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CourseName).HasMaxLength(50);
            entity.Property(e => e.Grade).HasMaxLength(5);
            entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.University).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffEducations)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.StaffEducation_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<StaffFamily>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.StaffFamily");

            entity.ToTable("StaffFamily");

            entity.HasIndex(e => e.RelatedAs, "IX_RelatedAs");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(50);

            entity.HasOne(d => d.RelatedAsNavigation).WithMany(p => p.StaffFamilies)
                .HasForeignKey(d => d.RelatedAs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffFamily_dbo.RelationType_RelatedAs");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffFamilies)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.StaffFamily_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<StaffOfficial>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_dbo.StaffOfficial");

            entity.ToTable("StaffOfficial");

            entity.HasIndex(e => e.BranchId, "IX_BranchId");

            entity.HasIndex(e => e.CategoryId, "IX_CategoryId");

            entity.HasIndex(e => e.CompanyId, "IX_CompanyId");

            entity.HasIndex(e => e.CostCentreId, "IX_CostCentreId");

            entity.HasIndex(e => e.DepartmentId, "IX_DepartmentId");

            entity.HasIndex(e => e.DesignationId, "IX_DesignationId");

            entity.HasIndex(e => e.DivisionId, "IX_DivisionId");

            entity.HasIndex(e => e.GradeId, "IX_GradeId");

            entity.HasIndex(e => e.HolidayGroupId, "IX_HolidayGroupId");

            entity.HasIndex(e => e.LeaveGroupId, "IX_LeaveGroupId");

            entity.HasIndex(e => e.LocationId, "IX_LocationId");

            entity.HasIndex(e => e.PolicyId, "IX_PolicyId");

            entity.HasIndex(e => e.SecurityGroupId, "IX_SecurityGroupId");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.HasIndex(e => e.VolumeId, "IX_VolumeId");

            entity.HasIndex(e => e.WorkingDayPatternId, "IX_WorkingDayPatternId");

            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Approver2).HasMaxLength(20);
            entity.Property(e => e.BranchId).HasMaxLength(10);
            entity.Property(e => e.CategoryId).HasMaxLength(10);
            entity.Property(e => e.CompanyId).HasMaxLength(10);
            entity.Property(e => e.ConfirmationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.CostCentreId).HasMaxLength(10);
            entity.Property(e => e.DateOfJoining).HasColumnType("smalldatetime");
            entity.Property(e => e.DateOfRelieving).HasColumnType("smalldatetime");
            entity.Property(e => e.DepartmentId).HasMaxLength(10);
            entity.Property(e => e.DesignationId).HasMaxLength(10);
            entity.Property(e => e.DivisionId).HasMaxLength(10);
            entity.Property(e => e.DomainId).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Esino)
                .HasMaxLength(50)
                .HasColumnName("ESINo");
            entity.Property(e => e.Fax).HasMaxLength(15);
            entity.Property(e => e.GradeId).HasMaxLength(10);
            entity.Property(e => e.IsOteligible).HasColumnName("IsOTEligible");
            entity.Property(e => e.LeaveGroupId).HasMaxLength(10);
            entity.Property(e => e.LocationId).HasMaxLength(10);
            entity.Property(e => e.Pfno)
                .HasMaxLength(50)
                .HasColumnName("PFNo");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.ReportingManager).HasMaxLength(20);
            entity.Property(e => e.ResignationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Tenure).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VolumeId).HasMaxLength(10);

            entity.HasOne(d => d.Branch).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Branch_BranchId");

            entity.HasOne(d => d.Category).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Category_CategoryId");

            entity.HasOne(d => d.Company).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Company_CompanyId");

            entity.HasOne(d => d.CostCentre).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.CostCentreId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.CostCentre_CostCentreId");

            entity.HasOne(d => d.Department).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Department_DepartmentId");

            entity.HasOne(d => d.Designation).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Designation_DesignationId");

            entity.HasOne(d => d.Division).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Division_DivisionId");

            entity.HasOne(d => d.Grade).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.GradeId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Grade_GradeId");

            entity.HasOne(d => d.HolidayGroup).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.HolidayGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.HolidayZone_HolidayGroupId");

            entity.HasOne(d => d.LeaveGroup).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.LeaveGroupId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.LeaveGroup_LeaveGroupId");

            entity.HasOne(d => d.Location).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Location_LocationId");

            entity.HasOne(d => d.Policy).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.RuleGroup_PolicyId");

            entity.HasOne(d => d.SecurityGroup).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.SecurityGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.SecurityGroup_SecurityGroupId");

            entity.HasOne(d => d.Staff).WithOne(p => p.StaffOfficial)
                .HasForeignKey<StaffOfficial>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Staff_StaffId");

            entity.HasOne(d => d.Volume).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.VolumeId)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.Volume_VolumeId");

            entity.HasOne(d => d.WorkingDayPattern).WithMany(p => p.StaffOfficials)
                .HasForeignKey(d => d.WorkingDayPatternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffOfficial_dbo.WorkingDayPattern_WorkingDayPatternId");
        });

        modelBuilder.Entity<StaffPersonal>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_dbo.StaffPersonal");

            entity.ToTable("StaffPersonal");

            entity.HasIndex(e => e.StaffBloodGroup, "IX_StaffBloodGroup");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.HasIndex(e => e.StaffMaritalStatus, "IX_StaffMaritalStatus");

            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.AadharNo).HasMaxLength(12);
            entity.Property(e => e.Addr).HasMaxLength(200);
            entity.Property(e => e.BankAcno)
                .HasMaxLength(50)
                .HasColumnName("BankACNo");
            entity.Property(e => e.BankBranch).HasMaxLength(50);
            entity.Property(e => e.BankIfsccode)
                .HasMaxLength(50)
                .HasColumnName("BankIFSCCode");
            entity.Property(e => e.BankName).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("smalldatetime");
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.DrivingLicense).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmergencyContactNo1).HasMaxLength(12);
            entity.Property(e => e.EmergencyContactNo2).HasMaxLength(12);
            entity.Property(e => e.EmergencyContactPerson1).HasMaxLength(150);
            entity.Property(e => e.EmergencyContactPerson2).HasMaxLength(150);
            entity.Property(e => e.FatherAadharNo).HasMaxLength(12);
            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MarriageDate).HasColumnType("smalldatetime");
            entity.Property(e => e.MotherAadharNo).HasMaxLength(12);
            entity.Property(e => e.MotherName).HasMaxLength(100);
            entity.Property(e => e.Panno)
                .HasMaxLength(10)
                .HasColumnName("PANNo");
            entity.Property(e => e.PassportNo).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Qualification).HasMaxLength(150);
            entity.Property(e => e.State).HasMaxLength(50);

            entity.HasOne(d => d.StaffBloodGroupNavigation).WithMany(p => p.StaffPersonals)
                .HasForeignKey(d => d.StaffBloodGroup)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffPersonal_dbo.BloodGroup_StaffBloodGroup");

            entity.HasOne(d => d.Staff).WithOne(p => p.StaffPersonal)
                .HasForeignKey<StaffPersonal>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffPersonal_dbo.Staff_StaffId");

            entity.HasOne(d => d.StaffMaritalStatusNavigation).WithMany(p => p.StaffPersonals)
                .HasForeignKey(d => d.StaffMaritalStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.StaffPersonal_dbo.MaritalStatus_StaffMaritalStatus");
        });

        modelBuilder.Entity<StaffStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.StaffStatus");

            entity.ToTable("StaffStatus");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Staffview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("STAFFVIEW");

            entity.Property(e => e.AadharNo).HasMaxLength(12);
            entity.Property(e => e.Approver2).HasMaxLength(20);
            entity.Property(e => e.BranchAddress).HasMaxLength(100);
            entity.Property(e => e.BranchCity).HasMaxLength(50);
            entity.Property(e => e.BranchCompanyId).HasMaxLength(10);
            entity.Property(e => e.BranchCountry).HasMaxLength(50);
            entity.Property(e => e.BranchDistrict).HasMaxLength(50);
            entity.Property(e => e.BranchEmail).HasMaxLength(50);
            entity.Property(e => e.BranchFax).HasMaxLength(15);
            entity.Property(e => e.BranchId)
                .HasMaxLength(10)
                .HasColumnName("BranchID");
            entity.Property(e => e.BranchIsActiveCaption)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.BranchName).HasMaxLength(50);
            entity.Property(e => e.BranchPhone).HasMaxLength(15);
            entity.Property(e => e.BranchShortName).HasMaxLength(5);
            entity.Property(e => e.BranchState).HasMaxLength(50);
            entity.Property(e => e.CardCode).HasMaxLength(8);
            entity.Property(e => e.CategoryId).HasMaxLength(10);
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CompanyCstNo).HasMaxLength(20);
            entity.Property(e => e.CompanyId).HasMaxLength(10);
            entity.Property(e => e.CompanyLegalName).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CompanyPanNo).HasMaxLength(20);
            entity.Property(e => e.CompanyPfNo).HasMaxLength(20);
            entity.Property(e => e.CompanyRegisterNo).HasMaxLength(20);
            entity.Property(e => e.CompanyServiceTaxNo).HasMaxLength(20);
            entity.Property(e => e.CompanyShortName).HasMaxLength(50);
            entity.Property(e => e.CompanyTinNo).HasMaxLength(20);
            entity.Property(e => e.CompanyTngsno)
                .HasMaxLength(20)
                .HasColumnName("CompanyTNGSNo");
            entity.Property(e => e.CompanyWebSite).HasMaxLength(50);
            entity.Property(e => e.CostCentreId).HasMaxLength(10);
            entity.Property(e => e.CostCentreName).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DateOfJoining)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DateOfRelieving)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DateOfResignation)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.DeptEmail).HasMaxLength(50);
            entity.Property(e => e.DeptFax).HasMaxLength(15);
            entity.Property(e => e.DeptId).HasMaxLength(10);
            entity.Property(e => e.DeptName).HasMaxLength(100);
            entity.Property(e => e.DeptPhone).HasMaxLength(15);
            entity.Property(e => e.DeptShortName).HasMaxLength(5);
            entity.Property(e => e.DesignationId).HasMaxLength(10);
            entity.Property(e => e.DesignationName).HasMaxLength(150);
            entity.Property(e => e.DesignationShortName).HasMaxLength(10);
            entity.Property(e => e.DivisionId).HasMaxLength(10);
            entity.Property(e => e.DivisionName).HasMaxLength(50);
            entity.Property(e => e.DivisionShortName).HasMaxLength(5);
            entity.Property(e => e.DomainId).HasMaxLength(50);
            entity.Property(e => e.DrivingLicense).HasMaxLength(50);
            entity.Property(e => e.EmergencyContactNo1).HasMaxLength(12);
            entity.Property(e => e.EmergencyContactNo2).HasMaxLength(12);
            entity.Property(e => e.EmployeeGroupId).HasMaxLength(10);
            entity.Property(e => e.EmployeeGroupName).HasMaxLength(50);
            entity.Property(e => e.Esicno)
                .HasMaxLength(50)
                .HasColumnName("ESICNo");
            entity.Property(e => e.FatherName).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(1);
            entity.Property(e => e.GradeId).HasMaxLength(10);
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.GradeShortName).HasMaxLength(5);
            entity.Property(e => e.HolidayGroupName).HasMaxLength(50);
            entity.Property(e => e.HomeAddress).HasMaxLength(200);
            entity.Property(e => e.HomeCity).HasMaxLength(50);
            entity.Property(e => e.HomeCountry).HasMaxLength(50);
            entity.Property(e => e.HomeDistrict).HasMaxLength(50);
            entity.Property(e => e.HomeLocation).HasMaxLength(50);
            entity.Property(e => e.HomePhone).HasMaxLength(15);
            entity.Property(e => e.HomePostalCode).HasMaxLength(10);
            entity.Property(e => e.HomeState).HasMaxLength(50);
            entity.Property(e => e.IsHeadOfficeCaption)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsOteligible).HasColumnName("IsOTEligible");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LeaveGroupId).HasMaxLength(10);
            entity.Property(e => e.LeaveGroupName).HasMaxLength(50);
            entity.Property(e => e.LocationId).HasMaxLength(10);
            entity.Property(e => e.LocationName).HasMaxLength(50);
            entity.Property(e => e.MarriageDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MotherName).HasMaxLength(100);
            entity.Property(e => e.OfficalEmail).HasMaxLength(50);
            entity.Property(e => e.OfficalFax).HasMaxLength(15);
            entity.Property(e => e.OfficialPhone).HasMaxLength(15);
            entity.Property(e => e.PassportNo).HasMaxLength(10);
            entity.Property(e => e.PersonalBankAccount).HasMaxLength(50);
            entity.Property(e => e.PersonalBankBranch).HasMaxLength(50);
            entity.Property(e => e.PersonalBankIfsccode)
                .HasMaxLength(50)
                .HasColumnName("PersonalBankIFSCCode");
            entity.Property(e => e.PersonalBankName).HasMaxLength(50);
            entity.Property(e => e.PersonalEmail).HasMaxLength(50);
            entity.Property(e => e.PersonalPanNo).HasMaxLength(10);
            entity.Property(e => e.Qualification).HasMaxLength(150);
            entity.Property(e => e.Repmgrfirstname)
                .HasMaxLength(50)
                .HasColumnName("REPMGRFIRSTNAME");
            entity.Property(e => e.Repmgrlastname)
                .HasMaxLength(50)
                .HasColumnName("REPMGRLASTNAME");
            entity.Property(e => e.Repmgrmiddlename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("REPMGRMIDDLENAME");
            entity.Property(e => e.Reportingmgrid)
                .HasMaxLength(20)
                .HasColumnName("REPORTINGMGRID");
            entity.Property(e => e.ReviewerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.SecurityGroupName).HasMaxLength(50);
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StatusName).HasMaxLength(50);
            entity.Property(e => e.Uanno)
                .HasMaxLength(50)
                .HasColumnName("UANNo");
            entity.Property(e => e.VolumeId).HasMaxLength(10);
            entity.Property(e => e.VolumeName).HasMaxLength(50);
            entity.Property(e => e.WeeklyOffId).HasMaxLength(10);
            entity.Property(e => e.WeeklyOffName).HasMaxLength(50);
            entity.Property(e => e.WorkStationId).HasMaxLength(10);
            entity.Property(e => e.WorkStationName).HasMaxLength(50);
        });

        modelBuilder.Entity<SubordinateTree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SubordinateTree");

            entity.ToTable("SubordinateTree");

            entity.Property(e => e.ReportingStaffId).HasMaxLength(20);
            entity.Property(e => e.Signature).HasMaxLength(30);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<SwipeDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SwipeData");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.SwipeDate).HasColumnType("smalldatetime");
            entity.Property(e => e.SwipeTime).HasColumnType("smalldatetime");

            entity.HasOne(d => d.Staff).WithMany(p => p.SwipeData)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_dbo.SwipeData_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<TeamHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.TeamHierarchy");

            entity.ToTable("TeamHierarchy");

            entity.HasIndex(e => e.ReportingManagerId, "IX_ReportingManagerId");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.ReportingManagerId).HasMaxLength(50);
            entity.Property(e => e.StaffId)
                .HasMaxLength(50)
                .HasDefaultValue("");

            entity.HasOne(d => d.ReportingManager).WithMany(p => p.TeamHierarchyReportingManagers)
                .HasForeignKey(d => d.ReportingManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.TeamHierarchy_dbo.Staff_ReportingManagerId");

            entity.HasOne(d => d.Staff).WithMany(p => p.TeamHierarchyStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.TeamHierarchy_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<UploadControlTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.UploadControlTable");

            entity.ToTable("UploadControlTable");

            entity.Property(e => e.Filename).HasMaxLength(100);
            entity.Property(e => e.ProcessStatus).HasMaxLength(20);
            entity.Property(e => e.ProcessedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.TypeOfData).HasMaxLength(5);
            entity.Property(e => e.UploadedBy).HasMaxLength(50);
            entity.Property(e => e.UploadedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VaccinatedDetail>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK_dbo.VaccinatedDetails");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(300);
            entity.Property(e => e.FirstVaccinatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.IsFirstVaccination).HasDefaultValue(true);
            entity.Property(e => e.SecondVaccinatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UploadedBy).HasMaxLength(50);
            entity.Property(e => e.UploadedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");

            entity.HasOne(d => d.Staff).WithOne(p => p.VaccinatedDetail)
                .HasForeignKey<VaccinatedDetail>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.VaccinatedDetails_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<VaccinationDetailsDump>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.VaccinationDetailsDump");

            entity.ToTable("VaccinationDetailsDump");

            entity.HasIndex(e => e.StaffId, "IX_StaffId");

            entity.Property(e => e.Comments).HasMaxLength(300);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ErrorMessage).HasMaxLength(500);
            entity.Property(e => e.ExcelFileName).HasMaxLength(150);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.VaccinatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.VaccinationNumber).HasDefaultValue(1);

            entity.HasOne(d => d.Staff).WithMany(p => p.VaccinationDetailsDumps)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.VaccinationDetailsDump_dbo.Staff_StaffId");
        });

        modelBuilder.Entity<ViewApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ViewApproval");

            entity.ToTable("ViewApproval");

            entity.HasIndex(e => e.ApproverId, "IX_ApproverID");

            entity.HasIndex(e => e.LeaveApplicationId, "IX_LeaveApplicationID");

            entity.Property(e => e.ApproverId)
                .HasMaxLength(50)
                .HasColumnName("ApproverID");
            entity.Property(e => e.LeaveApplicationId)
                .HasMaxLength(10)
                .HasColumnName("LeaveApplicationID");

            entity.HasOne(d => d.Approver).WithMany(p => p.ViewApprovals)
                .HasForeignKey(d => d.ApproverId)
                .HasConstraintName("FK_dbo.ViewApproval_dbo.Staff_ApproverID");

            entity.HasOne(d => d.LeaveApplication).WithMany(p => p.ViewApprovals)
                .HasForeignKey(d => d.LeaveApplicationId)
                .HasConstraintName("FK_dbo.ViewApproval_dbo.LeaveApplication_LeaveApplicationID");
        });

        modelBuilder.Entity<VisitorPassApprovalHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.VisitorPassApprovalHierarchy");

            entity.ToTable("VisitorPassApprovalHierarchy");

            entity.Property(e => e.CreateOn).HasColumnType("smalldatetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.GradeId).HasMaxLength(10);
            entity.Property(e => e.ModifiedBy).HasMaxLength(20);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VleadAugBreakExceedPr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Vlead_Aug_BreakExceedPR");

            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BreakHours)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Comments)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ExpectedWorkingHours)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsBreakExceed)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsManualStatus).HasDefaultValue(false);
            entity.Property(e => e.PermissionId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductiveHours)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ShiftInDate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShiftName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Staffid)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VleadDec23AttStatusChange>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Vlead_Dec_23_AttStatusChange");

            entity.Property(e => e.AbsentCount)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.AttendanceStatus)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.DayAccount)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VleadPermissionUpdate010223>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VleadPermissionUpdate_010223");

            entity.Property(e => e.AttStatus)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("FHStatus");
            entity.Property(e => e.PermissionDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Shstatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Volume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Volume");

            entity.ToTable("Volume");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn)
                .HasColumnType("smalldatetime")
                .HasColumnName("MOdifiedOn");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode).HasMaxLength(10);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<VwAbsentList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAbsentList");

            entity.Property(e => e.ActualInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(5);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.ShiftInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwActiveHoliday>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwActiveHolidays");

            entity.Property(e => e.HolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayDateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.LeaveTypeShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<VwApprovedAbsenceDate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwApprovedAbsenceDates");

            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveApplicationReason).HasMaxLength(200);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VwAttendanceDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAttendanceDetails");

            entity.Property(e => e.ActualInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.BranchName).HasMaxLength(50);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Othours)
                .HasColumnType("smalldatetime")
                .HasColumnName("OTHours");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwAttendanceDetailsNew>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAttendanceDetailsNew");

            entity.Property(e => e.AccountedEarlyComingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedEarlyGoingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedLateComingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedLateGoingTime).HasColumnType("datetime");
            entity.Property(e => e.AccountedOttime)
                .HasColumnType("datetime")
                .HasColumnName("AccountedOTTime");
            entity.Property(e => e.ActualInDate).HasColumnType("datetime");
            entity.Property(e => e.ActualInTime).HasColumnType("datetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("datetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("datetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(5);
            entity.Property(e => e.BranchName).HasMaxLength(50);
            entity.Property(e => e.CardCode).HasMaxLength(8);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.CompanyShortName).HasMaxLength(50);
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.DesignationName).HasMaxLength(50);
            entity.Property(e => e.DesignationShortName).HasMaxLength(10);
            entity.Property(e => e.DivisionName).HasMaxLength(50);
            entity.Property(e => e.EarlyComing).HasColumnType("datetime");
            entity.Property(e => e.EarlyGoing).HasColumnType("datetime");
            entity.Property(e => e.ExpectedWorkingHours).HasColumnType("datetime");
            entity.Property(e => e.ExtraBreakTime).HasColumnType("datetime");
            entity.Property(e => e.Fhstatus)
                .HasMaxLength(5)
                .HasColumnName("FHStatus");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.IsOt).HasColumnName("IsOT");
            entity.Property(e => e.IsOtvalid).HasColumnName("IsOTValid");
            entity.Property(e => e.LateComing).HasColumnType("datetime");
            entity.Property(e => e.LateGoing).HasColumnType("datetime");
            entity.Property(e => e.NetWorkedHours).HasColumnType("datetime");
            entity.Property(e => e.Othours)
                .HasColumnType("datetime")
                .HasColumnName("OTHours");
            entity.Property(e => e.ShiftInDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftOutDate).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("datetime");
            entity.Property(e => e.Shstatus)
                .HasMaxLength(5)
                .HasColumnName("SHStatus");
            entity.Property(e => e.StaffId).HasMaxLength(20);
            entity.Property(e => e.VolumeName).HasMaxLength(50);
        });

        modelBuilder.Entity<VwAttendanceList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwAttendanceList");

            entity.Property(e => e.ActualInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(5);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.ShiftInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwBtapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwBTApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.From)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.OdapplicationId)
                .HasMaxLength(20)
                .HasColumnName("ODApplicationId");
            entity.Property(e => e.Oduration)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ODuration");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.To)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Total)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwCoffApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCOffApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2On)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffName).HasMaxLength(50);
            entity.Property(e => e.Approval2statusName).HasMaxLength(60);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.Approved1OnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Approved1OnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CoffAvailDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("COffAvailDate");
            entity.Property(e => e.CoffId)
                .HasMaxLength(20)
                .HasColumnName("COffId");
            entity.Property(e => e.CoffReason)
                .HasMaxLength(200)
                .HasColumnName("COffReason");
            entity.Property(e => e.CoffReqDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("COffReqDate");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ExpiryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VwCoffAvailApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCOffAvailApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2On)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2statusName).HasMaxLength(60);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.Approved1OnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Approved1OnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CoffId)
                .HasMaxLength(20)
                .HasColumnName("COffId");
            entity.Property(e => e.CoffReason)
                .HasMaxLength(200)
                .HasColumnName("COffReason");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.EndDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EndDuration)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.StartDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.StartDuration)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WorkedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VwCoffAvailedHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwCOffAvailedHistory");

            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.AvailDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.CoffId)
                .HasMaxLength(20)
                .HasColumnName("COffId");
            entity.Property(e => e.CoffReason)
                .HasMaxLength(200)
                .HasColumnName("COffReason");
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasMaxLength(20);
            entity.Property(e => e.WorkedDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwEmployeegroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwEMPLOYEEGROUP");

            entity.Property(e => e.Employeegroupid)
                .HasMaxLength(10)
                .HasColumnName("EMPLOYEEGROUPID");
        });

        modelBuilder.Entity<VwEmployeegroupview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwEMPLOYEEGROUPVIEW");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
            entity.Property(e => e.Staffcount).HasColumnName("STAFFCOUNT");
        });

        modelBuilder.Entity<VwHolidayCalendarView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwHolidayCalendarView");

            entity.Property(e => e.Hid).HasColumnName("HID");
            entity.Property(e => e.HolidayDateFrom)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.HolidayDateTo)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Leavetypeid)
                .HasMaxLength(10)
                .HasColumnName("leavetypeid");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<VwHolidayFixedDayView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwHolidayFixedDayView");

            entity.Property(e => e.HolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayDateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Isfixed).HasColumnName("isfixed");
        });

        modelBuilder.Entity<VwHolidayView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwHolidayView");

            entity.Property(e => e.FixedHolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.FixedHolidayDateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<VwHolidayWorkingList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwHolidayWorkingList");

            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApproverStatus1).HasMaxLength(60);
            entity.Property(e => e.ApproverStatus2).HasMaxLength(60);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.InTime)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OutTime)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TransactionDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwLaterOffApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwLaterOffApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(10);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(10);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(10);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CoffId)
                .HasMaxLength(10)
                .HasColumnName("COffId");
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.LaterOffAvailDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.LaterOffReason).HasMaxLength(200);
            entity.Property(e => e.LaterOffReqDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwLeaveApplicationApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwLeaveApplicationApproval");

            entity.Property(e => e.ApplicationApprovalId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval1StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval1StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2On)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval2StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2StatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOn1Date)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOn1Time)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveApplicationId).HasMaxLength(20);
            entity.Property(e => e.LeaveApplicationReason)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.LeaveEndDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.LeaveEndDurationId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveEndDurationName).HasMaxLength(50);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveStartDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.LeaveStartDurationId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveStartDurationName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.LeaveTypeName).HasMaxLength(50);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<VwLeaveApplicationList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwLeaveApplicationList");

            entity.Property(e => e.ApprovalStatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.EndDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.EndDuration).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Iscancelled).HasColumnName("ISCANCELLED");
            entity.Property(e => e.LeaveEndDurationId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.LeaveStartDurationId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.Reason).HasColumnName("REASON");
            entity.Property(e => e.Reasonid).HasColumnName("REASONID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(200)
                .HasColumnName("REMARKS");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StartDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.StartDuration).HasMaxLength(50);
            entity.Property(e => e.Totaldays)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("TOTALDAYS");
        });

        modelBuilder.Entity<VwMaintenanceOffApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwMaintenanceOffApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(10);
            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(10);
            entity.Property(e => e.ApprovalOwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(10);
            entity.Property(e => e.ApprovalStaffName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FromDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.Isflexible)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISFLEXIBLE");
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.MaintenanceOffId).HasMaxLength(10);
            entity.Property(e => e.MaintenanceOffReason).HasMaxLength(200);
            entity.Property(e => e.MoffYear).HasColumnName("MOffYear");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.ToDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwManualPunchApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwManualPunchApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffName).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2OnDate)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval2statusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2statusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.InDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.InTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.ManualPunchId).HasMaxLength(20);
            entity.Property(e => e.ManualPunchReason).HasMaxLength(200);
            entity.Property(e => e.OutDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.OutTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.PunchType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<VwManualPunchList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwManualPunchList");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.InDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.InTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.OutDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.OutTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(60);
            entity.Property(e => e.StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwOdBtWfhApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_OD_BT_WFH_Approval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.OdapplicationId)
                .HasMaxLength(20)
                .HasColumnName("ODApplicationId");
            entity.Property(e => e.Odduration)
                .HasMaxLength(20)
                .HasColumnName("ODDuration");
            entity.Property(e => e.OdfromDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODFromDate");
            entity.Property(e => e.OdfromTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODFromTime");
            entity.Property(e => e.Odreason)
                .HasMaxLength(200)
                .HasColumnName("ODReason");
            entity.Property(e => e.OdtoDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODToDate");
            entity.Property(e => e.OdtoTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODToTime");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Total)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwOdapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwODApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.OdapplicationId)
                .HasMaxLength(20)
                .HasColumnName("ODApplicationId");
            entity.Property(e => e.Odduration)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ODDuration");
            entity.Property(e => e.OdfromDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODFromDate");
            entity.Property(e => e.OdfromTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODFromTime");
            entity.Property(e => e.Odreason)
                .HasMaxLength(200)
                .HasColumnName("ODReason");
            entity.Property(e => e.OdtoDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODToDate");
            entity.Property(e => e.OdtoTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODToTime");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.Total)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwOdrequestList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwODRequestList");

            entity.Property(e => e.CreatedBy).HasMaxLength(20);
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.From)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Oddate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODDate");
            entity.Property(e => e.Odduration)
                .HasMaxLength(15)
                .HasColumnName("ODDuration");
            entity.Property(e => e.Odreason)
                .HasMaxLength(200)
                .HasColumnName("ODReason");
            entity.Property(e => e.StaffId).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(60);
            entity.Property(e => e.StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.To)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwOtapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwOTApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(10);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(20);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.OtapplicationId)
                .HasMaxLength(10)
                .HasColumnName("OTApplicationId");
            entity.Property(e => e.Otdate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("OTDate");
            entity.Property(e => e.Otduration)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("OTDuration");
            entity.Property(e => e.Otreason)
                .HasMaxLength(200)
                .HasColumnName("OTReason");
            entity.Property(e => e.Ottime)
                .HasMaxLength(20)
                .HasColumnName("OTTime");
            entity.Property(e => e.StaffId).HasMaxLength(20);
        });

        modelBuilder.Entity<VwOtrequestList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwOTRequestList");

            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Otdate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("OTDate");
            entity.Property(e => e.Otduration)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("OTDuration");
            entity.Property(e => e.Otreason)
                .HasMaxLength(200)
                .HasColumnName("OTReason");
            entity.Property(e => e.Ottime)
                .HasMaxLength(20)
                .HasColumnName("OTTime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(60);
        });

        modelBuilder.Entity<VwPermanantShiftChangeList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwPermanantShiftChangeList");

            entity.Property(e => e.CreatedBy).HasMaxLength(10);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.PatternName).HasMaxLength(50);
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.StaffName).HasMaxLength(50);
            entity.Property(e => e.WithEffectFrom)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwPermissionApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwPermissionApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1Owner).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval1StaffName).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2On)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2OwnerName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Approval2StatusId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Approval2statusName).HasMaxLength(60);
            entity.Property(e => e.Approved1OnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Approved1OnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FromTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Iscancelledword)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLEDWORD");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.PermissionDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.PermissionId).HasMaxLength(20);
            entity.Property(e => e.PermissionOffReason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.TimeTo)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TotalHours)
                .HasMaxLength(8)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwPresentList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwPresentList");

            entity.Property(e => e.ActualInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ActualWorkedHours).HasColumnType("smalldatetime");
            entity.Property(e => e.AttendanceStatus).HasMaxLength(5);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GradeName).HasMaxLength(50);
            entity.Property(e => e.LeaveName).HasMaxLength(50);
            entity.Property(e => e.ShiftInDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftInTime).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftOutDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("smalldatetime");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwRhapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwRHApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(10);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(10);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(10);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.RhapplicationId)
                .HasMaxLength(128)
                .HasColumnName("RHApplicationId");
            entity.Property(e => e.RhfromDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("RHFromDate");
            entity.Property(e => e.Rhreason).HasColumnName("RHReason");
            entity.Property(e => e.RhtoDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("RHToDate");
            entity.Property(e => e.StaffId).HasMaxLength(10);
        });

        modelBuilder.Entity<VwShiftChangeApproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwShiftChangeApproval");

            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffId).HasMaxLength(50);
            entity.Property(e => e.Approval2StaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffName).HasMaxLength(50);
            entity.Property(e => e.ApprovalStatusName).HasMaxLength(60);
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FromDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.NewShiftId).HasMaxLength(10);
            entity.Property(e => e.NewShiftName).HasMaxLength(50);
            entity.Property(e => e.ShiftChangeId).HasMaxLength(20);
            entity.Property(e => e.ShiftChangeReason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwShiftChangeList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwShiftChangeList");

            entity.Property(e => e.ApplicationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ApproverStatus1).HasMaxLength(60);
            entity.Property(e => e.ApproverStatus2).HasMaxLength(60);
            entity.Property(e => e.DeptName).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.FromDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasMaxLength(20);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.NewShiftId).HasMaxLength(10);
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.ToDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwShiftRelay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwShiftRelay");

            entity.Property(e => e.BreakEndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.BreakStartTime).HasColumnType("smalldatetime");
            entity.Property(e => e.EndTime).HasColumnType("smalldatetime");
            entity.Property(e => e.GraceEarlyBy)
                .HasColumnType("smalldatetime")
                .HasColumnName("GraceEarlyBY");
            entity.Property(e => e.GraceLateBy).HasColumnType("smalldatetime");
            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(5);
            entity.Property(e => e.StartTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VwShiftsandleaf>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwSHIFTSANDLEAVES");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
            entity.Property(e => e.Shortname)
                .HasMaxLength(5)
                .HasColumnName("SHORTNAME");
        });

        modelBuilder.Entity<VwSmaxtransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwSMAXTransaction");

            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.DeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.DeReaderType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("DE_ReaderType");
            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrIpAddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tr_IpAddress");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrNodeId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_NodeId");
            entity.Property(e => e.TrReason)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Reason");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTrackCard)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TrackCard");
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<VwWfhapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwWFHApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.ApprovedOnTime)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.Duration)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.From)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.OdapplicationId)
                .HasMaxLength(20)
                .HasColumnName("ODApplicationId");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.StaffId).HasMaxLength(50);
            entity.Property(e => e.To)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Total)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwZoneWiseHolidayCalendar>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vwZoneWiseHolidayCalendar");

            entity.Property(e => e.HolidayDateFrom).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayDateTo).HasColumnType("smalldatetime");
            entity.Property(e => e.HolidayGroupId).HasMaxLength(10);
            entity.Property(e => e.HolidayGroupName).HasMaxLength(50);
            entity.Property(e => e.HolidayZoneName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.LeaveTypeName).HasMaxLength(50);
            entity.Property(e => e.LeaveTypeShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<Vwabsencedate>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("VWABSENCEDATES");

            entity.Property(e => e.FromDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Iscancelled)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISCANCELLED");
            entity.Property(e => e.LeaveShortName).HasMaxLength(5);
            entity.Property(e => e.LeaveTypeId).HasMaxLength(10);
            entity.Property(e => e.StaffId).HasMaxLength(10);
            entity.Property(e => e.ToDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WeeklyOff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WeeklyOffs");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Wfhapproval>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("WFHApproval");

            entity.Property(e => e.Applicantname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPLICANTNAME");
            entity.Property(e => e.ApplicationApprovalId).HasMaxLength(20);
            entity.Property(e => e.ApplicationId).HasMaxLength(20);
            entity.Property(e => e.ApplicationType).HasMaxLength(5);
            entity.Property(e => e.AppliedBy).HasMaxLength(50);
            entity.Property(e => e.Approval1StatusName).HasMaxLength(60);
            entity.Property(e => e.Approval2By).HasMaxLength(50);
            entity.Property(e => e.Approval2Owner).HasMaxLength(50);
            entity.Property(e => e.Approval2on)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2ON");
            entity.Property(e => e.Approval2ownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVAL2OWNERNAME");
            entity.Property(e => e.Approval2statusname)
                .HasMaxLength(60)
                .HasColumnName("APPROVAL2STATUSNAME");
            entity.Property(e => e.ApprovalOwner).HasMaxLength(50);
            entity.Property(e => e.ApprovalStaffId).HasMaxLength(50);
            entity.Property(e => e.Approvalownername)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALOWNERNAME");
            entity.Property(e => e.Approvalstaffname)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("APPROVALSTAFFNAME");
            entity.Property(e => e.ApprovedOnDate)
                .HasMaxLength(8000)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(200)
                .HasColumnName("COMMENT");
            entity.Property(e => e.IsApprover1Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsApprover2Cancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.IsCancelled)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveShortName)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.LeaveTypeId)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Od)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("OD");
            entity.Property(e => e.Odduration)
                .HasMaxLength(20)
                .HasColumnName("ODDuration");
            entity.Property(e => e.OdfromDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODFromDate");
            entity.Property(e => e.OdfromTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODFromTime");
            entity.Property(e => e.Odreason)
                .HasMaxLength(200)
                .HasColumnName("ODReason");
            entity.Property(e => e.OdtoDate)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("ODToDate");
            entity.Property(e => e.OdtoTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("ODToTime");
            entity.Property(e => e.ParentType).HasMaxLength(5);
            entity.Property(e => e.StaffId).HasMaxLength(50);
        });

        modelBuilder.Entity<WorkingDayPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WorkingDayPattern");

            entity.ToTable("WorkingDayPattern");

            entity.Property(e => e.PatternDesc).HasDefaultValue("");
            entity.Property(e => e.PsCode).HasMaxLength(100);
        });

        modelBuilder.Entity<Workstation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Workstation");

            entity.ToTable("Workstation");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        modelBuilder.Entity<WorkstationAllocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WorkstationAllocation");

            entity.ToTable("WorkstationAllocation");

            entity.HasIndex(e => e.WorkstationId, "IX_WorkstationId");

            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Staffid).HasMaxLength(20);
            entity.Property(e => e.TransactionDate).HasColumnType("smalldatetime");
            entity.Property(e => e.WorkstationId).HasMaxLength(10);

            entity.HasOne(d => d.Workstation).WithMany(p => p.WorkstationAllocations)
                .HasForeignKey(d => d.WorkstationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.WorkstationAllocation_dbo.Workstation_WorkstationId");
        });

        modelBuilder.Entity<Zone>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Zone");

            entity.ToTable("Zone");

            entity.Property(e => e.Id).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PeopleSoftCode).HasMaxLength(10);
            entity.Property(e => e.ShortName).HasMaxLength(5);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
