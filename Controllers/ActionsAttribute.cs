using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
public class ActionsAttribute:ActionFilterAttribute{
    Stopwatch? watch;
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        watch?.Stop();
        Action("OnActionExecuted",context.RouteData);
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        Action("OnActionExecuting",context.RouteData);
        watch = Stopwatch.StartNew();
    }
    public override void OnResultExecuted(ResultExecutedContext context)
    {
        Action("OnResultExecuted",context.RouteData);
    }
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        Action("OnResultExecuting",context.RouteData);
    }
    private void Action(string MethodName, RouteData routedata){
        var controllerName = routedata.Values["controller"];
        var actionName = routedata.Values["action"];
        var message = MethodName + "- Controller: " + controllerName + ", Action: " + actionName + ", Time: " + watch?.ElapsedMilliseconds.ToString()+"\n";
        Console.WriteLine(message);
    }
}

public class ExceptionFilter : ActionFilterAttribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;
        Console.WriteLine(exception);
        context.ExceptionHandled = true;
        context.Result = new ViewResult(){
            ViewName="CustomError"
        };
    }
}