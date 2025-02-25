using AttendanceManagement.Input_Models;
using AttendanceManagement.Input_Models.AttendanceManagement.Models;
using AttendanceManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Services
{
    public class EmergencyContactService
    {
        private readonly AttendanceManagementSystemContext _context;

        public EmergencyContactService(AttendanceManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<List<EmergencyContactResponseModel>> GetAllAsync()
        {
            var emergency = await _context.EmergencyContacts
                .Select(ec => new EmergencyContactResponseModel
                {
                    EmergencyContactId = ec.Id,
                    Name = ec.Name,
                    Relationship = ec.Relationship,
                    DateOfBirth = ec.DateOfBirth,
                    LandlineNo = ec.LandlineNo,
                    MobileNo = ec.MobileNo,
                    EmailId = ec.EmailId,
                    OfficeExtensionPhoneNumber = ec.OfficeExtensionPhoneNumber,
                    Address = ec.Address,
                    CreatedBy = ec.CreatedBy
                })
                .ToListAsync();
            if (emergency.Count == 0)
                throw new MessageNotFoundException("No Emergency Contacts found");
            return emergency;
        }

        public async Task<EmergencyContactResponseModel> GetByIdAsync(int emergencyContactId)
        {
            var emergency = await _context.EmergencyContacts
                .Where(ec => ec.Id == emergencyContactId && ec.IsActive)
                .Select(ec => new EmergencyContactResponseModel
                {
                    EmergencyContactId = ec.Id,
                    Name = ec.Name,
                    Relationship = ec.Relationship,
                    DateOfBirth = ec.DateOfBirth,
                    LandlineNo = ec.LandlineNo,
                    MobileNo = ec.MobileNo,
                    EmailId = ec.EmailId,
                    OfficeExtensionPhoneNumber = ec.OfficeExtensionPhoneNumber,
                    Address = ec.Address
                })
                .FirstOrDefaultAsync();
            if (emergency == null)
                throw new MessageNotFoundException("Emergency Contact not found");
            return emergency;
        }

        public async Task<string> CreateAsync(EmergencyContactRequestModel model)
        {
            var message = "Emergency Contact created successfully";
            var newEmergencyContact = new EmergencyContact
            {
                Name = model.Name,
                Relationship = model.Relationship,
                DateOfBirth = model.DateOfBirth,
                LandlineNo = model.LandlineNo,
                MobileNo = model.MobileNo,
                EmailId = model.EmailId,
                OfficeExtensionPhoneNumber = model.OfficeExtensionPhoneNumber,
                Address = model.Address,
                IsActive = true,
                CreatedBy = model.CreatedBy,
                CreatedUtc = DateTime.UtcNow,
                StaffCreationId = model.StaffCreationId
            };

            _context.EmergencyContacts.Add(newEmergencyContact);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<string> UpdateAsync(EmergencyContactUpdateModel model)
        {
            var message = "Emergency Contact updated successfully";
            var emergencyContact = await _context.EmergencyContacts.FirstOrDefaultAsync(lg => lg.Id == model.EmergencyContactId && lg.IsActive);
            if (emergencyContact == null) throw new MessageNotFoundException("Emergency Contact not found");

            emergencyContact.Name = model.Name;
            emergencyContact.Relationship = model.Relationship;
            emergencyContact.DateOfBirth = model.DateOfBirth;
            emergencyContact.LandlineNo = model.LandlineNo;
            emergencyContact.MobileNo = model.MobileNo;
            emergencyContact.EmailId = model.EmailId;
            emergencyContact.StaffCreationId = model.StaffCreationId;
            emergencyContact.OfficeExtensionPhoneNumber = model.OfficeExtensionPhoneNumber;
            emergencyContact.Address = model.Address;
            emergencyContact.IsActive = model.IsActive;
            emergencyContact.UpdatedBy = model.UpdatedBy;
            emergencyContact.UpdatedUtc = DateTime.UtcNow;

            _context.EmergencyContacts.Update(emergencyContact);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
