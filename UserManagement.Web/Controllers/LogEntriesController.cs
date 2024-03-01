
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.LogEntries;

namespace UserManagement.Web.Controllers;

[Route("[controller]")]
public class LogEntriesController : Controller
{
    private readonly ILoggedEntriesService _logService;

    public LogEntriesController(ILoggedEntriesService logEntriesService)
    {
        _logService = logEntriesService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Log Entries";

        IEnumerable<LogListItemViewModel> items;

        var allLogs = await _logService.GetAllLogEntriesAsync();

        items = allLogs.Select(p => new LogListItemViewModel
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
        });


        var model = new LogListViewModel
        {
            Items = items.ToList()
        };



        return View(model);
    }

    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Logs Entries | Log Details";

        var log = await _logService.GetLogEntryByIdAsync(id);

        if (log == null)
        {
            return RedirectToAction("Index", "LogEntries");
        }
        var logViewModel = new LogListItemViewModel
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            UserId = log.UserId,
            LogEvent = log.LogEvent,
            Severity = log.Severity,
            Msg = log.Msg,
            ActionName = log.ActionName,
            SpanId = log.SpanId,
            TraceId = log.TraceId,
        };
        return View(logViewModel);
    }

}
