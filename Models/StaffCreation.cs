using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class StaffCreation
{
    public int Id { get; set; }

    public string CardCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? ShortName { get; set; }

    public string Gender { get; set; } = null!;

    public bool Hide { get; set; }

    public string BloodGroup { get; set; } = null!;

    public string? ProfilePhoto { get; set; }

    public string MaritalStatus { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public DateOnly? MarriageDate { get; set; }

    public long PersonalPhone { get; set; }

    public DateOnly JoiningDate { get; set; }

    public bool Confirmed { get; set; }

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

    public bool OtEligible { get; set; }

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

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string? MiddleName { get; set; }

    public long? OfficialPhone { get; set; }

    public string PersonalLocation { get; set; } = null!;

    public string PersonalEmail { get; set; } = null!;

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

    public string Qualification { get; set; } = null!;

    public string HomeAddress { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string? EmergencyContactPerson1 { get; set; }

    public string? EmergencyContactPerson2 { get; set; }

    public long EmergencyContactNo1 { get; set; }

    public long? EmergencyContactNo2 { get; set; }

    public string MotherName { get; set; } = null!;

    public long? FatherAadharNo { get; set; }

    public long? MotherAadharNo { get; set; }

    public int OrganizationTypeId { get; set; }

    public string WorkingStatus { get; set; } = null!;

    public DateOnly? ConfirmationDate { get; set; }

    public int PostalCode { get; set; }

    public string ApprovalLevel { get; set; } = null!;

    public string? OfficialEmail { get; set; }

    public DateOnly? ResignationDate { get; set; }

    public DateOnly? RelievingDate { get; set; }

    public string? AadharCardFilePath { get; set; }

    public string? PanCardFilePath { get; set; }

    public string? DrivingLicenseFilePath { get; set; }

    public virtual ICollection<AccessLevel> AccessLevelCreatedByNavigations { get; set; } = new List<AccessLevel>();

    public virtual ICollection<AccessLevel> AccessLevelUpdatedByNavigations { get; set; } = new List<AccessLevel>();

    public virtual ICollection<AddressVerification> AddressVerificationCreatedByNavigations { get; set; } = new List<AddressVerification>();

    public virtual ICollection<AddressVerification> AddressVerificationStaffCreations { get; set; } = new List<AddressVerification>();

    public virtual ICollection<AddressVerification> AddressVerificationUpdatedByNavigations { get; set; } = new List<AddressVerification>();

    public virtual ICollection<Approval> ApprovalCreatedByNavigations { get; set; } = new List<Approval>();

    public virtual StaffCreation ApprovalLevel1Navigation { get; set; } = null!;

    public virtual StaffCreation? ApprovalLevel2Navigation { get; set; }

    public virtual ICollection<ApprovalLevel> ApprovalLevelCreatedByNavigations { get; set; } = new List<ApprovalLevel>();

    public virtual ICollection<ApprovalLevel> ApprovalLevelUpdatedByNavigations { get; set; } = new List<ApprovalLevel>();

    public virtual ICollection<ApprovalNotification> ApprovalNotificationCreatedByNavigations { get; set; } = new List<ApprovalNotification>();

    public virtual ICollection<ApprovalNotification> ApprovalNotificationStaffs { get; set; } = new List<ApprovalNotification>();

    public virtual ICollection<ApprovalNotification> ApprovalNotificationUpdatedByNavigations { get; set; } = new List<ApprovalNotification>();

    public virtual ICollection<ApprovalOwner> ApprovalOwnerCreatedByNavigations { get; set; } = new List<ApprovalOwner>();

    public virtual ICollection<ApprovalOwner> ApprovalOwnerUpdatedByNavigations { get; set; } = new List<ApprovalOwner>();

    public virtual ICollection<Approval> ApprovalUpdatedByNavigations { get; set; } = new List<Approval>();

    public virtual ICollection<AssignLeaveType> AssignLeaveTypeCreatedByNavigations { get; set; } = new List<AssignLeaveType>();

    public virtual ICollection<AssignLeaveType> AssignLeaveTypeUpdatedByNavigations { get; set; } = new List<AssignLeaveType>();

    public virtual ICollection<AssignShift> AssignShiftCreatedByNavigations { get; set; } = new List<AssignShift>();

    public virtual ICollection<AssignShift> AssignShiftStaffs { get; set; } = new List<AssignShift>();

    public virtual ICollection<AssignShift> AssignShiftUpdatedByNavigations { get; set; } = new List<AssignShift>();

    public virtual ICollection<Attendance> AttendanceCreatedByNavigations { get; set; } = new List<Attendance>();

    public virtual ICollection<AttendanceStatus> AttendanceStatusCreatedByNavigations { get; set; } = new List<AttendanceStatus>();

    public virtual ICollection<AttendanceStatus> AttendanceStatusUpdatedByNavigations { get; set; } = new List<AttendanceStatus>();

    public virtual ICollection<Attendance> AttendanceUpdatedByNavigations { get; set; } = new List<Attendance>();

    public virtual ICollection<BloodGroup> BloodGroupCreatedByNavigations { get; set; } = new List<BloodGroup>();

    public virtual ICollection<BloodGroup> BloodGroupUpdatedByNavigations { get; set; } = new List<BloodGroup>();

    public virtual BranchMaster Branch { get; set; } = null!;

    public virtual ICollection<BranchMaster> BranchMasterCreatedByNavigations { get; set; } = new List<BranchMaster>();

    public virtual ICollection<BranchMaster> BranchMasterUpdatedByNavigations { get; set; } = new List<BranchMaster>();

    public virtual ICollection<BusinessTravel> BusinessTravelCreatedByNavigations { get; set; } = new List<BusinessTravel>();

    public virtual ICollection<BusinessTravel> BusinessTravelStaffs { get; set; } = new List<BusinessTravel>();

    public virtual ICollection<BusinessTravel> BusinessTravelUpdatedByNavigations { get; set; } = new List<BusinessTravel>();

    public virtual CategoryMaster Category { get; set; } = null!;

    public virtual ICollection<CategoryMaster> CategoryMasterCreatedByNavigations { get; set; } = new List<CategoryMaster>();

    public virtual ICollection<CategoryMaster> CategoryMasterUpdatedByNavigations { get; set; } = new List<CategoryMaster>();

    public virtual ICollection<CertificateTracking> CertificateTrackingCreatedByNavigations { get; set; } = new List<CertificateTracking>();

    public virtual ICollection<CertificateTracking> CertificateTrackingStaffCreations { get; set; } = new List<CertificateTracking>();

    public virtual ICollection<CertificateTracking> CertificateTrackingUpdatedByNavigations { get; set; } = new List<CertificateTracking>();

    public virtual ICollection<CommonPermission> CommonPermissionCreatedByNavigations { get; set; } = new List<CommonPermission>();

    public virtual ICollection<CommonPermission> CommonPermissionStaffs { get; set; } = new List<CommonPermission>();

    public virtual ICollection<CommonPermission> CommonPermissionUpdatedByNavigations { get; set; } = new List<CommonPermission>();

    public virtual ICollection<CompOffAvail> CompOffAvailCreatedByNavigations { get; set; } = new List<CompOffAvail>();

    public virtual ICollection<CompOffAvail> CompOffAvailStaffs { get; set; } = new List<CompOffAvail>();

    public virtual ICollection<CompOffAvail> CompOffAvailUpdatedByNavigations { get; set; } = new List<CompOffAvail>();

    public virtual ICollection<CompOffCredit> CompOffCreditCreatedByNavigations { get; set; } = new List<CompOffCredit>();

    public virtual ICollection<CompOffCredit> CompOffCreditStaffs { get; set; } = new List<CompOffCredit>();

    public virtual ICollection<CompOffCredit> CompOffCreditUpdatedByNavigations { get; set; } = new List<CompOffCredit>();

    public virtual CompanyMaster CompanyMaster { get; set; } = null!;

    public virtual ICollection<CompanyMaster> CompanyMasterCreatedByNavigations { get; set; } = new List<CompanyMaster>();

    public virtual ICollection<CompanyMaster> CompanyMasterUpdatedByNavigations { get; set; } = new List<CompanyMaster>();

    public virtual ICollection<ComplianceDocument> ComplianceDocumentCreatedByNavigations { get; set; } = new List<ComplianceDocument>();

    public virtual ICollection<ComplianceDocument> ComplianceDocumentStaffCreations { get; set; } = new List<ComplianceDocument>();

    public virtual ICollection<ComplianceDocument> ComplianceDocumentUpdatedByNavigations { get; set; } = new List<ComplianceDocument>();

    public virtual CostCentreMaster CostCenter { get; set; } = null!;

    public virtual ICollection<CostCentreMaster> CostCentreMasterCreatedByNavigations { get; set; } = new List<CostCentreMaster>();

    public virtual ICollection<CostCentreMaster> CostCentreMasterUpdatedByNavigations { get; set; } = new List<CostCentreMaster>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DailyReport> DailyReportCreatedByNavigations { get; set; } = new List<DailyReport>();

    public virtual ICollection<DailyReport> DailyReportUpdatedByNavigations { get; set; } = new List<DailyReport>();

    public virtual DepartmentMaster Department { get; set; } = null!;

    public virtual ICollection<DepartmentMaster> DepartmentMasterCreatedByNavigations { get; set; } = new List<DepartmentMaster>();

    public virtual ICollection<DepartmentMaster> DepartmentMasterUpdatedByNavigations { get; set; } = new List<DepartmentMaster>();

    public virtual DesignationMaster Designation { get; set; } = null!;

    public virtual ICollection<DesignationMaster> DesignationMasterCreatedByNavigations { get; set; } = new List<DesignationMaster>();

    public virtual ICollection<DesignationMaster> DesignationMasterUpdatedByNavigations { get; set; } = new List<DesignationMaster>();

    public virtual DivisionMaster Division { get; set; } = null!;

    public virtual ICollection<DivisionMaster> DivisionMasterCreatedByNavigations { get; set; } = new List<DivisionMaster>();

    public virtual ICollection<DivisionMaster> DivisionMasterUpdatedByNavigations { get; set; } = new List<DivisionMaster>();

    public virtual ICollection<DropDownMaster> DropDownMasterCreatedByNavigations { get; set; } = new List<DropDownMaster>();

    public virtual ICollection<DropDownMaster> DropDownMasterUpdatedByNavigations { get; set; } = new List<DropDownMaster>();

    public virtual ICollection<EducationalCertificate> EducationalCertificateCreatedByNavigations { get; set; } = new List<EducationalCertificate>();

    public virtual ICollection<EducationalCertificate> EducationalCertificateStaffCreations { get; set; } = new List<EducationalCertificate>();

    public virtual ICollection<EducationalCertificate> EducationalCertificateUpdatedByNavigations { get; set; } = new List<EducationalCertificate>();

    public virtual ICollection<EducationalQualification> EducationalQualificationCreatedByNavigations { get; set; } = new List<EducationalQualification>();

    public virtual ICollection<EducationalQualification> EducationalQualificationStaffCreations { get; set; } = new List<EducationalQualification>();

    public virtual ICollection<EducationalQualification> EducationalQualificationUpdatedByNavigations { get; set; } = new List<EducationalQualification>();

    public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();

    public virtual ICollection<EmergencyContact> EmergencyContactCreatedByNavigations { get; set; } = new List<EmergencyContact>();

    public virtual ICollection<EmergencyContact> EmergencyContactStaffCreations { get; set; } = new List<EmergencyContact>();

    public virtual ICollection<EmergencyContact> EmergencyContactUpdatedByNavigations { get; set; } = new List<EmergencyContact>();

    public virtual ICollection<EmploymentHistory> EmploymentHistoryCreatedByNavigations { get; set; } = new List<EmploymentHistory>();

    public virtual ICollection<EmploymentHistory> EmploymentHistoryStaffCreations { get; set; } = new List<EmploymentHistory>();

    public virtual ICollection<EmploymentHistory> EmploymentHistoryUpdatedByNavigations { get; set; } = new List<EmploymentHistory>();

    public virtual ICollection<ExcelImport> ExcelImportCreatedByNavigations { get; set; } = new List<ExcelImport>();

    public virtual ICollection<ExcelImport> ExcelImportUpdatedByNavigations { get; set; } = new List<ExcelImport>();

    public virtual ICollection<FamilyDetail> FamilyDetailCreatedByNavigations { get; set; } = new List<FamilyDetail>();

    public virtual ICollection<FamilyDetail> FamilyDetailStaffCreations { get; set; } = new List<FamilyDetail>();

    public virtual ICollection<FamilyDetail> FamilyDetailUpdatedByNavigations { get; set; } = new List<FamilyDetail>();

    public virtual ICollection<Feedback> FeedbackCreatedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackUpdatedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Gender> GenderCreatedByNavigations { get; set; } = new List<Gender>();

    public virtual ICollection<Gender> GenderUpdatedByNavigations { get; set; } = new List<Gender>();

    public virtual ICollection<GeoStatus> GeoStatusCreatedByNavigations { get; set; } = new List<GeoStatus>();

    public virtual ICollection<GeoStatus> GeoStatusUpdatedByNavigations { get; set; } = new List<GeoStatus>();

    public virtual GradeMaster Grade { get; set; } = null!;

    public virtual ICollection<GradeMaster> GradeMasterCreatedByNavigations { get; set; } = new List<GradeMaster>();

    public virtual ICollection<GradeMaster> GradeMasterUpdatedByNavigations { get; set; } = new List<GradeMaster>();

    public virtual ICollection<HeadCount> HeadCountCreatedByNavigations { get; set; } = new List<HeadCount>();

    public virtual ICollection<HeadCount> HeadCountUpdatedByNavigations { get; set; } = new List<HeadCount>();

    public virtual HolidayCalendarConfiguration HolidayCalendar { get; set; } = null!;

    public virtual ICollection<HolidayCalendarConfiguration> HolidayCalendarConfigurationCreatedByNavigations { get; set; } = new List<HolidayCalendarConfiguration>();

    public virtual ICollection<HolidayCalendarConfiguration> HolidayCalendarConfigurationUpdatedByNavigations { get; set; } = new List<HolidayCalendarConfiguration>();

    public virtual ICollection<HolidayCalendarTransaction> HolidayCalendarTransactionCreatedByNavigations { get; set; } = new List<HolidayCalendarTransaction>();

    public virtual ICollection<HolidayCalendarTransaction> HolidayCalendarTransactionUpdatedByNavigations { get; set; } = new List<HolidayCalendarTransaction>();

    public virtual ICollection<HolidayMaster> HolidayMasterCreatedByNavigations { get; set; } = new List<HolidayMaster>();

    public virtual ICollection<HolidayMaster> HolidayMasterUpdatedByNavigations { get; set; } = new List<HolidayMaster>();

    public virtual ICollection<HolidayZoneConfiguration> HolidayZoneConfigurationCreatedByNavigations { get; set; } = new List<HolidayZoneConfiguration>();

    public virtual ICollection<HolidayZoneConfiguration> HolidayZoneConfigurationUpdatedByNavigations { get; set; } = new List<HolidayZoneConfiguration>();

    public virtual ICollection<IdentityProof> IdentityProofCreatedByNavigations { get; set; } = new List<IdentityProof>();

    public virtual ICollection<IdentityProof> IdentityProofStaffCreations { get; set; } = new List<IdentityProof>();

    public virtual ICollection<IdentityProof> IdentityProofUpdatedByNavigations { get; set; } = new List<IdentityProof>();

    public virtual ICollection<IndividualLeaveCreditDebit> IndividualLeaveCreditDebitCreatedByNavigations { get; set; } = new List<IndividualLeaveCreditDebit>();

    public virtual ICollection<IndividualLeaveCreditDebit> IndividualLeaveCreditDebitStaffCreations { get; set; } = new List<IndividualLeaveCreditDebit>();

    public virtual ICollection<IndividualLeaveCreditDebit> IndividualLeaveCreditDebitUpdatedByNavigations { get; set; } = new List<IndividualLeaveCreditDebit>();

    public virtual ICollection<StaffCreation> InverseApprovalLevel1Navigation { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StaffCreation> InverseApprovalLevel2Navigation { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StaffCreation> InverseCreatedByNavigation { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StaffCreation> InverseUpdatedByNavigation { get; set; } = new List<StaffCreation>();

    public virtual ICollection<LeaveAvailability> LeaveAvailabilityCreatedByNavigations { get; set; } = new List<LeaveAvailability>();

    public virtual ICollection<LeaveAvailability> LeaveAvailabilityUpdatedByNavigations { get; set; } = new List<LeaveAvailability>();

    public virtual ICollection<LeaveCreditDebitReason> LeaveCreditDebitReasonCreatedByNavigations { get; set; } = new List<LeaveCreditDebitReason>();

    public virtual ICollection<LeaveCreditDebitReason> LeaveCreditDebitReasonUpdatedByNavigations { get; set; } = new List<LeaveCreditDebitReason>();

    public virtual LeaveGroup LeaveGroup { get; set; } = null!;

    public virtual ICollection<LeaveGroupConfiguration> LeaveGroupConfigurationCreatedByNavigations { get; set; } = new List<LeaveGroupConfiguration>();

    public virtual ICollection<LeaveGroupConfiguration> LeaveGroupConfigurationUpdatedByNavigations { get; set; } = new List<LeaveGroupConfiguration>();

    public virtual ICollection<LeaveGroup> LeaveGroupCreatedByNavigations { get; set; } = new List<LeaveGroup>();

    public virtual ICollection<LeaveGroupTransaction> LeaveGroupTransactionCreatedByNavigations { get; set; } = new List<LeaveGroupTransaction>();

    public virtual ICollection<LeaveGroupTransaction> LeaveGroupTransactionUpdatedByNavigations { get; set; } = new List<LeaveGroupTransaction>();

    public virtual ICollection<LeaveGroup> LeaveGroupUpdatedByNavigations { get; set; } = new List<LeaveGroup>();

    public virtual ICollection<LeaveRequisition> LeaveRequisitionCreatedByNavigations { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<LeaveRequisition> LeaveRequisitionStaffs { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<LeaveRequisition> LeaveRequisitionUpdatedByNavigations { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<LeaveType> LeaveTypeCreatedByNavigations { get; set; } = new List<LeaveType>();

    public virtual ICollection<LeaveType> LeaveTypeUpdatedByNavigations { get; set; } = new List<LeaveType>();

    public virtual ICollection<LetterGeneration> LetterGenerationCreatedByNavigations { get; set; } = new List<LetterGeneration>();

    public virtual ICollection<LetterGeneration> LetterGenerationStaffCreations { get; set; } = new List<LetterGeneration>();

    public virtual ICollection<LetterGeneration> LetterGenerationUpdatedByNavigations { get; set; } = new List<LetterGeneration>();

    public virtual LocationMaster LocationMaster { get; set; } = null!;

    public virtual ICollection<LocationMaster> LocationMasterCreatedByNavigations { get; set; } = new List<LocationMaster>();

    public virtual ICollection<LocationMaster> LocationMasterUpdatedByNavigations { get; set; } = new List<LocationMaster>();

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessingCreatedByNavigations { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessingUpdatedByNavigations { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<ManualPunchRequistion> ManualPunchRequistionCreatedByNavigations { get; set; } = new List<ManualPunchRequistion>();

    public virtual ICollection<ManualPunchRequistion> ManualPunchRequistionStaffs { get; set; } = new List<ManualPunchRequistion>();

    public virtual ICollection<ManualPunchRequistion> ManualPunchRequistionUpdatedByNavigations { get; set; } = new List<ManualPunchRequistion>();

    public virtual ICollection<MaritalStatus> MaritalStatusCreatedByNavigations { get; set; } = new List<MaritalStatus>();

    public virtual ICollection<MaritalStatus> MaritalStatusUpdatedByNavigations { get; set; } = new List<MaritalStatus>();

    public virtual ICollection<Menu> MenuCreatedByNavigations { get; set; } = new List<Menu>();

    public virtual ICollection<Menu> MenuUpdatedByNavigations { get; set; } = new List<Menu>();

    public virtual ICollection<MonthRange> MonthRangeCreatedByNavigations { get; set; } = new List<MonthRange>();

    public virtual ICollection<MonthRange> MonthRangeUpdatedByNavigations { get; set; } = new List<MonthRange>();

    public virtual ICollection<MyApplication> MyApplicationCreatedByNavigations { get; set; } = new List<MyApplication>();

    public virtual ICollection<MyApplication> MyApplicationUpdatedByNavigations { get; set; } = new List<MyApplication>();

    public virtual ICollection<OnBehalfApplicationApproval> OnBehalfApplicationApprovalCreatedByNavigations { get; set; } = new List<OnBehalfApplicationApproval>();

    public virtual ICollection<OnBehalfApplicationApproval> OnBehalfApplicationApprovalStaffCreations { get; set; } = new List<OnBehalfApplicationApproval>();

    public virtual ICollection<OnBehalfApplicationApproval> OnBehalfApplicationApprovalUpdatedByNavigations { get; set; } = new List<OnBehalfApplicationApproval>();

    public virtual ICollection<OnDutyOvertime> OnDutyOvertimeCreatedByNavigations { get; set; } = new List<OnDutyOvertime>();

    public virtual ICollection<OnDutyOvertime> OnDutyOvertimeStaffs { get; set; } = new List<OnDutyOvertime>();

    public virtual ICollection<OnDutyOvertime> OnDutyOvertimeUpdatedByNavigations { get; set; } = new List<OnDutyOvertime>();

    public virtual ICollection<OnDutyRequisition> OnDutyRequisitionCreatedByNavigations { get; set; } = new List<OnDutyRequisition>();

    public virtual ICollection<OnDutyRequisition> OnDutyRequisitionStaffs { get; set; } = new List<OnDutyRequisition>();

    public virtual ICollection<OnDutyRequisition> OnDutyRequisitionUpdatedByNavigations { get; set; } = new List<OnDutyRequisition>();

    public virtual OrganizationType OrganizationType { get; set; } = null!;

    public virtual ICollection<OrganizationType> OrganizationTypeCreatedByNavigations { get; set; } = new List<OrganizationType>();

    public virtual ICollection<OrganizationType> OrganizationTypeUpdatedByNavigations { get; set; } = new List<OrganizationType>();

    public virtual ICollection<PaySlipComponent> PaySlipComponentCreatedByNavigations { get; set; } = new List<PaySlipComponent>();

    public virtual ICollection<PaySlipComponent> PaySlipComponentStaffs { get; set; } = new List<PaySlipComponent>();

    public virtual ICollection<PaySlipComponent> PaySlipComponentUpdatedByNavigations { get; set; } = new List<PaySlipComponent>();

    public virtual ICollection<PaySlip> PaySlipCreatedByNavigations { get; set; } = new List<PaySlip>();

    public virtual ICollection<PaySlip> PaySlipStaffs { get; set; } = new List<PaySlip>();

    public virtual ICollection<PaySlip> PaySlipUpdatedByNavigations { get; set; } = new List<PaySlip>();

    public virtual ICollection<PermissionRequistion> PermissionRequistionCreatedByNavigations { get; set; } = new List<PermissionRequistion>();

    public virtual ICollection<PermissionRequistion> PermissionRequistionUpdatedByNavigations { get; set; } = new List<PermissionRequistion>();

    public virtual ICollection<PermissionType> PermissionTypeCreatedByNavigations { get; set; } = new List<PermissionType>();

    public virtual ICollection<PermissionType> PermissionTypeUpdatedByNavigations { get; set; } = new List<PermissionType>();

    public virtual ICollection<PolicyGroup> PolicyGroupCreatedByNavigations { get; set; } = new List<PolicyGroup>();

    public virtual ICollection<PolicyGroup> PolicyGroupUpdatedByNavigations { get; set; } = new List<PolicyGroup>();

    public virtual ICollection<PrefixAndSuffix> PrefixAndSuffixCreatedByNavigations { get; set; } = new List<PrefixAndSuffix>();

    public virtual ICollection<PrefixAndSuffix> PrefixAndSuffixUpdatedByNavigations { get; set; } = new List<PrefixAndSuffix>();

    public virtual ICollection<Probation> ProbationCreatedByNavigations { get; set; } = new List<Probation>();

    public virtual ICollection<Probation> ProbationStaffCreations { get; set; } = new List<Probation>();

    public virtual ICollection<Probation> ProbationUpdatedByNavigations { get; set; } = new List<Probation>();

    public virtual ICollection<ProfessionalCertification> ProfessionalCertificationCreatedByNavigations { get; set; } = new List<ProfessionalCertification>();

    public virtual ICollection<ProfessionalCertification> ProfessionalCertificationStaffCreations { get; set; } = new List<ProfessionalCertification>();

    public virtual ICollection<ProfessionalCertification> ProfessionalCertificationUpdatedByNavigations { get; set; } = new List<ProfessionalCertification>();

    public virtual ICollection<PunchRegularizationApproval> PunchRegularizationApprovalCreatedByNavigations { get; set; } = new List<PunchRegularizationApproval>();

    public virtual ICollection<PunchRegularizationApproval> PunchRegularizationApprovalUpdatedByNavigations { get; set; } = new List<PunchRegularizationApproval>();

    public virtual ICollection<ReaderConfiguration> ReaderConfigurationCreatedByNavigations { get; set; } = new List<ReaderConfiguration>();

    public virtual ICollection<ReaderConfiguration> ReaderConfigurationUpdatedByNavigations { get; set; } = new List<ReaderConfiguration>();

    public virtual ICollection<RegularShift> RegularShiftCreatedByNavigations { get; set; } = new List<RegularShift>();

    public virtual ICollection<RegularShift> RegularShiftStaffCreations { get; set; } = new List<RegularShift>();

    public virtual ICollection<RegularShift> RegularShiftUpdatedByNavigations { get; set; } = new List<RegularShift>();

    public virtual ICollection<RoleMenuMapping> RoleMenuMappingCreatedByNavigations { get; set; } = new List<RoleMenuMapping>();

    public virtual ICollection<RoleMenuMapping> RoleMenuMappingUpdatedByNavigations { get; set; } = new List<RoleMenuMapping>();

    public virtual ICollection<SalaryComponent> SalaryComponentCreatedByNavigations { get; set; } = new List<SalaryComponent>();

    public virtual ICollection<SalaryComponent> SalaryComponentStaffs { get; set; } = new List<SalaryComponent>();

    public virtual ICollection<SalaryComponent> SalaryComponentUpdatedByNavigations { get; set; } = new List<SalaryComponent>();

    public virtual ICollection<SalaryStructure> SalaryStructureCreatedByNavigations { get; set; } = new List<SalaryStructure>();

    public virtual ICollection<SalaryStructure> SalaryStructureStaffs { get; set; } = new List<SalaryStructure>();

    public virtual ICollection<SalaryStructure> SalaryStructureUpdatedByNavigations { get; set; } = new List<SalaryStructure>();

    public virtual ICollection<ShiftChange> ShiftChangeCreatedByNavigations { get; set; } = new List<ShiftChange>();

    public virtual ICollection<ShiftChange> ShiftChangeStaffs { get; set; } = new List<ShiftChange>();

    public virtual ICollection<ShiftChange> ShiftChangeUpdatedByNavigations { get; set; } = new List<ShiftChange>();

    public virtual ICollection<Shift> ShiftCreatedByNavigations { get; set; } = new List<Shift>();

    public virtual ICollection<ShiftExtension> ShiftExtensionCreatedByNavigations { get; set; } = new List<ShiftExtension>();

    public virtual ICollection<ShiftExtension> ShiftExtensionStaffs { get; set; } = new List<ShiftExtension>();

    public virtual ICollection<ShiftExtension> ShiftExtensionUpdatedByNavigations { get; set; } = new List<ShiftExtension>();

    public virtual ICollection<ShiftPattern> ShiftPatternCreatedByNavigations { get; set; } = new List<ShiftPattern>();

    public virtual ICollection<ShiftPattern> ShiftPatternUpdatedByNavigations { get; set; } = new List<ShiftPattern>();

    public virtual ICollection<ShiftType> ShiftTypeCreatedByNavigations { get; set; } = new List<ShiftType>();

    public virtual ICollection<ShiftType> ShiftTypeUpdatedByNavigations { get; set; } = new List<ShiftType>();

    public virtual ICollection<Shift> ShiftUpdatedByNavigations { get; set; } = new List<Shift>();

    public virtual ICollection<SkillInventory> SkillInventoryCreatedByNavigations { get; set; } = new List<SkillInventory>();

    public virtual ICollection<SkillInventory> SkillInventoryStaffCreations { get; set; } = new List<SkillInventory>();

    public virtual ICollection<SkillInventory> SkillInventoryUpdatedByNavigations { get; set; } = new List<SkillInventory>();

    public virtual ICollection<StaffLeaveOption> StaffLeaveOptionCreatedByNavigations { get; set; } = new List<StaffLeaveOption>();

    public virtual ICollection<StaffLeaveOption> StaffLeaveOptionUpdatedByNavigations { get; set; } = new List<StaffLeaveOption>();

    public virtual ICollection<StaffVaccination> StaffVaccinationCreatedByNavigations { get; set; } = new List<StaffVaccination>();

    public virtual ICollection<StaffVaccination> StaffVaccinationStaffs { get; set; } = new List<StaffVaccination>();

    public virtual ICollection<StaffVaccination> StaffVaccinationUpdatedByNavigations { get; set; } = new List<StaffVaccination>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Status> StatusCreatedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<Status> StatusUpdatedByNavigations { get; set; } = new List<Status>();

    public virtual ICollection<StatutoryReport> StatutoryReportCreatedByNavigations { get; set; } = new List<StatutoryReport>();

    public virtual ICollection<StatutoryReport> StatutoryReportUpdatedByNavigations { get; set; } = new List<StatutoryReport>();

    public virtual ICollection<SubFunctionMaster> SubFunctionMasterCreatedByNavigations { get; set; } = new List<SubFunctionMaster>();

    public virtual ICollection<SubFunctionMaster> SubFunctionMasterUpdatedByNavigations { get; set; } = new List<SubFunctionMaster>();

    public virtual ICollection<TeamApplication> TeamApplicationCreatedByNavigations { get; set; } = new List<TeamApplication>();

    public virtual ICollection<TeamApplication> TeamApplicationUpdatedByNavigations { get; set; } = new List<TeamApplication>();

    public virtual ICollection<Title> TitleCreatedByNavigations { get; set; } = new List<Title>();

    public virtual ICollection<Title> TitleUpdatedByNavigations { get; set; } = new List<Title>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual ICollection<UserManagement> UserManagementCreatedByNavigations { get; set; } = new List<UserManagement>();

    public virtual ICollection<UserManagement> UserManagementStaffCreations { get; set; } = new List<UserManagement>();

    public virtual ICollection<UserManagement> UserManagementUpdatedByNavigations { get; set; } = new List<UserManagement>();

    public virtual ICollection<Volume> VolumeCreatedByNavigations { get; set; } = new List<Volume>();

    public virtual ICollection<Volume> VolumeUpdatedByNavigations { get; set; } = new List<Volume>();

    public virtual ICollection<WeeklyOff> WeeklyOffCreatedByNavigations { get; set; } = new List<WeeklyOff>();

    public virtual ICollection<WeeklyOffDetail> WeeklyOffDetailCreatedByNavigations { get; set; } = new List<WeeklyOffDetail>();

    public virtual ICollection<WeeklyOffDetail> WeeklyOffDetailUpdatedByNavigations { get; set; } = new List<WeeklyOffDetail>();

    public virtual ICollection<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkingCreatedByNavigations { get; set; } = new List<WeeklyOffHolidayWorking>();

    public virtual ICollection<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkingStaffs { get; set; } = new List<WeeklyOffHolidayWorking>();

    public virtual ICollection<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkingUpdatedByNavigations { get; set; } = new List<WeeklyOffHolidayWorking>();

    public virtual ICollection<WeeklyOffMaster> WeeklyOffMasterCreatedByNavigations { get; set; } = new List<WeeklyOffMaster>();

    public virtual ICollection<WeeklyOffMaster> WeeklyOffMasterUpdatedByNavigations { get; set; } = new List<WeeklyOffMaster>();

    public virtual ICollection<WeeklyOffType> WeeklyOffTypeCreatedByNavigations { get; set; } = new List<WeeklyOffType>();

    public virtual ICollection<WeeklyOffType> WeeklyOffTypeUpdatedByNavigations { get; set; } = new List<WeeklyOffType>();

    public virtual ICollection<WeeklyOff> WeeklyOffUpdatedByNavigations { get; set; } = new List<WeeklyOff>();

    public virtual ICollection<WorkFromHome> WorkFromHomeCreatedByNavigations { get; set; } = new List<WorkFromHome>();

    public virtual ICollection<WorkFromHome> WorkFromHomeStaffs { get; set; } = new List<WorkFromHome>();

    public virtual ICollection<WorkFromHome> WorkFromHomeUpdatedByNavigations { get; set; } = new List<WorkFromHome>();

    public virtual WorkstationMaster WorkStation { get; set; } = null!;

    public virtual ICollection<WorkingDayPattern> WorkingDayPatternCreatedByNavigations { get; set; } = new List<WorkingDayPattern>();

    public virtual ICollection<WorkingDayPattern> WorkingDayPatternUpdatedByNavigations { get; set; } = new List<WorkingDayPattern>();

    public virtual ICollection<WorkingStatus> WorkingStatusCreatedByNavigations { get; set; } = new List<WorkingStatus>();

    public virtual ICollection<WorkingStatus> WorkingStatusUpdatedByNavigations { get; set; } = new List<WorkingStatus>();

    public virtual ICollection<WorkstationMaster> WorkstationMasterCreatedByNavigations { get; set; } = new List<WorkstationMaster>();

    public virtual ICollection<WorkstationMaster> WorkstationMasterUpdatedByNavigations { get; set; } = new List<WorkstationMaster>();

    public virtual ICollection<ZoneMaster> ZoneMasterCreatedByNavigations { get; set; } = new List<ZoneMaster>();

    public virtual ICollection<ZoneMaster> ZoneMasterUpdatedByNavigations { get; set; } = new List<ZoneMaster>();
}
