using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Factory
{
    public class ObstacleFactory : Factory
    {

        public Obstacle createObstacle(String input)
        {
            if (input.Equals("R"))
            {
                return new Red(1, 100);
            }
            if (input.Equals("B"))
            {
                return new Blue(2, 200);
            }
            if (input.Equals("G"))
            {
                return new Green(3, 300);
            }
            return null;
        }

        public override Obstacle createObstacle(int input1, int input2)
        {
            throw new NotImplementedException();
        }
    }
}
