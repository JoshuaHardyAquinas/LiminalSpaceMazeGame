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
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1280;  // Width of the window
            _graphics.PreferredBackBufferHeight = 720;  // Height of the window
            _graphics.ApplyChanges();
        }
        protected override void Initialize()
        {
            TheHero = new Hero();
            TheMaze = new GenerateMaze();
            // TODO: Add your initialization logic here

            base.Initialize();
            int[,] maze = TheMaze.GenerateNewMaze(mazeWidth,mazeHieght);
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
            foreach (Wall wall in walls)
            {
                /*
                float updis = Math.Abs(wall.Edge.Top + wall.Location.Y- TheHero.Location.Y);
                float downdis = Math.Abs(wall.Edge.Bottom + wall.Location.Y - TheHero.Location.Y);
                float leftdis = Math.Abs(wall.Edge.Right +wall.Location.X - TheHero.Location.X);
                float rightdis = Math.Abs(wall.Edge.Left + wall.Location.X - TheHero.Location.X);
                */
                
                
                if (wall.Edge.Intersects(TheHero.Edge))
                {
                    Vector2 centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheHero.Location;
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
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].draw(spriteBatch);
            }
            TheHero.draw(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
