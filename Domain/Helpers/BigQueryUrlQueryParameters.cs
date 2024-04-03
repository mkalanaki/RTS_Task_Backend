namespace Application.Helper;

public class BigQueryUrlQueryParameters:IUrlQueryParameters
{
    const int maxPageSize = 10;
    private int _pageSize = 10;
    public int PageNumber { get; set; } = 1;

    public int TotalRecords { get; set; } = 0;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    public bool IncludeCount { get; set; } = true;
}
