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

        public int Damage { get => damage; set => damage = value; }

        public Monster(Vector2 startingLoc, int text, int MaxDamage)
        {
            Random random = new Random();
            damage = random.Next(4,MaxDamage+1);
            spawn(startingLoc);
            textnum = 0;
            rotation = 0f;
            nextCoords[0] = (int)Math.Round(getLocation().X / 40, 0);
            nextCoords[1] = (int)Math.Round(getLocation().Y / 40, 0);
        }
        public void update(Hero theHero, int [,] theMaze)
        {
            
            base.update();
            //creates player edge
            Vector2 centreDis = theHero.getLocation() - getLocation();
            double tangent = (double)Math.Sqrt(centreDis.X* centreDis.X + centreDis.Y* centreDis.Y);
            int speed = 1;
            if (tangent<200 || lineOfSight == true)
            {
                Movement.X = speed * (float)Math.Sin(rotation);//trig to edit players directional movement
                Movement.Y = -speed * (float)Math.Cos(rotation);
            }
            else
            {
                move(theMaze);
            }
            //rotation = PI / 32;
            //Movement.X = 1 * (float)Math.Sin(rotation);//trig to edit players directional movement
            //Movement.Y = -1* (float)Math.Cos(rotation);
            setLocation(getLocation() + Movement);
        }
        public void move(int[,] maze)
        {
            Random rnd = new Random();
            Vector2 tile = new Vector2((float)Math.Round(getLocation().X/40,0), (float)Math.Round(getLocation().Y/40,0));

            int[] currentCoords = { (int)tile.X, (int)tile.Y };
            if (currentCoords[0] != nextCoords[0] || currentCoords[1] != nextCoords[1])
            {
                Vector2 distance = new Vector2(-1*(nextCoords[1] - currentCoords[1]), -1*(nextCoords[0] - currentCoords[0]));
                Movement = distance;
                return;
            }
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            //sneaky hack to make walls on the outside of the play field by attempting to check parts of the map that don't exist and then setting that direction as null
            try
            {
                if (currentCoords[0] - 1 < 0 || maze[currentCoords[0] - 1, currentCoords[1]] != 0) // check north
                {
                    dir[0] = Direction.none;//set respective direction in array to null so it cannot be picked by rng alg as it already has path #1
                }
            }
            catch { dir[0] = Direction.none; }//set respective direction in array to null so it cannot be picked by rng alg as it does not exist #2
            try
            {
                if (currentCoords[0] + 1 > 17 - 1 || maze[currentCoords[0] + 1, currentCoords[1]] != 0) // check south 
                {
                    dir[1] = Direction.none;// --//-- #1
                }
            }
            catch { dir[1] = Direction.none; }//--//-- #2
            try
            {
                if (currentCoords[1] + 1 > 17 - 1 || maze[currentCoords[0], currentCoords[1] + 1] != 0) // check east 
                {
                    dir[2] = Direction.none;// --//-- #1
                }
            }
            catch { dir[2] = Direction.none; }// --//-- #2
            try
            {
                if (currentCoords[1] - 1 < 0 || maze[currentCoords[0], currentCoords[1] - 1] != 0) // check west 
                {
                    dir[3] = Direction.none;// --//-- #1
                }
            }
            catch { dir[3] = Direction.none; }
            bool nullCase = true;
            foreach (Direction checkFree in dir)
            {
                if (checkFree != Direction.none)
                {
                    nullCase = false;
                    break;
                }
            }
            List<Direction> available = new List<Direction>();
            bool breakCase = true;
            for (int i = 0; i < dir.Length; i++)//optimization to stop rng calls for directions that are not possible
            {
                if (dir[i] != Direction.none)
                {
                    available.Add(dir[i]);
                }
            }
            
            do//loop though setting the necessary cords depending on direction
            {
                breakCase = true;
                int number = rnd.Next(0, available.Count);
                switch (available[number])
                {
                    case Direction.North:
                        nextCoords[0] = currentCoords[0] - 1;//set x and y cords
                        nextCoords[1] = currentCoords[1];
                        break;
                    case Direction.South:
                        nextCoords[0] = currentCoords[0] + 1;
                        nextCoords[1] = currentCoords[1];
                        break;
                    case Direction.East:
                        nextCoords[0] = currentCoords[0];
                        nextCoords[1] = currentCoords[1] + 1;
                        break;
                    case Direction.West:
                        nextCoords[0] = currentCoords[0];
                        nextCoords[1] = currentCoords[1] - 1;
                        break;
                    case Direction.none:
                        breakCase = false;
                        break;
                }
            } while (breakCase == false);

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
