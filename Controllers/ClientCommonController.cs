using Microsoft.AspNetCore.Mvc;

namespace GRC_NewClientPortal.Controllers
{
    public class ClientCommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
