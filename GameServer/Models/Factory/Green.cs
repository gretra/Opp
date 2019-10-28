using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Factory
{
    public class Green : Obstacle
    {
        public Green(int id, int life_points)
        {
            this.id = id;
            this.life_points = life_points;
        }
    }
}
