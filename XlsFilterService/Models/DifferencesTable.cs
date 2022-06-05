namespace XlsFilterService.Models;

public class DifferencesTable
{
    public string FileHash { get; }
    public string FileType { get; }
    public IEnumerable<int> SystemRows { get; }
    public IEnumerable<IndicatorsDifference> Rows { get; }

    public DifferencesTable OnlyHasYearDifference() => new(
        this.FileHash,
        this.FileType,
        this.SystemRows,
        this.Rows.Where(diff => diff.HasYearDifference())
    );

    public DifferencesTable(string fileHash, string fileType, IEnumerable<int> systemRows,
        IEnumerable<IndicatorsDifference> rows)
    {
        this.FileHash = fileHash;
        this.FileType = fileType;
        this.SystemRows = systemRows;
        this.Rows = rows;
    }
}