using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    internal class Ray : MovingObject
    {
        public Ray()
        {
        }
        public override void update()
        {
            Edge = new Rectangle((int)Location.X, (int)Location.Y, Texture.Width/2+1, Texture.Height/2+1);
            //Edge = new Rectangle((int)Location.X, (int)Location.Y, Texture.Width-1, Texture.Height-1);
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
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        static public Vector2 cast(int chAnge, Hero TheHero, Ray TheRay,List<Wall> walls,ref Vector2 centreDis)
        {
            float speed = 2f;
            TheRay.Location = TheHero.Location;
            TheRay.rotation = TheHero.rotation;
            TheRay.rotation = TheRay.rotation + (chAnge / 180f) * 3.14159265f / 2;//come fix later josh u lazy aaaaaaa....
            TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));
            Vector2 startloc = TheRay.Location;
            while (true)
            {
                TheRay.Location = TheRay.Location + TheRay.Movement;//move ray forward
                TheRay.update();//update hitbox
                foreach (Wall wall in walls)//loop though all walls (ik its slow but its easy)
                {
                    if (wall.Edge.Intersects(TheRay.Edge))//check collision with hitbox
                    {
                        
                        TheRay.Location = TheRay.Location - (TheRay.Movement + TheRay.Movement / 10);//move ray backwards a lil further than the last hit
                        while (true)//increase ray accuracy for a known hit by moving slower to the actual location
                        {
                            TheRay.Location = TheRay.Location + TheRay.Movement * 0.1f;//move 
                            TheRay.update();
                            if (wall.Edge.Intersects(TheRay.Edge))//check collision with hitbox
                            {
                                centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheRay.Location;
                                return startloc - TheRay.Location;
                            }
                        }
                    }
                }
            }
        }
    }
}