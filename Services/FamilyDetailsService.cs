using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class FamilyDetailsService
    {
        private readonly AttendanceManagementSystemContext _context;

        public FamilyDetailsService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FamilyDetailsResponse>> GetAllFamilyDetails()
        {
            var allFamilyDetails = await _context.FamilyDetails
                .Select(f => new FamilyDetailsResponse
                {
                    FamilyDetailsId = f.Id,
                    MemberName = f.MemberName,
                    Relationship = f.Relationship,
                    DateOfBirth = f.DateOfBirth,
                    IncomePerAnnum = f.IncomePerAnnum,
                    Occupation = f.Occupation,
                    NomineeForPF = f.NomineeForPf,
                    PFSharePercentage = f.PfsharePercentage,
                    NomineeForGratuity = f.NomineeForGratuity,
                    GratuitySharePercentage = f.GratuitySharePercentage,
                    StaffCreationId = f.StaffCreationId,
                    IsActive = f.IsActive,
                    CreatedBy = f.CreatedBy
                })
                .ToListAsync();
            if (allFamilyDetails.Count == 0)
            {
                throw new MessageNotFoundException("No family details found");
            }
            return allFamilyDetails;
        }
        public async Task<string> CreateFamilyDetail(FamilyDetailsDTO familyDetailDto)
        {
            var message = "Family detail added successfully.";
            var familyDetail = new FamilyDetail
            {
                MemberName = familyDetailDto.MemberName,
                Relationship = familyDetailDto.Relationship,
                DateOfBirth = familyDetailDto.DateOfBirth,
                IncomePerAnnum = familyDetailDto.IncomePerAnnum,
                Occupation = familyDetailDto.Occupation,
                NomineeForPf = familyDetailDto.NomineeForPF,
                PfsharePercentage = familyDetailDto.PFSharePercentage,
                NomineeForGratuity = familyDetailDto.NomineeForGratuity,
                GratuitySharePercentage = familyDetailDto.GratuitySharePercentage,
                StaffCreationId = familyDetailDto.StaffCreationId,
                IsActive = familyDetailDto.IsActive,
                CreatedBy = familyDetailDto.CreatedBy,
                CreatedUtc = DateTime.UtcNow
            };
            _context.FamilyDetails.Add(familyDetail);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateFamilyDetail(UpdateFamilyDetails familyDetailDto)
        {
            var message = "Family detail updated successfully.";
            var existingDetail = _context.FamilyDetails.FirstOrDefault(f => f.Id == familyDetailDto.FamilyDetailsId);
            if (existingDetail == null)
            {
                throw new MessageNotFoundException("Family detail not found");
            }
            existingDetail.MemberName = familyDetailDto.MemberName;
            existingDetail.Relationship = familyDetailDto.Relationship;
            existingDetail.DateOfBirth = familyDetailDto.DateOfBirth;
            existingDetail.IncomePerAnnum = familyDetailDto.IncomePerAnnum;
            existingDetail.Occupation = familyDetailDto.Occupation;
            existingDetail.NomineeForPf = familyDetailDto.NomineeForPF;
            existingDetail.PfsharePercentage = familyDetailDto.PFSharePercentage;
            existingDetail.NomineeForGratuity = familyDetailDto.NomineeForGratuity;
            existingDetail.GratuitySharePercentage = familyDetailDto.GratuitySharePercentage;
            existingDetail.StaffCreationId = familyDetailDto.StaffCreationId;
            existingDetail.UpdatedBy = familyDetailDto.UpdatedBy;
            existingDetail.UpdatedUtc = DateTime.UtcNow;

            _context.Update(existingDetail);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}