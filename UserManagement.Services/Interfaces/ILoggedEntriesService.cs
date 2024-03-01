
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Interfaces;
public interface ILoggedEntriesService
{
    /// <summary>
    /// Retrieves all log entries asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, yielding a collection of all log entries.</returns>
    Task<IEnumerable<LogEntry>> GetAllLogEntriesAsync();

    /// <summary>
    /// Retrieves all log entries associated with a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A task representing the asynchronous operation, yielding a collection of log entries for the user.</returns>
    Task<IEnumerable<LogEntry>> GetAllLogEntriesForUserAsync(string userId);

    /// <summary>
    /// Retrieves a log entry by its ID asynchronously.
    /// </summary>
    /// <param name="logEntryId">The ID of the log entry.</param>
    /// <returns>A task representing the asynchronous operation, yielding the log entry with the specified ID.</returns>
    Task<LogEntry> GetLogEntryByIdAsync(int logEntryId);

}
