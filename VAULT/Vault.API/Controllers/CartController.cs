using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Vault.Services;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Cart")]
    public class CartController : Controller
    {

        private readonly CreditCardService _creditCardService;

        public CartController(CreditCardService creditCardService)
        {
            this._creditCardService = creditCardService;
        }

        public async Task<IList<CreditCard>> GetUserCards()
        {
            var user = User.Identity.Name;
            return await _creditCardService.GetUserCards(user);
        }

        public async Task<bool> AddUserCard(int userId, CreditCard newCard)
        {
            return await _creditCardService.AddUserCard(userId, newCard);
        }

    }
}