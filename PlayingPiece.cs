using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChasingDots
{
    public enum Role
    {
        Hunter,
        Hunted
    }

    public class PlayingPiece : GameSprite
    {

        #region Properties
        public Role Role { get; set; }
        private Vector2 _direction;
        public Vector2 Direction { get { return _direction; } set { PreviousDirection = Direction; _direction = value; } }
        public int Points { get; set; }
        public float Speed { get; set; }
        public Vector2 PreviousDirection { get; private set; }
        #endregion

        public PlayingPiece(Game game, Texture2D texture, Role role, float speed) : base (game, texture)
        {
            this.Speed = speed;
            this.Role = role;
        }

        public void ToggleRole()
        {
            if (Role == Role.Hunted)
            {
                Role = Role.Hunter;
            }
            else
            {
                Role = Role.Hunted;
            }
        }

        public override object Clone()
        {
            PlayingPiece clone = new PlayingPiece(Game, Texture, Role, Speed);
            clone.Direction = this.Direction;
            clone.Points = this.Points;
            clone.ShadowOffset = this.ShadowOffset;
            clone.TextureOffset = this.TextureOffset;
            clone.Scale = this.Scale;
            clone.Position = this.Position;
            return clone;
        }

    }
}
