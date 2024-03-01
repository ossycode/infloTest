using Microsoft.Extensions.Logging;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;

/// <summary>
/// Service for logging CRUD (Create, Read, Update, Delete) actions.
/// </summary>
public class CRUDActionsLoggerService : ICRUDActionsLoggerService
{
    private readonly ILogger<CRUDActionsLoggerService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CRUDActionsLoggerService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public CRUDActionsLoggerService(ILogger<CRUDActionsLoggerService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs a CRUD action.
    /// </summary>
    /// <param name="actionName">The name of the action (e.g., Create, Read, Update, Delete).</param>
    /// <param name="entityId">The ID of the entity affected by the action.</param>
    public void logCrudActions(string actionName, string entityId)
    {
        _logger.LogInformation("{ActionName} action performed on User with ID {UserId} ", actionName, entityId);

    }
}
