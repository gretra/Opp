using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Factory
{
    public class ObstacleFactory : Factory
    {

        public override Obstacle createObstacle(String input, int life_points)
        {
            if (input.Equals("R"))
            {
                return new Red(1, life_points);
            }
            if (input.Equals("B"))
            {
                return new Blue(2, life_points);
            }
            if (input.Equals("G"))
            {
                return new Green(3, life_points);
            }
            return null;
        }

       
    }
}
