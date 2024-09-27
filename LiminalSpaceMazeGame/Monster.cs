﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LiminalSpaceMazeGame
{
    internal class Monster : MovingObject
    {
        Monster(Vector2 startingLoc, Texture2D texture)
        {
            Location = startingLoc;
            Texture = texture;
            rotation = 0f;
        }
        public override void update()
        {
            //creates player edge
            Edge = new Rectangle((int)Location.X - Texture.Width / 2, (int)Location.Y - Texture.Height / 2, Texture.Width, Texture.Height);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture
            Texture = Content.Load<Texture2D>(@"ray");
        }
        public void spawn()
        {
            Location = new Vector2(60, 60);
        }
        protected override void die()
        {
            if (Health <= 0)//monster dies with 0 health
            {
                //die
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, (float)rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }

    }
}
