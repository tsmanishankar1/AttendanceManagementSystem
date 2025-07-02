using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IWeeklyOffService
    {
        Task<string> CreateWeeklyOffAsync(WeeklyOffRequest weeklyOffRequest);
        Task<List<WeeklyOffResponse>> GetAllWeeklyOffsAsync();
        Task<string> UpdateWeeklyOffAsync(UpdateWeeklyOff updatedWeeklyOff);
    }
}
