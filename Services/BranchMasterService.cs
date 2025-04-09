using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services;

public class BranchMasterService
{
    private readonly AttendanceManagementSystemContext _context;
    public BranchMasterService(AttendanceManagementSystemContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BranchMasterResponse>> GetAllBranches()
    {
        var allBranch = await (from b in _context.BranchMasters
                               join c in _context.CompanyMasters
                               on b.CompanyMasterId equals c.Id
                               select new BranchMasterResponse
                               {
                                   BranchMasterId = b.Id,
                                   FullName = b.FullName,
                                   ShortName = b.ShortName,
                                   Address = b.Address,
                                   City = b.City,
                                   District = b.District,
                                   State = b.State,
                                   Country = b.Country,
                                   PostalCode = b.PostalCode,
                                   PhoneNumber = b.PhoneNumber,
                                   Fax = b.Fax,
                                   Email = b.Email,
                                   CompanyMasterId = b.CompanyMasterId,
                                   CompanyMasterName = c.FullName,
                                   IsHeadOffice = b.IsHeadOffice,
                                   IsActive = b.IsActive,
                                   CreatedBy = b.CreatedBy
                               })
                          .ToListAsync();

        if (allBranch.Count == 0)
        {
            throw new MessageNotFoundException("No branches found");
        }
        return allBranch;
    }
    public async Task<BranchMasterResponse> GetBranchById(int branchMasterId)
    {
        var branch = await (from b in _context.BranchMasters
                            join c in _context.CompanyMasters
                            on b.CompanyMasterId equals c.Id
                            where b.Id == branchMasterId
                            select new BranchMasterResponse
                            {
                                BranchMasterId = b.Id,
                                FullName = b.FullName,
                                ShortName = b.ShortName,
                                Address = b.Address,
                                City = b.City,
                                District = b.District,
                                State = b.State,
                                Country = b.Country,
                                PostalCode = b.PostalCode,
                                PhoneNumber = b.PhoneNumber,
                                Fax = b.Fax,
                                Email = b.Email,
                                CompanyMasterId = b.CompanyMasterId,
                                CompanyMasterName = c.FullName,
                                IsHeadOffice = b.IsHeadOffice,
                                IsActive = b.IsActive,
                                CreatedBy = b.CreatedBy
                            })
                          .FirstOrDefaultAsync();

        if (branch == null)
        {
            throw new MessageNotFoundException("Branch not found");
        }
        return branch;
    }
    public async Task<string> CreateBranch(BranchMasterRequest branchMasterRequest)
    {
        var message = "Branch master added successfully.";
        if (branchMasterRequest == null)
        {
            throw new ArgumentNullException(nameof(branchMasterRequest), "Branch details cannot be null.");
        }

        var branchMaster = new BranchMaster
        {
            FullName = branchMasterRequest.FullName,
            ShortName = branchMasterRequest.ShortName,
            Address = branchMasterRequest.Address,
            City = branchMasterRequest.City,
            District = branchMasterRequest.District,
            State = branchMasterRequest.State,
            Country = branchMasterRequest.Country,
            PostalCode = branchMasterRequest.PostalCode,
            PhoneNumber = branchMasterRequest.PhoneNumber,
            Fax = branchMasterRequest.Fax,
            Email = branchMasterRequest.Email,
            IsHeadOffice = branchMasterRequest.IsHeadOffice,
            IsActive = branchMasterRequest.IsActive,
            CreatedBy = branchMasterRequest.CreatedBy,
            CreatedUtc = DateTime.UtcNow,
            CompanyMasterId = branchMasterRequest.CompanyMasterId
        };

        _context.BranchMasters.Add(branchMaster);
        await _context.SaveChangesAsync();

        return message;
    }
    public async Task<string> UpdateBranch(UpdateBranch branchMasterRequest)
    {
        var message = "Branch master updated successfully.";
        if (branchMasterRequest == null)
        {
            throw new ArgumentNullException(nameof(branchMasterRequest), "Branch details cannot be null.");
        }

        var existingBranch = _context.BranchMasters.FirstOrDefault(b => b.Id == branchMasterRequest.BranchMasterId);
        if (existingBranch == null)
        {
            throw new MessageNotFoundException("Branch not found");
        }

        existingBranch.FullName = branchMasterRequest.FullName ?? existingBranch.FullName;
        existingBranch.ShortName = branchMasterRequest.ShortName ?? existingBranch.ShortName;
        existingBranch.Address = branchMasterRequest.Address ?? existingBranch.Address;
        existingBranch.City = branchMasterRequest.City ?? existingBranch.City;
        existingBranch.District = branchMasterRequest.District ?? existingBranch.District;
        existingBranch.State = branchMasterRequest.State ?? existingBranch.State;
        existingBranch.Country = branchMasterRequest.Country ?? existingBranch.Country;
        existingBranch.PostalCode = branchMasterRequest.PostalCode;
        existingBranch.PhoneNumber = branchMasterRequest.PhoneNumber;
        existingBranch.Fax = branchMasterRequest.Fax ?? existingBranch.Fax;
        existingBranch.Email = branchMasterRequest.Email ?? existingBranch.Email;
        existingBranch.IsHeadOffice = branchMasterRequest.IsHeadOffice;
        existingBranch.IsActive = branchMasterRequest.IsActive;
        existingBranch.UpdatedBy = branchMasterRequest.UpdatedBy;
        existingBranch.UpdatedUtc = DateTime.UtcNow;
        existingBranch.CompanyMasterId = branchMasterRequest.CompanyMasterId;

        _context.Update(existingBranch);
        await _context.SaveChangesAsync();
        return message;
    }

/*    public async Task<List<AttendanceDatum>> GetStaffAttendance(string staffId)
    {
        var staffAttendance = await _atrakContext.AttendanceData.Where(s => s.StaffId == staffId).ToListAsync();
        if (staffAttendance.Count == 0)
        {
            throw new MessageNotFoundException("No attendance found for the staff");
        }
        return staffAttendance;
    }

    public async Task<string> UpdateStaffAttendance(AttendanceDatum attendanceDatum)
    {
        if (attendanceDatum == null)
        {
            throw new ArgumentNullException(nameof(attendanceDatum), "Attendance details cannot be null.");
        }

        var existingAttendance = await _atrakContext.AttendanceData
            .FirstOrDefaultAsync(a => a.Id == attendanceDatum.Id);

        if (existingAttendance == null)
        {
            throw new MessageNotFoundException("Attendance not found");
        }

        // Check if shift dates match actual dates
        if (existingAttendance.ShiftInDate.Date != attendanceDatum.ActualInDate.Date ||
            existingAttendance.ShiftOutDate.Date != attendanceDatum.ActualOutDate.Date)
        {
            throw new InvalidOperationException("Shift timing does not match actual attendance timing.");
        }

        // Calculate actual worked hours
        if (attendanceDatum.ActualInTime.HasValue && attendanceDatum.ActualOutTime.HasValue)
        {
            attendanceDatum.ActualWorkedHours = attendanceDatum.ActualOutTime.Value - attendanceDatum.ActualInTime.Value;
        }
        else
        {
            throw new InvalidOperationException("Actual in time or actual out time is missing.");
        }

        // Update existing attendance record
        existingAttendance.StaffId = attendanceDatum.StaffId ?? existingAttendance.StaffId;
        existingAttendance.AttendanceDate = attendanceDatum.AttendanceDate;
        existingAttendance.InTime = attendanceDatum.InTime;
        existingAttendance.OutTime = attendanceDatum.OutTime;
        existingAttendance.TotalHours = attendanceDatum.TotalHours;
        existingAttendance.Status = attendanceDatum.Status;
        existingAttendance.UpdatedBy = attendanceDatum.UpdatedBy;
        existingAttendance.UpdatedUtc = DateTime.UtcNow;
        existingAttendance.ActualWorkedHours = attendanceDatum.ActualWorkedHours;

        _atrakContext.Update(existingAttendance);
        await _atrakContext.SaveChangesAsync();

        return "Staff attendance updated successfully.";
    }
*/}