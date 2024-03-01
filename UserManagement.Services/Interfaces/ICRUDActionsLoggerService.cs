

namespace UserManagement.Services.Interfaces;

/// <summary>
/// Interface for logging CRUD (Create, Read, Update, Delete) actions.
/// </summary>
public interface ICRUDActionsLoggerService
{
    /// <summary>
    /// Logs a CRUD action.
    /// </summary>
    /// <param name="action">The name of the action (e.g., Create, Read, Update, Delete).</param>
    /// <param name="entityId">The ID of the entity affected by the action.</param>
    void logCrudActions(string action, string entityId);

}
