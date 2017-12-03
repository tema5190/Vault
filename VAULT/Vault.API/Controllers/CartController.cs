using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.DATA;
using Vault.Services;
using Vault.DATA.DTOs.Cards;
using Microsoft.AspNetCore.Authorization;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("cards")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserCardService _creditCardService;

        public CartController(UserCardService creditCardService)
        {
            this._creditCardService = creditCardService;
        }

        [HttpGet("")]
        public async Task<IList<UserCardDto>> GetUserCards()
        {
            var userName = User.Identity.Name;
            if (userName == null)
                return null;
            return await _creditCardService.GetUserCards(userName);
        }

        [HttpGet("{id}")]
        public async Task<UserCardDto> GetCreditCardById(int id)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.GetCreditCardById(userName, id);
        }

        [HttpPost("add")]
        public async Task<bool> AddUserCard([FromBody] UserCardDto newCard)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.AddUserCard(userName, newCard);
        }

        [HttpDelete("delete/{cardId}")]
        public async Task<bool> DeleteUserCard([FromBody] int cardId)
        {
            var userName = User.Identity.Name;
            return await _creditCardService.DeleteUserCard(userName, cardId);
        }
    }
}