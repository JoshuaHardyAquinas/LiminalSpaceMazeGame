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
        KeyboardState ks1, ks2;
        Hero TheHero;
        Ray TheRay;
        UI TheUI;
        GenerateMaze TheMaze;
        SpriteFont GameFont;

        List<Monster> monsters = new List<Monster>();

        List<Wall> walls = new List<Wall>();
        List<ObjInGame> gameObjects = new List<ObjInGame>();

        List<wall3d> walls3d = new List<wall3d>();
        List<wall3d> object3d = new List<wall3d>();

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
            TheUI = new UI();

            //makes 1st maze
            maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
            //creates wall entities to be written to the screen
            CreateWallEntities();
            Monster newMonster = new Monster(new Vector2(620, 620), 1);
            newMonster.LoadContent(Content);
            monsters.Add((newMonster));
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TheHero.LoadContent(Content);
            TheRay.LoadContent(Content);
            TheUI.LoadContent(Content);
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
                    Monster newMonster = new Monster(new Vector2(180,100),1);
                    newMonster.LoadContent(Content);
                    monsters.Add(newMonster);
                    maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
                    //maze = new int[mazeWidth, mazeHeight];
                    CreateWallEntities();
                    
                    foreach (var wall in walls)
                    {
                        ObjInGame newObj = new ObjInGame();
                        newObj.objectEdge = wall.Edge;
                        newObj.objectLocation = wall.getLocation();
                        newObj.name = 'W';
                        gameObjects.Add(newObj);
                    }
                    TheHero.spawn(new Vector2(60,60));//put the hero back at its spawn location
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.InGame;
                    }
                    break;
                case GameState.InGame:
                    TheHero.update();
                    foreach(Monster monster in monsters)
                    {
                        monster.update(TheHero);
                    }
                    Vector2 centreDis = new Vector2(0, 0);
                    foreach (Monster monster in monsters)
                    {
                        monster.update();
                        if (monster.Edge.Intersects(TheHero.Edge))
                        {
                            TheHero.loseHealth(100);
                        }
                    }
                        //checks every singe wall for a collision, inefficient but not intensive enough that it causes issues since the 1st check is a collision check
                        
                    foreach (Wall wall in walls)
                    {
                        if (wall.Edge.Intersects(TheHero.Edge))
                        {
                            centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheHero.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move player away depending on what side is further on collision
                            {
                                TheHero.setLocation(new Vector2(centreDis.X * -0.125f+TheHero.getLocation().X, TheHero.getLocation().Y));
                            }
                            else
                            {
                                TheHero.setLocation(new Vector2(TheHero.getLocation().X, centreDis.Y * -0.125f+TheHero.getLocation().Y));
                            }
                            break;
                        }
                        foreach (Monster monster in monsters)
                        {
                            if (wall.Edge.Intersects(monster.Edge))
                            {
                                centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - monster.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                                if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move monster away depending on what side is further on collision
                                {
                                    monster.setLocation(new Vector2(centreDis.X * -0.125f + monster.getLocation().X, monster.getLocation().Y));
                                }
                                else
                                {
                                    monster.setLocation(new Vector2(monster.getLocation().X, centreDis.Y * -0.125f + monster.getLocation().Y));
                                }
                                break;
                            }
                        }
                    }
                    if (CurrentDimension == Dimension.D3)
                    {
                        walls3d.Clear();//clear wall list
                        
                        foreach (var monster in monsters)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = monster.Edge;
                            newObj.objectLocation = monster.getLocation();
                            newObj.name = 'M';
                            gameObjects.Add(newObj);
                        }
                        rayCast(330, 'M');
                        foreach (var wall in walls)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = wall.Edge;
                            newObj.objectLocation = wall.getLocation();
                            newObj.name = 'W';
                            gameObjects.Add(newObj);
                        }
                        rayCast(660, 'W');
                    }
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.Dead;
                    }
                    break;
                case GameState.Dead:
                    monsters.Clear();
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
                            for (int i = 0; i < monsters.Count; i++)
                            {
                                monsters[i].draw(spriteBatch);
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
                            List<wall3d> CURRENT = new List<wall3d>();
                            foreach (wall3d wall in walls3d)
                            {
                                if (wall.type == 'W')
                                {
                                    wall.draw(spriteBatch);
                                }
                                CURRENT.Add(wall);
                            }
                            foreach (wall3d wall in walls3d)
                            {
                                if (wall.type != 'W')
                                {
                                    foreach(wall3d pre in CURRENT)
                                    {
                                        if (wall.getLocation().X == pre.getLocation().X)
                                        {
                                            if(wall.Height > pre.Height)
                                            {
                                                wall.draw(spriteBatch);
                                            }
                                        }
                                    }
                                }
                            }
                            TheUI.draw(spriteBatch);
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
        public void rayCast(int castlength,char toHit)
        {
            for (int i = -TheHero.FOV; i < TheHero.FOV; i++)
            {
                char objHit = ' ';
                Vector2 centreDis = new Vector2(0, 0);
                Vector2 distanceTraveled = Ray.cast(i, TheHero, TheRay, gameObjects, ref centreDis, castlength, ref objHit, toHit);
                if (distanceTraveled != new Vector2(castlength, castlength))
                {
                    wall3d newSlice = wall3d.generate3dWall(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit);
                    newSlice.LoadContent(Content);
                    walls3d.Add(newSlice);
                }
            }
            foreach (wall3d wall in walls3d)
            {
                wall.LoadContent(Content);
            }
            int t = 0;
            for (int j = 0; j < gameObjects.Count; j++)
            {
                if (gameObjects[j].name != 'W')
                {
                    t = j-1;
                    break;
                }
            }
            //gameObjects.RemoveRange(t, gameObjects.Count);
            gameObjects.Clear();
        }
        public struct ObjInGame()
        {
            public Rectangle objectEdge;
            public Vector2 objectLocation;
            public char name;
        }
    }
}