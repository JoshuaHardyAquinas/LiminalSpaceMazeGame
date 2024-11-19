using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiminalSpaceMazeGame
{
    public class GenerateMaze
    {
        protected enum Direction
        {
            none,
            North,
            East,
            South,
            West
        }
        protected static Random rnd = new Random();
        public int[,] GenerateNewMaze(int mazeWidth,int mazeHeight)
        {
            //create maze using pre sizes
            int[,] newMaze = new int[mazeWidth, mazeHeight];
            //starting cords are 1,1 so there is an outer wall
            int[] startingCoords = { 1, 1 };
            int[] nextCoords = { 1, 1 };
            //begin recursive backtracking using variables that are already pre defined
            newMaze = backtrackingMazeAlg(nextCoords, startingCoords, newMaze, mazeWidth, mazeHeight);

            return newMaze;
        }
        protected static int[,] backtrackingMazeAlg(int[] prevCoords, int[] currentCoords, int[,] maze, int length, int width)
        {
            //array to tell program what random direction it can pick from
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            //sneaky hack to make walls on the outside of the play field by attempting to check parts of the map that don't exist and then setting that direction as null
            try{
                if (currentCoords[0] - 2 < 0 || maze[currentCoords[0] - 2, currentCoords[1]] != 0) // check north
                {
                    dir[0] = Direction.none;//set respective direction in array to null so it cannot be picked by rng alg as it already has path #1
                }
            }
            catch{dir[0] = Direction.none; }//set respective direction in array to null so it cannot be picked by rng alg as it does not exist #2
            try
            {
                if(currentCoords[0] + 2 > length - 1 || maze[currentCoords[0] + 2, currentCoords[1]] != 0) // check south 
                {
                    dir[1] = Direction.none;// --//-- #1
                }
            }
            catch { dir[1] = Direction.none; }//--//-- #2
            try
            {
                if (currentCoords[1] + 2 > width - 1 || maze[currentCoords[0], currentCoords[1] + 2] != 0) // check east 
                {
                    dir[2] = Direction.none;// --//-- #1
                }
            }
            catch { dir[2] = Direction.none; }// --//-- #2
            try
            {
                if (currentCoords[1] - 2 < 0 || maze[currentCoords[0], currentCoords[1] - 2] != 0) // check west 
                {
                    dir[3] = Direction.none;// --//-- #1
                }
            }
            catch { dir[3] = Direction.none; }// --//-- #2

            bool nullCase = true;
            foreach (Direction checkFree in dir)
            {
                if (checkFree != Direction.none)
                {
                    nullCase = false;
                    break;
                }
            }
            if (nullCase)//if there is no locations then backtrack to prec location
            {
                maze[currentCoords[0], currentCoords[1]] = 3;
                return maze;
            }
            else//if tehre is
            {
                int[] nextCoords = currentCoords;
                bool breakCase = true;
                List<Direction> available = new List<Direction>();
                for (int i = 0;i < dir.Length;i++)//optimization to stop rng calls for directions that are not possible
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
                            nextCoords[0] = currentCoords[0] - 2;//set x and y cords
                            nextCoords[1] = currentCoords[1];
                            maze[currentCoords[0] + 1, currentCoords[1]] = 1;//change null space to 1
                            break;
                        case Direction.South:
                            nextCoords[0] = currentCoords[0] + 2;
                            nextCoords[1] = currentCoords[1];
                            maze[currentCoords[0] - 1, currentCoords[1]] = 1;
                            break;
                        case Direction.East:
                            nextCoords[0] = currentCoords[0];
                            nextCoords[1] = currentCoords[1] + 2;
                            maze[currentCoords[0], currentCoords[1] - 1] = 1;
                            break;
                        case Direction.West:
                            nextCoords[0] = currentCoords[0];
                            nextCoords[1] = currentCoords[1] - 2;
                            maze[currentCoords[0], currentCoords[1] + 1] = 1;
                            break;
                        case Direction.none:
                            breakCase = false;
                            break;
                    }
                } while (breakCase == false);
                maze[currentCoords[0], currentCoords[1]] = 1;//adds next cords to maze
                prevCoords = (int[])currentCoords.Clone();
                currentCoords = (int[])nextCoords.Clone();

                backtrackingMazeAlg(currentCoords, nextCoords, maze, length, width);//backtracking
                backtrackingMazeAlg(currentCoords, prevCoords, maze, length, width);//move to next space in maze
            }
            return maze;//once backtracking is complete there is no other space to be than the start so maze is complete!
        }
    }
}
