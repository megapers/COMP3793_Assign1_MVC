using Microsoft.AspNetCore.Mvc;

namespace Assign1.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}