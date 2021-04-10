using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Tanky tanky;
        private readonly World world = new World(new Vector2(0, 9.82f));
        private Vector2 _cameraPosition;
        private readonly Node stage = new Node();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            var screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            Vector2 groundPosition = ConvertUnits.ToSimUnits(screenCenter) + new Vector2(0, 1.25f);

          
            tanky = new Tanky(Content, world,new Vector2(4, 0));
            stage.AddChild(tanky);
            
            var platform = new Platform(Content.Load<Texture2D>("GroundSprite"), world, groundPosition);
            stage.AddChild(platform);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var dt = gameTime.GetDt();
            HandleKeypad(dt);
            HandleKeyboard(dt);
           
            world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        private void HandleKeyboard(float dt)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                tanky.GoBack(dt);
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                tanky.RiseCannon(dt);
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                tanky.LowerCannon(dt);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                tanky.GoForward(dt);
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                tanky.Jump();
            }
        }

        private void HandleKeypad(float dt)
        {
            GamePadState padState = GamePad.GetState(0);

            if (padState.IsConnected)
            {
                if (padState.IsButtonDown(Buttons.A))
                {
                    tanky.Jump();
                }

                if (padState.IsButtonDown(Buttons.X))
                {
                    tanky.RiseCannon(dt);
                }

                if (padState.IsButtonDown(Buttons.Y))
                {
                    tanky.LowerCannon(dt);
                }

                if (padState.IsButtonDown(Buttons.DPadRight))
                {
                    tanky.GoForward(dt);
                }

                if (padState.IsButtonDown(Buttons.DPadLeft))
                {
                    tanky.GoBack(dt);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            stage.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class TimeSpanMixin
    {
        public static float GetDt(this GameTime gt)
        {
            return (float) gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
