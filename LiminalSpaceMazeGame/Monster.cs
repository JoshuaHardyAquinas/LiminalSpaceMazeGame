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

namespace LiminalSpaceMazeGame
{
     class Monster : MovingObject
    {
        private int textnum = 0;
        private int damage;
        public bool lineOfSight = false;
        public int memory;
        private bool newNode = false;
        private int[] nextCoords = {0,0 };
        int[] currentCoords = { 0,0 };

        public int Damage { get => damage; set => damage = value; }

        public Monster(Vector2 startingLoc, int text, int MaxDamage)
        {
            Random random = new Random();
            damage = random.Next(4,MaxDamage+1);
            spawn(startingLoc);
            textnum = 0;
            rotation = 0f;
            nextCoords[0] = (int)(getLocation().X / 40);
            nextCoords[1] = (int)(getLocation().Y / 40);
            Vector2 tile = new Vector2((float)Math.Round(getLocation().X / 40, 0), (float)Math.Round(getLocation().Y / 40, 0));

            currentCoords[0] = (int)tile.X;
            currentCoords[1] = (int)tile.Y;
        }
        public void update(Hero theHero, int [,] theMaze)
        {
            
            base.update();
            //creates player edge
            Vector2 centreDis = theHero.getLocation() - getLocation();
            double tangent = (double)Math.Sqrt(centreDis.X* centreDis.X + centreDis.Y* centreDis.Y);
            int speed = 1;
            /*if (tangent<200 || lineOfSight == true)
            {
                Movement.X = speed * (float)Math.Sin(rotation);//trig to edit players directional movement
                Movement.Y = -speed * (float)Math.Cos(rotation);
            }*/
            move(theMaze);
            //rotation = PI / 32;
            //Movement.X = 1 * (float)Math.Sin(rotation);//trig to edit players directional movement
            //Movement.Y = -1* (float)Math.Cos(rotation);
            setLocation(getLocation() + Movement);
        }
        public void move(int[,] maze)
        {
            Random rnd = new Random();
            Vector2 tile = new Vector2((getLocation().X/40),(getLocation().Y/40));

            currentCoords[0] = (int)tile.X;
            currentCoords[1] = (int)tile.Y;

            if (currentCoords[0] != nextCoords[0] || currentCoords[1] != nextCoords[1])
            {
                Vector2 distance = new Vector2(currentCoords[0] - nextCoords[0], currentCoords[1] - nextCoords[1]);
                Movement = distance;
                return;
            }
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            if(maze[currentCoords[0], currentCoords[1] - 1] == 0)
            {
                dir[0] = Direction.none;
            }
            if (maze[currentCoords[0], currentCoords[1] + 1] == 0)
            {
                dir[1] = Direction.none;
            }
            if (maze[currentCoords[0] + 1, currentCoords[1]] == 0)
            {
                dir[2] = Direction.none;
            }
            if (maze[currentCoords[0] - 1, currentCoords[1]] == 0)
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
                    nextCoords[0] = 0;
                    nextCoords[1] -= 1;
                    break;
                case Direction.South:
                    nextCoords[0] += 0;
                    nextCoords[1] += 1;
                    break;
                case Direction.East:
                    nextCoords[0] += 1;
                    nextCoords[1] += 0;
                    break;
                case Direction.West:
                    nextCoords[0] -= 1;
                    nextCoords[1] += 0;
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
