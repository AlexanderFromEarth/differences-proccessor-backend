using Microsoft.AspNetCore.Mvc;
using XlsFilterService.Factories;
using XlsFilterService.Models;

namespace XlsFilterService.Controllers;

[ApiController]
[Route("differences")]
public class DifferencesProcessingController : ControllerBase
{
    [HttpPost("upload", Name = "UploadFile")]
    public DifferencesTable UploadFile(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        return new DifferencesStreamerFactory(file.FileName.Split('.').Last())
            .Create()
            .Parse(stream)
            .OnlyHasYearDifference();
    }

    [HttpPost("download", Name = "DownloadFile")]
    public FileStreamResult DownloadFile(DifferencesTable differences)
    {
        var stream = new DifferencesStreamerFactory(differences.FileType)
            .Create()
            .Dump(differences);
        
        stream.Position = 0;
        
        return File(
            stream,
            "application/octet-stream",
            "result.xls"
        );
    }
}