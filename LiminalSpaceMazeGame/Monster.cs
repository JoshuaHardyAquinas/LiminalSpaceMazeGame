using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using System.Security.Permissions;
using SharpDX.Direct2D1.Effects;

namespace LiminalSpaceMazeGame
{
     class Monster : MovingObject
    {
        private int textnum = 0;
        private int damage;
        private int memoryStrength;
        public bool lineOfSight = false;
        public int memory = 0;
        private bool newNode = false;
        private int[] nextCoords = {0,0 };
        int[] currentCoords = { 0,0 };
        int disToGo = 0;

        public int Damage { get => damage; set => damage = value; }

        public Monster(Vector2 startingLoc, int text, int MaxDamage)
        {
            
            Random random = new Random();
            damage = random.Next(4,MaxDamage+1);
            memoryStrength = 5 * random.Next(1,MaxDamage);
            spawn(startingLoc);
            textnum = 0;
            rotation = 0f;
            currentCoords[0] = (int)startingLoc.X/40;
            currentCoords[1] = (int)startingLoc.Y/40;
            nextCoords[0] = currentCoords[0];
            nextCoords[1] = currentCoords[1];
        }
        public void update(Hero theHero, int [,] theMaze)
        {
            
            base.update();
            //creates player edge
            Vector2 centreDis = theHero.getLocation() - getLocation();
            double tangent = (double)Math.Sqrt(centreDis.X* centreDis.X + centreDis.Y* centreDis.Y);
            

            if (lineOfSight == true)// direct follow movement
            {
                follow();
                memory = memoryStrength;
            }
            else if (memory > 0)
            {
                follow();
                memory --;
            }
            else // wander movement
            {
                move(theMaze, Direction.none);
            }
            if (memory == 1)
            {
                nextCoords[0] = (int)((getLocation().X+20) / 40);
                nextCoords[1] = (int)((getLocation().Y+20) / 40);
                currentCoords[0] = nextCoords[0];
                currentCoords[1] = nextCoords[1];
                memory = 0;
            }
            setLocation(getLocation() - Movement);
            //rotation = PI / 32;
            //Movement.X = 1 * (float)Math.Sin(rotation);//trig to edit players directional movement
            //Movement.Y = -1* (float)Math.Cos(rotation);

        }
        private void follow()
        {
            int speed = 1;
            Movement.X = speed * 1.1f * (float)Math.Sin(rotation);// --//--
            Movement.Y = -speed * 1.1f * (float)Math.Cos(rotation);
        }
        private void move(int[,] maze, Direction last)
        {
            Random rnd = new Random();

            if (disToGo > 0)
            {
                disToGo--;
                Vector2 distance = new Vector2(currentCoords[0] - nextCoords[0], currentCoords[1] - nextCoords[1]);
                Movement = distance;
                return;
            }
            currentCoords[0] = nextCoords[0];
            currentCoords[1] = nextCoords[1];
            setLocation(new Vector2( currentCoords[0] * 40, currentCoords[1]*40));
            disToGo = 40;
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            
            if(maze[currentCoords[0], currentCoords[1] - 1] == 0 )
            {
                dir[0] = Direction.none;
            }
            if (maze[currentCoords[0], currentCoords[1] + 1] == 0 )
            {
                dir[1] = Direction.none;
            }
            if (maze[currentCoords[0] + 1, currentCoords[1]] == 0 )
            {
                dir[2] = Direction.none;
            }
            if (maze[currentCoords[0] - 1, currentCoords[1]] == 0 )
            {
                dir[3] = Direction.none;
            }
            int value;
            while (true)
            {
                value = rnd.Next(4);
                if (dir[value] != Direction.none)
                {
                    break;
                }
            }
            switch (dir[value])
            {
                case Direction.North:
                    nextCoords[0] += 0;
                    nextCoords[1] -= 1;
                    last = Direction.South;
                    break;
                case Direction.South:
                    nextCoords[0] += 0;
                    nextCoords[1] += 1;
                    last = Direction.North;
                    break;
                case Direction.East:
                    nextCoords[0] += 1;
                    nextCoords[1] += 0;
                    last = Direction.West;
                    break;
                case Direction.West:
                    nextCoords[0] -= 1;
                    nextCoords[1] += 0;
                    last = Direction.East;
                    break;
                default:
                    nextCoords[0] = 0;
                    nextCoords[1] = 0;
                    break;
            }

        }
        public override void LoadContent(ContentManager Content)
        {
            //load monster texture
            if (textnum == 0)
            {
                Texture = Content.Load<Texture2D>(@"Monster2d");
            }
            else
            {
                throw new Exception("ID10T");
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, (float)rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        protected enum Direction
        {
            none,
            North,
            East,
            South,
            West
        }
    }
}
