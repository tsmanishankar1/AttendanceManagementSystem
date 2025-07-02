using AttendanceManagement.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagement.Services.Interface
{
    public interface IDesignationMasterService
    {
        Task<List<DesignationResponse>> GetAllDesignations();
        Task<string> AddDesignation(DesignationRequest designationRequest);
        Task<string> UpdateDesignation(UpdateDesignation designation);
    }
}
