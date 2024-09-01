using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using System.Threading;

namespace LiminalSpaceMazeGame
{
    internal class Ray : MovingObject
    {
        public Ray(MovingObject TheHero)
        {
            Location = new Vector2(10, 10);//ray spawn location
            Movement = new Vector2(0, 0);//no movment for player to begin with
            rotation = 0;//starting rotation
            int speed = 2;
            Location = TheHero.Location;
            rotation = TheHero.rotation;
            Movement = new Vector2(-speed * (float)Math.Sin(rotation), speed * (float)Math.Cos(rotation));
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
            if (Health <= 0)//player dies if health reaches 0
            {
                //die
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
    }
}