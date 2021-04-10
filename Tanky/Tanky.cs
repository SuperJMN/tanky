using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Collision.ContactSystem;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    public class Tanky : Node
    {
        private readonly ContentManager contentManager;
        private readonly Texture2D sprite;
        private readonly Texture2D cannonSprite;
        private Body tankyBody;
        private readonly float walkImpulse = 1.2f;
        private readonly float jumpImpulse = 0.2f;
        private float cannonRotation;
        private float cannonRotationSpeed = 0.4f;
        private float width;
        private float height;
        private Vector2 rotationCenter;

        public Tanky(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            sprite = contentManager.Load<Texture2D>("TankyBody");
            cannonSprite = contentManager.Load<Texture2D>("TankyCannon");
        }

        protected override void OnAttach(Node node)
        {
            var initialPosition = new Vector2(4, 0);
            rotationCenter = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            width = ConvertUnits.ToSimUnits(sprite.Width);
            height = ConvertUnits.ToSimUnits(sprite.Width);

            tankyBody = BodyFactory.CreateRectangle(World, width, height, 1, initialPosition, bodyType: BodyType.Dynamic);
            tankyBody.LocalCenter = new Vector2(0, height / 2);
        }

        protected override void DrawMe(SpriteBatch spriteBatch)
        {
            var tankySpritePos = ConvertUnits.ToDisplayUnits(tankyBody.Position);
            var cannonPos = new Vector2(tankySpritePos.X + 7, tankySpritePos.Y + 2);
            var cannonCenter = new Vector2(0, cannonSprite.Height);
            spriteBatch.Draw(cannonSprite, cannonPos, null, Color.White, cannonRotation, cannonCenter, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(sprite, tankySpritePos, null, Color.White, tankyBody.Rotation, rotationCenter, 1f, SpriteEffects.None, 0f);
        }

        public void GoForward(float dt)
        {
            Walk(1f, dt);
        }

        private void Walk(float multiplier, float dt)
        {
            if (tankyBody.LinearVelocity.Length() > 5.5)
            {
                return;
            }

            var impulse = walkImpulse * dt;

            //if (!isTouchingGround)
            //{
            //    impulse /= 3;
            //}

            tankyBody.ApplyLinearImpulse(new Vector2(impulse * multiplier, 0));
        }

        public void GoBack(float dt)
        {
            Walk(-1f, dt);
        }

        public void Jump()
        {
            if (tankyBody.ContactList != null)
            {
                tankyBody.ApplyLinearImpulse(new Vector2(0, -jumpImpulse));
            }
        }

        public void RiseCannon(float dt)
        {
            var newValue = cannonRotation - cannonRotationSpeed * dt;

            if (newValue >= -0.6)
            {
                cannonRotation = newValue;
            }
        }

        public void LowerCannon(float dt)
        {
            var newValue = cannonRotation + cannonRotationSpeed * dt;

            if (newValue < 0.0)
            {
                cannonRotation = newValue;
            }
        }

        public void Shot()
        {
            Parent.AddChild(new Bullet(contentManager, this.tankyBody.Position, new Vector2(0.2f, 0)));
        }
    }
}