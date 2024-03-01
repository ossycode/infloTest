namespace UserManagement.UI.ViewModels;

public class LogEntries
{
    public int Id { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Severity { get; set; }
    public string? Msg { get; set; }
    public string? Ex { get; set; }

    public string? ActionName { get; set; }
    public string? UserId { get; set; }

    public string? TraceId { get; set; }
    public string? SpanId { get; set; }

    public string? LogEvent { get; set; }
}



public class LogEntriesList
{
    public List<LogEntries> Items { get; set; } = new();

    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
}
