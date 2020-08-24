using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChasingDots
{
    public class SmallPop
    {
        public float Timeout { get; set; }
        public Vector2 Position { get; set; }

        public SmallPop(float timeout, Vector2 position)
        {
            this.Timeout = timeout;
            this.Position = position;
        }
    }
}
