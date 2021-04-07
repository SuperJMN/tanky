using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;

namespace Tanky
{
    class Tanky
    {
        private readonly World world;
        private Body tankyBody;
        private Vector2 origin;
        private float horzSpeed = 0.02f;
        private float jumpImpulse = 0.5f;
        private bool isTouchingGround;

        public Tanky(Texture2D sprite, World world)
        {
            this.world = world;
            Sprite = sprite;
            tankyBody = BodyFactory.CreateRectangle(world, 0.5f, 0.5f, 1, new Vector2(4, 0), bodyType: BodyType.Dynamic);
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            tankyBody.OnCollision += (a, b, contact) =>
            {
                if (Math.Abs(Math.Abs(contact.Manifold.LocalNormal.Y) - 1) < 0.02)
                {
                    isTouchingGround = true;
                }
            };

            tankyBody.OnSeparation += (a, b, contact) =>
            {
                isTouchingGround = false;
            };
        }


        public Texture2D Sprite { get; }
        public Vector2 Position => tankyBody.Position;
        public Vector2 Origin => origin;

        public void GoForward()
        {
            tankyBody.ApplyLinearImpulse(new Vector2(horzSpeed, 0));
        }

        public void GoBack()
        {
            tankyBody.ApplyLinearImpulse(new Vector2(-horzSpeed, 0));
        }

        public void Jump()
        {
            if (isTouchingGround)
            {
                tankyBody.ApplyLinearImpulse(new Vector2(0, -jumpImpulse));
            }
        }
    }
}