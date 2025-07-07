using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class HolidayRequest
    {
        [MaxLength(255)]
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
        [MaxLength(255)]
        public string HolidayName { get; set; } = null!;
        public int HolidayTypeId { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class HolidyTypeRequest
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string HolidayTypeName { get; set; } = null!;
    }
}