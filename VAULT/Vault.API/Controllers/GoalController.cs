using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vault.Services;
using Vault.DATA.DTOs.Goal;

namespace Vault.API.Controllers
{
    [Produces("application/json")]
    [Route("goals")]
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
        public bool CreateGoal(GoalDto goalDto)
        {
            var userName = User.Identity.Name;
            if (userName == null) return false;
            return _goalService.CreateGoal(userName, goalDto);
        }
    }
}