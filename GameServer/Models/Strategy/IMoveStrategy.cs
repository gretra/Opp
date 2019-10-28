using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServer.Models
{
    public interface IMoveStrategy
    {
        void behaveDifferently(Player player);
    }
}
