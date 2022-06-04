namespace XlsFilterService.Models;

public interface IDifferencesLoader
{
    public IEnumerable<IndicatorsDifference> Parse(Stream stream);
}
