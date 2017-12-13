using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vault.DATA;
using Vault.DATA.Enums;
using Vault.DATA.Models;
using Vault.Services;

namespace Vault.Admin.Controllers
{
    public class UserController : Controller
    {

        private readonly VaultContext _db;
        private readonly UserService _userService;

        public UserController(VaultContext db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _db.Users.AsNoTracking().ToList();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser(int? id)
        {
            if (id.HasValue)
            {
                var user = await _userService.GetUserById(id.Value);
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(VaultUser user)
        {
            this._db.Users.Add(user);
            await this._db.SaveChangesAsync();

            var id = user.Id;

            if(user.IsRegistrationFinished)
                return RedirectToAction("CreateClientInfo", new { id = user.Id });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CreateClientInfo(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null) return StatusCode(400);

            ViewBag.UserName = user.UserName;
            ViewBag.AuthType = user.AuthModelType;
            ViewBag.UserId = user.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientInfo(ClientInfo newClientInfo)
        {
            var user = await this._userService.GetUserById(newClientInfo.UserId);

            var clientInfo = new ClientInfo()
            {
                Email = user.AuthModelType == AuthModelType.Email ? newClientInfo.Email : null,
                Phone = user.AuthModelType == AuthModelType.Phone ? newClientInfo.Phone : null,

                Transactions = new List<RefillTransaction>(),
                Cards = new List<UserCard>(),
                Goals = new List<Goal>(),
            };

            user.ClientInfo = clientInfo;
            _db.ClientInfos.Add(clientInfo);
            user.IsRegistrationFinished = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VaultUser editedUser)
        {
            var user = await _userService.GetUserById(editedUser.Id);
            user.IsRegistrationFinished = editedUser.IsRegistrationFinished;
            user.Password = editedUser.Password;
            user.Role = editedUser.Role;
            user.AuthModelType = editedUser.AuthModelType;
            await _db.SaveChangesAsync();

            if (user.IsRegistrationFinished)
                return RedirectToAction("CreateClientInfo", new { id = user.Id });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var completed = await _userService.DeleteUserById(id);

            if (completed)
                return RedirectToAction("Index");

            return StatusCode(400);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteClientInfo(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user.IsRegistrationFinished)
            {

                user.IsRegistrationFinished = false;
                user.AuthModelType = null;

                var clientInfo = user.ClientInfo;
                if (clientInfo.Goals != null)
                    _db.Goals.RemoveRange(clientInfo.Goals);

                if (clientInfo.Transactions != null)
                    _db.Transactions.RemoveRange(clientInfo.Transactions);

                if (clientInfo.Cards != null)
                    _db.UserCards.RemoveRange(clientInfo.Cards);

                _db.ClientInfos.Remove(clientInfo);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return StatusCode(400);
        }
    }
}
