using AttendanceManagement.Input_Models;
using AttendanceManagement.Models;
using AttendanceManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace AttendanceManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CertificateTrackingController : ControllerBase
{
    private readonly CertificateTrackingService _service;
    private readonly LoggingService _loggingService;

    public CertificateTrackingController(CertificateTrackingService service, LoggingService loggingService)
    {
        _service = service;
        _loggingService = loggingService;
    }

    [HttpGet("GetAllCertificates")]
    public async Task<IActionResult> GetAllCertificates()
    {
        try
        {
            var certificates = await _service.GetAllCertificates();
            var response = new
            {
                Success = true,
                Message = certificates
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpGet("GetCertificateById")]
    public async Task<IActionResult> GetCertificateById(int certificateId)
    {
        try
        {
            var certificate = await _service.GetCertificateById(certificateId);
            var response = new
            {
                Success = true,
                Message = certificate
            };
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("CreateCertificate")]
    public async Task<IActionResult> CreateCertificate(CertificateTrackingDto certificate)
    {
        try
        {
            var createdCertificate = await _service.CreateCertificate(certificate);
            var response = new
            {
                Success = true,
                Message = createdCertificate
            };
            await _loggingService.AuditLog("Certificate Tracking", "POST", "/CertificateTracking/CreateCertificate", createdCertificate, certificate.CreatedBy, JsonSerializer.Serialize(certificate));
            return Ok(response);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Certificate Tracking", "POST", "/CertificateTracking/CreateCertificate", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, certificate.CreatedBy, JsonSerializer.Serialize(certificate));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }

    [HttpPost("UpdateCertificate")]
    public async Task<IActionResult> UpdateCertificate(UpdateCertificateTracking updatedCertificate)
    {
        try
        {
            var success = await _service.UpdateCertificate(updatedCertificate);
            var response = new
            {
                Success = true,
                Message = success
            };
            await _loggingService.AuditLog("Certificate Tracking", "POST", "/CertificateTracking/UpdateCertificate", success ?? string.Empty, updatedCertificate.UpdatedBy, JsonSerializer.Serialize(updatedCertificate));
            return Ok(response);
        }
        catch (MessageNotFoundException ex)
        {
            await _loggingService.LogError("Certificate Tracking", "POST", "/CertificateTracking/UpdateCertificate", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedCertificate.UpdatedBy, JsonSerializer.Serialize(updatedCertificate));
            return ErrorClass.NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            await _loggingService.LogError("Certificate Tracking", "POST", "/CertificateTracking/UpdateCertificate", ex.Message, ex.StackTrace ?? string.Empty, ex.InnerException?.ToString() ?? string.Empty, updatedCertificate.UpdatedBy, JsonSerializer.Serialize(updatedCertificate));
            return ErrorClass.ErrorResponse(ex.Message);
        }
    }
}