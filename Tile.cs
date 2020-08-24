using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChasingDots
{
    public enum TileType
    { 
        Open = 0,
        Solid = 1,
        Teleporter = 2,
        StartHunter = 3,
        StartPrey = 4,
        Key = 9
    }

    public class Tile
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public TileType Type { get; set; }
        public Vector2 TileCenter { get; set; }

        public Tile(TileType type, int x, int y)
        {
            this.Type = type;
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return string.Format("(({0},{1}):{2}", this.X, this.Y, this.Type);
        }
    }
}
