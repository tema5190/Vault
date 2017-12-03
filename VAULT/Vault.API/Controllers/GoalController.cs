using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.Services;
using Vault.DATA.DTOs.Goal;
using Microsoft.AspNetCore.Authorization;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("goals")]
    [Authorize]
    public class GoalController : Controller
    {
        private readonly GoalService _goalService;

        public GoalController(GoalService goalService)
        {
            this._goalService = goalService;
        }

        [HttpGet("")]
        public IList<GoalDto> GetAllUserGoals()
        {
            var userName = User.Identity.Name;
            if (userName == null) return null;
            return _goalService.GetAllUserGoals(userName);
        }

        [HttpPost("add")]
        public bool CreateGoal([FromBody] GoalDto goalDto)
        {
            var userName = User.Identity.Name;
            if (userName == null) return false;
            return _goalService.CreateGoal(userName, goalDto);
        }

        [HttpPost("edit")]
        public bool UpdateGoal([FromBody] GoalDto goalDto)
        {
            var userName = User.Identity.Name;
            if (userName == null) return false;
            return _goalService.UpdateGoal(userName, goalDto);
        }
    }
}