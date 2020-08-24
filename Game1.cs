using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChasingDots
{
    public enum GameState
    {
        Introduction,
        Playing,
        BetweenRounds,
        Paused,
        GameOver,
        Credits,
        Help
    }

    public class Game1 : Game
    {
        bool EnterKeyHasBeenUp = false;
        Color shadowColor = new Color(0, 0, 0, 64);
        float betweenRoundsPauseLeft, betweenRoundsPause = 2000;
        int maxSimultaneousClones = 5;
        int initialClonesLeft = 10, clonesLeft;
        float millisecondsToNextPointPayout, millisecondsBetweenPayouts = 1000;
        KeyboardState keyState, oldState;
        SoundEffectInstance sound_music_intro, sound_music_a;
        SoundEffect sound_monster;
        int currentRound = 0;
        int numberOfRounds = 8;
        Random rnd = new Random();
        float pointsPerSecond = 100;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tile, black, white, key, tex_winPlayer1, tex_winPlayer2, tex_introduction, texture_draw, texture_help, texture_ready, pop, texture_credits;
        PlayingPiece[] pieces = new PlayingPiece[2];

        PlayingPiece player1, player2;
        float gameSpeed = 0.1F;

        GameState StateOfGame = GameState.Introduction;

        Vector2 levelOffset = new Vector2(140, 40);
        int tileSize = 40;
        SpriteFont debugFont, standardFont, bigFont;
        List<PlayingPiece> clones = new List<PlayingPiece>();


        Dictionary<PlayingPiece, List<Keys>> PlayerKeys = new Dictionary<PlayingPiece, List<Keys>>();
        Dictionary<Keys, Vector2> KeyDirections = new Dictionary<Keys, Vector2>();
        PlayingPiece hunter;
        Dictionary<PlayingPiece, PlayingPiece> Adversary = new Dictionary<PlayingPiece, PlayingPiece>();
        float hunterSpeed = 1.3F, huntedSpeed = 1;

        List<SoundEffect> soundEffect = new List<SoundEffect>();
        Random random = new Random();
        SoundEffect sound_OpenTeleports;
        List<SmallPop> pops = new List<SmallPop>();

        Level currentLevel;
        public int CurrentLevelIndex { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            pieces = new PlayingPiece[2];
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;

            graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //add spritebatch to services for use in GameSprite
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);

            tile = Content.Load<Texture2D>("tile");
            black = Content.Load<Texture2D>("black");
            white = Content.Load<Texture2D>("white");
            key = Content.Load<Texture2D>("key");
            pop = Content.Load<Texture2D>("pop");

            tex_introduction = Content.Load<Texture2D>("chasingdotstitle");
            tex_winPlayer1 = Content.Load<Texture2D>("player1wins");
            tex_winPlayer2 = Content.Load<Texture2D>("player2wins");
            texture_draw = Content.Load<Texture2D>("draw");
            texture_help = Content.Load<Texture2D>("help");
            texture_ready = Content.Load<Texture2D>("ready");

            player1 = new PlayingPiece(this, white, Role.Hunter, huntedSpeed);
            player2 = new PlayingPiece(this, black, Role.Hunted, hunterSpeed);

            pieces[0] = player1;
            pieces[1] = player2;
            foreach (PlayingPiece piece in pieces)
            {
                piece.Position = Vector2.One * -200;
            }
            hunter = player1;

            Adversary[player1] = player2;
            Adversary[player2] = player1;


            #region Sound
            SoundEffect introsong = Content.Load<SoundEffect>("Audio/HowlingTheme_intro");
            sound_music_intro = introsong.CreateInstance();

            soundEffect.Add(Content.Load<SoundEffect>("Audio/blub1"));
            soundEffect.Add(Content.Load<SoundEffect>("Audio/blub2"));
            soundEffect.Add(Content.Load<SoundEffect>("Audio/blub3"));

            SoundEffect music_a = Content.Load<SoundEffect>("Audio/HowlingTheme_A");
            sound_music_a = music_a.CreateInstance();
            sound_music_a.IsLooped = true;
            texture_credits = Content.Load<Texture2D>("credits");

            sound_OpenTeleports = Content.Load<SoundEffect>("Audio/openteleports");

            sound_monster = Content.Load<SoundEffect>("Audio/Monster");
            #endregion

            player1.SetSizeInPixels(tileSize * .8F);
            player2.SetSizeInPixels(tileSize * .8F);
            // TODO: use this.Content to load your game content here

            debugFont = Content.Load<SpriteFont>("debugFont");
            standardFont = Content.Load<SpriteFont>("standardFont");
            bigFont = Content.Load<SpriteFont>("bigFont");

            KeyDirections.Add(Keys.Up, Directions.Up);
            KeyDirections.Add(Keys.Down, Directions.Down);
            KeyDirections.Add(Keys.Left, Directions.Left);
            KeyDirections.Add(Keys.Right, Directions.Right);

            KeyDirections.Add(Keys.W, Directions.Up);
            KeyDirections.Add(Keys.S, Directions.Down);
            KeyDirections.Add(Keys.A, Directions.Left);
            KeyDirections.Add(Keys.D, Directions.Right);

            List<Keys> player1Keys = new List<Keys>();
            player1Keys.Add(Keys.W);
            player1Keys.Add(Keys.A);
            player1Keys.Add(Keys.S);
            player1Keys.Add(Keys.D);
            PlayerKeys.Add(player1, player1Keys);

            List<Keys> player2Keys = new List<Keys>();
            player2Keys.Add(Keys.Up);
            player2Keys.Add(Keys.Down);
            player2Keys.Add(Keys.Left);
            player2Keys.Add(Keys.Right);
            PlayerKeys.Add(player2, player2Keys);

            sound_music_intro.Play();
        }

        private void ResetData()
        {
            foreach (PlayingPiece piece in pieces)
            {
                piece.Points = 0;
            }
            currentRound = 0;
        }

        private void BeginLevel()
        {
            if (currentRound == numberOfRounds)
            {
                StateOfGame = GameState.GameOver;
                return;
            }
            LoadNextLevel();
            currentLevel.ResetLevel();

            currentRound++;

            clones.Clear();
            player1.Position = currentLevel.Tiles[1, 1].TileCenter;
            player1.Direction = Vector2.Zero;
            player2.Position = currentLevel.Tiles[currentLevel.TilesWide - 2, currentLevel.TilesHigh - 2].TileCenter;
            player2.Direction = Vector2.Zero;
            clonesLeft = initialClonesLeft;
            millisecondsToNextPointPayout = millisecondsBetweenPayouts;
            betweenRoundsPauseLeft = betweenRoundsPause;
            SwitchRoles();
            StateOfGame = GameState.BetweenRounds;
        }

        private void AddPoints(PlayingPiece piece)
        {
            piece.Points += (int)pointsPerSecond;
        }

        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            Keys[] pressedKeys = keyState.GetPressedKeys();

            if (keyState.IsKeyUp(Keys.Enter))
            {
                EnterKeyHasBeenUp = true;
            }

            // Allows the game to exit
            if (keyState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
            {
                if (StateOfGame != GameState.Credits)
                {
                    StateOfGame = GameState.Credits;
                }
                else
                {
                    this.Exit();
                }
            }

            if ((keyState.IsKeyDown(Keys.LeftAlt) && keyState.IsKeyDown(Keys.Enter)) && !(oldState.IsKeyDown(Keys.LeftAlt) && oldState.IsKeyDown(Keys.Enter)))
            {
                graphics.ToggleFullScreen();
                return;
            }

            // Allows the developers to fastforward to next level
            if (keyState.IsKeyDown(Keys.N) && !oldState.IsKeyDown(Keys.N) && keyState.IsKeyDown(Keys.LeftControl))
            {
                BeginLevel();
            }


            //<<<<<<< .mine======= 
            for (int i = 0; i < pops.Count; i++)
            {
                pops[i].Timeout -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //>>>>>>> .theirs/
                //<<<<<<< .mine
                //=======
                if (pops[i].Timeout < 0)
                    pops.Remove(pops[i]);
            }

            //>>>>>>> .theirs 
            switch (StateOfGame)
            {


                case GameState.Introduction:
                    if (EnterKeyHasBeenUp && keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                    {
                        sound_music_intro.Stop();
                        sound_music_a.Play();
                        BeginLevel();
                    }

                    if (keyState.IsKeyDown(Keys.F1) && !oldState.IsKeyDown(Keys.F1))
                    {
                        StateOfGame = GameState.Help;
                    }
                    break;


                case GameState.Help:
                    if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                    {
                        StateOfGame = GameState.Introduction;
                    }
                    break;

                case GameState.Playing:

                    #region Playing



                    if ((keyState.IsKeyDown(Keys.Pause) && !oldState.IsKeyDown(Keys.Pause)) || (keyState.IsKeyDown(Keys.P) && !oldState.IsKeyDown(Keys.P)))
                    {
                        StateOfGame = GameState.Paused;
                        sound_music_a.Pause();
                    }

                    millisecondsToNextPointPayout -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (millisecondsToNextPointPayout <= 0)
                    {
                        AddPoints(Adversary[hunter]);
                        millisecondsToNextPointPayout = millisecondsBetweenPayouts;
                    }

                    if (!oldState.IsKeyDown(Keys.LeftControl) && keyState.IsKeyDown(Keys.LeftControl) && player1.Role == Role.Hunted)
                    {
                        AddClone(player1);
                    }

                    if (!oldState.IsKeyDown(Keys.RightControl) && keyState.IsKeyDown(Keys.RightControl) && player2.Role == Role.Hunted)
                    {
                        AddClone(player2);
                    }

                    foreach (PlayingPiece piece in pieces)
                    {
                        //If the piece is not where it wants to be
                        if (piece.Direction != Vector2.Zero)
                        {
                            //move it!
                            MovePiece(piece, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                        }
                        else
                        {
                            //set new direction from keys
                            SetWantedDirection(piece, pressedKeys);
                        }
                    }

                    foreach (PlayingPiece clone in clones)
                    {
                        //If the piece is not where it wants to be
                        if (clone.Direction != Vector2.Zero)
                        {
                            //move it!
                            MovePiece(clone, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
                        }
                        else
                        {
                            //if we need to start up the clone
                            if (clone.Direction == Vector2.Zero)
                            {
                                Vector2 potentialTarget = clone.Position + clone.PreviousDirection * tileSize;

                                //if (clone.PreviousDirection != Vector2.Zero && TileIsClear(potentialTarget))
                                //{
                                //    clone.Direction = clone.PreviousDirection;
                                //}
                                //else
                                //{
                                //    //set new direction from keys
                                //    SetRandomValidDirection(clone);
                                //}

                                SetRandomValidDirectionButNotPrevious(clone);
                            }
                        }
                    }
                    DoCollisionDetection();

                    #endregion

                    break;
                case GameState.BetweenRounds:

                    betweenRoundsPauseLeft -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (betweenRoundsPauseLeft <= 0)
                    {
                        StateOfGame = GameState.Playing;
                        betweenRoundsPauseLeft = betweenRoundsPause;
                    }
                    break;
                case GameState.Paused:
                    if (keyState.GetPressedKeys().Length > 0)
                    {
                        StateOfGame = GameState.Playing;
                        sound_music_a.Resume();
                    }
                    break;
                case GameState.GameOver:
                    if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                    {
                        ResetData();
                        BeginLevel();
                    }
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
            oldState = keyState;
        }

        private void DoCollisionDetection()
        {
            PlayingPiece hunter = GetCurrentHunter();

            for (int i = clones.Count - 1; i >= 0; i--)
            {
                PlayingPiece clone = clones[i];
                if (Vector2.Distance(clone.Position, hunter.Position) < tileSize * .75)
                {
                    //Clone Kill
                    if (soundEffect.Count > 0)
                        soundEffect[random.Next(0, soundEffect.Count)].Play(0.5f, 0f, 0f);

                    pops.Add(new SmallPop(250, clone.Position));

                    clones.Remove(clone);
                }
            }

            if (Vector2.Distance(player1.Position, player2.Position) < tileSize * .8)
            {
                sound_monster.Play();

                BeginLevel();
            }

            if (!currentLevel.TeleportersOpen)
            {
                foreach (PlayingPiece piece in pieces)
                {
                    if (Vector2.Distance(piece.Position, currentLevel.KeyTile.TileCenter) < tileSize * 0.8F)
                    {
                        currentLevel.KeyTile.Type = TileType.Open;
                        sound_OpenTeleports.Play();
                        currentLevel.OpenTeleporters();
                    }
                }

            }
        }

        private PlayingPiece GetCurrentHunter()
        {
            if (player1.Role == Role.Hunter)
            {
                return player1;
            }
            else
            {
                return player2;
            }
        }

        private void SetRandomValidDirection(PlayingPiece clone)
        {
            bool newDirectionFound = false;
            Vector2 newPosition = clone.Position + clone.Direction * tileSize;
            if (clone.Direction == Vector2.Zero || (clone.Direction != Vector2.Zero && !TileIsClear(newPosition)))
            {
                do
                {
                    Vector2 direction = Directions.GetRandomDirection();
                    Vector2 alternateNewPosition = clone.Position + direction * tileSize;
                    if (TileIsClear(alternateNewPosition))
                    {
                        clone.Direction = direction;
                        newDirectionFound = true;
                    }
                } while (!newDirectionFound);
            }
        }

        private void SetRandomValidDirectionButNotPrevious(PlayingPiece clone)
        {
            bool newDirectionFound = false;
            int tries = 0;
            Vector2 newPosition = clone.Position + clone.Direction * tileSize;
            if (clone.Direction == Vector2.Zero || (clone.Direction != Vector2.Zero && !TileIsClear(newPosition)))

                do
                {
                    Vector2 direction = Directions.GetRandomDirection();
                    Vector2 alternateNewPosition = clone.Position + direction * tileSize;

                    if (TileIsClear(alternateNewPosition))
                    {
                        if (clone.PreviousDirection == (direction * -1) && tries < 20)
                        {
                            tries++;
                            continue;
                        }
                        clone.Direction = direction;
                        newDirectionFound = true;
                    }
                } while (!newDirectionFound);
        }

        private void AddClone(PlayingPiece player)
        {
            if (clones.Count < maxSimultaneousClones && clonesLeft > 0)
            {
                clones.Add((PlayingPiece)player.Clone());
                clonesLeft--;
            }

        }

        private void SetWantedDirection(PlayingPiece piece, Keys[] pressedKeys)
        {
            Vector2 direction = Vector2.Zero;

            //Console.WriteLine("SetWantedDirection() for " + piece.Role);
            Vector2 potentialTarget = Vector2.Zero;

            //Console.WriteLine("Keyspressed: ");
            foreach (Keys keyPressed in pressedKeys)
            {
                if (PlayerKeys[piece].Contains(keyPressed))
                {
                    direction += KeyDirections[keyPressed];
                    //Console.Write("[contained], " + KeyDirections[keyPressed]);
                    potentialTarget += KeyDirections[keyPressed] * tileSize;
                }
            }

            //if both vertical and horizontal keys are pressed
            if (direction.X * direction.Y != 0)
            {
                direction.Y = 0;
            }
            potentialTarget = piece.Position + direction * tileSize;
            if (!TileIsClear(potentialTarget))
            {
                direction = Vector2.Zero;
            }

            piece.Direction = direction;
        }

        private void MovePiece(PlayingPiece piece, float elapsedMilliseconds)
        {
            //Vector2 directionToMove = new Vector2(Math.Sign(piece.Direction.X - piece.Position.X), Math.Sign(piece.Direction.Y - piece.Position.Y));
            if (piece.Direction != Vector2.Zero)
            {
                Vector2 newPosition = GetNewPosition(piece, elapsedMilliseconds);
                Point index = GetIndexInArray(piece.Position);
                Tile currentTile = currentLevel.Tiles[index.X, index.Y];
                Vector2 centerOfTilePlayerIsIn = currentTile.TileCenter;

                if (piece.Position != centerOfTilePlayerIsIn && centerOfTilePlayerIsIn.IsBetween(piece.Position, newPosition))
                {
                    if (currentTile.Type == TileType.Teleporter)
                    {
                        piece.Position = currentLevel.TeleporterTiles[currentTile].TileCenter;
                    }
                    else
                    {
                        piece.Position = centerOfTilePlayerIsIn;
                        piece.Direction = Vector2.Zero;
                    }
                }
                else
                {
                    piece.Position = newPosition;
                }
            }

        }

        private Vector2 GetNewPosition(PlayingPiece piece, float elapsedMilliseconds)
        {
            return piece.Position + piece.Direction * piece.Speed * gameSpeed * elapsedMilliseconds;
        }

        private bool TileIsClear(Vector2 potentialTarget)
        {
            Point target = GetIndexInArray(potentialTarget);
            return (currentLevel.Tiles[target.X, target.Y].Type != TileType.Solid && currentLevel.Tiles[target.X, target.Y].Type != TileType.Teleporter || currentLevel.Tiles[target.X, target.Y].Type != TileType.Solid && currentLevel.TeleportersOpen);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
            spriteBatch.Begin();


            //HELP
            if (StateOfGame == GameState.Help)
            {
                spriteBatch.Draw(texture_help, Vector2.Zero, Color.White);
                spriteBatch.End();
                return;
            }


            //CREDITS
            if (StateOfGame == GameState.Credits)
            {
                spriteBatch.Draw(texture_credits, Vector2.Zero, Color.White);
                spriteBatch.End();
                return;
            }


            //INTRODUCTION
            if (StateOfGame == GameState.Introduction)
            {
                // spriteBatch.Draw(pixel, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Width), new Color(0, 0, 0, 200));
                spriteBatch.Draw(tex_introduction, new Vector2(150, 150), Color.White);
                spriteBatch.End();

                return;

            }
            //POINTS IN THE RIGHT COLOR
            spriteBatch.DrawString(standardFont, "Points ", new Vector2(20, 20), player1.Role == Role.Hunted ? Color.White : Color.Black);
            spriteBatch.DrawString(standardFont, player1.Points.ToString(), new Vector2(20, 50), player1.Role == Role.Hunted ? Color.White : Color.Black);
            spriteBatch.DrawString(standardFont, "Points ", new Vector2(670, 20), player2.Role == Role.Hunted ? Color.White : Color.Black);
            spriteBatch.DrawString(standardFont, player2.Points.ToString(), new Vector2(670, 50), player2.Role == Role.Hunted ? Color.White : Color.Black);


            //DRAW ALL TILES

            for (int x = 0; x <= currentLevel.Tiles.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= currentLevel.Tiles.GetUpperBound(1); y++)
                {
                    switch (currentLevel.Tiles[x, y].Type)
                    {
                        case TileType.Open:
                        case TileType.StartHunter:
                        case TileType.StartPrey:
                            spriteBatch.Draw(tile, new Rectangle((int)(tileSize * x + levelOffset.X), (int)(tileSize * y + levelOffset.Y), tileSize, tileSize), Color.Silver);
                            break;

                        case TileType.Solid:
                            spriteBatch.Draw(tile, new Rectangle((int)(tileSize * x + levelOffset.X), (int)(tileSize * y + levelOffset.Y), tileSize, tileSize), Color.Black);
                            break;

                        case TileType.Teleporter:
                            spriteBatch.Draw(tile, new Rectangle((int)(tileSize * x + levelOffset.X), (int)(tileSize * y + levelOffset.Y), tileSize, tileSize), Color.Red);
                            break;
                        case TileType.Key:
                            spriteBatch.Draw(tile, new Rectangle((int)(tileSize * x + levelOffset.X), (int)(tileSize * y + levelOffset.Y), tileSize, tileSize), Color.Silver);
                            int shadowOffset = 2;

                            spriteBatch.Draw(key, new Rectangle((int)(currentLevel.KeyTile.TileCenter.X - key.Width / 6) + shadowOffset, (int)(currentLevel.KeyTile.TileCenter.Y - key.Height / 6) + shadowOffset, key.Width / 3, key.Height / 3), shadowColor);
                            spriteBatch.Draw(key, new Rectangle((int)(currentLevel.KeyTile.TileCenter.X - key.Width / 6), (int)(currentLevel.KeyTile.TileCenter.Y - key.Height / 6), key.Width / 3, key.Height / 3), Color.Red);

                            break;

                        default:
                            break;
                    }
                }
            }

            //DRAW PLAYINGPIECES
            foreach (PlayingPiece piece in pieces)
            {
                spriteBatch.Draw(piece.Texture, new Rectangle((int)(piece.Position.X - piece.TextureOffset.X), (int)(piece.Position.Y - piece.TextureOffset.Y), (int)(piece.Texture.Width * piece.Scale), (int)(piece.Texture.Height * piece.Scale)), Color.White);
            }


            //DRAW CLONES
            foreach (PlayingPiece clone in clones)
            {
                spriteBatch.Draw(clone.Texture, new Rectangle((int)(clone.Position.X - clone.TextureOffset.X), (int)(clone.Position.Y - clone.TextureOffset.Y), (int)(clone.Texture.Width * clone.Scale), (int)(clone.Texture.Height * clone.Scale)), Color.White);
            }



            if (StateOfGame != GameState.GameOver)
            {
                string roundsText = "Round " + currentRound + " of " + numberOfRounds;
                Vector2 sizeOfRoundsText = bigFont.MeasureString(roundsText);
                Vector2 topCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, 20);

                spriteBatch.DrawString(bigFont, roundsText, topCenter - sizeOfRoundsText / 2, Color.White);
            }
            else
            {
                if (player1.Points > player2.Points)
                {
                    spriteBatch.Draw(tex_winPlayer1, new Vector2(150, 150), Color.White);
                }
                else if (player1.Points < player2.Points)
                {
                    spriteBatch.Draw(tex_winPlayer2, new Vector2(150, 150), Color.White);
                }
                else
                {
                    spriteBatch.Draw(texture_draw, new Vector2(150, 150), Color.White);
                }
            }


            for (int i = 0; i < pops.Count; i++)
            {
                spriteBatch.Draw(pop, new Rectangle((int)pops[i].Position.X - (tileSize / 2), (int)pops[i].Position.Y - (tileSize / 2), tileSize, tileSize), Color.White);
            }

            if (StateOfGame == GameState.BetweenRounds)
            {
                spriteBatch.Draw(texture_ready, new Vector2(250, 240), Color.White);
            }

            if (StateOfGame == GameState.Paused)
            {
                //show paused
            }

            //spriteBatch.DrawString(debugFont, StateOfGame.ToString(), Vector2.One * 50, Color.Green);

            spriteBatch.End();
        }

        public Point GetIndexInArray(Vector2 position)
        {
            return new Point((int)((position.X - levelOffset.X) / tileSize), (int)((position.Y - levelOffset.Y) / tileSize));
        }

        private void SwitchRoles()
        {
            foreach (PlayingPiece piece in pieces)
            {
                piece.ToggleRole();
                if (piece.Role == Role.Hunted)
                {
                    piece.Speed = huntedSpeed;
                }
                else
                {
                    piece.Speed = hunterSpeed;
                }
                piece.Texture = piece.Role == Role.Hunted ? white : black;
            }

            hunter = Adversary[hunter];

        }

        //<<<<<<< .mine

        private List<Point> GetTeleporterIndexes(Level level)
        {

            List<Point> teleporters = new List<Point>();
            for (int x = 0; x < level.TilesWide; x++)
            {
                for (int y = 0; y < level.TilesHigh; y++)
                {

                    if (level.Tiles[x, y].Type == TileType.Teleporter)
                    {
                        teleporters.Add(new Point(x, y));
                    }


                }
            }
            return teleporters;
        }


        private void ShowTeleporters(Level level, List<Point> teleporterIndexes)
        {
            foreach (Point teleporterIndex in teleporterIndexes)
            {
                level.Tiles[teleporterIndex.X, teleporterIndex.Y].Type = TileType.Teleporter;
            }
        }


        private void LoadNextLevel()
        {
            currentLevel = LevelLoader.GetLevel(currentRound / 2, tileSize, levelOffset);
        }
    }

    public static class ext
    {
        public static bool IsBetween(this Vector2 pointToCheck, Vector2 a, Vector2 b)
        {
            return pointToCheck.X >= Math.Min(a.X, b.X)
                && pointToCheck.X <= Math.Max(a.X, b.X)
                && pointToCheck.Y >= Math.Min(a.Y, b.Y)
                && pointToCheck.Y <= Math.Max(a.Y, b.Y);
        }
    }
}