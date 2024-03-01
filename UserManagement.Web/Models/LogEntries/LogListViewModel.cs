using System;

namespace UserManagement.Web.Models.LogEntries;

/// <summary>
/// Represents a view model for displaying a list of logs.
/// </summary>
public class LogListViewModel
{
    /// <summary>
    /// Gets or sets the list of log items in the view model.
    /// </summary>
    public List<LogListItemViewModel> Items { get; set; } = new();
}

/// <summary>
/// Represents a view model for displaying a single log item in a list.
/// </summary>
public class LogListItemViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the log.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the log entry.
    /// </summary>
    public DateTime? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the severity level of the log entry.
    /// </summary>
    public string? Severity { get; set; }

    /// <summary>
    /// Gets or sets the message associated with the log entry.
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    /// Gets or sets the exception information associated with the log entry.
    /// </summary>
    public string? Ex { get; set; }

    /// <summary>
    /// Gets or sets the name of the action associated with the log entry.
    /// </summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with the log entry.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the trace ID associated with the log entry.
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Gets or sets the span ID associated with the log entry.
    /// </summary>
    public string? SpanId { get; set; }

    /// <summary>
    /// Gets or sets the event information associated with the log entry.
    /// </summary>
    public string? LogEvent { get; set; }
}
