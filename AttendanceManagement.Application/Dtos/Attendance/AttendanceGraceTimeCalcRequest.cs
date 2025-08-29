namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class AttendanceGraceTimeCalcRequest
    {
        public int GraceTimeId { get; set; }
        public int Value { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AttendanceGraceTimeCalcResponse
    {
        public int Id { get; set; }
        public int GraceTimeId { get; set; }
        public int Value { get; set; }
    }

    public class UpdateAttendanceGraceTimeCalc
    {
        public int Id { get; set; }
        public int GraceTimeId { get; set; }
        public int Value { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class AttendanceStatusResponse
    {
        public List<int> StaffId { get; set; } = null!;
        public List<int>? DepartmentId { get; set; }
        public List<int>? DivisionId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }
    }

    public class GetStaffByDepartmentDivision
    {
        public List<int>? DepartmentId { get; set; }
        public List<int>? DivisionId { get; set; }
    }

    public class AttendanceFreezeRequest
    {
        public bool IsFreezed { get; set; }
        public int FreezedBy { get; set; }
        public IEnumerable<AttendanceFreezeRecord> SelectedRows { get; set; } = null!;
    }

    public class AttendanceFreezeRecord
    {
        public int Id { get; set; }
    }
}