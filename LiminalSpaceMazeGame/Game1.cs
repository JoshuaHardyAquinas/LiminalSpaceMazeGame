using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;


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
        UILoadingBar StaminaBar;
        UILoadingBar HealthBar;
        UILoadingBar ShieldBar;

        public float levelNumber = 0;
        public bool levelGen = false;
        public bool experimental = true;


        List<Monster> monsters = new List<Monster>();

        List<Wall> walls = new List<Wall>();
        List<ObjInGame> gameObjects = new List<ObjInGame>();

        List<wall3d> walls3d = new List<wall3d>();

        public GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        List<ExitDoor> Exits = new List<ExitDoor>();


        int[,] maze;
        int mazeHeight = 17;
        int mazeWidth = 17;
        int monstermax = 1;

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
            TheHero = new Hero(90,1200,1000);
            TheMaze = new GenerateMaze();
            TheRay = new Ray();
            TheUI = new UI(); 
            StaminaBar = new UILoadingBar(new Vector2(TheUI.getLocation().X+590f,TheUI.getLocation().Y+20f),TheHero.StaminaMax,120,20);
            HealthBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 69f, TheUI.getLocation().Y+12f), TheHero.maxHealth, 69, 6);
            ShieldBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 69f, TheUI.getLocation().Y + 40f), TheHero.StaminaMax, 69, 86);
            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TheHero.LoadContent(Content);
            TheRay.LoadContent(Content);
            TheUI.LoadContent(Content);
            GameFont = Content.Load<SpriteFont>(@"File");
            StaminaBar.LoadContent(Content);
            HealthBar.LoadContent(Content);
            ShieldBar.LoadContent(Content);
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
                    if (!levelGen)
                    {
                        levelNumber++;
                        maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
                        maze[1, 1] = 1;
                        createEntities();
                        TheHero.spawn(new Vector2(40, 40));//put the hero back at its spawn location
                        levelGen = true;
                        
                    }
                    if (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))
                    {
                        currentState = GameState.InGame;
                    }
                    break;
                case GameState.InGame:
                    TheHero.update();
                    StaminaBar.update(TheHero.Stamina);
                    HealthBar.update(TheHero.checkHealth());
                    ShieldBar.update(TheHero.shield);

                    foreach (Monster monster in monsters)
                    {
                        monster.update(TheHero, maze);
                        gameObjects.Clear();
                        ObjInGame player = new ObjInGame();
                        player.objectEdge = TheHero.Edge;
                        player.objectLocation = TheHero.getLocation();
                        player.name = 'P';
                        Vector2 anglemath = monster.getLocation() - TheHero.getLocation();
                        double angle = Math.Atan(anglemath.X / -anglemath.Y);
                        monster.rotation = +angle;
                        if (monster.rotation > 3.14 / 2 && monster.rotation < (3 * 3.14) / 2)
                        {
                            monster.rotation = +3.14;
                        }
                        if (monster.rotation > 3.14 * 2)
                        {
                            monster.rotation = -(3.14 / 2);
                        }
                        shotsFired(monster.rotation, monster.getLocation());
                        gameObjects.Clear();

                    }
                    foreach (ExitDoor exitDoor in Exits)
                    {
                        exitDoor.update();
                    }
                    Vector2 centreDis = new Vector2(0, 0);
                    foreach (Wall wall in walls)
                    {
                        if (wall.Edge.Intersects(TheHero.Edge))
                        {
                            centreDis = new Vector2(wall.Edge.Center.X, wall.Edge.Center.Y) - TheHero.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move player away depending on what side is further on collision
                            {
                                TheHero.setLocation(new Vector2(centreDis.X * -0.125f + TheHero.getLocation().X, TheHero.getLocation().Y));
                            }
                            else
                            {
                                TheHero.setLocation(new Vector2(TheHero.getLocation().X, centreDis.Y * -0.125f + TheHero.getLocation().Y));
                            }
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
                            }
                        }
                    }
                    foreach (Monster monster in monsters)
                    {
                        monster.update();
                        
                        if (monster.Edge.Intersects(TheHero.Edge))
                        {
                            centreDis = TheHero.getLocation() - monster.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move monster away depending on what side is further on collision
                            {
                                monster.setLocation(new Vector2(centreDis.X * -0.25f + monster.getLocation().X, monster.getLocation().Y));
                                TheHero.setLocation(new Vector2(centreDis.X * +0.125f + TheHero.getLocation().X, TheHero.getLocation().Y));
                            }
                            else
                            {
                                monster.setLocation(new Vector2(monster.getLocation().X, centreDis.Y * -0.25f + monster.getLocation().Y));
                                TheHero.setLocation(new Vector2(TheHero.getLocation().X, centreDis.Y * +0.125f + TheHero.getLocation().Y));
                            }
                            TheHero.loseHealth(monster.Damage);
                        }
                        foreach (Monster mns in monsters)
                        {
                            if (mns.Edge.Intersects(monster.Edge) && mns.getLocation() != monster.getLocation())
                            {
                                centreDis = new Vector2(mns.Edge.Center.X, mns.Edge.Center.Y) - monster.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                                if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move monster away depending on what side is further on collision
                                {
                                    monster.setLocation(new Vector2(centreDis.X * -0.125f + monster.getLocation().X, monster.getLocation().Y));
                                    mns.setLocation(new Vector2(centreDis.X * +0.25f + mns.getLocation().X, mns.getLocation().Y));
                                }
                                else
                                {
                                    monster.setLocation(new Vector2(monster.getLocation().X, centreDis.Y * -0.125f + monster.getLocation().Y));
                                    mns.setLocation(new Vector2(mns.getLocation().X, centreDis.Y * +0.25f + mns.getLocation().Y));
                                }
                            }
                        }
                        foreach (ExitDoor exit in Exits)
                        {
                            if (monster.Edge.Intersects(exit.Edge))
                            {
                                centreDis = new Vector2(exit.Edge.Center.X, exit.Edge.Center.Y) - monster.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                                if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move player away depending on what side is further on collision
                                {
                                    monster.setLocation(new Vector2(centreDis.X * -0.125f + monster.getLocation().X, monster.getLocation().Y));
                                }
                                else
                                {
                                    monster.setLocation(new Vector2(monster.getLocation().X, centreDis.Y * -0.125f + monster.getLocation().Y));
                                }
                            }
                        }
                    }
                        //checks every singe wall for a collision, inefficient but not intensive enough that it causes issues since the 1st check is a collision check
                        
                    
                    foreach (ExitDoor exit in Exits)
                    {
                        if (exit.Edge.Intersects(TheHero.Edge))
                        {
                            centreDis = new Vector2(exit.Edge.Center.X, exit.Edge.Center.Y) - TheHero.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move player away depending on what side is further on collision
                            {
                                TheHero.setLocation(new Vector2(centreDis.X * -0.125f + TheHero.getLocation().X, TheHero.getLocation().Y));
                            }
                            else
                            {
                                TheHero.setLocation(new Vector2(TheHero.getLocation().X, centreDis.Y * -0.125f + TheHero.getLocation().Y));
                            }
                        }
                    }
                    if (CurrentDimension == Dimension.D3)
                    {
                        walls3d.Clear();//clear wall list
                        foreach (var wall in walls)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = wall.Edge;
                            newObj.objectLocation = wall.getLocation();
                            newObj.name = 'W';
                            gameObjects.Add(newObj);
                        }
                        rayCast(660, 'W');
                        foreach (var monster in monsters)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = monster.Edge;
                            newObj.objectLocation = monster.getLocation();
                            newObj.name = 'M';
                            gameObjects.Add(newObj);
                        }
                        rayCast(400, 'M');
                        foreach (var exit in Exits)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = exit.Edge;
                            newObj.objectLocation = exit.getLocation();
                            newObj.name = 'E';
                            gameObjects.Add(newObj);
                        }
                        rayCast(200, 'E');
                        gameObjects.Clear();
                    }
                    if (TheHero.checkHealth() <= 0 || (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter)))
                    {
                        currentState = GameState.Dead;
                    }
                    break;
                case GameState.Dead:
                    levelGen = false;
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

        private void createEntities()
        {
            Random rnd = new Random();
            monsters.Clear();
            walls.Clear();
            Exits.Clear();
            int monstercount = 0;
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeHeight; j++)
                {
                    switch (maze[i, j])
                    {
                        case 0:
                            Wall newWall = new Wall(i, j);
                            newWall.LoadContent(Content);
                            walls.Add(newWall);
                            break;
                        case 2:
                            break;
                        case 3:
                            if (rnd.Next(2) == 1 && monstercount < monstermax)
                            {
                                Monster newMonster = new Monster(new Vector2(i * 40, j * 40), 1, 5 * (int)levelNumber);//spawn monster at end of corridor
                                newMonster.LoadContent(Content);
                                monsters.Add(newMonster);
                                monstercount++;
                            }
                            else
                            {
                                ExitDoor newDoor = new ExitDoor(new Vector2((i * 40) - 15, (j * 40) - 15));//spawn door at end of corridor and offset to centre of a tile
                                newDoor.LoadContent(Content);
                                Exits.Add(newDoor);
                            }
                            break;
                        default:
                            break;
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
                    spriteBatch.DrawString(GameFont, "shop", new Vector2(10, 10), Color.Black, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(GameFont, "example item 1", new Vector2(10, 40), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(GameFont, "example item 2", new Vector2(10, 60), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(GameFont, "upgrade health", new Vector2(10, 80), Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(GameFont, "Level: " + levelNumber, new Vector2(360, 360), Color.Black, 0f, new Vector2(0, 0), 2.5f, SpriteEffects.None, 0f);
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
                            for (int i = 0; i< monsters.Count; i++)
                            {
                                monsters[i].draw(spriteBatch);
                            }
                            for (int i = 0; i < Exits.Count; i++)
                            {
                                Exits[i].draw(spriteBatch);
                            }
                            //draw hero on top
                            TheHero.draw(spriteBatch);
                            TheRay.draw(spriteBatch);
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
                            StaminaBar.draw(spriteBatch);
                            HealthBar.draw(spriteBatch);
                            ShieldBar.draw(spriteBatch);
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
        public void rayCast(int castLength,char toHit)
        {
            for (int i = -TheHero.FOV; i < TheHero.FOV; i++)
            {
                char objHit = ' ';
                Vector2 centreDis = new Vector2(0, 0);
                Vector2 distanceTraveled = Ray.cast(i, TheHero, TheRay, gameObjects, ref centreDis, castLength, ref objHit, toHit);
                if (distanceTraveled != new Vector2(castLength, castLength))
                {
                    wall3d newSlice = new wall3d(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit); //wall3d.generate3dWall(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit);
                    newSlice.LoadContent(Content);
                    walls3d.Add(newSlice);

                }
            }
        }
        public bool shotsFired(double rotation, Vector2 loc)
        {
            float speed = 3f;
            TheRay.setLocation(loc);
            TheRay.rotation = rotation;
            TheRay.Movement = new Vector2(-speed * (float)Math.Sin(TheRay.rotation), speed * (float)Math.Cos(TheRay.rotation));
            while (true)
            {
                TheRay.setLocation(TheRay.getLocation() + TheRay.Movement);//move ray forward
                TheRay.update();//update hitbox
                foreach (Wall wall in walls)
                {
                    if (wall.Edge.Intersects(TheRay.Edge) || (Math.Abs(TheRay.getLocation().X - TheHero.getLocation().X) > 300 || Math.Abs(TheRay.getLocation().Y - TheHero.getLocation().Y) > 300))
                    {
                        return false;
                    }
                }
                foreach (ObjInGame Obj in gameObjects)//loop though all walls (ik its slow but its easy)
                {
                    if (Obj.objectEdge.Intersects(TheRay.Edge))//check collision with hitbox
                    {
                        return true;
                    }
                }
            }
        }
        public struct ObjInGame()//simplified list of all entities that will be in the game
        {
            public Rectangle objectEdge;//for raycasting
            public Vector2 objectLocation;
            public char name;//for when an object it to be searched for/hit
        }
    }
}