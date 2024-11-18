using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.Design.Serialization;
using static LiminalSpaceMazeGame.Game1;

namespace LiminalSpaceMazeGame
{
    internal class Ray : MovingObject
    {
        public Ray()
        {
            
        }
        public override void update()
        {
            Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, 1, 1);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture
            Texture = Content.Load<Texture2D>(@"ray");
        }
        protected override void checkDeath()
        {
            if (Health <= 0)//player dies if health reaches 0
            {
                //die
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            //spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        static public Vector2 cast(int angle, Hero TheHero, Ray TheRay,List<ObjInGame> ingameObjects,ref Vector2 centreDis,int maxCastLength, ref char objHit, char toHit)
        {
            float speed = 3f;
            TheRay.setLocation(TheHero.getLocation());
            TheRay.rotation = TheHero.rotation;
            TheRay.rotation = TheRay.rotation + (angle / 180f) * 3.14159265f / 2;
            TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));
            Vector2 startloc = TheRay.getLocation();
            while (true)
            {
                TheRay.setLocation(TheRay.getLocation() + TheRay.Movement);//move ray forward
                TheRay.update();//update hitbox
                foreach (ObjInGame Obj in ingameObjects)//loop though all walls (ik its slow but its easy)
                {
                    if (Obj.objectEdge.Intersects(TheRay.Edge) && Obj.name == toHit)//check collision with hitbox
                    {
                        TheRay.setLocation(TheRay.getLocation() - TheRay.Movement * 1.1f);//move ray backwards a lil further than the last hit
                        for (int i = 0; i<11;i++)//increase ray accuracy for a known hit by moving slower to the actual location
                        {
                            TheRay.setLocation(TheRay.getLocation() + TheRay.Movement*0.1f);//move 
                            TheRay.update();
                            if (Obj.objectEdge.Intersects(TheRay.Edge))//check collision with hitbox
                            {
                                centreDis = new Vector2(Obj.objectEdge.Center.X, Obj.objectEdge.Center.Y) - TheRay.getLocation();
                                objHit = Obj.name;
                                return startloc - TheRay.getLocation();
                            }
                        }
                    }
                    
                }
                if(Math.Abs(TheRay.getLocation().X-TheHero.getLocation().X) > maxCastLength || Math.Abs(TheRay.getLocation().Y - TheHero.getLocation().Y) > maxCastLength)
                {
                    return new Vector2(maxCastLength, maxCastLength);
                }
            }
        }
    }
}