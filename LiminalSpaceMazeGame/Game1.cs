using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace LiminalSpaceMazeGame
{
    //test push
    public class Game1 : Game
    {
        KeyboardState ks1, ks2;
        Hero TheHero;
        Ray TheRay;
        GenerateMaze TheMaze;
        SpriteFont GameFont;

        List<Monster> monsters = new List<Monster>();

        List<Wall> walls = new List<Wall>();

        List<wall3d> walls3d = new List<wall3d>();

        public GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        int[,] maze;
        int mazeHeight = 17;
        int mazeWidth = 17;

        Vector2 distanceMoved;

        int rayHits = 0;

        //states to switch game between its respective screens
        enum GameState
        {
            StartMenu,
            LevelGen,
            InGame,
            Dead
        }
        //states to switch logic between dimensions
        enum Dimension
        {
            D2,
            D3
        }
        GameState currentState = GameState.StartMenu;
        Dimension CurrentDimension = Dimension.D2;

        Vector2 gameResolution = new Vector2(720, 720);
        public Game1()
        {
            ks1 = Keyboard.GetState();
            ks2 = Keyboard.GetState();
            this.IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            //change screen size
            
            _graphics.PreferredBackBufferWidth = (int)gameResolution.X;
            _graphics.PreferredBackBufferHeight = (int)gameResolution.Y;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //create hero and maze object
            TheHero = new Hero(90);
            TheMaze = new GenerateMaze();
            TheRay = new Ray();
            // TODO: Add your initialization logic here

            //makes 1st maze
            maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
            //creates wall entities to be written to the screen
            CreateWallEntities();
            Monster newMonster = new Monster(new Vector2(600, 600), 1);
            newMonster.LoadContent(Content);
            monsters.Add((newMonster));
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TheHero.LoadContent(Content);
            TheRay.LoadContent(Content);
            GameFont = Content.Load<SpriteFont>(@"File");
        }

        protected override void Update(GameTime gameTime)
        {
            ks1 = Keyboard.GetState();
            switch (currentState)
            {
                case GameState.StartMenu:
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.LevelGen;
                    }
                    break;
                case GameState.LevelGen:
                    monsters.Clear();
                    foreach (var wall in walls3d)// delete all textures to free up ram temp fix
                    {
                        wall.rectangle.Dispose();
                    }
                    Monster newMonster = new Monster(new Vector2(600,600),1);
                    monsters.Add((newMonster));
                    maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
                    CreateWallEntities();
                    TheHero.spawn();//put the hero back at its spawn location
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.InGame;
                    }
                    break;
                case GameState.InGame:
                    TheHero.update();
                    Vector2 centreDis = new Vector2(0, 0);
                    foreach (Monster monster in monsters)
                    {
                        monster.update();
                        if (monster.Edge.Intersects(TheHero.Edge))
                        {
                            TheHero.lo
                        }
                    }
                        //checks every singe wall for a collision, inefficient but not intensive enough that it causes issues since the 1st check is a collision check
                        
                    foreach (Wall wall in walls)
                    {
                        if (wall.Edge.Intersects(TheHero.Edge))
                        {
                            centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheHero.Location;//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move player away depending on what side is further on collision
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
                    if (CurrentDimension == Dimension.D3)
                    {
                        rayCast();
                    }
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.Dead;
                    }
                    break;
                case GameState.Dead:
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.StartMenu;
                    }
                    break;
                default:
                    break;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (ks1.IsKeyDown(Keys.NumPad1) && ks2.IsKeyUp(Keys.NumPad1))
            {
                CurrentDimension = Dimension.D2;
            }
            if (ks1.IsKeyDown(Keys.NumPad2) && ks2.IsKeyUp(Keys.NumPad2))
            {
                CurrentDimension = Dimension.D3;
            }
            ks2 = ks1;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void CreateWallEntities()
        {
            walls.Clear();
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeHeight; j++)
                {
                    if (maze[i, j] == 0)
                    {
                        Wall newWall = new Wall(i, j);
                        newWall.LoadContent(Content);
                        walls.Add(newWall);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentState)
            {
                case GameState.StartMenu:
                    this.IsMouseVisible = true;
                    GraphicsDevice.Clear(Color.Yellow);
                    spriteBatch.DrawString(GameFont, "welcome", new Vector2(0, 0), Color.Black);
                    break;

                case GameState.LevelGen:
                    GraphicsDevice.Clear(Color.Peru);
                    this.IsMouseVisible = true;
                    spriteBatch.DrawString(GameFont, "shop", new Vector2(0, 0), Color.Black);
                    spriteBatch.DrawString(GameFont, "example item 1", new Vector2(0, 0), Color.Black);
                    spriteBatch.DrawString(GameFont, "example item 2", new Vector2(10, 20), Color.Black);
                    spriteBatch.DrawString(GameFont, "upgrade health", new Vector2(10, 40), Color.Black);
                    spriteBatch.DrawString(GameFont, "upgrade armour", new Vector2(10, 60), Color.Black);
                    break;

                case GameState.InGame:
                    this.IsMouseVisible = false;
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    switch (CurrentDimension)
                    {
                        case Dimension.D2://2d representation
                            
                            //draw walls below player
                            for (int i = 0; i < walls.Count; i++)
                            {
                                walls[i].draw(spriteBatch);
                            }
                            //draw hero on top
                            TheHero.draw(spriteBatch);
                            TheRay.draw(spriteBatch);
                            string test = "ray hits" + rayHits.ToString();
                            spriteBatch.DrawString(GameFont, test, new Vector2(50, 0), Color.Black);
                            test = "time" + distanceMoved.ToString();
                            spriteBatch.DrawString(GameFont, test, new Vector2(150, 0), Color.Black);
                            break;
                        case Dimension.D3://3d representation
                            spriteBatch.DrawString(GameFont, "welcome", new Vector2(0, 360), Color.Black);
                            for (int i = 0; i < walls3d.Count; i++)
                            {
                                walls3d[i].draw(spriteBatch);
                            }
                            break;
                    }
                    break;

                case GameState.Dead:
                    GraphicsDevice.Clear(Color.Red);
                    this.IsMouseVisible = true;
                    spriteBatch.DrawString(GameFont, "dead", new Vector2(0, 0), Color.Black);
                    break;

                default:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.DrawString(GameFont, "how did you get here?", new Vector2(0, 0), Color.Black);
                    break;

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void rayCast()
        {
            if (TheHero.Movement != new Vector2(0, 0) || TheHero.changeRotation != 0)
            {
                foreach (var wall in walls3d)// delete all textures to free up ram temp fix
                {
                    wall.rectangle.Dispose();
                }
                walls3d.Clear();//clear wall list
                for (int i = -TheHero.FOV; i < TheHero.FOV; i++)
                {
                    Vector2 centreDis = new Vector2(0, 0);
                    Vector2 distanceTraveled = Ray.cast(i, TheHero, TheRay, walls, ref centreDis);

                    walls3d.Add(wall3d.generate3dWall(distanceTraveled, i + TheHero.FOV, gameResolution, GraphicsDevice, centreDis));
                }
            }
        }
    }
}