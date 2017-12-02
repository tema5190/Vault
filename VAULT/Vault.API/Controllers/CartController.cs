using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Vault.Services;
using Vault.DATA.DTOs.Cards;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly CreditCardService _creditCardService;

        public CartController(CreditCardService creditCardService)
        {
            this._creditCardService = creditCardService;
        }

        [HttpGet("/cards")]
        public async Task<IList<CreditCard>> GetUserCards()
        {
            var userName = User.Identity.Name;
            return await _creditCardService.GetUserCards(userName);
        }

        [HttpPost("/cards/add")]
        public async Task<bool> AddUserCard([FromBody] CreditCardDto newCard)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.AddUserCard(userName, newCard);
        }

    }
}