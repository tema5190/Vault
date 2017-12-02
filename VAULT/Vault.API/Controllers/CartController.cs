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
    [Route("cards")]
    public class CartController : Controller
    {
        private readonly CreditCardService _creditCardService;

        public CartController(CreditCardService creditCardService)
        {
            this._creditCardService = creditCardService;
        }

        [HttpGet("")]
        public async Task<IList<CreditCard>> GetUserCards()
        {
            var userName = User.Identity.Name;
            return await _creditCardService.GetUserCards(userName);
        }

        [HttpPost("add")]
        public async Task<bool> AddUserCard([FromBody] CreditCardDto newCard)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.AddUserCard(userName, newCard);
        }

        [HttpDelete("delete")]
        public async Task<bool> DeleteUserCard([FromBody] CreditCardDto cardToDelete)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.DeleteUserCard(userName, cardToDelete);
        }

        [HttpGet("{id}")]
        public async Task<CreditCardDto> GetCreditCardById(int id)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.GetCreditCardById(userName, id);
        }

    }
}