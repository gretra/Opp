using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public abstract class Player
    {
        public int id;
        public string username;
        public int life_points;
        public double coordinate_x;
        public double coordinate_y;

        private IMoveStrategy algorithm;

        public int speed;


        public void setStrategy(IMoveStrategy algorithm)
        {
            this.algorithm = algorithm;
        }

        public void behaveDifferently()
        {
            this.algorithm.behaveDifferently(this);
        }
    }
}
