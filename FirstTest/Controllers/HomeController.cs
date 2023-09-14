using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FirstTest.Models;
using Rystem.OpenAi;
using System.Text;

namespace FirstTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private string _models;
    private IOpenAiFactory _openAiFactory;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IOpenAiFactory openAiFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _openAiFactory = openAiFactory;
    }

    public IActionResult Index()
    {
        GetModelNames().GetAwaiter().GetResult(); 
        ViewBag.ModelNames = _models;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
    private async Task GetModelNames()
    {
        var apiKeyValue = _configuration["OpenAi:ApiKey"];

        var openAiApi = _openAiFactory.Create();

        StringBuilder sb = new StringBuilder();
        var results = await openAiApi.Model.ListAsync();

        foreach(var model in results)
        {
            sb.Append("<p> " + model.Id + " </p>");
        }

        _models = sb.ToString();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

