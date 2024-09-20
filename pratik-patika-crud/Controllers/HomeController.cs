using Microsoft.AspNetCore.Mvc;
using pratik_patika_crud.Models;
using System.Diagnostics;

namespace pratik_patika_crud.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("List","Task");
        }

        public IActionResult Privacy()
        {
            return View();
        }


    }
}
