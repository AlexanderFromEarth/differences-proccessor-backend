using XlsFilterService.Streamers;
using XlsFilterService.Models;

namespace XlsFilterService.Factories;

public class DifferencesStreamerFactory
{
    public string Type { get; }
    
    public DifferencesStreamerFactory(string type)
    {
        this.Type = type;
    }

    public IDifferencesStreamer Create()
    {
        return this.Type switch
        {
            "xls" => new XlsDifferencesStreamer(),
            _ => throw new Exception()
        };
    }
}
