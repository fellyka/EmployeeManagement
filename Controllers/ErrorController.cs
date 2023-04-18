using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;
        /* 1.Inject an instance of ILogger
           2.Specify the type of the Controller into which ILogger is injected,
             in this case <ErrorController> as the generic argument */
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";

                    /* Use Logging - Do not Display all these info to the client
                     * ViewBag.Path = statusCodeResult.OriginalPath;
                     ViewBag.QS = statusCodeResult.OriginalQueryString;
                    */
                    _logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}" +
                        $" and QueryString = {statusCodeResult.OriginalQueryString}");

                    break;

                    /* We can Add others Errors here*/

            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            /* It is a security risk to display these information in production.
             * It's better to log these information for the dev team
             * 
             iewBag.ExceptionPath = exceptionDetails.Path;
             ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
             ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

             */
            _logger.LogError($"The Path {exceptionDetails.Path} threw an exception" +
                $" {exceptionDetails.Error}");

            return View("Error");
        }
    }
}
