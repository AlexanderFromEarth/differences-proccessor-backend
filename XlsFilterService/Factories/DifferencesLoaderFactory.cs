using XlsFilterService.Loaders;
using XlsFilterService.Models;

namespace XlsFilterService.Factories;

public class DifferencesLoaderFactory
{
    public string Type { get; }
    
    public DifferencesLoaderFactory(string type)
    {
        this.Type = type;
    }

    public IDifferencesLoader Create()
    {
        return this.Type switch
        {
            "xls" => new XlsDifferencesLoader(),
            _ => throw new Exception()
        };
    }
}
