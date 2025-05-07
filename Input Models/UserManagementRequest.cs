using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class UserManagementRequest
    {
        [MaxLength(100)]
        public string Username { get; set; } = null!;
        [MaxLength(50)]
        public string Password { get; set; } = null!;
        public int CreatedBy { get; set; }
        public int StaffCreationId { get; set; }
    }

    public class UserManagementResponse
    {
        public int UserManagementId { get; set; }
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class MenuResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentMenuId { get; set; }
        public int CreatedBy { get; set; }
        public List<MenuResponse>? Children { get; set; }
    }
}