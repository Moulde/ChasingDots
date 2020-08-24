using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace ChasingDots
{
    public static class Directions
    {
        private static Random rnd = new Random();
        public static readonly Vector2 Up = new Vector2(0, -1);
        public static readonly Vector2 Down = new Vector2(0, 1);
        public static readonly Vector2 Left = new Vector2(-1, 0);
        public static readonly Vector2 Right = new Vector2(1, 0);

        public static List<Vector2> FourDirections = new List<Vector2>();

        static Directions()
        {
            FourDirections.Add(Up);
            FourDirections.Add(Down);
            FourDirections.Add(Right);
            FourDirections.Add(Left);
        }
        public static Vector2 GetRandomDirection()
        {
            return FourDirections[rnd.Next(4)];
        }
    }
}
