using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;

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

        List<Wall> walls = new List<Wall>();

        List<wall3d> walls3d = new List<wall3d>();

        public GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        int[,] maze;
        int mazeHieght = 17;
        int mazeWidth = 17;

        Vector2 dist;

        int rayHits = 0;

        float PI = 3.141592f;

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
            ks1 = Keyboard.GetState();
            ks2 = Keyboard.GetState();
            this.IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            //change screen size
            _graphics.PreferredBackBufferWidth = 720;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //create hero and maze object
            TheHero = new Hero(PI / 2);
            TheMaze = new GenerateMaze();
            TheRay = new Ray();
            // TODO: Add your initialization logic here

            //makes 1st maze
            maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHieght);
            //creats wall entities to be writen to the screen
            CreateWallEntities();
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
                case gamestate.StartMenu:
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = gamestate.LevelGen;
                    }
                    break;
                case gamestate.LevelGen:
                    maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHieght);
                    CreateWallEntities();
                    TheHero.spawn();//put the hero back at its spawn location
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
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
                    if (Dimension == dimension.D3)
                    {
                        rayCast();
                    }
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = gamestate.Dead;
                    }
                    break;
                case gamestate.Dead:
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
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
            if (ks1.IsKeyDown(Keys.NumPad1) && ks2.IsKeyUp(Keys.NumPad1))
            {
                Dimension = dimension.D2;
            }
            if (ks1.IsKeyDown(Keys.NumPad2) && ks2.IsKeyUp(Keys.NumPad2))
            {
                Dimension = dimension.D3;
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
                    switch (Dimension)
                    {
                        case dimension.D2://2d representation
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
                            test = "time" + dist.ToString();
                            spriteBatch.DrawString(GameFont, test, new Vector2(150, 0), Color.Black);
                            break;
                        case dimension.D3://3d representation
                            //TheHero.draw(spriteBatch);
                            //TheRay.draw(spriteBatch);
                            for (int i = 0; i < walls3d.Count; i++)
                            {
                                walls3d[i].draw(spriteBatch);
                            }
                            break;
                    }
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
        public void rayCast()
        {
            foreach (var wall in walls3d)
            {
                wall.rectangle.Dispose();
            }
            walls3d.Clear();
            for (int i = -45;i< 45; i++)
            {
                Vector2 distanceTraveled = cast(i);
                walls3d.Add(generate3dWall(distanceTraveled, i + 45));
            }
        }
        public Vector2 cast(int chAnge)
        {
            int speed = 10;

            
            TheRay.Location = TheHero.Location;
            //TheRay.Location = TheRay.Location + new Vector2(-chAnge * (float)Math.Sin(TheRay.rotation), chAnge * (float)Math.Cos(TheRay.rotation));
            TheRay.rotation = TheHero.rotation;
            TheRay.rotation = TheRay.rotation + (chAnge/180f)*PI;
            TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));
            Vector2 startloc = TheRay.Location;
            while (true)
            {
                TheRay.Location = TheRay.Location + TheRay.Movement;//move ray forward
                TheRay.update();//update hitbox
                foreach (Wall wall in walls)//loop though all wallys (ik its slow but its easy)
                {
                    if (wall.Edge.Intersects(TheRay.Edge))//check collision with hitbox
                    {
                        TheRay.Location = TheRay.Location - (TheRay.Movement + TheRay.Movement / 10);//move ray backwards a lil further than the last hit
                        while (true)//increase ray accuracy for a known hit by moving slower to the actual location
                        {
                            TheRay.Location = TheRay.Location + TheRay.Movement * 0.1f;//move 
                            TheRay.update();
                            if (wall.Edge.Intersects(TheRay.Edge))//check collision with hitbox
                            {
                                rayHits++;//increment for testing
                                return startloc - TheRay.Location;
                            }
                        }
                    }
                }
            }
        }
        public wall3d generate3dWall(Vector2 distance, int slice)
        {
            int XDist = Convert.ToInt32( Math.Abs(distance.X));
            int YDist = Convert.ToInt32(Math.Abs(distance.Y));

            int hieght;
            if(XDist > YDist)
            {
                hieght = XDist;
            }
            else
            {
                hieght = YDist;
            }
            Vector2 location = new Vector2(slice * 5+20,_graphics.PreferredBackBufferHeight / 2 + hieght/2);

            wall3d newWall = new wall3d(5, 128/(hieght)*16, location, GraphicsDevice, 1);
            return newWall;
        }
    }
}