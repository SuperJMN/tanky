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
        private Platform platform;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            
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

            var width = ConvertUnits.ToSimUnits(512f);
            var height = ConvertUnits.ToSimUnits(64f);
            
            tanky = new Tanky(Content.Load<Texture2D>("TankyBody"), Content.Load<Texture2D>("TankyCannon"), world);

            platform = new Platform(Content.Load<Texture2D>("GroundSprite"), world, groundPosition, width, height);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleKeypad();
            HandleKeyboard();
           
            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001F);

            base.Update(gameTime);
        }

        private void HandleKeyboard()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                tanky.GoBack();
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                tanky.RiseCannon();
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                tanky.LowerCannon();
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                tanky.GoForward();
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                tanky.Jump();
            }
        }

        private void HandleKeypad()
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
                    tanky.RiseCannon();
                }

                if (padState.IsButtonDown(Buttons.Y))
                {
                    tanky.LowerCannon();
                }

                if (padState.IsButtonDown(Buttons.DPadRight))
                {
                    tanky.GoForward();
                }

                if (padState.IsButtonDown(Buttons.DPadLeft))
                {
                    tanky.GoBack();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            tanky.Draw(spriteBatch);
            spriteBatch.Draw(platform.Sprite, ConvertUnits.ToDisplayUnits(platform.Position), null, Color.White, 0f, platform.Origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
