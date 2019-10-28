using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Strategy
{
    public class StopStrategy : IMoveStrategy
    {
        public void behaveDifferently(Player player)
        {
            player.speed = 0;
        }
    }
}
