namespace AttendanceManagement.Input_Models
{
    public class WorkStationRequest
    {

        public string FullName { get; set; } = null!;

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

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }
}
