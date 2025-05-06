namespace AttendanceManagement.Input_Models
{
    public class CostMasterRequest
    {
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateCostMaster
    {
        public int CostCentreMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class CostMasterResponse
    {
        public int CostCentreMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
}