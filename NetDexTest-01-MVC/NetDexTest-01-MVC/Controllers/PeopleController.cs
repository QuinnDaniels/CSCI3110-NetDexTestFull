using Microsoft.AspNetCore.Mvc;

namespace NetDexTest_01_MVC.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
