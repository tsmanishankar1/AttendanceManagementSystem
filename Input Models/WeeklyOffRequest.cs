namespace AttendanceManagement.Input_Models
{
    public class WeeklyOffRequest
    {
        public string WeeklyOffName { get; set; } = null!;

        public int MarkWeeklyOff { get; set; }
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }

    public class WeeklyOffResponse
    {
        public int WeeklyOffId { get; set; }

        public string WeeklyOffName { get; set; } = null!;

        public string MarkWeeklyOff { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateWeeklyOff
    {
        public int WeeklyOffId { get; set; }

        public string WeeklyOffName { get; set; } = null!;

        public int MarkWeeklyOff { get; set; }

        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }

    public enum WeekdaysEnum
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
}
