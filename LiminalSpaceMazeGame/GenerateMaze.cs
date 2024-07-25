using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiminalSpaceMazeGame
{
    public class GenerateMaze
    {
        enum Direction
        {
            none,
            North,
            East,
            South,
            West
        }
        public static Random rnd = new Random();
        public int[,] GenerateNewMaze(int mazeWidth,int mazeHeight)
        {
            
            int[,] newMaze = new int[mazeWidth, mazeHeight];
            int[] startingCoords = { 1, 1 };
            int[] nextCoords = { 1, 1 };
            newMaze = backtrackingMazeAlg(nextCoords, startingCoords, newMaze, mazeWidth, mazeHeight);

            return newMaze;
        }
        protected static int[,] backtrackingMazeAlg(int[] prevCoords, int[] currentCoords, int[,] maze, int length, int width)
        {
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            try{
                if (currentCoords[0] - 2 < 0 || maze[currentCoords[0] - 2, currentCoords[1]] != 0) // north
                {
                    dir[0] = Direction.none;
                }
            }
            catch{dir[0] = Direction.none;}
            try
            {
                if(currentCoords[0] + 2 > length - 1 || maze[currentCoords[0] + 2, currentCoords[1]] != 0) // south
                {
                    dir[1] = Direction.none;
                }
            }
            catch { dir[1] = Direction.none;}
            try
            {
                if (currentCoords[1] + 2 > width - 1 || maze[currentCoords[0], currentCoords[1] + 2] != 0) // east
                {
                    dir[2] = Direction.none;
                }
            }
            catch { dir[2] = Direction.none; }
            try
            {
                if (currentCoords[1] - 2 < 0 || maze[currentCoords[0], currentCoords[1] - 2] != 0) // west
                {
                    dir[3] = Direction.none;
                }
            }
            catch { dir[3] = Direction.none; }
            bool nullCase = true;
            for (int i = 0; i < dir.Length; i++)
            {
                if (dir[i] != Direction.none)
                {
                    nullCase = false;
                    break;
                }
            }
            if (nullCase)
            {
                return maze;
            }
            else
            {
                int[] nextCoords = currentCoords;
                bool breakCase = true;
                do
                {
                    breakCase = true;
                    int number = rnd.Next(0, 4);
                    switch (dir[number])
                    {
                        case Direction.North:
                            nextCoords[0] = currentCoords[0] - 2;
                            nextCoords[1] = currentCoords[1];//Ai fix
                            maze[currentCoords[0] + 1, currentCoords[1]] = 1;
                            break;
                        case Direction.South:
                            nextCoords[0] = currentCoords[0] + 2;
                            nextCoords[1] = currentCoords[1];//Ai fix
                            maze[currentCoords[0] - 1, currentCoords[1]] = 1;
                            break;
                        case Direction.East:
                            nextCoords[0] = currentCoords[0];//Ai fix
                            nextCoords[1] = currentCoords[1] + 2;
                            maze[currentCoords[0], currentCoords[1] - 1] = 1;
                            break;
                        case Direction.West:
                            nextCoords[0] = currentCoords[0];//Ai fix
                            nextCoords[1] = currentCoords[1] - 2;
                            maze[currentCoords[0], currentCoords[1] + 1] = 1;

                            break;
                        case Direction.none:
                            breakCase = false;
                            break;
                    }
                } while (breakCase == false);
                maze[currentCoords[0], currentCoords[1]] = 1;
                prevCoords = (int[])currentCoords.Clone(); // Ai Fix
                currentCoords = (int[])nextCoords.Clone(); // Ai Fix

                backtrackingMazeAlg(currentCoords, nextCoords, maze, length, width);//backtracking ai fix
                backtrackingMazeAlg(currentCoords, prevCoords, maze, length, width);
            }
            return maze;
        }
    }
}
// https://chatgpt.com/c/8862f603-f6a6-4a79-8c4f-68f82d294ffa
//used to fix some bugs
