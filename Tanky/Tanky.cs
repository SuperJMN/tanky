using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Tanky
{
    class Tanky
    {
        private readonly Texture2D sprite;
        private readonly Texture2D cannonSprite;
        private readonly World world;
        private readonly Body tankyBody;
        private readonly Vector2 origin;
        private readonly float horzSpeed = 0.02f;
        private readonly float jumpImpulse = 0.5f;
        private bool isTouchingGround;
        private float cannonRotation;
        private float cannonRotationSpeed = 0.01f;

        public Tanky(Texture2D sprite, Texture2D cannonSprite, World world)
        {
            this.sprite = sprite;
            this.cannonSprite = cannonSprite;
            this.world = world;
            origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
            var width = ConvertUnits.ToSimUnits(sprite.Width);
            var height = ConvertUnits.ToSimUnits(sprite.Width);
            tankyBody = BodyFactory.CreateRectangle(world, width, height, 1, new Vector2(4, 0), bodyType: BodyType.Dynamic);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            var tankySpritePos = ConvertUnits.ToDisplayUnits(tankyBody.Position);
            var cannonPos = new Vector2(tankySpritePos.X + 7, tankySpritePos.Y + 2);
            var cannonCenter = new Vector2(0, cannonSprite.Height);
            spriteBatch.Draw(cannonSprite, cannonPos, null, Color.White, cannonRotation, cannonCenter, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(sprite, tankySpritePos, null, Color.White, 0, origin, 1f, SpriteEffects.None, 0f);
        }

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

        public void RiseCannon()
        {
            var newValue = cannonRotation - cannonRotationSpeed;

            if (newValue >= -0.6)
            {
                cannonRotation = newValue;
            }
        }

        public void LowerCannon()
        {
            var newValue = cannonRotation + cannonRotationSpeed;

            if (newValue < 0.0)
            {
                cannonRotation = newValue;
            }
        }
    }
}