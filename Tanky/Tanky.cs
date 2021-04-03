using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tanky
{
    class Tanky
    {
        public Tanky(Texture2D texture)
        {
            Texture = texture;
        }

        public Texture2D Texture { get; }
        public Vector2 Position { get; set; }
    }
}