using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Syncfusion.Drawing;
using Syncfusion.XlsIO;
using XlsFilterService.Models;

namespace XlsFilterService.Streamers;

public class XlsDifferencesStreamer : IDifferencesStreamer
{
    public DifferencesTable Parse(Stream stream)
    {
        using var excelEngine = new ExcelEngine();
        var application = excelEngine.Excel;
        var workbook = application.Workbooks.Open(stream);
        var worksheet = workbook.Worksheets[0];
        var regex = new Regex("[XxХх]");

        string? ParseString(IRange cell) =>
            !regex.IsMatch(cell.Text?.Trim() ?? "") ? cell.Text ?? "" : null;

        var rows = worksheet.UsedRange.Rows
            .Where(row => (row?.Cells?.Length ?? 0) >= 11)
            .Where(row => row.CellStyle?.Color != Color.FromArgb(255, 255, 255, 255))
            .Select(
                row =>
                    new IndicatorsDifference(
                        row.Row,
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
        var selectedRows = rows
            .Select(row => row.Id)
            .ToHashSet();
        var systemRows = worksheet.UsedRange.Rows
            .Where(row => !selectedRows.Contains(row.Row))
            .Select(row => row.Row)
            .ToArray();
        
        var hashStream = new MemoryStream();
        workbook.SaveAs(hashStream);
        
        using var md5 = MD5.Create();
        var hash = BitConverter
            .ToString(md5.ComputeHash(hashStream))
            .Replace("-", "")
            .ToLowerInvariant();

        using var outStream = new FileStream($@"/tmp/{hash}", FileMode.Create);
        workbook.SaveAs(outStream);

        return new DifferencesTable(hash, "xls", systemRows, rows);
    }

    public Stream Dump(DifferencesTable differences)
    {
        using var stream = new FileStream($@"/tmp/{differences.FileHash}", FileMode.Open);
        using var excelEngine = new ExcelEngine();
        var application = excelEngine.Excel;
        var workbook = application.Workbooks.Open(stream);
        var worksheet = workbook.Worksheets[0];
        var rows = differences.SystemRows
            .Concat(differences.Rows.Select(row => row.Id))
            .ToHashSet();
        foreach (var row in worksheet.Rows.Reverse().ToArray())
        {
            if (!rows.Contains(row.Row))
            {
                worksheet.DeleteRow(row.Row);
            }
        }
        
        var outStream = new MemoryStream();

        workbook.SaveAs(outStream);

        return outStream;
    }
}