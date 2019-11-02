using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Factory
{
    public abstract class Factory
    {
       public abstract Obstacle createObstacle(String id, int life_points);
    }
}
