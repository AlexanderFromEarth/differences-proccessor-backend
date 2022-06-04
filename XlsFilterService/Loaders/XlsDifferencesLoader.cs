using System.Text.RegularExpressions;
using Syncfusion.XlsIO;
using XlsFilterService.Models;

namespace XlsFilterService.Loaders;

public class XlsDifferencesLoader : IDifferencesLoader
{
    public IEnumerable<IndicatorsDifference> Parse(Stream stream)
    {
        using var excelEngine = new ExcelEngine();
        var application = excelEngine.Excel;
        var workbook = application.Workbooks.Open(stream);
        var worksheet = workbook.Worksheets[0];
        var regex = new Regex("[XxХх]");
        
        string? ParseString(IRange cell) =>
            !regex.IsMatch(cell.Text?.Trim() ?? "") ? cell.Text ?? "" : null;

        return worksheet.Rows
            .Where(row => (row?.Cells?.Length ?? 0) >= 11)
            .Select(
                row =>
                    new IndicatorsDifference(
                        new Indicator(
                            ParseString(row.Cells[0]),
                            ParseString(row.Cells[1]),
                            ParseString(row.Cells[2])
                        ),
                        new Indicator(
                            ParseString(row.Cells[3]),
                            ParseString(row.Cells[4]),
                            ParseString(row.Cells[5])
                        ),
                        new Difference(
                            ParseString(row.Cells[6]),
                            ParseString(row.Cells[7])
                        ),
                        new Difference(
                            ParseString(row.Cells[8]),
                            ParseString(row.Cells[9])
                        ),
                        regex.IsMatch(row.Cells[10].Text?.Trim() ?? "")
                    )
            )
            .ToArray();
    }
}