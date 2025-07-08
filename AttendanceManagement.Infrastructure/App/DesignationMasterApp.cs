using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class DesignationMasterApp : IDesignationMasterApp
    {
        private readonly IDesignationMasterInfra _designationMasterInfra;
        public DesignationMasterApp(IDesignationMasterInfra designationMasterInfra)
        {
            _designationMasterInfra = designationMasterInfra;
        }

        public async Task<string> AddDesignation(DesignationRequest designationRequest)
            => await _designationMasterInfra.AddDesignation(designationRequest);

        public async Task<List<DesignationResponse>> GetAllDesignations()
            => await _designationMasterInfra.GetAllDesignations();

        public async Task<string> UpdateDesignation(UpdateDesignation designation)
            => await _designationMasterInfra.UpdateDesignation(designation);
    }
}