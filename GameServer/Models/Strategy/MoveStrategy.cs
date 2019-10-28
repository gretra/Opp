using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Strategy
{
    public class MoveStrategy : IMoveStrategy
    {
        public void behaveDifferently(Player player)
        {
            player.speed = 100;
        }
    }
}
