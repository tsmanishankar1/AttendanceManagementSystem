using AttendanceManagement.Application.Dtos.Attendance;

namespace AttendanceManagement.Application.Interfaces.Application
{
    public interface IStaffTransactionApp
    {
        Task<List<AcademicDetailResponse>> GetByStaffIdAsync(int staffId);
        Task<string> CreateAsync(ListAcademicDetailRequest academicDetailRequests);
        Task<string> UpdateAsync(ListAcademicDetailUpdateRequest academicDetailsRequests);
        Task<string> DeleteAcademicDetailAsync(int academicDetailId, int deletedBy);
        Task<List<CertificationCourseResponse>> GetByCerticateStaffIdAsync(int staffId);
        Task<string> CreateAsync(ListCertificationCourseRequest certificationCourseRequests);
        Task<string> UpdateAsync(ListCertificationCourseUpdateRequest certificationCourseRequests);
        Task<string> DeleteCertificationCourseAsync(int certificationCourseId, int deletedBy);
        Task<List<PreviousEmploymentResponse>> GetWorkhistoryByStaffIdAsync(int staffId);
        Task<string> CreateAsync(ListPreviousEmploymentRequest previousEmploymentRequests);
        Task<string> UpdateAsync(ListPreviousEmploymentUpdateRequest previousEmploymentUpdateRequest);
        Task<string> DeleteAsync(int previousEmploymentId, int deletedBy);
    }
}
