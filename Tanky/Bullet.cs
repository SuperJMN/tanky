using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    public class Bullet : Node
    {
        private readonly Vector2 initialPosition;
        private readonly Vector2 velocity;
        private readonly Texture2D sprite;
        private Body body;
        private float renderSize = 0.2f;
        private float radius;

        public Bullet(ContentManager contentManager, Vector2 initialPosition, Vector2 velocity)
        {
            sprite = contentManager.Load<Texture2D>("Shot");
            this.initialPosition = initialPosition;
            this.velocity = velocity;
        }

        protected override void OnAttach(Node node)
        {
            radius = renderSize / 2;
            body = BodyFactory.CreateCircle(World, radius, 1, initialPosition, BodyType.Dynamic);
            body.ApplyLinearImpulse(velocity);
        }

        protected override void DrawMe(SpriteBatch spriteBatch)
        {
            int sideInPixels = (int)ConvertUnits.ToDisplayUnits(renderSize);
            var spriteWidth = (float)sideInPixels / sprite.Width;
            var scale = new Vector2(spriteWidth, spriteWidth);
            spriteBatch.Draw(sprite, ConvertUnits.ToDisplayUnits(new Vector2(body.Position.X - radius, body.Position.Y - radius)), null, Color.White, 0f, new Vector2(0,0), scale, SpriteEffects.None, 0f);
        }
    }
}