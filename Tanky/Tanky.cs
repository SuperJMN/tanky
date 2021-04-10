using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    class Tanky : Node
    {
        private readonly Texture2D sprite;
        private readonly Texture2D cannonSprite;
        private readonly Body tankyBody;
        private readonly Vector2 rotationCenter;
        private readonly float walkImpulse = 1.2f;
        private readonly float jumpImpulse = 0.5f;
        private bool isTouchingGround;
        private float cannonRotation;
        private float cannonRotationSpeed = 0.4f;

        public Tanky(ContentManager contentManager, World world, Vector2 initialPosition)
        {
            sprite = contentManager.Load<Texture2D>("TankyBody");
            cannonSprite = contentManager.Load<Texture2D>("TankyCannon");

            rotationCenter = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            var width = ConvertUnits.ToSimUnits(sprite.Width);
            var height = ConvertUnits.ToSimUnits(sprite.Width);
            tankyBody = BodyFactory.CreateRectangle(world, width, height, 1, initialPosition, bodyType: BodyType.Dynamic);

            tankyBody.OnCollision += (a, b, contact) =>
            {
                isTouchingGround = true;
            };

            tankyBody.OnSeparation += (a, b, contact) =>
            {
                isTouchingGround = false;
            };
        }

        protected override void DrawMe(SpriteBatch spriteBatch)
        {
            var tankySpritePos = ConvertUnits.ToDisplayUnits(tankyBody.Position);
            var cannonPos = new Vector2(tankySpritePos.X + 7, tankySpritePos.Y + 2);
            var cannonCenter = new Vector2(0, cannonSprite.Height);
            spriteBatch.Draw(cannonSprite, cannonPos, null, Color.White, cannonRotation, cannonCenter, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(sprite, tankySpritePos, null, Color.White, 0, rotationCenter, 1f, SpriteEffects.None, 0f);
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

            if (!isTouchingGround)
            {
                impulse /= 3;
            }

            tankyBody.ApplyLinearImpulse(new Vector2(impulse * multiplier, 0));
        }

        public void GoBack(float dt)
        {
            Walk(-1f, dt);
        }

        public void Jump()
        {
            if (isTouchingGround)
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
    }
}