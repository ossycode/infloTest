using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Web.Filters.ActionFilters;

/// <summary>
/// An action filter responsible for validating model state in user creation and editing actions.
/// If the model state is invalid, it populates ViewBag.Errors with error messages 
/// and returns the view with the invalid model. Otherwise, it continues with the action execution.
/// </summary>
public class UserCreateAndEditActionFilter : IAsyncActionFilter
{
    /// <summary>
    /// Performs model state validation before executing user creation and editing actions.
    /// </summary>
    /// <param name="context">The action execution context.</param>
    /// <param name="next">The delegate representing the next action execution.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is UsersController usersController)
        {
            if (!usersController.ModelState.IsValid)
            {
                usersController.ViewBag.Errors = usersController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                var viewModel = context.ActionArguments["viewModel"];
                context.Result = usersController.View(viewModel);
            }
            else
            {
                await next();

            }
        }
        else
        {
            await next();
        }
    }
}
