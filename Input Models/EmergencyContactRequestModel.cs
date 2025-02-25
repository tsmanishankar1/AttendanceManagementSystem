namespace AttendanceManagement.Input_Models
{
    namespace AttendanceManagement.Models
    {
        public class EmergencyContactRequestModel
        {
            public string Name { get; set; } = null!;
            public string Relationship { get; set; } = null!;
            public DateOnly DateOfBirth { get; set; }
            public long LandlineNo { get; set; }
            public long MobileNo { get; set; }
            public string? EmailId { get; set; }
            public int StaffCreationId { get; set; }
            public long? OfficeExtensionPhoneNumber { get; set; }
            public string Address { get; set; } = null!;

            public int CreatedBy { get; set; }

            public DateTime CreatedUtc { get; set; }
        }
    }
    namespace AttendanceManagement.Models
    {
        public class EmergencyContactUpdateModel
        {
            public int EmergencyContactId { get; set; }
            public string Name { get; set; } = null!;
            public string Relationship { get; set; } = null!;
            public DateOnly DateOfBirth { get; set; }
            public long LandlineNo { get; set; }
            public long MobileNo { get; set; }
            public string? EmailId { get; set; }
            public int StaffCreationId { get; set; }
            public long? OfficeExtensionPhoneNumber { get; set; }
            public string Address { get; set; } = null!;
            public bool IsActive { get; set; }

            public int UpdatedBy { get; set; }


        }
    }
    namespace AttendanceManagement.Models
    {
        public class EmergencyContactResponseModel
        {
            public int EmergencyContactId { get; set; }
            public string Name { get; set; } = null!;
            public string Relationship { get; set; } = null!;
            public DateOnly DateOfBirth { get; set; }
            public long? LandlineNo { get; set; }
            public long MobileNo { get; set; }
            public string? EmailId { get; set; }
            public long? OfficeExtensionPhoneNumber { get; set; }
            public string Address { get; set; } = null!;

            public int CreatedBy { get; set; }
        }
    }
}
