using ASPNETCoreMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreMVC.Controllers
{
    [Route("teste-di")]
    public class DiLifecycleController : Controller
    {
        public OperacaoService OperacaoService { get; }
        public OperacaoService OperacaoService2 { get; }
        public IServiceProvider ServiceProvider { get; }

        public DiLifecycleController(OperacaoService operacaoService,
                                     OperacaoService operacaoService2,
                                     IServiceProvider serviceProvider)
        {
            OperacaoService = operacaoService;
            OperacaoService2 = operacaoService2;
            ServiceProvider = serviceProvider;
        }

        public string Index()
        {
            return
                "Primeira instância: " + Environment.NewLine +
                OperacaoService.Transiente.OperacaoId + Environment.NewLine +
                OperacaoService.Scoped.OperacaoId + Environment.NewLine +
                OperacaoService.Singleton.OperacaoId + Environment.NewLine +
                OperacaoService.SingletonInstance.OperacaoId + Environment.NewLine +

                Environment.NewLine +
                Environment.NewLine +

                "Segunda instância: " + Environment.NewLine +
                OperacaoService.Transiente.OperacaoId + Environment.NewLine +
                OperacaoService.Scoped.OperacaoId + Environment.NewLine +
                OperacaoService.Singleton.OperacaoId + Environment.NewLine +
                OperacaoService.SingletonInstance.OperacaoId + Environment.NewLine;
        }

        [Route("teste")]
        public string Teste([FromServices] OperacaoService operacaoService)
        {
            return operacaoService.Transiente.OperacaoId + Environment.NewLine +
                operacaoService.Scoped.OperacaoId + Environment.NewLine +
                operacaoService.Singleton.OperacaoId + Environment.NewLine +
                operacaoService.SingletonInstance.OperacaoId + Environment.NewLine;
        }

        [Route("view")]
        public IActionResult TesteView()
        {
            return View("Index");
        }

        [Route("container")]
        public string TesteContainer()
        {
            using (var serviceScope = ServiceProvider.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var singService = services.GetRequiredService<IOperacaoSingleton>();

                return "Segunda instância: " + Environment.NewLine +
                    singService.OperacaoId + Environment.NewLine;
            }
        }
    }
}
