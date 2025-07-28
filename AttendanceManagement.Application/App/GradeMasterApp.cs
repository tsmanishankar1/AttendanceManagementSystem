using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App;

public class GradeMasterApp : IGradeMasterApp
{
    private readonly IGradeMasterInfra _gradeMasterInfra;
    public GradeMasterApp(IGradeMasterInfra gradeMasterInfra)
    {
        _gradeMasterInfra = gradeMasterInfra;
    }

    public async Task<string> CreateGrade(GradeMasterRequest gradeMasterRequest)
        => await _gradeMasterInfra.CreateGrade(gradeMasterRequest);

    public async Task<List<GradeMasterResponse>> GetAllGrades()
        => await _gradeMasterInfra.GetAllGrades();

    public async Task<string> UpdateGrade(UpdateGradeMaster gradeMaster)
        => await _gradeMasterInfra.UpdateGrade(gradeMaster);
}