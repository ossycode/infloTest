

using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UserManagement.Data;
using UserManagement.Services.Interfaces;
using UserManagement.Models;
using System.Linq;

namespace UserManagement.Services.Implementations;
public class LoggedEntriesService : ILoggedEntriesService
{
    private readonly DataContext _dataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggedEntriesService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context instance.</param>
    public LoggedEntriesService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    /// <summary>
    /// Retrieves all logged entries asynchronously.
    /// </summary>
    /// <returns>A collection of logged entries.</returns>
    public async Task<IEnumerable<LogEntry>> GetAllLogEntriesAsync()
    {
        return await _dataContext.GetAllAsync<LogEntry>();
    }

    /// <summary>
    /// Retrieves all logged entries for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A collection of logged entries belonging to the specified user.</returns>
    public async Task<IEnumerable<LogEntry>> GetAllLogEntriesForUserAsync(string userId)
    {
        var allLogs = await _dataContext.GetAllAsync<LogEntry>();
        return allLogs.Where(log => log.UserId == userId);
    }

    /// <summary>
    /// Retrieves a logged entry by its identifier asynchronously.
    /// </summary>
    /// <param name="logEntryId">The ID of the logged entry.</param>
    /// <returns>The logged entry with the specified ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the logged entry with the specified ID is not found.</exception>
    public async Task<LogEntry> GetLogEntryByIdAsync(int logEntryId)
    {
        var logEntry = await _dataContext.GetEntityByIdAsync<LogEntry, int>(logEntryId);

        if (logEntry == null)
        {
            throw new InvalidOperationException($"Log entry with ID {logEntryId} not found.");
        }

        return logEntry;
    }
}
