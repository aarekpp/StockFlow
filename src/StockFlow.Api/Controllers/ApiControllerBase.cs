using Microsoft.AspNetCore.Mvc;

namespace StockFlow.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult NoContentResult() => NoContent();
    protected ActionResult<T> CreatedAtResult<T>(T value, string actionName, Func<T, object> routeValues) => CreatedAtAction(actionName, routeValues(value), value);
}
