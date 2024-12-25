using ASPNETCoreMVC.Configuration;
using ASPNETCoreMVC.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ASPNETCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApiConfiguration _apiConfiguration;
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(ILogger<HomeController> logger,
                              IConfiguration configuration,
                              IOptions<ApiConfiguration> apiConfiguration,
                              IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _configuration = configuration;
            _apiConfiguration = apiConfiguration.Value;
            _localizer = localizer;
        }

        //[ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {
            _logger.LogInformation("Information");
            _logger.LogCritical("Critical");
            _logger.LogWarning("Warning");
            _logger.LogError("Error");

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Lendo arquivos de configurações
            var apiConfig = new ApiConfiguration();
            _configuration.GetSection(ApiConfiguration.ConfigName).Bind(apiConfig);

            var secret = apiConfig.UserSecret;

            // obter os dados bruto sem precisar criar nenhum modelo/classe
            var user = _configuration[$"{ApiConfiguration.ConfigName}:UserKey"];

            var domain = _apiConfiguration.Domain;

            ViewData["Message"] = _localizer["Seja bem vindo!"];

            ViewData["Horario"] = DateTime.Now;

            if (Request.Cookies.TryGetValue("MeuCookie", out string? cookieValue))
            {
                ViewData["MeuCookie"] = cookieValue;
            }

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [Route("cookies")]
        public IActionResult Cookie()
        { 
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            };

            Response.Cookies.Append("MeuCookie", "Dados do Cookie", cookieOptions);

            return View();
        }

        [Route("teste")]
        public IActionResult Teste()
        {
            throw new Exception("Algo errado não está certo!");

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem =
                    "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Titulo = "Ops! Página não encontrada.";
                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                modelErro.Titulo = "Acesso Negado";
                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}
