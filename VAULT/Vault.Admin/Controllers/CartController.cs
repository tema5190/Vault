using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankModel;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Vault.DATA.DTOs.Cards;
using Vault.Services;

namespace Vault.Admin.Controllers
{
    public class CartController : Controller
    {

        private readonly VaultContext _db;
        private readonly UserService _userService;
        private readonly UserCardService _userCardService;
        private readonly BankOperationService _bankOperationService;

        public CartController(
            VaultContext db,
            UserService userService,
            UserCardService userCardService,
            BankOperationService bankOperationService
        ) {
            _db = db;
            _userService = userService;
            _userCardService = userCardService;
            _bankOperationService = bankOperationService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = await _userService.GetUserById(id);
            var cards = await _userCardService.GetUserCardsByUserId(id);

            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;
            return View(cards);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userService.GetUserById(id);
            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCard newCard, int id)
        {
            var user = await _userService.GetUserById(id);
            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            await _userCardService.AddUserCard(user.UserName, new UserCardDto(newCard, true));
 
            return RedirectToAction("Index", new { id });
        }

        [HttpGet("switch-pause/{userId}")]
        public async Task<IActionResult> SwitchCardPause(int userId, int cardId)
        {
            var user = await _userService.GetUserById(userId);
            await _userCardService.SwitchCardPause(user.UserName, cardId);

            return RedirectToAction("Index", new { id = user.Id });
        }

        [HttpGet("delete/{userId}")]
        public async Task<IActionResult> Delete(int userId, int cardId)
        {
            var user = await _userService.GetUserById(userId);

            await _userCardService.DeleteUserCard(user.UserName, cardId);

            return RedirectToAction("Index", new { id = user.Id});
        }

        [HttpGet("bank-card/{userId}")]
        public async Task<IActionResult> BankCard(int userId, int cardId)
        {
            var user = await _userService.GetUserById(userId);
            var card = await _userCardService.GetCreditCardById(user.UserName, cardId);

            var bankCard = await _bankOperationService.GetBankCard(card);

            return View(bankCard);
        }

        [HttpPost("bank-card")]
        public async Task<IActionResult> BankCardPost(BankCard bankCard,int id)
        {
            var user = await _userService.GetUserById(id);
            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            _bankOperationService.UpdateBankCard(bankCard);
            return RedirectToAction("Index", new { id = user.Id });
        }
    }
}