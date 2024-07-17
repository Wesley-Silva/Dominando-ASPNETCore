using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreMVC.Components
{
    public class CounterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int modelcounter)
        {
            return View(modelcounter);
        }
    }
}
