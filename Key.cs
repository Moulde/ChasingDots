using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChasingDots
{
    public class Key : GameSprite
    {
        public Key(Game game, Texture2D texture) : base(game, texture)
        {

        }

        public Vector2 ForPosition { get; set; }
    }
}
