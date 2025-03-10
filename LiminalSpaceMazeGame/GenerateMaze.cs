﻿using System;

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
        private static Random rnd = new Random();
        public int[,] GenerateNewMaze(int mazeWidth, int mazeHeight)
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
        protected static int[,] backtrackingMazeAlg(int[] nextCoords, int[] currentCoords, int[,] maze, int length, int width)
        {
            //array to tell program what random direction it can pick from
            Direction[] dir = {
                Direction.North,
                Direction.South,
                Direction.East,
                Direction.West
            };
            //sneaky hack to make walls on the outside of the play field by attempting to check parts of the map that don't exist and then setting that direction as null
            if (currentCoords[0] - 2 < 0 || maze[currentCoords[0] - 2, currentCoords[1]] != 0) // check north
            {
                dir[0] = Direction.none;//set respective direction in array to null so it cannot be picked by rng alg as it already has path #1
            }
            if (currentCoords[0] + 2 > length - 1 || maze[currentCoords[0] + 2, currentCoords[1]] != 0) // check south 
            {
                dir[1] = Direction.none;// --//-- #1
            }
            if (currentCoords[1] + 2 > width - 1 || maze[currentCoords[0], currentCoords[1] + 2] != 0) // check east 
            {
                dir[2] = Direction.none;// --//-- #1
            }
            if (currentCoords[1] - 2 < 0 || maze[currentCoords[0], currentCoords[1] - 2] != 0) // check west 
            {
                dir[3] = Direction.none;// --//-- #1
            }

            bool nullCase = true;
            foreach (Direction checkFree in dir)
            {
                if (checkFree != Direction.none)
                {
                    nullCase = false;
                    break;
                }
            }
            if (nullCase)//if there is no locations then backtrack to prev location
            {
                maze[currentCoords[0], currentCoords[1]] += 1;
                return maze;
            }
            else//if there is
            {
                nextCoords = currentCoords;
                bool breakCase;
                do//loop though setting the necessary cords depending on direction
                {
                    breakCase = true;
                    int number = rnd.Next(0, 4);
                    switch (dir[number])
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
                currentCoords = (int[])nextCoords.Clone();
                backtrackingMazeAlg(nextCoords, nextCoords, maze, length, width);//backtracking
                backtrackingMazeAlg(currentCoords, currentCoords, maze, length, width);//move to next space in maze
            }
            return maze;//once backtracking is complete there is no other space to be than the start so maze is complete!
        }
    }
}
