using Microsoft.AspNetCore.Mvc;

namespace AccountingSystem.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Students()
        {                                  
            return View();
        }
    }
}