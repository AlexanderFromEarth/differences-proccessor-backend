namespace XlsFilterService.Models;

public record Indicator(string? Name, string? Code, string? Amount)
{
    public bool HasDifference => this.Name == null || this.Code == null || this.Amount == null;
}
