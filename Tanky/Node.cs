using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Tanky
{
    public class Node
    {
        readonly IList<Node> children = new List<Node>();

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMe(spriteBatch);
            DrawChildren(spriteBatch);
        }

        protected virtual void DrawMe(SpriteBatch spriteBatch)
        {
        }

        private void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach (var child in children)
            {
                child.Draw(spriteBatch);
            }
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }
    }
}