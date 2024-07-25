﻿using Microsoft.Xna.Framework.Content;
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
        public Vector2 Location;
        public float rotation;
        protected Texture2D Texture;
        public Rectangle Edge;
        public virtual void update()
        {

        }
        public virtual void LoadContent(ContentManager Content)
        {

        }
        protected virtual void spawn()
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
