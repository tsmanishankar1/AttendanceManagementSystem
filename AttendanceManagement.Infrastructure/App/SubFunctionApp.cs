using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class SubFunctionMasterApp : ISubFunctionMasterApp
{
    private readonly ISubFunctionMasterInfra _subFunctionMasterInfra;
    public SubFunctionMasterApp(ISubFunctionMasterInfra subFunctionMasterInfra)
    {
        _subFunctionMasterInfra = subFunctionMasterInfra;
    }

    public async Task<string> CreateSubFunctionAsync(SubFunctionRequest subFunctionMaster)
        => await _subFunctionMasterInfra.CreateSubFunctionAsync(subFunctionMaster);

    public async Task<List<SubFunctionResponse>> GetAllSubFunctionsAsync()
        => await _subFunctionMasterInfra.GetAllSubFunctionsAsync();

    public async Task<string> UpdateSubFunctionAsync(UpdateSubFunction subFunctionMaster)
        => await _subFunctionMasterInfra.UpdateSubFunctionAsync(subFunctionMaster);
}