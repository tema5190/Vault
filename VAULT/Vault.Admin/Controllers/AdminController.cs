using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Vault.Admin.Models;

namespace Vault.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
