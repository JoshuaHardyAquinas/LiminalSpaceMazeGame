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

        List<walls> wall = new List<walls>();

        private GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        SpriteFont GameFont;
        Random Rng = new Random();

        int mazeHieght = 11;
        int mazeWidth = 15;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
                        walls newWall = new walls(i, j);
                        newWall.LoadContent(Content);
                        wall.Add(newWall);
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
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < wall.Count; i++)
            {
                wall[i].draw(spriteBatch);
            }
            TheHero.draw(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
