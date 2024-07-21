using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    internal class StationaryObject
    {
        public Vector2 Location;
        protected Texture2D Texture;

        public virtual void LoadContent(ContentManager Content)
        {

        }
        protected virtual void spawn()
        {

        }
        protected virtual void die()
        {

        }
        protected virtual void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
