using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.SmaxModels;

public partial class SmaxV2BioVleadNewContext : DbContext
{
    public SmaxV2BioVleadNewContext()
    {
    }

    public SmaxV2BioVleadNewContext(DbContextOptions<SmaxV2BioVleadNewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgtCardHolder> AgtCardHolders { get; set; }

    public virtual DbSet<Emp> Emps { get; set; }

    public virtual DbSet<Entitlement> Entitlements { get; set; }

    public virtual DbSet<IcfBioAttn> IcfBioAttns { get; set; }

    public virtual DbSet<IcfEmpShift> IcfEmpShifts { get; set; }

    public virtual DbSet<IcfEmpShiftHi> IcfEmpShiftHis { get; set; }

    public virtual DbSet<IcfIntimeTemp> IcfIntimeTemps { get; set; }

    public virtual DbSet<IcfValidBu> IcfValidBus { get; set; }

    public virtual DbSet<IcfValidDesgination> IcfValidDesginations { get; set; }

    public virtual DbSet<IcfValidEmp> IcfValidEmps { get; set; }

    public virtual DbSet<IcfValidHoliday> IcfValidHolidays { get; set; }

    public virtual DbSet<IcfValidShift> IcfValidShifts { get; set; }

    public virtual DbSet<IcfValidUser> IcfValidUsers { get; set; }

    public virtual DbSet<MissingEmployeeList> MissingEmployeeLists { get; set; }

    public virtual DbSet<New58missingList> New58missingLists { get; set; }

    public virtual DbSet<New58missingListEmployee> New58missingListEmployees { get; set; }

    public virtual DbSet<Passwordhistory> Passwordhistories { get; set; }

    public virtual DbSet<ScreenName> ScreenNames { get; set; }

    public virtual DbSet<SmxAccessLevel> SmxAccessLevels { get; set; }

    public virtual DbSet<SmxAccessLevelDetail> SmxAccessLevelDetails { get; set; }

    public virtual DbSet<SmxArea> SmxAreas { get; set; }

    public virtual DbSet<SmxAutoDownload> SmxAutoDownloads { get; set; }

    public virtual DbSet<SmxBranch> SmxBranches { get; set; }

    public virtual DbSet<SmxCardHolder> SmxCardHolders { get; set; }

    public virtual DbSet<SmxCardHolderBk> SmxCardHolderBks { get; set; }

    public virtual DbSet<SmxCardHolderBk20180226> SmxCardHolderBk20180226s { get; set; }

    public virtual DbSet<SmxCardHolderDel> SmxCardHolderDels { get; set; }

    public virtual DbSet<SmxCardHolderDel1802> SmxCardHolderDel1802s { get; set; }

    public virtual DbSet<SmxCardHolderTemp> SmxCardHolderTemps { get; set; }

    public virtual DbSet<SmxCardHolderTest> SmxCardHolderTests { get; set; }

    public virtual DbSet<SmxCardReissue> SmxCardReissues { get; set; }

    public virtual DbSet<SmxCardStatus> SmxCardStatuses { get; set; }

    public virtual DbSet<SmxCardholderAccessLevel> SmxCardholderAccessLevels { get; set; }

    public virtual DbSet<SmxCardholderAccessLevelTest> SmxCardholderAccessLevelTests { get; set; }

    public virtual DbSet<SmxCategory> SmxCategories { get; set; }

    public virtual DbSet<SmxCompany> SmxCompanies { get; set; }

    public virtual DbSet<SmxConfiguration> SmxConfigurations { get; set; }

    public virtual DbSet<SmxDepartment> SmxDepartments { get; set; }

    public virtual DbSet<SmxDesignation> SmxDesignations { get; set; }

    public virtual DbSet<SmxDevice> SmxDevices { get; set; }

    public virtual DbSet<SmxDownloadEmployee> SmxDownloadEmployees { get; set; }

    public virtual DbSet<SmxDownloadHotlist> SmxDownloadHotlists { get; set; }

    public virtual DbSet<SmxEmployeeStatus> SmxEmployeeStatuses { get; set; }

    public virtual DbSet<SmxEventTask> SmxEventTasks { get; set; }

    public virtual DbSet<SmxExceptionCardHistory> SmxExceptionCardHistories { get; set; }

    public virtual DbSet<SmxGetAllTransaction> SmxGetAllTransactions { get; set; }

    public virtual DbSet<SmxGroup> SmxGroups { get; set; }

    public virtual DbSet<SmxGroupDetail> SmxGroupDetails { get; set; }

    public virtual DbSet<SmxHoliday> SmxHolidays { get; set; }

    public virtual DbSet<SmxInactiveDevice> SmxInactiveDevices { get; set; }

    public virtual DbSet<SmxLastAutoupdate> SmxLastAutoupdates { get; set; }

    public virtual DbSet<SmxLeave> SmxLeaves { get; set; }

    public virtual DbSet<SmxLeavedetail> SmxLeavedetails { get; set; }

    public virtual DbSet<SmxLocation> SmxLocations { get; set; }

    public virtual DbSet<SmxMessage> SmxMessages { get; set; }

    public virtual DbSet<SmxPermission> SmxPermissions { get; set; }

    public virtual DbSet<SmxShiftAssignment> SmxShiftAssignments { get; set; }

    public virtual DbSet<SmxShiftAssignmentDetail> SmxShiftAssignmentDetails { get; set; }

    public virtual DbSet<SmxShiftDetail> SmxShiftDetails { get; set; }

    public virtual DbSet<SmxTempDownloadEmployee> SmxTempDownloadEmployees { get; set; }

    public virtual DbSet<SmxTimeZone> SmxTimeZones { get; set; }

    public virtual DbSet<SmxTimeZoneDetail> SmxTimeZoneDetails { get; set; }

    public virtual DbSet<SmxTransaction> SmxTransactions { get; set; }

    public virtual DbSet<SmxTransactionBeforeUpdate04122018> SmxTransactionBeforeUpdate04122018s { get; set; }

    public virtual DbSet<SmxTransactionIcf> SmxTransactionIcfs { get; set; }

    public virtual DbSet<SmxTransactionType> SmxTransactionTypes { get; set; }

    public virtual DbSet<SmxUnit> SmxUnits { get; set; }

    public virtual DbSet<SmxWebApiTransCount> SmxWebApiTransCounts { get; set; }

    public virtual DbSet<SmxWeeklyoff> SmxWeeklyoffs { get; set; }

    public virtual DbSet<SwipeTransaction> SwipeTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<VBioIcf> VBioIcfs { get; set; }

    public virtual DbSet<VwAreaName> VwAreaNames { get; set; }

    public virtual DbSet<VwLeaveDetail> VwLeaveDetails { get; set; }

    public virtual DbSet<VwLeaveSummary> VwLeaveSummaries { get; set; }

    public virtual DbSet<VwSmxAccessLevelReaderDetail> VwSmxAccessLevelReaderDetails { get; set; }

    public virtual DbSet<VwSmxAreasdownload> VwSmxAreasdownloads { get; set; }

    public virtual DbSet<VwSmxCardAccesslevel> VwSmxCardAccesslevels { get; set; }

    public virtual DbSet<VwSmxCardAccesslevelAutoDelete> VwSmxCardAccesslevelAutoDeletes { get; set; }

    public virtual DbSet<VwSmxCardAccesslevelAutoDownload> VwSmxCardAccesslevelAutoDownloads { get; set; }

    public virtual DbSet<VwSmxCardAccesslevelDelete> VwSmxCardAccesslevelDeletes { get; set; }

    public virtual DbSet<VwSmxCardAccesslevelDownload> VwSmxCardAccesslevelDownloads { get; set; }

    public virtual DbSet<VwSmxCardAccesslevelEmployee> VwSmxCardAccesslevelEmployees { get; set; }

    public virtual DbSet<VwSmxCardIssuedReport> VwSmxCardIssuedReports { get; set; }

    public virtual DbSet<VwSmxCardnotIssuedReport> VwSmxCardnotIssuedReports { get; set; }

    public virtual DbSet<VwSmxDownloadEmployee> VwSmxDownloadEmployees { get; set; }

    public virtual DbSet<VwSmxGrantedTransaction> VwSmxGrantedTransactions { get; set; }

    public virtual DbSet<VwSmxGrantedTransactionApi> VwSmxGrantedTransactionApis { get; set; }

    public virtual DbSet<VwSmxGrantedTransactionApix> VwSmxGrantedTransactionApixes { get; set; }

    public virtual DbSet<VwSmxLatecomesRpt> VwSmxLatecomesRpts { get; set; }

    public virtual DbSet<VwSmxTransaction> VwSmxTransactions { get; set; }

    public virtual DbSet<VwSmxTransactionTimedifference> VwSmxTransactionTimedifferences { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KTP-RLT-KIT025;Initial Catalog=SmaxV2_Bio_VleadNew;Persist Security info=False;User ID=sa;Password=Password@1;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;Connect Timeout=600;Command Timeout=600;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgtCardHolder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AGT_CardHolder");

            entity.Property(e => e.Bio)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("BIO");
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.CardNo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Cardtype).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.ChId)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.ContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.DisableAntiPassback).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Dob).HasColumnType("datetime");
            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.Dos)
                .HasColumnType("datetime")
                .HasColumnName("DOS");
            entity.Property(e => e.EmpNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeStatus)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Finger1)
                .HasMaxLength(2500)
                .IsUnicode(false);
            entity.Property(e => e.Finger2)
                .HasMaxLength(2500)
                .IsUnicode(false);
            entity.Property(e => e.FingerId1).HasColumnName("FingerID1");
            entity.Property(e => e.FingerId2).HasColumnName("FingerID2");
            entity.Property(e => e.Fname)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("FName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Grade).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Identification)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Issued).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Lname)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("LName");
            entity.Property(e => e.LocofPosting).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.MailId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mname)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("MName");
            entity.Property(e => e.MsgId).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Nationality).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.NatureOfWork)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pcc)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("PCC");
            entity.Property(e => e.Pfcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PFCode");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Photo).HasColumnType("image");
            entity.Property(e => e.Pin)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Pno).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.PrevWorkExp)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qualification).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.SeviceProCardNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ShortName)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.Signature).HasColumnType("image");
            entity.Property(e => e.SupervisorCno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SupervisorCNo");
            entity.Property(e => e.SupervisorName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.TrackCard).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ValidFrom).HasColumnType("datetime");
            entity.Property(e => e.ValidTo).HasColumnType("datetime");
            entity.Property(e => e.VoidCard).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<Emp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("emp");

            entity.Property(e => e.Empcode)
                .HasMaxLength(50)
                .HasColumnName("EMPCODE");
            entity.Property(e => e.Logdate).HasColumnName("LOGDATE");
            entity.Property(e => e.Logtime).HasColumnName("LOGTIME");
            entity.Property(e => e.Workcode)
                .HasMaxLength(50)
                .HasColumnName("WORKCODE");
        });

        modelBuilder.Entity<Entitlement>(entity =>
        {
            entity.HasKey(e => e.EnId);

            entity.Property(e => e.EnId).HasColumnName("EN_Id");
            entity.Property(e => e.FkScScreenName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("FK_SC_ScreenName");
            entity.Property(e => e.FkUgGroupId).HasColumnName("FK_UG_GroupId");
        });

        modelBuilder.Entity<IcfBioAttn>(entity =>
        {
            entity.HasKey(e => e.IbaId);

            entity.ToTable("icf_bio_attn");

            entity.Property(e => e.IbaId).HasColumnName("IBA_ID");
            entity.Property(e => e.IbaBillunit)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IBA_BILLUNIT");
            entity.Property(e => e.IbaCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("S")
                .HasColumnName("IBA_CODE");
            entity.Property(e => e.IbaCreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("IBA_CREATED_ON");
            entity.Property(e => e.IbaDate).HasColumnName("IBA_DATE");
            entity.Property(e => e.IbaEmpno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IBA_EMPNO");
            entity.Property(e => e.IbaEmpsec)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("IBA_EMPSEC");
            entity.Property(e => e.IbaIntime)
                .HasColumnType("datetime")
                .HasColumnName("IBA_INTIME");
            entity.Property(e => e.IbaOuttime)
                .HasColumnType("datetime")
                .HasColumnName("IBA_OUTTIME");
            entity.Property(e => e.IbaReader)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBA_READER");
            entity.Property(e => e.IbaRemarks)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IBA_REMARKS");
            entity.Property(e => e.IbaShiftcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("IBA_SHIFTCODE");
        });

        modelBuilder.Entity<IcfEmpShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_emp_shift");

            entity.Property(e => e.IesCreateddate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("ies_createddate");
            entity.Property(e => e.IesDate).HasColumnName("ies_date");
            entity.Property(e => e.IesEmpno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_empno");
            entity.Property(e => e.IesEmpsec)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_empsec");
            entity.Property(e => e.IesId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ies_id");
            entity.Property(e => e.IesShift)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_shift");
        });

        modelBuilder.Entity<IcfEmpShiftHi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_emp_shift_his");

            entity.Property(e => e.IesCreateddate).HasColumnName("ies_createddate");
            entity.Property(e => e.IesDate).HasColumnName("ies_date");
            entity.Property(e => e.IesEmpno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_empno");
            entity.Property(e => e.IesEmpsec)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_empsec");
            entity.Property(e => e.IesId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ies_id");
            entity.Property(e => e.IesShift)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ies_shift");
        });

        modelBuilder.Entity<IcfIntimeTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_intime_temp");

            entity.Property(e => e.TrDate1).HasColumnName("TR_DATE1");
            entity.Property(e => e.TrEmpid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TR_EMPID");
            entity.Property(e => e.TrId).HasColumnName("TR_ID");
            entity.Property(e => e.TrIpaddress)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TR_IPADDRESS");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("TR_TIME");
        });

        modelBuilder.Entity<IcfValidBu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_valid_bu");

            entity.Property(e => e.IvbId).HasColumnName("ivb_id");
            entity.Property(e => e.IvdBu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_bu");
            entity.Property(e => e.IvdBuname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ivd_buname");
            entity.Property(e => e.IvdCat)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_cat");
            entity.Property(e => e.IvdCreatedon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ivd_createdon");
            entity.Property(e => e.IvdInd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_ind");
            entity.Property(e => e.IvdIoInd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_io_ind");
            entity.Property(e => e.IvdSfInd)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_sf_ind");
        });

        modelBuilder.Entity<IcfValidDesgination>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_valid_desgination");

            entity.Property(e => e.IvdCreatedon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ivd_createdon");
            entity.Property(e => e.IvdDesgcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivd_desgcode");
            entity.Property(e => e.IvdDesgname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ivd_desgname");
            entity.Property(e => e.IvdId).HasColumnName("ivd_id");
        });

        modelBuilder.Entity<IcfValidEmp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_valid_emp");

            entity.Property(e => e.IveBillunit)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_billunit");
            entity.Property(e => e.IveBu)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_bu");
            entity.Property(e => e.IveDeptid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_deptid");
            entity.Property(e => e.IveDesgcode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_desgcode");
            entity.Property(e => e.IveEmpname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ive_empname");
            entity.Property(e => e.IveEmpno)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_empno");
            entity.Property(e => e.IveGender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ive_gender");
            entity.Property(e => e.IveId).HasColumnName("ive_id");
        });

        modelBuilder.Entity<IcfValidHoliday>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_valid_holiday");

            entity.Property(e => e.IvhCreatedon)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ivh_createdon");
            entity.Property(e => e.IvhDate).HasColumnName("ivh_date");
            entity.Property(e => e.IvhHoliday)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ivh_holiday");
            entity.Property(e => e.IvhId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ivh_id");
        });

        modelBuilder.Entity<IcfValidShift>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("icf_valid_shift");

            entity.Property(e => e.IvsBreakEnd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_break_end");
            entity.Property(e => e.IvsBreakStart)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_break_start");
            entity.Property(e => e.IvsCreatedby)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Admin")
                .HasColumnName("ivs_createdby");
            entity.Property(e => e.IvsCreateddate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ivs_createddate");
            entity.Property(e => e.IvsEnd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_end");
            entity.Property(e => e.IvsId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ivs_id");
            entity.Property(e => e.IvsSatEnd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_sat_end");
            entity.Property(e => e.IvsSatStart)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_sat_start");
            entity.Property(e => e.IvsScode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_scode");
            entity.Property(e => e.IvsSind)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ivs_sind");
            entity.Property(e => e.IvsStart)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ivs_start");
        });

        modelBuilder.Entity<IcfValidUser>(entity =>
        {
            entity.HasKey(e => e.IvuUsername).HasName("PK_icf_valid_users1");

            entity.ToTable("icf_valid_users");

            entity.Property(e => e.IvuUsername)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IVU_USERNAME");
            entity.Property(e => e.IvuCreatedBy)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Admin")
                .HasColumnName("IVU_CREATED_BY");
            entity.Property(e => e.IvuCreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("IVU_CREATED_ON");
            entity.Property(e => e.IvuId)
                .ValueGeneratedOnAdd()
                .HasColumnName("IVU_ID");
            entity.Property(e => e.IvuPassword)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IVU_PASSWORD");
            entity.Property(e => e.IvuUsertype)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IVU_USERTYPE");
        });

        modelBuilder.Entity<MissingEmployeeList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MissingEmployeeList");

            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
        });

        modelBuilder.Entity<New58missingList>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("New58missingList");

            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
        });

        modelBuilder.Entity<New58missingListEmployee>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
        });

        modelBuilder.Entity<Passwordhistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Passwordhistory");

            entity.Property(e => e.FkUsId).HasColumnName("FK_US_ID");
            entity.Property(e => e.PhId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PH_Id");
            entity.Property(e => e.PhUsCreated)
                .HasColumnType("datetime")
                .HasColumnName("PH_US_Created");
            entity.Property(e => e.PhUsModified)
                .HasColumnType("datetime")
                .HasColumnName("PH_US_Modified");
            entity.Property(e => e.PhUsPassword).HasColumnName("PH_US_Password");

            entity.HasOne(d => d.FkUs).WithMany()
                .HasForeignKey(d => d.FkUsId)
                .HasConstraintName("FK_Passwordhistory_Users");
        });

        modelBuilder.Entity<ScreenName>(entity =>
        {
            entity.HasKey(e => e.ScScreenName);

            entity.ToTable("ScreenName");

            entity.Property(e => e.ScScreenName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SC_ScreenName");
            entity.Property(e => e.ScDescription)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("SC_Description");
        });

        modelBuilder.Entity<SmxAccessLevel>(entity =>
        {
            entity.HasKey(e => e.AlId);

            entity.ToTable("Smx_AccessLevel");

            entity.Property(e => e.AlId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AL_ID");
            entity.Property(e => e.AlCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("AL_CREATED");
            entity.Property(e => e.AlModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("AL_MODIFIED");
            entity.Property(e => e.AlModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AL_MODIFIEDBY");
            entity.Property(e => e.AlName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AL_NAME");
        });

        modelBuilder.Entity<SmxAccessLevelDetail>(entity =>
        {
            entity.HasKey(e => e.AldId);

            entity.ToTable("Smx_AccessLevelDetails");

            entity.Property(e => e.AldId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_ID");
            entity.Property(e => e.AldAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_AL_ID");
            entity.Property(e => e.AldCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ALD_CREATED");
            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ALD_MODIFIED");
            entity.Property(e => e.AldModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ALD_MODIFIEDBY");
            entity.Property(e => e.AldNodeid).HasColumnName("ALD_NODEID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");

            entity.HasOne(d => d.AldAl).WithMany(p => p.SmxAccessLevelDetails)
                .HasForeignKey(d => d.AldAlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_AccessLevelDetails_Smx_AccessLevel");

            entity.HasOne(d => d.AldLn).WithMany(p => p.SmxAccessLevelDetails)
                .HasForeignKey(d => d.AldLnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_AccessLevelDetails_Smx_Location");

            entity.HasOne(d => d.AldTz).WithMany(p => p.SmxAccessLevelDetails)
                .HasForeignKey(d => d.AldTzId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_AccessLevelDetails_Smx_TimeZone");
        });

        modelBuilder.Entity<SmxArea>(entity =>
        {
            entity.HasKey(e => e.ArId);

            entity.ToTable("Smx_Areas");

            entity.Property(e => e.ArId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AR_ID");
            entity.Property(e => e.ArApb)
                .HasColumnType("numeric(19, 0)")
                .HasColumnName("AR_APB");
            entity.Property(e => e.ArApbnumber)
                .HasColumnType("numeric(19, 0)")
                .HasColumnName("AR_APBNUMBER");
            entity.Property(e => e.ArDeleted)
                .HasDefaultValue(false)
                .HasColumnName("AR_DELETED");
            entity.Property(e => e.ArIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("AR_IPADDRESS");
            entity.Property(e => e.ArLnId).HasColumnName("AR_LN_ID");
            entity.Property(e => e.ArName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AR_NAME");
            entity.Property(e => e.ArNodeid).HasColumnName("AR_NODEID");
            entity.Property(e => e.ArStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("AR_STATUS");
            entity.Property(e => e.ArType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("AR_TYPE");

            entity.HasOne(d => d.ArLn).WithMany(p => p.SmxAreas)
                .HasForeignKey(d => d.ArLnId)
                .HasConstraintName("FK_Smx_Areas_Smx_Location");
        });

        modelBuilder.Entity<SmxAutoDownload>(entity =>
        {
            entity.HasKey(e => e.AdId);

            entity.ToTable("Smx_AutoDownload");

            entity.Property(e => e.AdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AD_Id");
            entity.Property(e => e.AdAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AD_AL_ID");
            entity.Property(e => e.AdChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AD_CH_Cardno");
            entity.Property(e => e.AdChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("AD_CH_Csnnumber");
            entity.Property(e => e.AdCreated)
                .HasColumnType("datetime")
                .HasColumnName("AD_Created");
            entity.Property(e => e.AdDeleted).HasColumnName("AD_Deleted");
            entity.Property(e => e.AdDwflag).HasColumnName("AD_DWFlag");
            entity.Property(e => e.AdDwstatus).HasColumnName("AD_DWStatus");
            entity.Property(e => e.AdIpaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AD_IPAddress");
            entity.Property(e => e.AdIsbio).HasColumnName("AD_ISBio");
            entity.Property(e => e.AdIscard).HasColumnName("AD_ISCard");
            entity.Property(e => e.AdIscardBio).HasColumnName("AD_ISCardBio");
            entity.Property(e => e.AdIspin).HasColumnName("AD_ISPin");
            entity.Property(e => e.AdModified)
                .HasColumnType("datetime")
                .HasColumnName("AD_Modified");
            entity.Property(e => e.AdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AD_Modifiedby");
        });

        modelBuilder.Entity<SmxBranch>(entity =>
        {
            entity.HasKey(e => e.BrId).HasName("PK_Smx_Branch_1");

            entity.ToTable("Smx_Branch");

            entity.Property(e => e.BrId).HasColumnName("BR_ID");
            entity.Property(e => e.BrCreated)
                .HasColumnType("datetime")
                .HasColumnName("BR_CREATED");
            entity.Property(e => e.BrModified)
                .HasColumnType("datetime")
                .HasColumnName("BR_MODIFIED");
            entity.Property(e => e.BrModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BR_MODIFIEDBY");
            entity.Property(e => e.BrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("BR_Name");
        });

        modelBuilder.Entity<SmxCardHolder>(entity =>
        {
            entity.HasKey(e => e.ChId);

            entity.ToTable("Smx_CardHolder");

            entity.HasIndex(e => e.ChEmpId, "IX_Smx_CardHolder").IsUnique();

            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger1Identity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Finger1Identity");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFinger2Identity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Finger2Identity");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");

            entity.HasOne(d => d.ChBr).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChBrId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Branch");

            entity.HasOne(d => d.ChCg).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChCgId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Company");

            entity.HasOne(d => d.ChCs).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChCsId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_CardStatus");

            entity.HasOne(d => d.ChCt).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChCtId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Category");

            entity.HasOne(d => d.ChDn).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChDnId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Designation");

            entity.HasOne(d => d.ChDp).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChDpId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Department");

            entity.HasOne(d => d.ChEs).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChEsId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_EmployeeStatus");

            entity.HasOne(d => d.ChLn).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChLnId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Location");

            entity.HasOne(d => d.ChMs).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChMsId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Message");

            entity.HasOne(d => d.ChUt).WithMany(p => p.SmxCardHolders)
                .HasForeignKey(d => d.ChUtId)
                .HasConstraintName("FK_Smx_CardHolder_Smx_Unit");
        });

        modelBuilder.Entity<SmxCardHolderBk>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_CardHolder_bk");

            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardHolderBk20180226>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_CardHolder_BK20180226");

            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardHolderDel>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_CardHolder_del");

            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardHolderDel1802>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_CardHolder_del_1802");

            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardHolderTemp>(entity =>
        {
            entity.HasKey(e => e.ChId);

            entity.ToTable("Smx_CardHolder_Temp");

            entity.HasIndex(e => e.ChEmpId, "IX_Smx_CardHolder_Temp").IsUnique();

            entity.Property(e => e.ChId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Id");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_Cardno");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardHolderTest>(entity =>
        {
            entity.HasKey(e => e.ChCardNo);

            entity.ToTable("Smx_CardHolder_Test");

            entity.HasIndex(e => e.ChEmpId, "IX_Smx_CardHolder_Test").IsUnique();

            entity.Property(e => e.ChCardNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChBrId).HasColumnName("Ch_Br_Id");
            entity.Property(e => e.ChCardIssued).HasColumnName("Ch_CardIssued");
            entity.Property(e => e.ChCgId).HasColumnName("Ch_Cg_Id");
            entity.Property(e => e.ChContactAddress)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Ch_ContactAddress");
            entity.Property(e => e.ChCreated)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Created");
            entity.Property(e => e.ChCsId).HasColumnName("Ch_CS_Id");
            entity.Property(e => e.ChCtId).HasColumnName("Ch_Ct_Id");
            entity.Property(e => e.ChDnId).HasColumnName("Ch_Dn_Id");
            entity.Property(e => e.ChDob)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Dob");
            entity.Property(e => e.ChDoj)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOJ");
            entity.Property(e => e.ChDos)
                .HasColumnType("datetime")
                .HasColumnName("Ch_DOS");
            entity.Property(e => e.ChDpId).HasColumnName("Ch_Dp_Id");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChEsId).HasColumnName("Ch_Es_ID");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Ch_Gender");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChLnId).HasColumnName("Ch_Ln_Id");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.ChMailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Ch_MailId");
            entity.Property(e => e.ChModified)
                .HasColumnType("datetime")
                .HasColumnName("Ch_Modified");
            entity.Property(e => e.ChModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_Modifiedby");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChNationality)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Ch_Nationality");
            entity.Property(e => e.ChPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_PhoneNumber");
            entity.Property(e => e.ChPhoto)
                .HasColumnType("image")
                .HasColumnName("Ch_Photo");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChShortName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_ShortName");
            entity.Property(e => e.ChTitle)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("Ch_Title");
            entity.Property(e => e.ChTrackCard).HasColumnName("Ch_TrackCard");
            entity.Property(e => e.ChUtId).HasColumnName("Ch_Ut_Id");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<SmxCardReissue>(entity =>
        {
            entity.HasKey(e => e.CrId);

            entity.ToTable("Smx_CardReissue");

            entity.Property(e => e.CrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CR_ID");
            entity.Property(e => e.CrChCardNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CR_Ch_CardNo");
            entity.Property(e => e.CrChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CR_Ch_EmpId");
            entity.Property(e => e.CrCreated)
                .HasColumnType("datetime")
                .HasColumnName("CR_Created");
        });

        modelBuilder.Entity<SmxCardStatus>(entity =>
        {
            entity.HasKey(e => e.CsId);

            entity.ToTable("Smx_CardStatus");

            entity.Property(e => e.CsId).HasColumnName("CS_Id");
            entity.Property(e => e.CsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CS_Name");
        });

        modelBuilder.Entity<SmxCardholderAccessLevel>(entity =>
        {
            entity.HasKey(e => e.CalId);

            entity.ToTable("Smx_CardholderAccessLevel");

            entity.Property(e => e.CalId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_Id");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.CalChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_CH_Cardno");
            entity.Property(e => e.CalChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CAL_CH_Csnnumber");
            entity.Property(e => e.CalCreated)
                .HasColumnType("datetime")
                .HasColumnName("CAL_Created");
            entity.Property(e => e.CalDeleted).HasColumnName("CAL_Deleted");
            entity.Property(e => e.CalDwstatus).HasColumnName("CAL_DWStatus");
            entity.Property(e => e.CalModified)
                .HasColumnType("datetime")
                .HasColumnName("CAL_Modified");
            entity.Property(e => e.CalModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CAL_Modifiedby");

            entity.HasOne(d => d.CalAl).WithMany(p => p.SmxCardholderAccessLevels)
                .HasForeignKey(d => d.CalAlId)
                .HasConstraintName("FK_Smx_CardholderAccessLevel_Smx_AccessLevel");
        });

        modelBuilder.Entity<SmxCardholderAccessLevelTest>(entity =>
        {
            entity.HasKey(e => e.CalId);

            entity.ToTable("Smx_CardholderAccessLevel_Test");

            entity.Property(e => e.CalId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_Id");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.CalChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_CH_Cardno");
            entity.Property(e => e.CalCreated)
                .HasColumnType("datetime")
                .HasColumnName("CAL_Created");
            entity.Property(e => e.CalDeleted).HasColumnName("CAL_Deleted");
            entity.Property(e => e.CalDwstatus).HasColumnName("CAL_DWStatus");
            entity.Property(e => e.CalModified)
                .HasColumnType("datetime")
                .HasColumnName("CAL_Modified");
            entity.Property(e => e.CalModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CAL_Modifiedby");
        });

        modelBuilder.Entity<SmxCategory>(entity =>
        {
            entity.HasKey(e => e.CtId);

            entity.ToTable("Smx_Category");

            entity.Property(e => e.CtId).HasColumnName("CT_ID");
            entity.Property(e => e.CtCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CT_CREATED");
            entity.Property(e => e.CtModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CT_MODIFIED");
            entity.Property(e => e.CtModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CT_MODIFIEDBY");
            entity.Property(e => e.CtName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CT_NAME");
            entity.Property(e => e.CtShortname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CT_SHORTNAME");
        });

        modelBuilder.Entity<SmxCompany>(entity =>
        {
            entity.HasKey(e => e.CgId);

            entity.ToTable("Smx_Company");

            entity.Property(e => e.CgId).HasColumnName("CG_ID");
            entity.Property(e => e.CgAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CG_ADDRESS");
            entity.Property(e => e.CgCity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CG_CITY");
            entity.Property(e => e.CgCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CG_CREATED");
            entity.Property(e => e.CgEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CG_EMAIL");
            entity.Property(e => e.CgFax)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CG_FAX");
            entity.Property(e => e.CgHead)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CG_HEAD");
            entity.Property(e => e.CgModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CG_MODIFIED");
            entity.Property(e => e.CgModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CG_MODIFIEDBY");
            entity.Property(e => e.CgName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CG_NAME");
            entity.Property(e => e.CgPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CG_PHONE");
            entity.Property(e => e.CgShortname)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CG_SHORTNAME");
        });

        modelBuilder.Entity<SmxConfiguration>(entity =>
        {
            entity.HasKey(e => e.CfId);

            entity.ToTable("Smx_Configuration");

            entity.Property(e => e.CfId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CF_Id");
            entity.Property(e => e.CfAccessvalidation).HasColumnName("CF_Accessvalidation");
            entity.Property(e => e.CfEndtime).HasColumnName("CF_Endtime");
            entity.Property(e => e.CfHoursettings).HasColumnName("CF_hoursettings");
            entity.Property(e => e.CfMaxFlag).HasColumnName("CF_MaxFlag");
            entity.Property(e => e.CfStarttime).HasColumnName("CF_Starttime");
        });

        modelBuilder.Entity<SmxDepartment>(entity =>
        {
            entity.HasKey(e => e.DpId);

            entity.ToTable("Smx_Department");

            entity.Property(e => e.DpId).HasColumnName("DP_ID");
            entity.Property(e => e.DpCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DP_CREATED");
            entity.Property(e => e.DpModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DP_MODIFIED");
            entity.Property(e => e.DpModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DP_MODIFIEDBY");
            entity.Property(e => e.DpName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DP_NAME");
            entity.Property(e => e.DpShortname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DP_SHORTNAME");
        });

        modelBuilder.Entity<SmxDesignation>(entity =>
        {
            entity.HasKey(e => e.DnId);

            entity.ToTable("Smx_Designation");

            entity.Property(e => e.DnId).HasColumnName("DN_ID");
            entity.Property(e => e.DnCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DN_CREATED");
            entity.Property(e => e.DnModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DN_MODIFIED");
            entity.Property(e => e.DnModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DN_MODIFIEDBY");
            entity.Property(e => e.DnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DN_NAME");
            entity.Property(e => e.DnShortname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DN_SHORTNAME");
        });

        modelBuilder.Entity<SmxDevice>(entity =>
        {
            entity.HasKey(e => e.DeId);

            entity.ToTable("Smx_Devices");

            entity.HasIndex(e => e.DeIpaddress, "UK_Smx_Devices").IsUnique();

            entity.Property(e => e.DeId).HasColumnName("DE_ID");
            entity.Property(e => e.DeBuzzerBeepCount).HasColumnName("DE_BUZZER_BEEP_COUNT");
            entity.Property(e => e.DeBuzzerInterval).HasColumnName("DE_BUZZER_INTERVAL");
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
            entity.Property(e => e.DeOptionaltrans).HasColumnName("DE_OPTIONALTRANS");
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
            entity.Property(e => e.DeTimesynctime).HasColumnName("DE_TIMESYNCTIME");

            entity.HasOne(d => d.DeLn).WithMany(p => p.SmxDevices)
                .HasForeignKey(d => d.DeLnId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_Devices_Smx_Location");
        });

        modelBuilder.Entity<SmxDownloadEmployee>(entity =>
        {
            entity.HasKey(e => e.AdeId);

            entity.ToTable("Smx_DownloadEmployees");

            entity.Property(e => e.AdeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_Id");
            entity.Property(e => e.AdeAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_AL_ID");
            entity.Property(e => e.AdeChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_CH_Cardno");
            entity.Property(e => e.AdeChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ADE_CH_Csnnumber");
            entity.Property(e => e.AdeChFinger1)
                .HasColumnType("image")
                .HasColumnName("ADE_Ch_Finger1");
            entity.Property(e => e.AdeChFinger2)
                .HasColumnType("image")
                .HasColumnName("ADE_Ch_Finger2");
            entity.Property(e => e.AdeChValidto)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Ch_Validto");
            entity.Property(e => e.AdeCreated)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Created");
            entity.Property(e => e.AdeDeleted).HasColumnName("ADE_Deleted");
            entity.Property(e => e.AdeDwflag).HasColumnName("ADE_DWFlag");
            entity.Property(e => e.AdeDwstatus).HasColumnName("ADE_DWStatus");
            entity.Property(e => e.AdeIpaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADE_IPAddress");
            entity.Property(e => e.AdeIsbio).HasColumnName("ADE_ISBio");
            entity.Property(e => e.AdeIscard).HasColumnName("ADE_ISCard");
            entity.Property(e => e.AdeIscardBio).HasColumnName("ADE_ISCardBio");
            entity.Property(e => e.AdeIspin).HasColumnName("ADE_ISPin");
            entity.Property(e => e.AdeModified)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Modified");
            entity.Property(e => e.AdeModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADE_Modifiedby");
            entity.Property(e => e.AdeMsId).HasColumnName("ADE_Ms_id");
            entity.Property(e => e.AdeTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_Tz_id");
        });

        modelBuilder.Entity<SmxDownloadHotlist>(entity =>
        {
            entity.HasKey(e => e.HtId);

            entity.ToTable("Smx_DownloadHotlist");

            entity.Property(e => e.HtId)
                .ValueGeneratedOnAdd()
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("HT_ID");
            entity.Property(e => e.HtCardNumber)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("HT_CardNumber");
            entity.Property(e => e.HtIpaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HT_IPADDRESS");
            entity.Property(e => e.HtUserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HT_UserId");
        });

        modelBuilder.Entity<SmxEmployeeStatus>(entity =>
        {
            entity.HasKey(e => e.EsId);

            entity.ToTable("Smx_EmployeeStatus");

            entity.Property(e => e.EsId).HasColumnName("ES_ID");
            entity.Property(e => e.EsCreated)
                .HasColumnType("datetime")
                .HasColumnName("ES_CREATED");
            entity.Property(e => e.EsModified)
                .HasColumnType("datetime")
                .HasColumnName("ES_MODIFIED");
            entity.Property(e => e.EsModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ES_MODIFIEDBY");
            entity.Property(e => e.EsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ES_NAME");
            entity.Property(e => e.EsShortname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ES_SHORTNAME");
        });

        modelBuilder.Entity<SmxEventTask>(entity =>
        {
            entity.HasKey(e => e.EtId);

            entity.ToTable("Smx_EventTask");

            entity.Property(e => e.EtId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ET_ID");
            entity.Property(e => e.EtDeviceIpaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ET_Device_IPAddress");
            entity.Property(e => e.EtIp3actR3Message).HasColumnName("ET_IP3ACT_R3_Message");
            entity.Property(e => e.EtIp3actR3MessageDuration).HasColumnName("ET_IP3ACT_R3_Message_Duration");
            entity.Property(e => e.EtIp3actR3Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ET_IP3ACT_R3_Status");
            entity.Property(e => e.EtIp3actR3Time).HasColumnName("ET_IP3ACT_R3_Time");
            entity.Property(e => e.EtIp3nrmR3Message).HasColumnName("ET_IP3NRM_R3_Message");
            entity.Property(e => e.EtIp3nrmR3MessageDuration).HasColumnName("ET_IP3NRM_R3_Message_Duration");
            entity.Property(e => e.EtIp3nrmR3Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ET_IP3NRM_R3_Status");
            entity.Property(e => e.EtIp3nrmR3Time).HasColumnName("ET_IP3NRM_R3_Time");
            entity.Property(e => e.EtIp4actR3Message).HasColumnName("ET_IP4ACT_R3_Message");
            entity.Property(e => e.EtIp4actR3MessageDuration).HasColumnName("ET_IP4ACT_R3_Message_Duration");
            entity.Property(e => e.EtIp4actR3Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ET_IP4ACT_R3_Status");
            entity.Property(e => e.EtIp4actR3Time).HasColumnName("ET_IP4ACT_R3_Time");
            entity.Property(e => e.EtIp4nrmR3Message).HasColumnName("ET_IP4NRM_R3_Message");
            entity.Property(e => e.EtIp4nrmR3MessageDuration).HasColumnName("ET_IP4NRM_R3_Message_Duration");
            entity.Property(e => e.EtIp4nrmR3Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ET_IP4NRM_R3_Status");
            entity.Property(e => e.EtIp4nrmR3Time).HasColumnName("ET_IP4NRM_R3_Time");
            entity.Property(e => e.EtNodeId).HasColumnName("ET_NodeId");
        });

        modelBuilder.Entity<SmxExceptionCardHistory>(entity =>
        {
            entity.ToTable("Smx_ExceptionCardHistory");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.ExpCardNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("EXP_CardNo");
            entity.Property(e => e.ExpComments)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("EXP_Comments");
            entity.Property(e => e.ExpCreatedDate).HasColumnName("EXP_CreatedDate");
            entity.Property(e => e.ExpStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXP_Status");
            entity.Property(e => e.ExpUserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EXP_UserId");
        });

        modelBuilder.Entity<SmxGetAllTransaction>(entity =>
        {
            entity.HasKey(e => e.TrId);

            entity.ToTable("Smx_GetAllTransactions");

            entity.Property(e => e.TrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_EmpId");
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

        modelBuilder.Entity<SmxGroup>(entity =>
        {
            entity.HasKey(e => e.PkGrId);

            entity.ToTable("Smx_Group");

            entity.Property(e => e.PkGrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PK_Gr_Id");
            entity.Property(e => e.GrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Gr_Created");
            entity.Property(e => e.GrModified)
                .HasColumnType("datetime")
                .HasColumnName("Gr_Modified");
            entity.Property(e => e.GrModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Gr_Modifiedby");
            entity.Property(e => e.GrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Gr_Name");
            entity.Property(e => e.GrShortName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Gr_ShortName");
        });

        modelBuilder.Entity<SmxGroupDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_GroupDetails");

            entity.Property(e => e.FkGdId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("FK_GD_Id");
            entity.Property(e => e.FkGdUnit).HasColumnName("FK_GD_Unit");
            entity.Property(e => e.FkSftId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("FK_Sft_Id");
            entity.Property(e => e.GdChid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GD_Chid");
            entity.Property(e => e.GdCreated)
                .HasColumnType("datetime")
                .HasColumnName("GD_Created");
            entity.Property(e => e.GdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("GD_ID");
            entity.Property(e => e.GdModified)
                .HasColumnType("datetime")
                .HasColumnName("GD_Modified");
            entity.Property(e => e.GdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GD_Modifiedby");

            entity.HasOne(d => d.FkGd).WithMany()
                .HasForeignKey(d => d.FkGdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_GroupDetails_Smx_Group");

            entity.HasOne(d => d.FkSft).WithMany()
                .HasForeignKey(d => d.FkSftId)
                .HasConstraintName("FK_Smx_GroupDetails_Smx_ShiftDetails");
        });

        modelBuilder.Entity<SmxHoliday>(entity =>
        {
            entity.HasKey(e => e.HdId);

            entity.ToTable("Smx_Holiday");

            entity.Property(e => e.HdId).HasColumnName("HD_ID");
            entity.Property(e => e.HdCreated)
                .HasColumnType("datetime")
                .HasColumnName("HD_CREATED");
            entity.Property(e => e.HdDate).HasColumnName("HD_DATE");
            entity.Property(e => e.HdDesc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HD_DESC");
            entity.Property(e => e.HdIsreaderdownload).HasColumnName("HD_ISREADERDOWNLOAD");
            entity.Property(e => e.HdModified)
                .HasColumnType("datetime")
                .HasColumnName("HD_MODIFIED");
            entity.Property(e => e.HdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HD_MODIFIEDBY");
            entity.Property(e => e.HdUpdateStatus)
                .HasDefaultValue(false)
                .HasColumnName("HD_UPDATE_STATUS");
        });

        modelBuilder.Entity<SmxInactiveDevice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_InactiveDevices");

            entity.Property(e => e.DsCreated)
                .HasColumnType("datetime")
                .HasColumnName("DS_Created");
            entity.Property(e => e.DsDeviceName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DS_DeviceName");
            entity.Property(e => e.DsId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("DS_Id");
            entity.Property(e => e.DsIpadress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DS_IPAdress");
            entity.Property(e => e.DsModified)
                .HasColumnType("datetime")
                .HasColumnName("DS_Modified");
            entity.Property(e => e.DsModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DS_Modifiedby");
            entity.Property(e => e.DsNotes)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("DS_Notes");
        });

        modelBuilder.Entity<SmxLastAutoupdate>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_LastAutoupdate");

            entity.Property(e => e.AuDatetime)
                .HasColumnType("datetime")
                .HasColumnName("Au_datetime");
        });

        modelBuilder.Entity<SmxLeave>(entity =>
        {
            entity.HasKey(e => e.LvId);

            entity.ToTable("Smx_Leave");

            entity.Property(e => e.LvId).HasColumnName("Lv_Id");
            entity.Property(e => e.LvCreated)
                .HasColumnType("datetime")
                .HasColumnName("Lv_Created");
            entity.Property(e => e.LvDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Lv_Description");
            entity.Property(e => e.LvMaxAllowed)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Lv_MaxAllowed");
            entity.Property(e => e.LvMaxDays).HasColumnName("Lv_MaxDays");
            entity.Property(e => e.LvModified)
                .HasColumnType("datetime")
                .HasColumnName("LV_Modified");
            entity.Property(e => e.LvModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Lv_Modifiedby");
            entity.Property(e => e.LvShortDesc)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Lv_ShortDesc");
        });

        modelBuilder.Entity<SmxLeavedetail>(entity =>
        {
            entity.HasKey(e => e.LdId);

            entity.ToTable("Smx_Leavedetails");

            entity.Property(e => e.LdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("LD_Id");
            entity.Property(e => e.FkLvId).HasColumnName("FK_Lv_Id");
            entity.Property(e => e.LdChId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LD_ChId");
            entity.Property(e => e.LdCreated)
                .HasColumnType("datetime")
                .HasColumnName("LD_Created");
            entity.Property(e => e.LdDateLdetails)
                .HasColumnType("datetime")
                .HasColumnName("LD_DateLDetails");
            entity.Property(e => e.LdDuration)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("LD_Duration");
            entity.Property(e => e.LdModified)
                .HasColumnType("datetime")
                .HasColumnName("LD_Modified");
            entity.Property(e => e.LdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LD_Modifiedby");
            entity.Property(e => e.LdUnit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("LD_Unit");
        });

        modelBuilder.Entity<SmxLocation>(entity =>
        {
            entity.HasKey(e => e.LnId);

            entity.ToTable("Smx_Location");

            entity.Property(e => e.LnId).HasColumnName("LN_ID");
            entity.Property(e => e.LnAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("LN_ADDRESS");
            entity.Property(e => e.LnCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("LN_CREATED");
            entity.Property(e => e.LnModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("LN_MODIFIED");
            entity.Property(e => e.LnModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_MODIFIEDBY");
            entity.Property(e => e.LnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_NAME");
            entity.Property(e => e.LnShortname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_SHORTNAME");
        });

        modelBuilder.Entity<SmxMessage>(entity =>
        {
            entity.HasKey(e => e.MsId);

            entity.ToTable("Smx_Message");

            entity.Property(e => e.MsId).HasColumnName("MS_ID");
            entity.Property(e => e.MsCreated)
                .HasColumnType("datetime")
                .HasColumnName("MS_CREATED");
            entity.Property(e => e.MsLine1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MS_LINE1");
            entity.Property(e => e.MsLine2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MS_LINE2");
            entity.Property(e => e.MsModified)
                .HasColumnType("datetime")
                .HasColumnName("MS_MODIFIED");
            entity.Property(e => e.MsModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MS_MODIFIEDBY");
            entity.Property(e => e.MsName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MS_NAME");
        });

        modelBuilder.Entity<SmxPermission>(entity =>
        {
            entity.HasKey(e => e.PrId);

            entity.ToTable("Smx_Permission");

            entity.Property(e => e.PrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PR_Id");
            entity.Property(e => e.FkDpId).HasColumnName("FK_DP_ID");
            entity.Property(e => e.PrChId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PR_ChId");
            entity.Property(e => e.PrCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("PR_CreatedDate");
            entity.Property(e => e.PrDate)
                .HasColumnType("datetime")
                .HasColumnName("PR_Date");
            entity.Property(e => e.PrDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PR_Description");
            entity.Property(e => e.PrEndTime)
                .HasColumnType("datetime")
                .HasColumnName("PR_EndTime");
            entity.Property(e => e.PrModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("PR_ModifiedDate");
            entity.Property(e => e.PrPermission)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PR_Permission");
            entity.Property(e => e.PrStartTime)
                .HasColumnType("datetime")
                .HasColumnName("PR_StartTime");
            entity.Property(e => e.PrUpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PR_UpdatedBy");
        });

        modelBuilder.Entity<SmxShiftAssignment>(entity =>
        {
            entity.HasKey(e => e.PkSftId).HasName("PK_Smx_Shift");

            entity.ToTable("Smx_ShiftAssignment");

            entity.Property(e => e.PkSftId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("PK_Sft_Id");
            entity.Property(e => e.SftChId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Sft_ChId");
            entity.Property(e => e.SftCreated)
                .HasColumnType("datetime")
                .HasColumnName("Sft_Created");
            entity.Property(e => e.SftD1)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D1");
            entity.Property(e => e.SftD10)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D10");
            entity.Property(e => e.SftD11)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D11");
            entity.Property(e => e.SftD12)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D12");
            entity.Property(e => e.SftD13)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D13");
            entity.Property(e => e.SftD14)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D14");
            entity.Property(e => e.SftD15)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D15");
            entity.Property(e => e.SftD16)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D16");
            entity.Property(e => e.SftD17)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D17");
            entity.Property(e => e.SftD18)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D18");
            entity.Property(e => e.SftD19)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D19");
            entity.Property(e => e.SftD2)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D2");
            entity.Property(e => e.SftD20)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D20");
            entity.Property(e => e.SftD21)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D21");
            entity.Property(e => e.SftD22)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D22");
            entity.Property(e => e.SftD23)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D23");
            entity.Property(e => e.SftD24)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D24");
            entity.Property(e => e.SftD25)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D25");
            entity.Property(e => e.SftD26)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D26");
            entity.Property(e => e.SftD27)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D27");
            entity.Property(e => e.SftD28)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D28");
            entity.Property(e => e.SftD29)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D29");
            entity.Property(e => e.SftD3)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D3");
            entity.Property(e => e.SftD30)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D30");
            entity.Property(e => e.SftD31)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D31");
            entity.Property(e => e.SftD4)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D4");
            entity.Property(e => e.SftD5)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D5");
            entity.Property(e => e.SftD6)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D6");
            entity.Property(e => e.SftD7)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D7");
            entity.Property(e => e.SftD8)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D8");
            entity.Property(e => e.SftD9)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_D9");
            entity.Property(e => e.SftModified)
                .HasColumnType("datetime")
                .HasColumnName("Sft_Modified");
            entity.Property(e => e.SftModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sft_Modifiedby");
            entity.Property(e => e.SftMonthYear)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Sft_MonthYear");
            entity.Property(e => e.SftUnit)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sft_Unit");
        });

        modelBuilder.Entity<SmxShiftAssignmentDetail>(entity =>
        {
            entity.HasKey(e => e.SftdId);

            entity.ToTable("Smx_ShiftAssignmentDetails");

            entity.Property(e => e.SftdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sftd_Id");
            entity.Property(e => e.FkEmpId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FK_Emp_Id");
            entity.Property(e => e.FkSftId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("FK_Sft_Id");
            entity.Property(e => e.SftdDateTime)
                .HasColumnType("datetime")
                .HasColumnName("Sftd_DateTime");

            entity.HasOne(d => d.FkSft).WithMany(p => p.SmxShiftAssignmentDetails)
                .HasForeignKey(d => d.FkSftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_ShiftAssignmentDetails_Smx_ShiftDetails");
        });

        modelBuilder.Entity<SmxShiftDetail>(entity =>
        {
            entity.HasKey(e => e.SftdId);

            entity.ToTable("Smx_ShiftDetails");

            entity.Property(e => e.SftdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sftd_Id");
            entity.Property(e => e.SftdCreated)
                .HasColumnType("datetime")
                .HasColumnName("Sftd_Created");
            entity.Property(e => e.SftdEndTime)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sftd_EndTime");
            entity.Property(e => e.SftdHoursId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Sftd_Hours_Id");
            entity.Property(e => e.SftdModified)
                .HasColumnType("datetime")
                .HasColumnName("Sftd_Modified");
            entity.Property(e => e.SftdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sftd_Modifiedby");
            entity.Property(e => e.SftdName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sftd_Name");
            entity.Property(e => e.SftdStartTime)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Sftd_StartTime");
        });

        modelBuilder.Entity<SmxTempDownloadEmployee>(entity =>
        {
            entity.HasKey(e => e.AdeId);

            entity.ToTable("Smx_TempDownloadEmployees");

            entity.Property(e => e.AdeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_Id");
            entity.Property(e => e.AdeAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_AL_ID");
            entity.Property(e => e.AdeChCardno)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_CH_Cardno");
            entity.Property(e => e.AdeChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ADE_CH_Csnnumber");
            entity.Property(e => e.AdeChFinger1)
                .HasColumnType("image")
                .HasColumnName("ADE_Ch_Finger1");
            entity.Property(e => e.AdeChFinger2)
                .HasColumnType("image")
                .HasColumnName("ADE_Ch_Finger2");
            entity.Property(e => e.AdeChValidto)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Ch_Validto");
            entity.Property(e => e.AdeCreated)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Created");
            entity.Property(e => e.AdeDeleted).HasColumnName("ADE_Deleted");
            entity.Property(e => e.AdeDwflag).HasColumnName("ADE_DWFlag");
            entity.Property(e => e.AdeDwstatus).HasColumnName("ADE_DWStatus");
            entity.Property(e => e.AdeIpaddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADE_IPAddress");
            entity.Property(e => e.AdeIsbio).HasColumnName("ADE_ISBio");
            entity.Property(e => e.AdeIscard).HasColumnName("ADE_ISCard");
            entity.Property(e => e.AdeIscardBio).HasColumnName("ADE_ISCardBio");
            entity.Property(e => e.AdeIspin).HasColumnName("ADE_ISPin");
            entity.Property(e => e.AdeModified)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Modified");
            entity.Property(e => e.AdeModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADE_Modifiedby");
            entity.Property(e => e.AdeMsId).HasColumnName("ADE_Ms_id");
            entity.Property(e => e.AdeTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ADE_Tz_id");
        });

        modelBuilder.Entity<SmxTimeZone>(entity =>
        {
            entity.HasKey(e => e.TzId);

            entity.ToTable("Smx_TimeZone");

            entity.Property(e => e.TzId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("TZ_ID");
            entity.Property(e => e.TzCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TZ_CREATED");
            entity.Property(e => e.TzModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TZ_MODIFIED");
            entity.Property(e => e.TzModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TZ_MODIFIEDBY");
            entity.Property(e => e.TzName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TZ_NAME");
            entity.Property(e => e.TzUpdateStatus)
                .HasDefaultValue(false)
                .HasColumnName("TZ_UPDATE_STATUS");
        });

        modelBuilder.Entity<SmxTimeZoneDetail>(entity =>
        {
            entity.HasKey(e => e.TzdId);

            entity.ToTable("Smx_TimeZoneDetails");

            entity.Property(e => e.TzdId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("TZD_ID");
            entity.Property(e => e.TzdCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TZD_CREATED");
            entity.Property(e => e.TzdDays)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TZD_DAYS");
            entity.Property(e => e.TzdEndTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("TZD_END_TIME");
            entity.Property(e => e.TzdModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("TZD_MODIFIED");
            entity.Property(e => e.TzdModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TZD_MODIFIEDBY");
            entity.Property(e => e.TzdSpecificDate)
                .HasColumnType("datetime")
                .HasColumnName("TZD_SPECIFIC_DATE");
            entity.Property(e => e.TzdStartTime)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("TZD_START_TIME");
            entity.Property(e => e.TzdTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("TZD_TZ_ID");

            entity.HasOne(d => d.TzdTz).WithMany(p => p.SmxTimeZoneDetails)
                .HasForeignKey(d => d.TzdTzId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Smx_TimeZoneDetails_Smx_TimeZone");
        });

        modelBuilder.Entity<SmxTransaction>(entity =>
        {
            entity.HasKey(e => e.TrId);

            entity.ToTable("Smx_Transaction");

            entity.Property(e => e.TrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_EmpId");
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

        modelBuilder.Entity<SmxTransactionBeforeUpdate04122018>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_TransactionBeforeUpdate#04122018");

            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_EmpId");
            entity.Property(e => e.TrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
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

        modelBuilder.Entity<SmxTransactionIcf>(entity =>
        {
            entity.HasKey(e => e.TrId);

            entity.ToTable("Smx_Transaction_ICF");

            entity.Property(e => e.TrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_EmpId");
            entity.Property(e => e.TrIpAddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tr_IpAddress");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<SmxTransactionType>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_TransactionType");

            entity.Property(e => e.TtCode).HasColumnName("TT_CODE");
            entity.Property(e => e.TtDescription)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TT_DESCRIPTION");
            entity.Property(e => e.TtId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TT_ID");
        });

        modelBuilder.Entity<SmxUnit>(entity =>
        {
            entity.HasKey(e => e.UtId);

            entity.ToTable("Smx_Unit");

            entity.Property(e => e.UtId).HasColumnName("UT_ID");
            entity.Property(e => e.UtCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UT_CREATED");
            entity.Property(e => e.UtDescription)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("UT_DESCRIPTION");
            entity.Property(e => e.UtModified)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UT_MODIFIED");
            entity.Property(e => e.UtModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UT_MODIFIEDBY");
            entity.Property(e => e.UtName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UT_NAME");
        });

        modelBuilder.Entity<SmxWebApiTransCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_WebApi_TransCount");

            entity.Property(e => e.LastTransId).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<SmxWeeklyoff>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Smx_Weeklyoff");

            entity.Property(e => e.WkChid)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("WK_Chid");
            entity.Property(e => e.WkCreated)
                .HasColumnType("datetime")
                .HasColumnName("WK_Created");
            entity.Property(e => e.WkD1)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D1");
            entity.Property(e => e.WkD10)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D10");
            entity.Property(e => e.WkD11)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D11");
            entity.Property(e => e.WkD12)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D12");
            entity.Property(e => e.WkD13)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D13");
            entity.Property(e => e.WkD14)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D14");
            entity.Property(e => e.WkD15)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D15");
            entity.Property(e => e.WkD16)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D16");
            entity.Property(e => e.WkD17)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D17");
            entity.Property(e => e.WkD18)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D18");
            entity.Property(e => e.WkD19)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D19");
            entity.Property(e => e.WkD2)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D2");
            entity.Property(e => e.WkD20)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D20");
            entity.Property(e => e.WkD21)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D21");
            entity.Property(e => e.WkD22)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D22");
            entity.Property(e => e.WkD23)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D23");
            entity.Property(e => e.WkD24)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D24");
            entity.Property(e => e.WkD25)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D25");
            entity.Property(e => e.WkD26)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D26");
            entity.Property(e => e.WkD27)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D27");
            entity.Property(e => e.WkD28)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D28");
            entity.Property(e => e.WkD29)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D29");
            entity.Property(e => e.WkD3)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D3");
            entity.Property(e => e.WkD30)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D30");
            entity.Property(e => e.WkD31)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D31");
            entity.Property(e => e.WkD4)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D4");
            entity.Property(e => e.WkD5)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D5");
            entity.Property(e => e.WkD6)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D6");
            entity.Property(e => e.WkD7)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D7");
            entity.Property(e => e.WkD8)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D8");
            entity.Property(e => e.WkD9)
                .HasDefaultValue(0m)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("WK_D9");
            entity.Property(e => e.WkModified)
                .HasColumnType("datetime")
                .HasColumnName("WK_Modified");
            entity.Property(e => e.WkModifiedby)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WK_Modifiedby");
            entity.Property(e => e.WkMonthyear)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("WK_Monthyear");
        });

        modelBuilder.Entity<SwipeTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("SwipeTransactions");

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
            entity.Property(e => e.TrChid)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_Chid");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Date");
            entity.Property(e => e.TrId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsId);

            entity.Property(e => e.UsId).HasColumnName("US_Id");
            entity.Property(e => e.FkUgGroupId).HasColumnName("FK_UG_GroupId");
            entity.Property(e => e.UsCreated)
                .HasColumnType("datetime")
                .HasColumnName("US_Created");
            entity.Property(e => e.UsLogin).HasColumnName("US_Login");
            entity.Property(e => e.UsModified)
                .HasColumnType("datetime")
                .HasColumnName("US_Modified");
            entity.Property(e => e.UsPassword).HasColumnName("US_Password");
            entity.Property(e => e.UsUser).HasColumnName("US_User");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.PkUgGroupId);

            entity.Property(e => e.PkUgGroupId).HasColumnName("PK_UG_GroupId");
            entity.Property(e => e.UgGroupName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UG_GroupName");
        });

        modelBuilder.Entity<VBioIcf>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_bio_icf");

            entity.Property(e => e.TrCardNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_CardNumber");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Tr_Csnnumber");
            entity.Property(e => e.TrDate1).HasColumnName("TR_DATE1");
            entity.Property(e => e.TrEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Tr_EmpId");
            entity.Property(e => e.TrId)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
            entity.Property(e => e.TrIpAddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Tr_IpAddress");
            entity.Property(e => e.TrMessage)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Tr_Message");
            entity.Property(e => e.TrTime)
                .HasColumnType("datetime")
                .HasColumnName("Tr_Time");
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<VwAreaName>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_AreaName");

            entity.Property(e => e.ArName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AR_Name");
        });

        modelBuilder.Entity<VwLeaveDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LeaveDetails");

            entity.Property(e => e.EmpId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LeaveDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Leave_Date");
            entity.Property(e => e.LeaveDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Leave_Description");
            entity.Property(e => e.LeaveDuration)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("Leave_Duration");
        });

        modelBuilder.Entity<VwLeaveSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LeaveSummary");

            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CH_FName");
            entity.Property(e => e.LeaveTaken).HasColumnType("numeric(38, 2)");
            entity.Property(e => e.LvShortDesc)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Lv_ShortDesc");
        });

        modelBuilder.Entity<VwSmxAccessLevelReaderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_AccessLevel_ReaderDetails");

            entity.Property(e => e.AlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AL_ID");
            entity.Property(e => e.AlName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AL_NAME");
            entity.Property(e => e.AldAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_AL_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.DeId).HasColumnName("DE_ID");
            entity.Property(e => e.DeIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("DE_IPADDRESS");
            entity.Property(e => e.DeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
        });

        modelBuilder.Entity<VwSmxAreasdownload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_Areasdownload");

            entity.Property(e => e.ArApb)
                .HasColumnType("numeric(19, 0)")
                .HasColumnName("Ar_APB");
            entity.Property(e => e.ArApbnumber)
                .HasColumnType("numeric(19, 0)")
                .HasColumnName("Ar_Apbnumber");
            entity.Property(e => e.ArIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Ar_Ipaddress");
            entity.Property(e => e.ArName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AR_Name");
            entity.Property(e => e.ArType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("Ar_Type");
            entity.Property(e => e.DeDotz)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("DE_DOTZ");
        });

        modelBuilder.Entity<VwSmxCardAccesslevel>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel");

            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.CalDeleted).HasColumnName("CAL_Deleted");
            entity.Property(e => e.CalDwstatus).HasColumnName("CAL_DWStatus");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<VwSmxCardAccesslevelAutoDelete>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel_AutoDelete");

            entity.Property(e => e.AdAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AD_AL_ID");
            entity.Property(e => e.AdDeleted).HasColumnName("AD_Deleted");
            entity.Property(e => e.AdDwstatus).HasColumnName("AD_DWStatus");
            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<VwSmxCardAccesslevelAutoDownload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel_AutoDownload");

            entity.Property(e => e.AdAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("AD_AL_ID");
            entity.Property(e => e.AdDeleted).HasColumnName("AD_Deleted");
            entity.Property(e => e.AdDwstatus).HasColumnName("AD_DWStatus");
            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFinger1)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger1");
            entity.Property(e => e.ChFinger2)
                .HasColumnType("image")
                .HasColumnName("Ch_Finger2");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("CH_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("CH_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("CH_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<VwSmxCardAccesslevelDelete>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel_Delete");

            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.CalDeleted).HasColumnName("CAL_Deleted");
            entity.Property(e => e.CalDwstatus).HasColumnName("CAL_DWStatus");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Ch_id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<VwSmxCardAccesslevelDownload>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel_Download");

            entity.Property(e => e.AldLnId).HasColumnName("ALD_LN_ID");
            entity.Property(e => e.AldReaderIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ALD_READER_IPADDRESS");
            entity.Property(e => e.AldTzId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ALD_TZ_ID");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.CalDeleted).HasColumnName("CAL_Deleted");
            entity.Property(e => e.CalDwstatus).HasColumnName("CAL_DWStatus");
            entity.Property(e => e.ChAccessValidation).HasColumnName("Ch_AccessValidation");
            entity.Property(e => e.ChAntiPassBack).HasColumnName("Ch_AntiPassBack");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.ChId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ch_id");
            entity.Property(e => e.ChIsCardBio).HasColumnName("Ch_IsCardBio");
            entity.Property(e => e.ChIsbio).HasColumnName("Ch_ISBio");
            entity.Property(e => e.ChIscard).HasColumnName("Ch_ISCard");
            entity.Property(e => e.ChIspin).HasColumnName("Ch_ISPin");
            entity.Property(e => e.ChMsId).HasColumnName("Ch_MS_Id");
            entity.Property(e => e.ChPinNo).HasColumnName("Ch_PinNo");
            entity.Property(e => e.ChValidTo)
                .HasColumnType("datetime")
                .HasColumnName("Ch_ValidTo");
        });

        modelBuilder.Entity<VwSmxCardAccesslevelEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_CardAccesslevel_Employee");

            entity.Property(e => e.AlName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AL_NAME");
            entity.Property(e => e.CalAlId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("CAL_AL_ID");
            entity.Property(e => e.ChCardNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_CardNo");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.DeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
            entity.Property(e => e.LnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_NAME");
        });

        modelBuilder.Entity<VwSmxCardIssuedReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_smx_CardIssuedReport");

            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwSmxCardnotIssuedReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_smx_CardnotIssuedReport");

            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwSmxDownloadEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_DownloadEmployees");

            entity.Property(e => e.AdeCreated)
                .HasColumnType("datetime")
                .HasColumnName("ADE_Created");
            entity.Property(e => e.AdeDwflag).HasColumnName("ADE_DWFlag");
            entity.Property(e => e.AdeIpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ADE_IpAddress");
            entity.Property(e => e.AdeIsbio).HasColumnName("ADE_ISBIO");
            entity.Property(e => e.ChCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ch_Csnnumber");
            entity.Property(e => e.ChEmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Ch_EmpId");
            entity.Property(e => e.ChFname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_FName");
            entity.Property(e => e.DeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DE_NAME");
        });

        modelBuilder.Entity<VwSmxGrantedTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_GrantedTransaction");

            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Card_Number");
            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DeLnId).HasColumnName("DE_LN_ID");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Device_Name");
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.LnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_NAME");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReaderType)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrDate).HasColumnName("Tr_Date");
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<VwSmxGrantedTransactionApi>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_GrantedTransaction_API");

            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReaderType)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
        });

        modelBuilder.Entity<VwSmxGrantedTransactionApix>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_GrantedTransaction_APIX");

            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ReaderType)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrId)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_Id");
        });

        modelBuilder.Entity<VwSmxLatecomesRpt>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_LatecomesRpt");

            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Card_Number");
            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrDate).HasColumnName("Tr_Date");
        });

        modelBuilder.Entity<VwSmxTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_Transaction");

            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Card_Number");
            entity.Property(e => e.ChLname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Ch_LName");
            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("Date_Time");
            entity.Property(e => e.DeLnId).HasColumnName("DE_LN_ID");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Device_Name");
            entity.Property(e => e.EmpId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("Emp_Id");
            entity.Property(e => e.LnName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LN_NAME");
            entity.Property(e => e.Message)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TrTtype)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("Tr_TType");
        });

        modelBuilder.Entity<VwSmxTransactionTimedifference>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Smx_Transaction_Timedifference");

            entity.Property(e => e.Timedifference).HasColumnType("datetime");
            entity.Property(e => e.TrCreated)
                .HasColumnType("datetime")
                .HasColumnName("tr_created");
            entity.Property(e => e.TrCsnnumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tr_csnnumber");
            entity.Property(e => e.TrDate)
                .HasColumnType("datetime")
                .HasColumnName("tr_date");
            entity.Property(e => e.TrIpaddress)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("tr_ipaddress");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
