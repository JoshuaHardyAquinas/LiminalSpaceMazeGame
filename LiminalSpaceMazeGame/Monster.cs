using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;

namespace LiminalSpaceMazeGame
{
     class Monster : MovingObject
    {
        private int textnum = 0;
        private int damage;
        private int memoryStrength;
        private int health;
        private int maxHealth;
        public bool lineOfSight = false;
        public float memory = 0;
        private bool blockoff = true;
        private int[] nextCoords = { 0, 0 };
        int[] currentCoords = { 0, 0 };
        float disToGo = 40;
        private bool dead = false;
        private Vector2 spawnLoc;
        int[,] maze;
        public bool shootable;
        Direction previous = Direction.none;

        public new int Damage { get => damage; set => damage = value; }

        public Monster(Vector2 startingLoc, int text, int multistat, int[,] theMaze)
        {
            maze = (int[,])theMaze.Clone();
            Random random = new Random();
            damage = 2 * random.Next((int)Math.Sqrt(multistat)*2,multistat*2);
            memoryStrength = 5 * multistat;
            health = random.Next(multistat*2, multistat*4);
            maxHealth = health;
            Speed = (float)random.NextDouble() *((float)Math.Sqrt(Math.Sqrt(multistat))-1) + 1f;
            spawn(startingLoc);
            textnum = 0;
            rotation = 0f;
            currentCoords[0] = (int)startingLoc.X/40;
            currentCoords[1] = (int)startingLoc.Y/40;
            nextCoords[0] = currentCoords[0];
            nextCoords[1] = currentCoords[1];
            spawnLoc = startingLoc;
        }
        public void update(Hero theHero)
        {
            base.update();
            //creates player edge
            Vector2 centreDis = theHero.getLocation() - getLocation();
            double tangent = (double)Math.Sqrt(centreDis.X* centreDis.X + centreDis.Y* centreDis.Y);
            

            if (lineOfSight == true && disToGo == 0)// direct follow movement
            {
                for (int i = 0; i > 17; i++)
                {
                    for (int j = 0; j > 17; i++)
                    {
                        if (maze[i,j] == 6)
                        {
                            maze[i,j] = 1;
                        }
                    }
                }
                follow();
                memory = memoryStrength/Speed;
            }
            else if (memory > 0)
            {
                previous = Direction.none;
                follow();
                memory--;
            }
            else // wander movement
            {
                move(theHero);
            }
            if (memory == 1)
            {
                nextCoords[0] = (int)((getLocation().X + 20) / 40);
                nextCoords[1] = (int)((getLocation().Y + 20) / 40);
                currentCoords[0] = nextCoords[0];
                currentCoords[1] = nextCoords[1];
                memory = 0;
            }
            setLocation(getLocation() - (Movement*new Vector2(Speed,Speed)));
            //rotation = PI / 32;
            //Movement.X = 1 * (float)Math.Sin(rotation);//trig to edit players directional movement
            //Movement.Y = -1* (float)Math.Cos(rotation);

        }
        private void follow()
        {
            Movement.X = 1.1f * (float)Math.Sin(rotation);// --//--
            Movement.Y = -1.1f * (float)Math.Cos(rotation);
        }
        private void move(Hero theHero)
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
            setLocation(new Vector2(currentCoords[0] * 40, currentCoords[1] * 40));
            disToGo = 40f/Speed;
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            //find all directions that the monster is able to move in
            if (maze[currentCoords[0], currentCoords[1] - 1] == 0 || maze[currentCoords[0], currentCoords[1] - 1] == 5 || maze[currentCoords[0], currentCoords[1] - 1] == 6 || previous == Direction.North)
            {
                dir[0] = Direction.none;
            }
            if (maze[currentCoords[0], currentCoords[1] + 1] == 0 || maze[currentCoords[0], currentCoords[1] + 1] == 5 || maze[currentCoords[0], currentCoords[1] + 1] == 6 || previous == Direction.South)
            {
                dir[1] = Direction.none;
            }
            if (maze[currentCoords[0] + 1, currentCoords[1]] == 0 || maze[currentCoords[0] + 1, currentCoords[1]] == 5 || maze[currentCoords[0] + 1, currentCoords[1]] == 6 || previous == Direction.East)
            {
                dir[2] = Direction.none;
            }
            if (maze[currentCoords[0] - 1, currentCoords[1]] == 0 || maze[currentCoords[0] - 1, currentCoords[1]] == 5 || maze[currentCoords[0] - 1, currentCoords[1]] == 6 || previous == Direction.West)
            {
                dir[3] = Direction.none;
            }
            int value;
            int count = 0;
            foreach (Direction direction in dir)//check if any directions are possible
            {
                if (direction != Direction.none)
                {
                    count++;
                }
            }
            if (count >1)//method to block off dead ends to prevent looping
            {
                blockoff = false;
            }
            if(blockoff&& count == 1)
            {
                maze[currentCoords[0], currentCoords[1]] = 6;
            }
            if (count == 0)//fail back and reset previous direction so the monster can backtrack
            {
                maze[currentCoords[0], currentCoords[1]] = 6;
                blockoff = true;
                previous = Direction.none;
                return;
            }
            Vector2 centreDis = theHero.getLocation() - getLocation();//check if the vertical or horizontal distance is greater
            if (Math.Abs(centreDis.Y) >= Math.Abs(centreDis. X))
            {
                if (getLocation().Y >= theHero.getLocation().Y && dir[0] != Direction.none)//if the player is lower move up if possible
                {
                    nextCoords[1] -= 1;
                    previous = Direction.South;
                    return;
                }
                else if (getLocation().Y < theHero.getLocation().Y && dir[1] != Direction.none)// if player is higher move down
                {
                    nextCoords[1] += 1;
                    previous = Direction.North;
                    return;
                }
            }
            else
            {
                if (getLocation().X < theHero.getLocation().X && dir[2] != Direction.none)//east if west
                {
                    nextCoords[0] += 1;
                    previous = Direction.West;
                    return;
                }
                else if (getLocation().X > theHero.getLocation().X && dir[3] != Direction.none)//west if east
                {
                    nextCoords[0] -= 1;
                    previous = Direction.East;
                    return;
                }
            }
            // fall back if the above algorithm, comes back with a constant/ equal value
            while (true)//rng the direction to get out of the error situation
            {
                value = rnd.Next(4);
                if (dir[value] != Direction.none)
                {
                    break;
                }
            }
            switch (dir[value])//send in direction and mark opposite direction as the direction i just came from
            {
                case Direction.North:
                    nextCoords[1] -= 1;
                    previous = Direction.South;
                    break;
                case Direction.South:
                    nextCoords[1] += 1;
                    previous = Direction.North;
                    break;
                case Direction.East:
                    nextCoords[0] += 1;
                    previous = Direction.West;
                    break;
                case Direction.West:
                    nextCoords[0] -= 1;
                    previous = Direction.East;
                    break;
                default:
                    previous = Direction.none;
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
                Texture = Content.Load<Texture2D>(@"NullVoidDead");
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, (float)rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public void loseHealth(int toLose)
        {
            health -= toLose;//teleport outside the map and disable monster 
            if (health <= 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (maze[i, j] == 6)
                        {
                            maze[i, j] = 1;
                        }
                    }
                }
                disToGo = 160;
                spawn(spawnLoc);
                health = maxHealth;
            }
        }
        public int gethealth()
        {
            return health;
        }
        public int gethealthMax()
        {
            return maxHealth;
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