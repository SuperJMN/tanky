using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using VelcroPhysics.Dynamics;

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
            child.Attach(this);
        }

        private void Attach(Node parent)
        {
            Parent = parent;
            World = parent.World;
            OnAttach(parent);
        }

        protected virtual void OnAttach(Node node)
        {
        }

        public World World { get; set; }

        public Node Parent { get; set; }
    }
}