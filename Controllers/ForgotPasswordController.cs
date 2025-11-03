using Microsoft.AspNetCore.Mvc;

namespace GRC_NewClientPortal.Controllers
{
    public class ForgotPasswordController : Controller
    {
        [HttpGet("/forgotpassword")] // <— this defines the route explicitly
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
