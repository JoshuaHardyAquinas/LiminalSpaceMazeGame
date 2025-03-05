using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static LiminalSpaceMazeGame.Game1;

namespace LiminalSpaceMazeGame
{
    internal class Ray : MovingObject
    {
        public Ray()//no constructor needed
        {

        }
        public override void update()//give ray an edge of 1*1
        {
            Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, 1, 1);
        }
        static public Vector2 Cast(int angle, Hero TheHero, Ray TheRay, List<ObjInGame> ingameObjects, ref Vector2 centreDis, int maxCastLength, ref char objHit, char toHit, char wallType)
        {
            //setup ray
            float speed = 1f;
            TheRay.setLocation(TheHero.getLocation());//Set the rays location to the heros location and rotation
            TheRay.rotation = TheHero.rotation;
            TheRay.rotation = TheRay.rotation + (angle / 180f) * 3.14159265f / 2;//set the angle of the ray to the direction taht needs casting to
            TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));//set speed to a constant and then multiply later
            Vector2 startloc = TheRay.getLocation();
            while (true)
            {
                TheRay.setLocation(TheRay.getLocation() + TheRay.Movement);//move ray forward
                TheRay.update();//update hitbox
                foreach (ObjInGame Obj in ingameObjects)//loop though all walls (ik its slow but its easy)
                {
                    if (Obj.objectEdge.Intersects(TheRay.Edge) && (Obj.name == toHit || Obj.name == wallType))//check collision with hitbox
                    {
                        TheRay.setLocation(TheRay.getLocation() - TheRay.Movement * 1.1f);//move ray backwards a lil further than the last hit
                        for (int i = 0; i < 11; i++)//increase ray accuracy for a known hit by moving slower to the actual location
                        {
                            TheRay.setLocation(TheRay.getLocation() + TheRay.Movement * 0.1f);//move 
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
                speed += 0.2f;
                TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));
                if (Math.Abs(TheRay.getLocation().X - TheHero.getLocation().X) > maxCastLength || Math.Abs(TheRay.getLocation().Y - TheHero.getLocation().Y) > maxCastLength)
                {
                    return new Vector2(maxCastLength, maxCastLength);//if the ray somehow exits the maze, 
                }
            }
        }
    }
}