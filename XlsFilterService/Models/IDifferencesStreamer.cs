namespace XlsFilterService.Models;

public interface IDifferencesStreamer
{
    public DifferencesTable Parse(Stream stream);
    public Stream Dump(DifferencesTable differences);
}
