using ASPNETCoreMVC.Configuration;
using ASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ASPNETCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfiguration;

        public HomeController(ILogger<HomeController> logger, 
                              IConfiguration configuration,
                              IOptions<ApiConfiguration> apiConfiguration)
        {
            _logger = logger;
            _configuration = configuration;
            _apiConfiguration = apiConfiguration.Value;
        }

        public IActionResult Index()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Lendo arquivos de configurações
            var apiConfig = new ApiConfiguration();
            _configuration.GetSection(ApiConfiguration.ConfigName).Bind(apiConfig);

            var secret = apiConfig.UserSecret;

            // obter os dados bruto sem precisar criar nenhum modelo/classe
            var user = _configuration[$"{ApiConfiguration.ConfigName}:UserKey"];

            var domain = _apiConfiguration.Domain;

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
}
