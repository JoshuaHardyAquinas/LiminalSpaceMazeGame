using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LiminalSpaceMazeGame
{
    //test push
    public class Game1 : Game
    {
        Hero TheHero;
        GenerateMaze TheMaze;

        List<Wall> walls = new List<Wall>();

        private GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;


        int mazeHieght = 17;
        int mazeWidth = 17;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //change screen size
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720; 
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //create hero and maze object
            TheHero = new Hero();
            TheMaze = new GenerateMaze();
            // TODO: Add your initialization logic here

            //makes 1st maze
            int[,] maze = TheMaze.GenerateNewMaze(mazeWidth,mazeHieght);
            //creats wall entities to be writen to the screen
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeHieght; j++)
                {
                    if (maze[i,j] == 0) {
                        Wall newWall = new Wall(i, j);
                        newWall.LoadContent(Content);
                        walls.Add(newWall);
                    }
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TheHero.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            TheHero.update();
            //checks every singe wall for a collision, ineficient but not intensive enough that it causes issues since the 1st check is a collision check
            foreach (Wall wall in walls)
            {
                if (wall.Edge.Intersects(TheHero.Edge))
                {
                    Vector2 centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheHero.Location;//make variable to determine the vector distance away from the center of  the wall in question
                    //move player away depending on what side is furthur on collision
                    if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))
                    {
                        TheHero.Location.X += centreDis.X * -0.125f;
                    }
                    else
                    {
                        TheHero.Location.Y += centreDis.Y * -0.125f;
                    }
                    break;
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //draw walls below player
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].draw(spriteBatch);
            }
            //draw hero on top
            TheHero.draw(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
