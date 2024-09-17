using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    public class StationaryObject
    {
        public float PI = 3.141592f;//pie for radians/rotation
        public Vector2 Location;
        public Vector2 Movement;
        public double rotation;
        public double changeRotation;
        protected Texture2D Texture;
        public Rectangle Edge;
        public virtual void update()
        {

        }
        public virtual void LoadContent(ContentManager Content)
        {

        }
        protected virtual void die()
        {

        }
        public virtual void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
