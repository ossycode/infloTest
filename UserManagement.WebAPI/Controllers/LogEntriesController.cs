using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Interfaces;
using UserManagement.WebAPI.DTO;


namespace UserManagement.WebAPI.Controllers;

/// <summary>
/// API controller for managing log entries.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LogEntriesController : ControllerBase
{
    private readonly ILoggedEntriesService _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogEntriesController"/> class.
    /// </summary>
    /// <param name="logEntriesService">The logged entries service.</param>
    public LogEntriesController(ILoggedEntriesService logEntriesService)
    {
        _logService = logEntriesService;
    }


    /// <summary>
    /// Retrieves a paginated list of log entries.
    /// </summary>
    /// <param name="pageNo">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetAllLogs(int pageNo = 1, int pageSize = 10)
    {
        try
        {
            var logs = await _logService.GetAllLogEntriesAsync();
            var paginatedLogs = Paginate(logs, pageNo, pageSize);
            var totalRecords = logs.Count();
            var totalPage = (int)Math.Ceiling((double)totalRecords / pageSize);

            var data = new PaginatedResult<LogEntry>
            {
                Items = paginatedLogs.Select(p => new LogEntry
                {
                    Id = p.Id,
                    Timestamp = p.Timestamp,
                    UserId = p.UserId,
                    LogEvent = p.LogEvent,
                    Severity = p.Severity,
                    Msg = p.Msg,
                    ActionName = p.ActionName,
                    SpanId = p.SpanId,
                    TraceId = p.TraceId,
                }).ToList(),
                TotalPages = totalPage,
                TotalRecords = totalRecords,
            };
            return Ok(data);
        }
        catch (Exception ex)
        {
            return Problem(detail: $"An error occurred while fetching the logs: {ex.Message}", statusCode: 500, title: "Get Logs");
        }
    }


    /// <summary>
    /// Retrieves the log entry with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the log entry.</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<LogEntry>> GetLogById(int id)
    {
        var log = await _logService.GetLogEntryByIdAsync(id);

        if (log == null)
        {
            return Problem(detail: "Log not found with given ID", statusCode: 400, title: "Get Log"); ;
        }
        return log;
    }

    /// <summary>
    /// Paginates a collection of log entries based on the specified page number and page size.
    /// </summary>
    /// <param name="logs">The collection of log entries to paginate.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The paginated collection of log entries.</returns>
    private IEnumerable<LogEntry> Paginate(IEnumerable<LogEntry> logs, int pageNumber, int pageSize)
    {
        return logs.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

}
