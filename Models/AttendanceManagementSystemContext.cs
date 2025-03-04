using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Models;

public partial class AttendanceManagementSystemContext : DbContext
{
    public AttendanceManagementSystemContext(DbContextOptions<AttendanceManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessLevel> AccessLevels { get; set; }

    public virtual DbSet<AddressVerification> AddressVerifications { get; set; }

    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    public virtual DbSet<Approval> Approvals { get; set; }

    public virtual DbSet<ApprovalLevel> ApprovalLevels { get; set; }

    public virtual DbSet<ApprovalNotification> ApprovalNotifications { get; set; }

    public virtual DbSet<ApprovalOwner> ApprovalOwners { get; set; }

    public virtual DbSet<AssignLeaveType> AssignLeaveTypes { get; set; }

    public virtual DbSet<AssignShift> AssignShifts { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BloodGroup> BloodGroups { get; set; }

    public virtual DbSet<BranchMaster> BranchMasters { get; set; }

    public virtual DbSet<BusinessTravel> BusinessTravels { get; set; }

    public virtual DbSet<CategoryMaster> CategoryMasters { get; set; }

    public virtual DbSet<CertificateTracking> CertificateTrackings { get; set; }

    public virtual DbSet<CommonPermission> CommonPermissions { get; set; }

    public virtual DbSet<CompOffAvail> CompOffAvails { get; set; }

    public virtual DbSet<CompOffCredit> CompOffCredits { get; set; }

    public virtual DbSet<CompanyMaster> CompanyMasters { get; set; }

    public virtual DbSet<ComplianceDocument> ComplianceDocuments { get; set; }

    public virtual DbSet<CostCentreMaster> CostCentreMasters { get; set; }

    public virtual DbSet<DailyReport> DailyReports { get; set; }

    public virtual DbSet<DepartmentMaster> DepartmentMasters { get; set; }

    public virtual DbSet<DesignationMaster> DesignationMasters { get; set; }

    public virtual DbSet<DivisionMaster> DivisionMasters { get; set; }

    public virtual DbSet<DropDownMaster> DropDownMasters { get; set; }

    public virtual DbSet<EducationalCertificate> EducationalCertificates { get; set; }

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<EmailLog> EmailLogs { get; set; }

    public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }

    public virtual DbSet<EmploymentHistory> EmploymentHistories { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<ExcelImport> ExcelImports { get; set; }

    public virtual DbSet<FamilyDetail> FamilyDetails { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<GeoStatus> GeoStatuses { get; set; }

    public virtual DbSet<GradeMaster> GradeMasters { get; set; }

    public virtual DbSet<HeadCount> HeadCounts { get; set; }

    public virtual DbSet<HolidayCalendarConfiguration> HolidayCalendarConfigurations { get; set; }

    public virtual DbSet<HolidayCalendarTransaction> HolidayCalendarTransactions { get; set; }

    public virtual DbSet<HolidayMaster> HolidayMasters { get; set; }

    public virtual DbSet<HolidayType> HolidayTypes { get; set; }

    public virtual DbSet<HolidayZoneConfiguration> HolidayZoneConfigurations { get; set; }

    public virtual DbSet<IdentityProof> IdentityProofs { get; set; }

    public virtual DbSet<IndividualLeaveCreditDebit> IndividualLeaveCreditDebits { get; set; }

    public virtual DbSet<LeaveAvailability> LeaveAvailabilities { get; set; }

    public virtual DbSet<LeaveCreditDebitReason> LeaveCreditDebitReasons { get; set; }

    public virtual DbSet<LeaveGroup> LeaveGroups { get; set; }

    public virtual DbSet<LeaveGroupConfiguration> LeaveGroupConfigurations { get; set; }

    public virtual DbSet<LeaveGroupTransaction> LeaveGroupTransactions { get; set; }

    public virtual DbSet<LeaveRequisition> LeaveRequisitions { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<LetterGeneration> LetterGenerations { get; set; }

    public virtual DbSet<LocationMaster> LocationMasters { get; set; }

    public virtual DbSet<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; }

    public virtual DbSet<ManualPunchRequistion> ManualPunchRequistions { get; set; }

    public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MyApplication> MyApplications { get; set; }

    public virtual DbSet<OnBehalfApplicationApproval> OnBehalfApplicationApprovals { get; set; }

    public virtual DbSet<OnDutyOvertime> OnDutyOvertimes { get; set; }

    public virtual DbSet<OnDutyRequisition> OnDutyRequisitions { get; set; }

    public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }

    public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<PermissionRequistion> PermissionRequistions { get; set; }

    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public virtual DbSet<PolicyGroup> PolicyGroups { get; set; }

    public virtual DbSet<PrefixAndSuffix> PrefixAndSuffixes { get; set; }

    public virtual DbSet<PrefixLeaveType> PrefixLeaveTypes { get; set; }

    public virtual DbSet<Probation> Probations { get; set; }

    public virtual DbSet<ProfessionalCertification> ProfessionalCertifications { get; set; }

    public virtual DbSet<ProfessionalTaxSlab> ProfessionalTaxSlabs { get; set; }

    public virtual DbSet<PunchRegularizationApproval> PunchRegularizationApprovals { get; set; }

    public virtual DbSet<ReaderConfiguration> ReaderConfigurations { get; set; }

    public virtual DbSet<RegularShift> RegularShifts { get; set; }

    public virtual DbSet<RoleMenuMapping> RoleMenuMappings { get; set; }

    public virtual DbSet<SalaryComponent> SalaryComponents { get; set; }

    public virtual DbSet<SalaryComponentType> SalaryComponentTypes { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<ShiftChange> ShiftChanges { get; set; }

    public virtual DbSet<ShiftExtension> ShiftExtensions { get; set; }

    public virtual DbSet<SkillInventory> SkillInventories { get; set; }

    public virtual DbSet<StaffCreation> StaffCreations { get; set; }

    public virtual DbSet<StaffLeaveOption> StaffLeaveOptions { get; set; }

    public virtual DbSet<StaffSalary> StaffSalaries { get; set; }

    public virtual DbSet<StaffSalaryBreakdown> StaffSalaryBreakdowns { get; set; }

    public virtual DbSet<StaffVaccination> StaffVaccinations { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatutoryReport> StatutoryReports { get; set; }

    public virtual DbSet<SubFunctionMaster> SubFunctionMasters { get; set; }

    public virtual DbSet<SuffixLeaveType> SuffixLeaveTypes { get; set; }

    public virtual DbSet<TaxSlab> TaxSlabs { get; set; }

    public virtual DbSet<TeamApplication> TeamApplications { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<TypesOfReport> TypesOfReports { get; set; }

    public virtual DbSet<UserManagement> UserManagements { get; set; }

    public virtual DbSet<Volume> Volumes { get; set; }

    public virtual DbSet<WeeklyOffDetail> WeeklyOffDetails { get; set; }

    public virtual DbSet<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkings { get; set; }

    public virtual DbSet<WeeklyOffMaster> WeeklyOffMasters { get; set; }

    public virtual DbSet<WorkFromHome> WorkFromHomes { get; set; }

    public virtual DbSet<WorkingDayPattern> WorkingDayPatterns { get; set; }

    public virtual DbSet<WorkingStatus> WorkingStatuses { get; set; }

    public virtual DbSet<WorkstationMaster> WorkstationMasters { get; set; }

    public virtual DbSet<ZoneMaster> ZoneMasters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccessLe__869767EADE6795EB");

            entity.ToTable("AccessLevel");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AccessLevelCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRACId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AccessLevelUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPACId");
        });

        modelBuilder.Entity<AddressVerification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AddressV__3214EC07B2366E4E");

            entity.ToTable("AddressVerification");

            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");
            entity.Property(e => e.VerificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AddressVerificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRE");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.AddressVerificationStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AddressVe__Staff__681373AD");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AddressVerificationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UPD");
        });

        modelBuilder.Entity<ApplicationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__2821513AB21B7CCC");

            entity.ToTable("ApplicationType");

            entity.Property(e => e.ApplicationTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Approval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__328477F4F1D43235");

            entity.ToTable("Approval");

            entity.Property(e => e.ApprovalComment).HasColumnType("text");
            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsApproved).HasDefaultValue(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CR");

            entity.HasOne(d => d.Feedback).WithMany(p => p.Approvals)
                .HasForeignKey(d => d.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Approval__Feedba__4DE98D56");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ApprovalUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UP");
        });

