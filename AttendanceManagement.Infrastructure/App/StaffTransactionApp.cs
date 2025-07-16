using AttendanceManagement.Application.Dtos.Attendance;
using AttendanceManagement.Application.Interfaces.Application;
using AttendanceManagement.Application.Interfaces.Infrastructure;

namespace AttendanceManagement.Application.App
{
    public class StaffTransactionApp : IStaffTransactionApp
    {
        private readonly IStaffTransactionInfra _staffTransactionInfra;

        public StaffTransactionApp(IStaffTransactionInfra staffTransactionInfra)
        {
            _staffTransactionInfra = staffTransactionInfra;
        }

        public async Task<string> CreateAsync(ListAcademicDetailRequest academicDetailRequests)
            => await _staffTransactionInfra.CreateAsync(academicDetailRequests);

        public async Task<string> UpdateAsync(ListAcademicDetailUpdateRequest academicDetailsRequests)
            => await _staffTransactionInfra.UpdateAsync(academicDetailsRequests);

        public async Task<string> DeleteAcademicDetailAsync(int academicDetailId, int deletedBy)
            => await _staffTransactionInfra.DeleteAcademicDetailAsync(academicDetailId, deletedBy);

        public async Task<List<AcademicDetailResponse>> GetByStaffIdAsync(int staffId)
            => await _staffTransactionInfra.GetByStaffIdAsync(staffId);

        public async Task<string> CreateAsync(ListCertificationCourseRequest certificationCourseRequests)
            => await _staffTransactionInfra.CreateAsync(certificationCourseRequests);

        public async Task<string> UpdateAsync(ListCertificationCourseUpdateRequest certificationCourseRequests)
            => await _staffTransactionInfra.UpdateAsync(certificationCourseRequests);

        public async Task<string> DeleteCertificationCourseAsync(int certificationCourseId, int deletedBy)
            => await _staffTransactionInfra.DeleteCertificationCourseAsync(certificationCourseId, deletedBy);

        public async Task<List<CertificationCourseResponse>> GetByCerticateStaffIdAsync(int staffId)
            => await _staffTransactionInfra.GetByCerticateStaffIdAsync(staffId);

        public async Task<string> CreateAsync(ListPreviousEmploymentRequest previousEmploymentRequests)
            => await _staffTransactionInfra.CreateAsync(previousEmploymentRequests);

        public async Task<string> UpdateAsync(ListPreviousEmploymentUpdateRequest previousEmploymentUpdateRequest)
            => await _staffTransactionInfra.UpdateAsync(previousEmploymentUpdateRequest);

        public async Task<string> DeleteAsync(int previousEmploymentId, int deletedBy)
            => await _staffTransactionInfra.DeleteAsync(previousEmploymentId, deletedBy);

        public async Task<List<PreviousEmploymentResponse>> GetWorkhistoryByStaffIdAsync(int staffId)
            => await _staffTransactionInfra.GetWorkhistoryByStaffIdAsync(staffId);

        public async Task<int> GetStaffCreation()
            => await _staffTransactionInfra.GetStaffCreation(); 
    }
}