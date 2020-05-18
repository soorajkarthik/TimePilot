using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimePilot
{
    class Boss : Enemy
    {
        public Boss()
        {

        }

        public Boss(Random r) : base(r)
        {
            health = 10;
        }
    }
}