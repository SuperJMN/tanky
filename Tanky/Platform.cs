using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    public class Platform : Node
    {
        private readonly Texture2D sprite;
        private readonly Vector2 origin;
        private readonly Body groundBody;

        public Platform(Texture2D sprite, World world, Vector2 position, float width, float height)
        {
            this.sprite = sprite;
            groundBody = BodyFactory.CreateRectangle(world, width, height, 1f, position);
            groundBody.BodyType = BodyType.Static;
            groundBody.Restitution = 0.3f;
            groundBody.Friction = 0.5f;
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

        protected override void DrawMe(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, ConvertUnits.ToDisplayUnits(groundBody.Position), null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}