using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test1.Services;

namespace Test1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        public TeamMemberController(IDbService dbService)
        {
            _dbService = dbService;
        }
        private readonly IDbService _dbService;
        
        [HttpGet("@IdTeamMember")]
        public IActionResult GetTeamMember(string id)
        {
            var teamMemberData = _dbService.GetTeamMember(id);

            if (teamMemberData is null) return Ok(teamMemberData);
            else return BadRequest();
        }
        [HttpDelete("@IdTeam")]
        public IActionResult DeleteProject(string id)
        {
            if (!_dbService.DeleteProject(id)) return BadRequest();
            else return Ok();
        }
    }
}