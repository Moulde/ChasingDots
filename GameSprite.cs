using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChasingDots
{

    /// <summary>
    /// Simple class to make it easy to create movable sprites in an XNA game.
    /// It also implements IClonable, so you can make copies of 
    /// for example enemy spaceships, shots, etc. with very little code
    /// 
    /// Jakob "XNAFAN" Lund Krarup, December 2009
    /// http://www.xnaFan.net
    /// </summary>
    public class GameSprite : DrawableGameComponent, ICloneable
    {

        private static Color shadow = new Color(Color.Black, 64);

        #region Membervariables

        /// <summary>
        /// The image to draw in the game
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The position on screen of the GameSprite
        /// </summary>
        public Vector2 Position { get; set; }

        public float Scale { get; set; }



        /// <summary>
        /// How much to move the Texture2D when drawing to center it
        /// on the GameSprite's position (basically width/2 and height/2)
        /// </summary>
        public Vector2 TextureOffset { get; set; }

        //stores the spritebatch for use in the Draw method
        private SpriteBatch _batch;

        public Vector2 ShadowOffset { get; set; }

        #endregion


        #region Properties

        /// <summary>
        ///The Game's SpriteBatch
        /// </summary>
        protected SpriteBatch SpriteBatch
        {
            get
            {
                //if we don't have the SpriteBatch yet...
                if (_batch == null)
                {
                    //get it from the Game's services
                    //See BasicGame.LoadContent to see how the SpriteBatch
                    //was stored to make this possible
                    //And store it for future use in a membervariable
                    _batch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
                }
                return _batch;
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Creates a new GameSprite with Position and Velocity (0,0).
        /// </summary>
        /// <param name="game">A reference to the Game to use the instance in</param>
        /// <param name="texture">The image to use for the GameSprite</param>
        public GameSprite(Game game, Texture2D texture) : this(game, texture, Vector2.Zero) { }

        /// <summary>
        /// Creates a new GameSprite with Position and Velocity (0,0).
        /// </summary>
        /// <param name="game">A reference to the Game to use the instance in</param>
        /// <param name="texture">The image to use for the GameSprite</param>
        /// <param name="position">Where to place the GameSprite onscreen</param>
        public GameSprite(Game game, Texture2D texture, Vector2 position)
            : base(game)
        {
            Texture = texture;
            Position = position;
            TextureOffset = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        #endregion


        #region Update and Draw


        /// <summary>
        /// Draws the GameSprite on the SpriteBatch
        /// It is important that the Draw method is called
        /// after "SpriteBatch.Begin()"
        /// and before "SpriteBatch.End()" in the BasicGame class.
        /// </summary>
        /// <param name="gameTime">Not used here</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (ShadowOffset != Vector2.Zero)
            {
                SpriteBatch.Draw(Texture, Position - TextureOffset + ShadowOffset, shadow);
            }

            //draw the Texture2D offset so it is centered
            //Using Color.White preserves all colors in the texture
            SpriteBatch.Draw(Texture, Position - TextureOffset, Color.White);
        }

        #endregion

        public void SetSizeInPixels(float pixelSize)
        {
            this.Scale = pixelSize / (float)this.Texture.Width;
            this.TextureOffset *= (Scale);
        }


        #region ICloneable Members

        /// <summary>
        /// Creates a new instance of the GameSprite with duplicate values 
        /// </summary>
        /// <returns>A copy of the GameSprite</returns>
        public virtual object Clone()
        {
            GameSprite sprite = new GameSprite(this.Game, this.Texture, this.Position);
            return sprite;
        }
        #endregion

    }
}