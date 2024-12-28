using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreMVC.Controllers
{
    public class TesteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
