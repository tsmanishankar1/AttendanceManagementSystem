namespace AttendanceManagement.Input_Models
{
    public class HolidayRequest
    {
        public string HolidayName { get; set; } = null!;

        public int HolidayTypeId { get; set; }

        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class HolidayResponse
    {
        public int HolidayMasterId { get; set; }

        public string HolidayName { get; set; } = null!;
        public int HolidayTypeId { get; set; }
        public string HolidayTypeName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateHoliday
    {
        public int HolidayMasterId { get; set; }

        public string HolidayName { get; set; } = null!;

        public int HolidayTypeId { get; set; }

        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
    public class HolidyTypeRequest
    {
        public int Id { get; set; }
        public string HolidayTypeName { get; set; } = null!;
    }

}
