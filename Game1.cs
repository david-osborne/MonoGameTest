using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace MonoGameTest
{
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont fontScore;
        private SpriteFont fontConsolas;
        private int score = 0;
        private int moveX = 0;
        private bool moveXflip = false;
        private int moveY = 0;
        private bool moveYflip = false;
        private int screenWidth;
        private int screenHeight;

        //fps
        SimpleFps fps = new SimpleFps();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // setting the ball's starting position to the center of the screen based on the dimensions of the screen determined by the current BackBufferWidth and BackBufferHeight that was obtained from the Graphics Device (the current resolution the game is running at)
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 200f;

            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            //fps related
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            this.IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");

            fontScore = Content.Load<SpriteFont>("Score"); //name of the SpriteFont
            fontConsolas = Content.Load<SpriteFont>("Terminal");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
            {
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                ballPosition.X = ballTexture.Width / 2;
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                ballPosition.Y = ballTexture.Height / 2;
            }

            if (moveX < screenWidth && !moveXflip)
            {
                moveXflip = false;
                moveX++;
            }
            else if (moveX > 0 && moveYflip)
            {
                moveXflip = true;
                moveX--;
            }

            //fps
            fps.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.DrawString(fontScore, "Bottom", new Vector2(200, 200), Color.Green);
            _spriteBatch.Draw(ballTexture,
                              ballPosition,
                              null,
                              Color.White,
                              0f,
                              new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                              Vector2.One,
                              SpriteEffects.None,
                              0f);
            _spriteBatch.DrawString(fontScore, "Top", new Vector2(100, 100), Color.Red);

            _spriteBatch.DrawString(fontConsolas, DateTime.Now.ToString(), new Vector2(moveX, moveY), Color.White);

            //fps
            fps.DrawFps(_spriteBatch, fontConsolas, new Vector2(10f, 10f), Color.MonoGameOrange);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}