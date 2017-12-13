using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vault.DATA;
using Vault.DATA.DTOs.Goal;
using Vault.DATA.Models;
using Vault.Services;

namespace Vault.Admin.Controllers
{
    public class GoalController : Controller
    {
        private readonly VaultContext _db;
        private readonly UserService _userService;
        private readonly UserCardService _userCardService;
        private readonly BankOperationService _bankOperationService;
        private readonly GoalService _goalService;

        public GoalController(
            VaultContext db,
            UserService userService,
            UserCardService userCardService,
            BankOperationService bankOperationService,
            GoalService goalService
        )
        {
            _db = db;
            _userService = userService;
            _userCardService = userCardService;
            _bankOperationService = bankOperationService;
            _goalService = goalService;
        }

        public async Task<IActionResult> Index(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null) return StatusCode(400);

            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            return View(user.ClientInfo.Goals);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null) return StatusCode(400);

            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            if (user.ClientInfo.Cards != null && user.ClientInfo.Cards.Count != 0) { 
                ViewBag.UserCards = user.ClientInfo.Cards;
                ViewBag.CardsNotFound = false;
            }
            else {
                ViewBag.UserCards = new List<UserCard>();
                ViewBag.CardsNotFound = true;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Goal goal, int userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user == null) return StatusCode(400);

            ViewBag.UserName = user.UserName;

            _goalService.CreateGoal(user.UserName, goal);

            return RedirectToAction("Index", new { userId = user.Id});
        }
    }
}