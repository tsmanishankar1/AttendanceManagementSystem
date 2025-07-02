using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IZoneMasterService
    {
        Task<List<ZoneMasterResponse>> GetAllZonesAsync();
        Task<string> CreateZoneAsync(ZoneMasterRequest zoneMaster);
        Task<string> UpdateZoneAsync(UpdateZoneMaster zoneMaster);
    }
}
