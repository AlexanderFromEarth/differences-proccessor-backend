using Microsoft.AspNetCore.Mvc;
using XlsFilterService.Factories;
using XlsFilterService.Models;

namespace XlsFilterService.Controllers;

[ApiController]
[Route("differences")]
public class DifferencesProcessingController : ControllerBase
{
    [HttpPost(Name = "UploadFile")]
    public IEnumerable<IndicatorsDifference> UploadFile(IFormFile file)
    {
        return new DifferencesLoaderFactory(file.FileName.Split('.').Last())
            .Create()
            .Parse(file.OpenReadStream())
            .Where(diff => diff.HasYearDifference);
    }
}