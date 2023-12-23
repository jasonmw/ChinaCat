using System.Diagnostics;
using ChinaCatSunflower.AppHelpers;
using Microsoft.AspNetCore.Mvc;
using ChinaCatSunflower.Models;
using ChinaCatSunflower.Repositories;

namespace ChinaCatSunflower.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FibLogRepository _fib_log_repository;

    public HomeController(ILogger<HomeController> logger, FibLogRepository fib_log_repository) {
        _logger = logger;
        _fib_log_repository = fib_log_repository;
    }

    public IActionResult Index() {
        return View();
    }

    public IActionResult ChangeMe() {
        return Content($"Changed at {DateTime.Now:O}", "text/html");
    }
    public IActionResult Fib() {
        return View();
    }

    public async Task<IActionResult> Stats() {
        var data = await _fib_log_repository.GetFibLogCounts();
        return View(data);
    }

    [Route("another-random")]
    public async Task<IActionResult> Random() {
        var val = System.Random.Shared.Next(5, 90);
        var fibb = Fibber.Fib(val);
        await _fib_log_repository.Add(fibb[val], User?.Identity?.Name);
        return PartialView("_BiggerPartial", new Tuple<long, long[]>(fibb[val], fibb));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}