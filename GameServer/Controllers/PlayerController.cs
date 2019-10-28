using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameServer.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        // GET api/players
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "player1", "player2" };
        }

        // GET api/player/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "player";
        }

        // POST api/player
        [HttpPost]
        public void Post([FromBody] string player)
        {
        }

        // PUT api/player/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string player)
        {
        }

        // DELETE api/player/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}




