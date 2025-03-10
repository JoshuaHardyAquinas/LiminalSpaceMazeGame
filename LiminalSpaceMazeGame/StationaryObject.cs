﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    public class StationaryObject
    {
        public float PI = 3.141592f;//pie for radians/rotation

        public double rotation;
        public double changeRotation;
        protected Texture2D Texture;
        protected int objWidth;
        protected int objHeight;
        public Rectangle Edge;
        private Vector2 Location;
        public virtual void update()
        {
        }
        public virtual void LoadContent(ContentManager Content)
        {
            Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, objWidth, objHeight);
        }
        protected virtual void checkDeath()
        {

        }
        public virtual void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, objWidth, objHeight), Color.Yellow);
        }
        public Vector2 getLocation()
        {
            return Location;
        }
        public void setLocation(Vector2 location)
        {
            Location = location;
        }
        public virtual void spawn(Vector2 loc)
        {
            setLocation(loc);
        }
    }
}
