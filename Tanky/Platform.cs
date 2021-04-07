using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Tanky
{
    public class Platform
    {
        private readonly Texture2D sprite;
        private Vector2 origin;
        private Body groundBody;

        public Platform(Texture2D sprite, World world, Vector2 position, float width, float height)
        {
            this.sprite = sprite;
            groundBody = BodyFactory.CreateRectangle(world, width, height, 1f, position);
            groundBody.BodyType = BodyType.Static;
            groundBody.Restitution = 0.3f;
            groundBody.Friction = 0.5f;
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }

        public Vector2 Origin => origin;
        public Vector2 Position => groundBody.Position;
        public Texture2D Sprite => sprite;
    }
}