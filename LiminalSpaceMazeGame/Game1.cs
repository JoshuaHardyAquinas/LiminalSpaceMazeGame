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
                if (wall.Edge.Intersects(TheHero.Edge))
                {
                    for (int i = 0;i<3;i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            TheHero.Location = TheHero.Location + new Vector2(i, j);
                            TheHero.Edge = new Rectangle((int)TheHero.Location.X, (int)TheHero.Location.Y, TheHero.textureHieght, TheHero.textureWidth);
                            if (wall.Edge.Intersects(TheHero.Edge) == false)
                            {
                                break;
                            }
                            else
                            {
                                TheHero.Location = TheHero.Location - new Vector2(i, j);
                            }
                            TheHero.Location = TheHero.Location + new Vector2(-i, -j);
                            TheHero.Edge = new Rectangle((int)TheHero.Location.X, (int)TheHero.Location.Y, TheHero.textureHieght, TheHero.textureWidth);
                            if (wall.Edge.Intersects(TheHero.Edge) == false)
                            {
                                break;
                            }
                            else
                            {
                                TheHero.Location = TheHero.Location - new Vector2(-i, -j);
                            }
                        }
                    }
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
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
