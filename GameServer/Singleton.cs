using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace GameServer
{
    public class Singleton
    {
        private static Singleton instance = null;

        public int GameLevel;
        public int PlayersCount;

        private Singleton()
        {
            this.GameLevel = 2;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Singleton getInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        } 
    }
}
