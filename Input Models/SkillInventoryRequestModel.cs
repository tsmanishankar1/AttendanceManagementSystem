namespace AttendanceManagement.Input_Models
{
    public class SkillInventoryRequestModel
    {

        public string Name { get; set; } = null!;
        public int LevelOfProficiency { get; set; }
        public string Notes { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SkillInventoryUpdateModel
    {
        public int SkillId { get; set; }
        public string Name { get; set; } = null!;
        public int LevelOfProficiency { get; set; }
        public string Notes { get; set; } = null!;
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class SkillInventoryResponseModel
    {
        public int SkillId { get; set; }
        public int StaffCreationId { get; set; }
        public string Name { get; set; } = null!;
        public int LevelOfProficiency { get; set; }
        public string Notes { get; set; } = null!;

    }
}
