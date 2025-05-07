using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class WorkStationRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class WorkStationResponse
    {
        public int WorkstationMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateWorkStation
    {
        public int WorkstationMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }
}