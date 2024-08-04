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
        SpriteFont GameFont;

        List<Wall> walls = new List<Wall>();

        private GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        int[,] maze;
        int mazeHieght = 17;
        int mazeWidth = 17;
        //int mazeHeight;

        //states to switch game between its respective screens
        enum gamestate
        {
            StartMenu,
            LevelGen,
            InGame,
            Dead
        }
        //states to swith logic between dimensions
        enum dimension
        {
            D2,
            D3
        }

        gamestate currentState = gamestate.StartMenu;
        dimension Dimension = dimension.D2;
        public Game1()
        {
            this.IsMouseVisible = true;
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
            maze = TheMaze.GenerateNewMaze(mazeWidth,mazeHieght);
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
            GameFont = Content.Load<SpriteFont>(@"File");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            switch (currentState)
            {
                case gamestate.StartMenu:
                    if (ks.IsKeyDown(Keys.Enter))
                    {
                        currentState = gamestate.LevelGen;
                    }
                    break;
                case gamestate.LevelGen:
                    maze = TheMaze.GenerateNewMaze(mazeWidth,mazeHieght);
                    walls.Clear();
                    for (int i = 0; i < mazeWidth; i++)
                    {
                        for (int j = 0; j < mazeHieght; j++)
                        {
                            if (maze[i, j] == 0)
                            {
                                Wall newWall = new Wall(i, j);
                                newWall.LoadContent(Content);
                                walls.Add(newWall);
                            }
                        }
                    }
                    if (ks.IsKeyDown(Keys.Enter))
                    {
                        currentState = gamestate.InGame;
                    }
                    break;
                case gamestate.InGame:
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
                    if (ks.IsKeyDown(Keys.Enter))
                    {
                        currentState = gamestate.Dead;
                    }
                    break;
                case gamestate.Dead:
                    if (ks.IsKeyDown(Keys.Enter))
                    {
                        currentState = gamestate.StartMenu;
                    }
                    break;
                default: 
                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (ks.IsKeyDown(Keys.Enter))
            {
                System.Threading.Thread.Sleep(100);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentState)
            {
                case gamestate.StartMenu:
                    this.IsMouseVisible = true;
                    GraphicsDevice.Clear(Color.Yellow);
                    spriteBatch.DrawString(GameFont, "welcome", new Vector2(0, 0), Color.Black);
                    break;
                case gamestate.LevelGen:
                    GraphicsDevice.Clear(Color.Peru);
                    this.IsMouseVisible = false;
                    spriteBatch.DrawString(GameFont, "maze gen", new Vector2(0, 0), Color.Black);
                    break;
                case gamestate.InGame:
                    this.IsMouseVisible = false;
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    //draw walls below player
                    for (int i = 0; i < walls.Count; i++)
                    {
                        walls[i].draw(spriteBatch);
                    }
                    //draw hero on top
                    TheHero.draw(spriteBatch);
                    break;
                case gamestate.Dead:
                    GraphicsDevice.Clear(Color.Red);

                    this.IsMouseVisible = true;
                    spriteBatch.DrawString(GameFont, "dead", new Vector2(0, 0), Color.Black);
                    break;
                default:
                    GraphicsDevice.Clear(Color.White);

                    spriteBatch.DrawString(GameFont, "error", new Vector2(0, 0), Color.Black);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}