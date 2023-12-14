using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ecommerce_web.Models;

namespace ecommerce_web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public static int visitCount = 0;
    private CookieOptions? visitInfo;
    public HomeController(ILogger<HomeController> logger)
    {
        ++visitCount;
        _logger = logger;
        Console.WriteLine(visitCount);
        // Response.Cookies.Append("Visit",visitCount.ToString(),visitInfo);
    }

    public IActionResult Index()
    {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
        _logger.LogInformation("In Home/Index page");
        ViewBag.Customer = TempData["Customer"];
        return View();
    }

    public IActionResult AboutUs()
    {
        if(HttpContext.Session.GetString("Customer") == "")
        {
            TempData["Session"] = "";    
        }
        else{
            TempData["Session"] = Request.Cookies["User"];
        }
        _logger.LogInformation("In AboutUs page");
        ViewBag.Customer = TempData["Customer"];
        return View();
    }    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
