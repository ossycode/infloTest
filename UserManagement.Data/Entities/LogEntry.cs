using System;


namespace UserManagement.Models;

/// <summary>
/// Represents a Log entry in the system.
/// </summary>
public class LogEntry
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
