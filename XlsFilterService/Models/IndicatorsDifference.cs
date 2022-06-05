namespace XlsFilterService.Models;

public class IndicatorsDifference
{
    public int Id { get; }
    public Indicator Previous { get; }
    public Indicator Current { get; }
    public Difference Budget { get; }
    public Difference Temporary { get; }
    public bool HasDifference { get; }
    public bool HasYearDifference() => HasDifference && (Previous.HasDifference() || Current.HasDifference());

    public IndicatorsDifference(
        int id,
        Indicator previous,
        Indicator current,
        Difference budget,
        Difference temporary,
        bool hasDifference
    )
    {
        this.Id = id;
        this.Previous = previous;
        this.Current = current;
        this.Budget = budget;
        this.Temporary = temporary;
        this.HasDifference = hasDifference;
    }
}