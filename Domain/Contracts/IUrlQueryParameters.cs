namespace Application.Helper;

public interface IUrlQueryParameters
{
    int PageSize { get; set; }
    int PageNumber { get; set; }
    int TotalRecords { get; set; }
    public bool IncludeCount { get; set; }
}