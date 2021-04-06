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
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Tanky tanky;
        private World world = new World(new Vector2(0, 9.82f));
        private Body tankyBody;
        private Vector2 _cameraPosition;
        private Body groundBody;
        private Texture2D groundSprite;
        private Vector2 groundOrigin;
        private Vector2 tankyOrigin;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            tankyBody = BodyFactory.CreateRectangle(world, 0.5f, 0.5f, 1, new Vector2(4, 0), bodyType: BodyType.Dynamic);
            var screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            Vector2 groundPosition = ConvertUnits.ToSimUnits(screenCenter) + new Vector2(0, 1.25f);

            groundBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(512f), ConvertUnits.ToSimUnits(64f), 1f, groundPosition);
            groundBody.BodyType = BodyType.Static;
            groundBody.Restitution = 0.3f;
            groundBody.Friction = 0.5f;

            tanky = new Tanky(Content.Load<Texture2D>("Tanky"));
            groundSprite = Content.Load<Texture2D>("GroundSprite"); // 512px x 64px =>   8m x 1m
            groundOrigin = new Vector2(groundSprite.Width / 2f, groundSprite.Height / 2f);
            tankyOrigin = new Vector2(tanky.Texture.Width / 2f, tanky.Texture.Height / 2f);
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

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                tankyBody.ApplyLinearImpulse(new Vector2(0, -0.2f));
            }

            var horzSpeed = 0.03f;
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                tankyBody.ApplyLinearImpulse(new Vector2(-horzSpeed, 0));
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                tankyBody.ApplyLinearImpulse(new Vector2(horzSpeed, 0));
            }
        }

        private void HandleKeypad()
        {
            GamePadState padState = GamePad.GetState(0);

            if (padState.IsConnected)
            {
                if (padState.IsButtonDown(Buttons.A))
                {
                    tankyBody.ApplyLinearImpulse(new Vector2(0, -0.2f));
                }

                var horzSpeed = 0.03f;
                if (padState.IsButtonDown(Buttons.DPadRight))
                {
                    tankyBody.ApplyLinearImpulse(new Vector2(horzSpeed, 0));
                }

                if (padState.IsButtonDown(Buttons.DPadLeft))
                {
                    tankyBody.ApplyLinearImpulse(new Vector2(-horzSpeed, 0));
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(tanky.Texture, ConvertUnits.ToDisplayUnits(tankyBody.Position), null, Color.White, 0, tankyOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(groundSprite, ConvertUnits.ToDisplayUnits(groundBody.Position), null, Color.White, 0f, groundOrigin, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
