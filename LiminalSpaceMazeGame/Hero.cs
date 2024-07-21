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
    internal class Hero : MovingObject
    {
        private Vector2 Movement;
        public Hero()
        {
            // constructor
            Location = new Vector2(500, 500);
            Movement = new Vector2(0, 0);
        }
        protected override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, 10, 10), Color.White);
        }
        public override void LoadContent(ContentManager Content)
        {
            //Texture = Content.Load<Texture2D>("");
        }
        protected override void spawn()
        {
            base.spawn();
        }
        protected override void die()
        {

        }
    }
}