        modelBuilder.Entity<ApprovalLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ApprovalLeval");

            entity.ToTable("ApprovalLevel");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalLevelCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRALId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ApprovalLevelUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPALId");
        });

        modelBuilder.Entity<ApprovalNotification>(entity =>
        {
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Message)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.ApprovalNotifications)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApTypeId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalNotificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_03CrId");

            entity.HasOne(d => d.Staff).WithMany(p => p.ApprovalNotificationStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_02StId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ApprovalNotificationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_04UpId");
        });

        modelBuilder.Entity<ApprovalOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ApprovalOwn__869767EADE6795EB");

            entity.ToTable("ApprovalOwner");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ApprovalOwnerCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRAOId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ApprovalOwnerUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPAOId");
        });

        modelBuilder.Entity<AssignLeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssignLe__3214EC072C760947");

            entity.ToTable("AssignLeaveType");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssignLeaveTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignLeaveType_CreatedBy");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.AssignLeaveTypes)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignLeaveType_LeaveType");

            entity.HasOne(d => d.OrganizationType).WithMany(p => p.AssignLeaveTypes)
                .HasForeignKey(d => d.OrganizationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignLeaveType_OrganizationType");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AssignLeaveTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_AssignLeaveType_UpdatedBy");
        });

        modelBuilder.Entity<AssignShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssignShift__C0A838818477FD71");

            entity.ToTable("AssignShift");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AssignShiftCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRASSHI");

            entity.HasOne(d => d.Shift).WithMany(p => p.AssignShifts)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SHID");

            entity.HasOne(d => d.Staff).WithMany(p => p.AssignShiftStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ASSTID");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AssignShiftUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPASSHI");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__8B69261CD8469DE2");

            entity.ToTable("Attendance");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.PunchInTime).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PunchOutTime).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AttendanceCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ATT");

            entity.HasOne(d => d.Department).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Depar__5EBF139D");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AttendanceUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPATT");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__UserM__5DCAEF64");
        });

        modelBuilder.Entity<AttendanceStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__7696A735ED63B0D2");

            entity.ToTable("AttendanceStatus");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Duration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.AttendanceStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STA");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.AttendanceStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSTA");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.AttendanceStatuses)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Updat__3493CFA7");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLog");

            entity.Property(e => e.ApiEndpoint)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.HttpMethod)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Module)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Payload).IsUnicode(false);
            entity.Property(e => e.SuccessMessage).IsUnicode(false);
        });

        modelBuilder.Entity<BloodGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BloodGr__869767EADE6795EB");

            entity.ToTable("BloodGroup");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BloodGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRBGId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BloodGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPBGId");
        });

        modelBuilder.Entity<BranchMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BranchMa__DA9B2015F0C244BF");

            entity.ToTable("BranchMaster");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CompanyMaster).WithMany(p => p.BranchMasters)
                .HasForeignKey(d => d.CompanyMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BranchMaster_CompanyMaster");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRBRAN");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BranchMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPBRAN");
        });

        modelBuilder.Entity<BusinessTravel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Business__3214EC0749A220F3");

            entity.ToTable("BusinessTravel");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.StartDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasColumnType("datetime");
            entity.Property(e => e.TotalDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.BusinessTravels)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BusinessTravel_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.BusinessTravels)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_BTANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BusinessTravelCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy7_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.BusinessTravelStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_BusinessTravel_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BusinessTravelUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy7_StaffCreation");
        });

        modelBuilder.Entity<CategoryMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__D1F9DABB4018C271");

            entity.ToTable("CategoryMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCATE");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CategoryMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCATE");
        });

        modelBuilder.Entity<CertificateTracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC07E34A5BA7");

            entity.ToTable("CertificateTracking");

            entity.Property(e => e.CertificationCourse)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CourseAppraisal)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.FileContent).IsUnicode(false);
            entity.Property(e => e.FilePath).IsUnicode(false);
            entity.Property(e => e.Institute)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CertificateTrackingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCERTI");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.CertificateTrackingStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Certifica__Staff__5E8A0973");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CertificateTrackingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCERTI");
        });

        modelBuilder.Entity<CommonPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CommonPe__EFA6FB2F77BA7162");

            entity.ToTable("CommonPermission");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.PermissionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalHours)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.CommonPermissions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .HasConstraintName("FK_CommonPermission_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.CommonPermissions)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_CPANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CommonPermissionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCOMMON");

            entity.HasOne(d => d.Staff).WithMany(p => p.CommonPermissionStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_CommonPermission_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CommonPermissionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCOMMON");
        });

        modelBuilder.Entity<CompOffAvail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompOffA__3214EC07300CCF6D");

            entity.ToTable("CompOffAvail");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FromDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.ToDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.CompOffAvails)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompOffAv__Updat__7C104AB9");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.CompOffAvails)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_CompOffAvail_ApprovalNotification");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompOffAvailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompOffAv__Creat__7DF8932B");

            entity.HasOne(d => d.Staff).WithMany(p => p.CompOffAvailStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_CompOffAvail_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CompOffAvailUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CompOffAv__Updat__7EECB764");
        });

        modelBuilder.Entity<CompOffCredit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompOffC__3214EC0738FA7E91");

            entity.ToTable("CompOffCredit");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.CompOffCredits)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompOffCr__Updat__774B959C");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.CompOffCredits)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_CompOffCredit_ApprovalNotification");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompOffCreditCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CompOffCr__Creat__783FB9D5");

            entity.HasOne(d => d.Staff).WithMany(p => p.CompOffCreditStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_CompOffCredit_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CompOffCreditUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__CompOffCr__Updat__7933DE0E");
        });

        modelBuilder.Entity<CompanyMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyM__8C30BA986E38A98D");

            entity.ToTable("CompanyMaster");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Cstnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CSTNumber");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LegalName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pannumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PANNumber");
            entity.Property(e => e.Pfnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PFNumber");
            entity.Property(e => e.RegisterNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceTaxNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tinnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TINNumber");
            entity.Property(e => e.Tngsnumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TNGSNumber");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Website)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompanyMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCOMP");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CompanyMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCOMP");
        });

        modelBuilder.Entity<ComplianceDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complian__3214EC07C892CBAF");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IssuedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ComplianceDocumentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCompl");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.ComplianceDocumentStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK__Complianc__Staff__6166761E");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ComplianceDocumentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCompl");
        });

        modelBuilder.Entity<CostCentreMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CostCent__3B7E1C8488925AFF");

            entity.ToTable("CostCentreMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CostCentreMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCost");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CostCentreMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCost");
        });

        modelBuilder.Entity<DailyReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DailyRep__3938473BD1FDE39F");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.BranchMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.BranchMasterId)
                .HasConstraintName("FK__DailyRepo__Branc__3A4CA8FD");

            entity.HasOne(d => d.CategoryMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.CategoryMasterId)
                .HasConstraintName("FK__DailyRepo__Categ__3864608B");

            entity.HasOne(d => d.CompanyMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.CompanyMasterId)
                .HasConstraintName("FK__DailyRepo__Updat__37703C52");

            entity.HasOne(d => d.CostCentreMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.CostCentreMasterId)
                .HasConstraintName("FK__DailyRepo__CostC__395884C4");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DailyReportCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRDA");

            entity.HasOne(d => d.DesignationMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.DesignationMasterId)
                .HasConstraintName("FK__DailyRepo__Desig__3B40CD36");

            entity.HasOne(d => d.GradeMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.GradeMasterId)
                .HasConstraintName("FK__DailyRepo__Grade__3D2915A8");

            entity.HasOne(d => d.LocationMaster).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.LocationMasterId)
                .HasConstraintName("FK__DailyRepo__Locat__3C34F16F");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DailyReportUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPDAILY");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.DailyReports)
                .HasForeignKey(d => d.UserManagementId)
                .HasConstraintName("FK__DailyRepo__UserM__3E1D39E1");
        });

        modelBuilder.Entity<DepartmentMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__D60808B324AC739B");

            entity.ToTable("DepartmentMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DepartmentMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRDEP");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DepartmentMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPDEP");
        });

        modelBuilder.Entity<DesignationMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Designat__BDB794A049D08E7A");

            entity.ToTable("DesignationMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DesignationMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRDESI");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DesignationMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPDESI");
        });

        modelBuilder.Entity<DivisionMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Division__4B18A62FE1351C3C");

            entity.ToTable("DivisionMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DivisionMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRDIV");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DivisionMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPDIV");
        });

        modelBuilder.Entity<DropDownMaster>(entity =>
        {
            entity.ToTable("DropDownMaster");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.DropDownMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRDMId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.DropDownMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPDMId");
        });

        modelBuilder.Entity<EducationalCertificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC07A93314E3");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentContent)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FieldOfStudy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InstituteName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");
            entity.Property(e => e.VerificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EducationalCertificateCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCERTIEDU");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.EducationalCertificateStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Education__Staff__6AEFE058");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EducationalCertificateUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCERTIEDU");
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC07393D3676");

            entity.ToTable("EducationalQualification");

            entity.Property(e => e.CourseAppraisal)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CourseType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Institute)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MediumOfInstruction)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Specilization)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.University)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EducationalQualificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREDUQUA");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.EducationalQualificationStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Education__Staff__58D1301D");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EducationalQualificationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPEDUQUA");
        });

        modelBuilder.Entity<EmailLog>(entity =>
        {
            entity.ToTable("EmailLog");

            entity.Property(e => e.Bcc)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("BCC");
            entity.Property(e => e.Cc)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CC");
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.EmailBody).IsUnicode(false);
            entity.Property(e => e.EmailSubject)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ErrorDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.From)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.To)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmailLogs)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREML");
        });

        modelBuilder.Entity<EmergencyContact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Emergenc__3214EC0719390EB4");

            entity.ToTable("EmergencyContact");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Relationship)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmergencyContactCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREMER");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.EmergencyContactStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Emergency__Staff__531856C7");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EmergencyContactUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPEMER");
        });

        modelBuilder.Entity<EmploymentHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employme__3214EC07B743F2F8");

            entity.ToTable("EmploymentHistory");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobLocation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.JobTitle)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastDrawnSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReasonForLeaving)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceContact)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EmploymentHistoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREMPL");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.EmploymentHistoryStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employmen__Staff__55F4C372");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EmploymentHistoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPEMPL");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLog");

            entity.Property(e => e.ApiEndpoint)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.ErrorMessage).IsUnicode(false);
            entity.Property(e => e.HttpMethod)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.InnerException).IsUnicode(false);
            entity.Property(e => e.Module)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Payload).IsUnicode(false);
            entity.Property(e => e.StackTrace).IsUnicode(false);
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.ToTable("EventType");

            entity.Property(e => e.EventName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExcelImport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ExcelImp__869767EADE6795EB");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ExcelImportCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREIId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ExcelImportUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPEIId");
        });

        modelBuilder.Entity<FamilyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FamilyDe__3214EC07F755A839");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.GratuitySharePercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IncomePerAnnum).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.MemberName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NomineeForPf).HasColumnName("NomineeForPF");
            entity.Property(e => e.Occupation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PfsharePercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("PFSharePercentage");
            entity.Property(e => e.Relationship)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FamilyDetailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRFAM");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.FamilyDetailStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FamilyDet__Staff__503BEA1C");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FamilyDetailUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPFAM");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__6A4BEDD6A1C71D09");

            entity.ToTable("Feedback");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FeedbackText).IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FeedbackCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRFEED");

            entity.HasOne(d => d.Probation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProbationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Probat__4830B400");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FeedbackUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPFEED");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Gender");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GenderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRGEId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GenderUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPGEId");
        });

        modelBuilder.Entity<GeoStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GeoStatus__869767EADE6795EB");

            entity.ToTable("GeoStatus");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GeoStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRGSId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GeoStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPGSId");
        });

        modelBuilder.Entity<GradeMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GradeMas__55678466C2C04EDA");

            entity.ToTable("GradeMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ScreenOption)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GradeMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRGRA");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GradeMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPGRA");
        });

        modelBuilder.Entity<HeadCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HeadC__D60808B324AC739B");

            entity.ToTable("HeadCount");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.HeadCount1).HasColumnName("HeadCount");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Category).WithMany(p => p.HeadCounts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HeadCountCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRHEAD");

            entity.HasOne(d => d.Department).WithMany(p => p.HeadCounts)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DID");

            entity.HasOne(d => d.Shift).WithMany(p => p.HeadCounts)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ShID");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HeadCountUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPHEAD");
        });

        modelBuilder.Entity<HolidayCalendarConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HolidayC__A6A351A75A6C1427");

            entity.ToTable("HolidayCalendarConfiguration");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.GroupName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HolidayCalendarConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRHOLIDAY");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HolidayCalendarConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPHOLIDAY");
        });

        modelBuilder.Entity<HolidayCalendarTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HolidayC__3214EC07BC7BC453");

            entity.ToTable("HolidayCalendarTransaction");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HolidayCalendarTransactionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCALEND");

            entity.HasOne(d => d.HolidayCalendar).WithMany(p => p.HolidayCalendarTransactions)
                .HasForeignKey(d => d.HolidayCalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HolidayCa__Updat__2BC97F7C");

            entity.HasOne(d => d.HolidayMaster).WithMany(p => p.HolidayCalendarTransactions)
                .HasForeignKey(d => d.HolidayMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HolidayCa__Holid__1AD3FDA4");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HolidayCalendarTransactionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCALEND");
        });

        modelBuilder.Entity<HolidayMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HolidayM__3214EC07D9C139BD");

            entity.ToTable("HolidayMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.HolidayName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HolidayMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMAST");

            entity.HasOne(d => d.HolidayType).WithMany(p => p.HolidayMasters)
                .HasForeignKey(d => d.HolidayTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HolidayMa__Updat__04E4BC85");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HolidayMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPMAST");
        });

        modelBuilder.Entity<HolidayType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HolidayT__C66A5463DD1939CB");

            entity.ToTable("HolidayType");

            entity.Property(e => e.HolidayName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HolidayZoneConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HolidayZ__B0C725AC55347AA8");

            entity.ToTable("HolidayZoneConfiguration");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.HolidayZoneName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.HolidayZoneConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRZON");

            entity.HasOne(d => d.HolidayCalendar).WithMany(p => p.HolidayZoneConfigurations)
                .HasForeignKey(d => d.HolidayCalendarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HolidayZoneConfiguration_HolidayCalendarConfigurations");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.HolidayZoneConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPZON");
        });

        modelBuilder.Entity<IdentityProof>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Identity__3214EC070F0B5ED4");

            entity.ToTable("IdentityProof");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.DocumentContent).IsUnicode(false);
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IssueDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.IdentityProofCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRIDEN");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.IdentityProofStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__IdentityP__Staff__6442E2C9");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IdentityProofUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UPIDEN");
        });

        modelBuilder.Entity<IndividualLeaveCreditDebit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Individu__65A046F57AD348EF");

            entity.ToTable("IndividualLeaveCreditDebit");

            entity.Property(e => e.ActualBalance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.AvailableBalance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.LeaveCount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.LeaveReason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Month)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.IndividualLeaveCreditDebitCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRINDI");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.IndividualLeaveCreditDebits)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Individua__Leave__1F98B2C1");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.IndividualLeaveCreditDebitStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK_IndividualLeaveCreditDebit_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.IndividualLeaveCreditDebitUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPINID");
        });

        modelBuilder.Entity<LeaveAvailability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveAva__9657F02CF4E511A9");

            entity.Property(e => e.BusinessTravel).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CasualLeave).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CompOff).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.MarriageLeave).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaternityLeave).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.NonConfirmedLeave).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OnDuty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SickLeave).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnProcessed).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WeeklyOff).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WorkFromHome).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveAvailabilityCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRAVAI");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveAvailabilityUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPAVAI");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.LeaveAvailabilities)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveAvai__Updat__73BA3083");
        });

        modelBuilder.Entity<LeaveCreditDebitReason>(entity =>
        {
            entity.ToTable("LeaveCreditDebitReason");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveCreditDebitReasonCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRLRId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveCreditDebitReasonUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPLRId");
        });

        modelBuilder.Entity<LeaveGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveGro__A5658C2001D9C025");

            entity.ToTable("LeaveGroup");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.LeaveGroupName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRGROU");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPGROU");
        });

        modelBuilder.Entity<LeaveGroupConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveGro__A3EE321C7C35FC1E");

            entity.ToTable("LeaveGroupConfiguration");

            entity.Property(e => e.ConsiderPh).HasColumnName("ConsiderPH");
            entity.Property(e => e.ConsiderWo).HasColumnName("ConsiderWO");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.LeaveGroupConfigurationName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoundOffValue).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveGroupConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRLGC");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveGroupConfigurations)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveGrou__Updat__797309D9");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveGroupConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPLGC");
        });

        modelBuilder.Entity<LeaveGroupTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveGro__3214EC078BA896B7");

            entity.ToTable("LeaveGroupTransaction");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveGroupTransactionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRTRR");

            entity.HasOne(d => d.LeaveGroup).WithMany(p => p.LeaveGroupTransactions)
                .HasForeignKey(d => d.LeaveGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveGroupTransaction_LeaveGroup");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveGroupTransactions)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveGroupTransaction_LeaveType");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveGroupTransactionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPTRR");
        });

        modelBuilder.Entity<LeaveRequisition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveReq__6094218E496A4B2D");

            entity.ToTable("LeaveRequisition");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.LeaveRequisitions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ATId");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.LeaveRequisitions)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_ANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveRequisitionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRREQ");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.LeaveRequisitions)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaveId22");

            entity.HasOne(d => d.Staff).WithMany(p => p.LeaveRequisitionStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_LeaveRequisition_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveRequisitionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPREQ");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveTyp__43BE8FF464A61EBF");

            entity.ToTable("LeaveType");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LeaveTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRLTY");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LeaveTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPLTY");
        });

        modelBuilder.Entity<LetterGeneration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LetterGe__1298B02779E5EACC");

            entity.ToTable("LetterGeneration");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LetterPath).HasMaxLength(500);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LetterGenerationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRLET");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.LetterGenerationStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK_LetterGeneration_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LetterGenerationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPLET");
        });

        modelBuilder.Entity<LocationMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__C59A633C78942A69");

            entity.ToTable("LocationMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LocationMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRLOC");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LocationMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPLOC");
        });

        modelBuilder.Entity<ManualAttendanceProcessing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ManualAt__3214EC076E8ED9CD");

            entity.ToTable("ManualAttendanceProcessing");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.BranchMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.BranchMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Branc__2739D489");

            entity.HasOne(d => d.CategoryMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.CategoryMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Categ__25518C17");

            entity.HasOne(d => d.CompanyMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.CompanyMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Updat__245D67DE");

            entity.HasOne(d => d.CostCentreMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.CostCentreMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__CostC__2645B050");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManualAttendanceProcessingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMANU");

            entity.HasOne(d => d.DesignationMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.DesignationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Desig__282DF8C2");

            entity.HasOne(d => d.GradeMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.GradeMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Grade__2A164134");

            entity.HasOne(d => d.LocationMaster).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.LocationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__Locat__29221CFB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ManualAttendanceProcessingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPMANU");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.ManualAttendanceProcessings)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ManualAtt__UserM__2B0A656D");
        });

        modelBuilder.Entity<ManualPunchRequistion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ManualPu__3214EC07A369BA43");

            entity.ToTable("ManualPunchRequistion");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.InPunch).HasColumnType("datetime");
            entity.Property(e => e.OutPunch).HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SelectPunch)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.ManualPunchRequistions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PunchDetails_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.ManualPunchRequistions)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_MPANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManualPunchRequistionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy6_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.ManualPunchRequistionStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_ManualPunch_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ManualPunchRequistionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy6_StaffCreation");
        });

        modelBuilder.Entity<MaritalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaritalSt__869767EADE6795EB");

            entity.ToTable("MaritalStatus");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaritalStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMSId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaritalStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPMSId");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MenuCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRMenuId");

            entity.HasOne(d => d.ParentMenu).WithMany(p => p.InverseParentMenu)
                .HasForeignKey(d => d.ParentMenuId)
                .HasConstraintName("FK_MenuId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MenuUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPMenuId");
        });

        modelBuilder.Entity<MyApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MyApplic__3214EC07445E925C");

            entity.ToTable("MyApplication");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TotalDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MyApplicationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MYAPCR");

            entity.HasOne(d => d.LeaveAvailability).WithMany(p => p.MyApplications)
                .HasForeignKey(d => d.LeaveAvailabilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MyApplica__Leave__0F624AF8");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.MyApplications)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MyApplica__Leave__10566F31");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MyApplicationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_MYAPUP");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.MyApplications)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MyApplica__UserM__0E6E26BF");
        });

        modelBuilder.Entity<OnBehalfApplicationApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OnBehalf__A45A7FB1AC885B9E");

            entity.ToTable("OnBehalfApplicationApproval");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Criteria)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.OnBehalfApplicationApprovals)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OnBehalfA__Appli__31B762FC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OnBehalfApplicationApprovalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ONBCR");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.OnBehalfApplicationApprovalStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK_OnBehalfApplicationApproval_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OnBehalfApplicationApprovalUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_ONBUP");
        });

        modelBuilder.Entity<OnDutyOvertime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OnDutyOv__3214EC073F142040");

            entity.ToTable("OnDutyOvertime");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Otdate).HasColumnName("OTDate");
            entity.Property(e => e.Ottype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OTType");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OnDutyOvertimeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnDutyOvertime_CreatedBy");

            entity.HasOne(d => d.Staff).WithMany(p => p.OnDutyOvertimeStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnDutyOvertime_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OnDutyOvertimeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_OnDutyOvertime_UpdatedBy");
        });

        modelBuilder.Entity<OnDutyRequisition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OnDutyRe__3214EC0734F1670C");

            entity.ToTable("OnDutyRequisition");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StartDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TotalDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.OnDutyRequisitions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OnDutyRequisition_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.OnDutyRequisitions)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_ODANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OnDutyRequisitionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy5_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.OnDutyRequisitionStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_OnDutyRequisition_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OnDutyRequisitionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy5_StaffCreation");
        });

        modelBuilder.Entity<OrganizationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrganizationType__80692F3C18716459");

            entity.ToTable("OrganizationType");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrganizationTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CROTId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.OrganizationTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPOTId");
        });

        modelBuilder.Entity<PasswordHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Password__3214EC07A7A422BB");

            entity.ToTable("PasswordHistory");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.NewPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OldPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PasswordHistoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PasswordH__Creat__4D2051A6");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PasswordHistoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__PasswordH__Updat__4E1475DF");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentD__3214EC07AA2E481B");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.IfscCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PaymentDetailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentDe__Creat__047AA831");

            entity.HasOne(d => d.StaffSalary).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.StaffSalaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentDe__Staff__0662F0A3");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PaymentDetailUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__PaymentDe__Updat__056ECC6A");
        });

        modelBuilder.Entity<PermissionRequistion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07D39FE1EC");

            entity.ToTable("PermissionRequistion");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.PermissionType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.PermissionRequistions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_ApplicationType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PermissionRequistionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PermissionRequistionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy_StaffCreation");
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PermissTyp__869767EADE6795EB");

            entity.ToTable("PermissionType");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PermissionTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPTId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PermissionTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPTId");
        });

        modelBuilder.Entity<PolicyGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PolicyGr__869767EADE6795EB");

            entity.ToTable("PolicyGroup");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PolicyGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPGId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PolicyGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPGId");
        });

        modelBuilder.Entity<PrefixAndSuffix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrefixAn__C66A57B7E97DC1AB");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PrefixAndSuffixCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPRSU");

            entity.HasOne(d => d.LeaveType).WithMany(p => p.PrefixAndSuffixes)
                .HasForeignKey(d => d.LeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrefixAndSuffixes_LeaveTypes");

            entity.HasOne(d => d.PrefixLeaveType).WithMany(p => p.PrefixAndSuffixes)
                .HasForeignKey(d => d.PrefixLeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrefixAnd__Updat__6B24EA82");

            entity.HasOne(d => d.SuffixLeaveType).WithMany(p => p.PrefixAndSuffixes)
                .HasForeignKey(d => d.SuffixLeaveTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrefixAnd__Suffi__6C190EBB");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PrefixAndSuffixUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPRSU");
        });

        modelBuilder.Entity<PrefixLeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PrefixLe__1C6FB9950D2E9C24");

            entity.ToTable("PrefixLeaveType");

            entity.Property(e => e.PrefixLeaveTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Probation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Probatio__96EF4E585DEEA181");

            entity.ToTable("Probation");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProbationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPROB");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.ProbationStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Probation__Staff__436BFEE3");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProbationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPROB");
        });

        modelBuilder.Entity<ProfessionalCertification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Professi__3214EC07A594EA41");

            entity.Property(e => e.CertificationCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CertificationName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CertificationStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CertificationType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.IssuingOrganization)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RenewalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProfessionalCertificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPROFE");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.ProfessionalCertificationStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Professio__Staff__6DCC4D03");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProfessionalCertificationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPROFE");
        });

        modelBuilder.Entity<ProfessionalTaxSlab>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Professi__3214EC07895A3239");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.MaxSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProfessionalTaxSlabCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Professio__Creat__00AA174D");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProfessionalTaxSlabUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__Professio__Updat__019E3B86");
        });

        modelBuilder.Entity<PunchRegularizationApproval>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PunchReg__F6292C23B7572EC0");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.InTime).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OutTime).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PunchType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.Attendance).WithMany(p => p.PunchRegularizationApprovals)
                .HasForeignKey(d => d.AttendanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PunchRegu__Atten__619B8048");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PunchRegularizationApprovalCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRPUNCH");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PunchRegularizationApprovalUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPPUNCH");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.PunchRegularizationApprovals)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PunchRegu__UserM__628FA481");
        });

        modelBuilder.Entity<ReaderConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReaderCo__8E67A5E1C154AAB5");

            entity.ToTable("ReaderConfiguration");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.ReaderIpAddress)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReaderName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReaderType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ReaderConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRREAD");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ReaderConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPREAD");
        });

        modelBuilder.Entity<RegularShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RegularS__CDE9F674DC30BD2F");

            entity.ToTable("RegularShift");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DayPattern)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShiftPattern)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShiftType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RegularShiftCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRREGU");

            entity.HasOne(d => d.Shift).WithMany(p => p.RegularShifts)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_RegularShifts_Shifts");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.RegularShiftStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK_RegularShift_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RegularShiftUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPREGU");
        });

        modelBuilder.Entity<RoleMenuMapping>(entity =>
        {
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RoleMenuMappingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRRoleMenuId");

            entity.HasOne(d => d.Menu).WithMany(p => p.RoleMenuMappings)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMenuId");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleMenuMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RoleMenuMappingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPRoleMenuId");
        });

        modelBuilder.Entity<SalaryComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalaryCo__3214EC07C6BE3DCD");

            entity.Property(e => e.ComponentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.ComponentType).WithMany(p => p.SalaryComponents)
                .HasForeignKey(d => d.ComponentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SalaryCom__Compo__6D9742D9");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SalaryComponentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SalaryCom__Creat__6BAEFA67");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SalaryComponentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__SalaryCom__Updat__6CA31EA0");
        });

        modelBuilder.Entity<SalaryComponentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalaryCo__3214EC07E77BCE53");

            entity.ToTable("SalaryComponentType");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shift__C0A838818477FD71");

            entity.ToTable("Shift");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndTime)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ShiftName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.StartTime)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShiftCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSHI");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ShiftUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSHI");
        });

        modelBuilder.Entity<ShiftChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShiftCha__3214EC07DD45A9BB");

            entity.ToTable("ShiftChange");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.ShiftChanges)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftChange_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.ShiftChanges)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_SCANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShiftChangeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy2_StaffCreation");

            entity.HasOne(d => d.Shift).WithMany(p => p.ShiftChanges)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftChange_ShiftMaster");

            entity.HasOne(d => d.Staff).WithMany(p => p.ShiftChangeStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_ShiftChange_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ShiftChangeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy2_StaffCreation");
        });

        modelBuilder.Entity<ShiftExtension>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShiftExt__3214EC07CE79A6D5");

            entity.ToTable("ShiftExtension");

            entity.Property(e => e.AfterShiftHours).HasColumnType("datetime");
            entity.Property(e => e.BeforeShiftHours).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.DurationHours)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.ShiftExtensions)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShiftExtension_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.ShiftExtensions)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_SEANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShiftExtensionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy3_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.ShiftExtensionStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_ShiftExtension_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ShiftExtensionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy3_StaffCreation");
        });

        modelBuilder.Entity<SkillInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SkillInv__3214EC07421BFCF2");

            entity.ToTable("SkillInventory");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SkillInventoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSKILL");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.SkillInventoryStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SkillInve__Staff__5BAD9CC8");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SkillInventoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSKILL");
        });

        modelBuilder.Entity<StaffCreation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffCre__549079222042F33E");

            entity.ToTable("StaffCreation");

            entity.Property(e => e.AccessLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApprovalLevel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankBranch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankIfscCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CardCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DrivingLicense)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactPerson1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactPerson2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EsiNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GeoStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HomeAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.MotherName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OfficialEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PanNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PassportNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonalLocation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PolicyGroup)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePhoto).IsUnicode(false);
            entity.Property(e => e.Qualification)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tenure).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UanNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Volume)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkingDayPattern)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkingStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ApprovalLevel1Navigation).WithMany(p => p.InverseApprovalLevel1Navigation)
                .HasForeignKey(d => d.ApprovalLevel1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Approval1");

            entity.HasOne(d => d.ApprovalLevel2Navigation).WithMany(p => p.InverseApprovalLevel2Navigation)
                .HasForeignKey(d => d.ApprovalLevel2)
                .HasConstraintName("FK_Approval2");

            entity.HasOne(d => d.Branch).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Branc__5535A963");

            entity.HasOne(d => d.Category).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Categ__5812160E");

            entity.HasOne(d => d.CompanyMaster).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.CompanyMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffCreation_CompanyMaster");

            entity.HasOne(d => d.CostCenter).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.CostCenterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__CostC__59063A47");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRCREAT");

            entity.HasOne(d => d.Department).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Depar__534D60F1");

            entity.HasOne(d => d.Designation).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.DesignationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Desig__5629CD9C");

            entity.HasOne(d => d.Division).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Divis__5441852A");

            entity.HasOne(d => d.Grade).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__Grade__571DF1D5");

            entity.HasOne(d => d.HolidayCalendar).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.HolidayCalendarId)
                .HasConstraintName("FK_StaffCreation_HolidayCalendarConfiguration");

            entity.HasOne(d => d.LeaveGroup).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.LeaveGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffCreation_LeaveGroup");

            entity.HasOne(d => d.LocationMaster).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.LocationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffCreation_LocationMaster");

            entity.HasOne(d => d.OrganizationType).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.OrganizationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationTypeId");

            entity.HasOne(d => d.Status).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Status");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPCREAT");

            entity.HasOne(d => d.WorkStation).WithMany(p => p.StaffCreations)
                .HasForeignKey(d => d.WorkStationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffCrea__WorkS__59FA5E80");
        });

        modelBuilder.Entity<StaffLeaveOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffLea__869767EADE6795EB");

            entity.ToTable("StaffLeaveOption");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StaffLeaveOptionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSLId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StaffLeaveOptionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSLId");
        });

        modelBuilder.Entity<StaffSalary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffSal__3214EC078C89BC14");

            entity.ToTable("StaffSalary");

            entity.Property(e => e.ArrearDays)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.BasicSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Lopdays)
                .HasDefaultValue(0.00m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("LOPDays");
            entity.Property(e => e.NetSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalDeductions).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalEarnings).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StaffSalaryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffSala__Creat__725BF7F6");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffSalaryStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffSala__Staff__74444068");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StaffSalaryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__StaffSala__Updat__73501C2F");
        });

        modelBuilder.Entity<StaffSalaryBreakdown>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffSal__3214EC073A84AD95");

            entity.ToTable("StaffSalaryBreakdown");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.Component).WithMany(p => p.StaffSalaryBreakdowns)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffSala__Compo__79FD19BE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StaffSalaryBreakdownCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffSala__Creat__7720AD13");

            entity.HasOne(d => d.StaffSalary).WithMany(p => p.StaffSalaryBreakdowns)
                .HasForeignKey(d => d.StaffSalaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StaffSala__Staff__7908F585");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StaffSalaryBreakdownUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__StaffSala__Updat__7814D14C");
        });

        modelBuilder.Entity<StaffVaccination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StaffVac__3214EC07559EDD81");

            entity.ToTable("StaffVaccination");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StaffVaccinationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffVaccination_CreatedBy");

            entity.HasOne(d => d.Staff).WithMany(p => p.StaffVaccinationStaffs)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffVaccination_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StaffVaccinationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_StaffVaccination_UpdatedBy");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__869767EADE6795EB");

            entity.ToTable("Status");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSTId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSTId");
        });

        modelBuilder.Entity<StatutoryReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Statutor__3214EC0789D95AEB");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.BranchMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.BranchMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Branc__43D61337");

            entity.HasOne(d => d.CategoryMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.CategoryMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Categ__41EDCAC5");

            entity.HasOne(d => d.CompanyMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.CompanyMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Updat__40F9A68C");

            entity.HasOne(d => d.CostCentreMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.CostCentreMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__CostC__42E1EEFE");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StatutoryReportCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSTAT");

            entity.HasOne(d => d.DesignationMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.DesignationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Desig__44CA3770");

            entity.HasOne(d => d.GradeMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.GradeMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Grade__46B27FE2");

            entity.HasOne(d => d.LocationMaster).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.LocationMasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__Locat__45BE5BA9");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StatutoryReportUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSTAT");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.StatutoryReports)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Statutory__UserM__47A6A41B");
        });

        modelBuilder.Entity<SubFunctionMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubFunct__EADF24CBEEA83577");

            entity.ToTable("SubFunctionMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SubFunctionMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRSUBF");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SubFunctionMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPSUBF");
        });

        modelBuilder.Entity<SuffixLeaveType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SuffixLe__96C2F7414D7C510E");

            entity.ToTable("SuffixLeaveType");

            entity.Property(e => e.SuffixLeaveTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TaxSlab>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaxSlabs__3214EC07E220D1E5");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.MaxSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TaxSlabCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaxSlabs__Create__7CD98669");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TaxSlabUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK__TaxSlabs__Update__7DCDAAA2");
        });

        modelBuilder.Entity<TeamApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TeamAppl__83B27CBE5EEBBE89");

            entity.ToTable("TeamApplication");

            entity.Property(e => e.ApplicationName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TeamApplicationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRAPPTE");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TeamApplicationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPAPPTE");

            entity.HasOne(d => d.UserManagement).WithMany(p => p.TeamApplications)
                .HasForeignKey(d => d.UserManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeamAppli__UserM__1332DBDC");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Title__869767EADE6795EB");

            entity.ToTable("Title");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TitleCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRTIId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TitleUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPTIId");
        });

        modelBuilder.Entity<TypesOfReport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReportType__869767EADE6795EB");

            entity.Property(e => e.ReportName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserManagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserMana__823BE6D66C34326D");

            entity.ToTable("UserManagement");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.UserManagementCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRUSER");

            entity.HasOne(d => d.StaffCreation).WithMany(p => p.UserManagementStaffCreations)
                .HasForeignKey(d => d.StaffCreationId)
                .HasConstraintName("FK_UserManagement_StaffCreation");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.UserManagementUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPUSER");
        });

        modelBuilder.Entity<Volume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Volume__869767EADE6795EB");

            entity.ToTable("Volume");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VolumeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRVLId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.VolumeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPVLId");
        });

        modelBuilder.Entity<WeeklyOffDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeeklyOf__3214EC07F362FADF");

            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WeeklyOffDetailCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeeklyOffDetails_CreatedBy");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WeeklyOffDetailUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_WeeklyOffDetails_UpdatedBy");

            entity.HasOne(d => d.WeeklyOffMaster).WithMany(p => p.WeeklyOffDetails)
                .HasForeignKey(d => d.WeeklyOffMasterId)
                .HasConstraintName("FK_WeeklyOffMaster");
        });

        modelBuilder.Entity<WeeklyOffHolidayWorking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeeklyOf__3214EC073B096F83");

            entity.ToTable("WeeklyOff_HolidayWorking");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.SelectShiftType).HasMaxLength(100);
            entity.Property(e => e.ShiftInTime).HasColumnType("datetime");
            entity.Property(e => e.ShiftOutTime).HasColumnType("datetime");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.WeeklyOffHolidayWorkings)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeeklyOff_HolidayWorking_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.WeeklyOffHolidayWorkings)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_WOHANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WeeklyOffHolidayWorkingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy8_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.WeeklyOffHolidayWorkingStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_WeeklyOff_HolidayWorking_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WeeklyOffHolidayWorkingUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy8_StaffCreation");
        });

        modelBuilder.Entity<WeeklyOffMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeeklyOf__80692F3C18716459");

            entity.ToTable("WeeklyOffMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");
            entity.Property(e => e.WeeklyOffName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WeeklyOffMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRWEEK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WeeklyOffMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPWEEK");
        });

        modelBuilder.Entity<WorkFromHome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFrom__3214EC070BEF0A30");

            entity.ToTable("WorkFromHome");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.EndDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.StartDuration)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasColumnType("datetime");
            entity.Property(e => e.TotalDays).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalHours)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.ApplicationType).WithMany(p => p.WorkFromHomes)
                .HasForeignKey(d => d.ApplicationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkFromHome_ApplicationType");

            entity.HasOne(d => d.ApprovalNotification).WithMany(p => p.WorkFromHomes)
                .HasForeignKey(d => d.ApprovalNotificationId)
                .HasConstraintName("FK_WFHANId");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkFromHomeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CreatedBy1_StaffCreation");

            entity.HasOne(d => d.Staff).WithMany(p => p.WorkFromHomeStaffs)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK_WorkFromHome_Staff");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WorkFromHomeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UpdatedBy1_StaffCreation");
        });

        modelBuilder.Entity<WorkingDayPattern>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkingDa__869767EADE6795EB");

            entity.ToTable("WorkingDayPattern");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkingDayPatternCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRWDPId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WorkingDayPatternUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPWDPId");
        });

        modelBuilder.Entity<WorkingStatus>(entity =>
        {
            entity.ToTable("WorkingStatus");

            entity.Property(e => e.CreatedUtc).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkingStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WSCRId");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WorkingStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_WSUPId");
        });

        modelBuilder.Entity<WorkstationMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Workstat__C309993D60D23B89");

            entity.ToTable("WorkstationMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkstationMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRWORK");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WorkstationMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPWORK");
        });

        modelBuilder.Entity<ZoneMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ZoneMast__9F38CDF76C9599B0");

            entity.ToTable("ZoneMaster");

            entity.Property(e => e.CreatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("CreatedUTC");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedUtc)
                .HasColumnType("datetime")
                .HasColumnName("UpdatedUTC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ZoneMasterCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CRZM");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ZoneMasterUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_UPZM");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
