using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models.Factory
{
    public abstract class Obstacle
    {
        public int id;
        public int life_points;

        public Obstacle(int id, int life_points)
        {
            this.id = id;
            this.life_points = life_points;
        }

        public int getLifePoints()
        {
            return life_points;
        }

        public void setLifePoints(int life_points)
        {
            this.life_points = life_points;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void sayHello()
        {
            Console.WriteLine("Si kliutis turi " + life_points + " gyvybes tasku");
        }
    }
}
