using Microsoft.AspNetCore.Mvc;

namespace Assign1.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetBookById(int id)
        {
            ViewBag.Message = id; 
            return View();
        }
    }
}