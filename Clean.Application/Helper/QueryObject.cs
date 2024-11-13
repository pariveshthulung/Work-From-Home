namespace Clean.Application.Helper;

public class QueryObject
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}
