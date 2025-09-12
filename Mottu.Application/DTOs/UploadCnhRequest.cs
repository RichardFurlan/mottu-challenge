using Microsoft.AspNetCore.Http;

namespace Mottu.Application.DTOs;

public record UploadCnhRequest(IFormFile File);