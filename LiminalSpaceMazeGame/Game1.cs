using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;
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
        stateClass startMenu;
        stateClass ShopMenu;
        stateClass deathMenu;
        UILoadingBar StaminaBar;
        UILoadingBar HealthBar;
        UILoadingBar ShieldBar;

        public int levelNumber = 0;
        public bool levelGen = false;


        List<Monster> monsters = new List<Monster>();

        List<Wall> walls = new List<Wall>();
        List<ObjInGame> gameObjects = new List<ObjInGame>();

        List<wall3d> walls3d = new List<wall3d>();
        List<Key> collectables = new List<Key>();

        public GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        List<ExitDoor> Exits = new List<ExitDoor>();

        List<stateButtons> stateButtonList = new List<stateButtons>();


        int[,] maze;
        int mazeHeight = 17;
        int mazeWidth = 17;

        bool toExit = false;

        char wallType = ' ';
        char[] availableWalls = { 'W', 'w', 'v' };
        Random rand = new Random();

        MouseState mouseState = Mouse.GetState();
        MouseState mouseState2 = Mouse.GetState();

        //states to switch game between its respective screens
        public enum GameState
        {
            StartMenu,
            Help,
            Settings,
            Leaderboard,
            Shop,
            InGame,
            Dead,
            win,
            pause,
            none
        }
        //states to switch logic between dimensions
        enum Dimension
        {
            D2,
            D3
        }
        public GameState currentState = GameState.StartMenu;
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
            StaminaBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 590f, TheUI.getLocation().Y + 20f), TheHero.StaminaMax, 120, 20);
            HealthBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 69f, TheUI.getLocation().Y+12f), TheHero.maxHealth, 69, 6);
            ShieldBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 69f, TheUI.getLocation().Y + 40f), TheHero.ShieldMax, 69, 86);
            startMenu = new stateClass();
            ShopMenu = new stateClass();
            deathMenu = new stateClass();

            stateButtonList.Add(new stateButtons(new Vector2( 85, 465), new Vector2(187, 82), GameState.StartMenu, 'b'));
            stateButtonList.Add(new stateButtons(new Vector2( 85, 580), new Vector2(187, 82), GameState.StartMenu, 'l'));
            stateButtonList.Add(new stateButtons(new Vector2(312, 465), new Vector2(187, 82), GameState.StartMenu, 's'));
            stateButtonList.Add(new stateButtons(new Vector2(312, 580), new Vector2(187, 82), GameState.StartMenu, 'h'));
            
            stateButtonList.Add(new stateButtons(new Vector2(283, 560), new Vector2(187, 82), GameState.Help, 'H'));
            //add new button script for size!
            /*startMenuButton newbut = new startMenuButton(new Vector2(283, 560), new Vector2(187, 82), GameState.Help, 'H');
            newbut.LoadContent(Content);
            startButtons.Add();*/
            stateButtonList.Add(new stateButtons(new Vector2(108, 535), new Vector2(220, 100), GameState.Dead, 'R'));
            stateButtonList.Add(new stateButtons(new Vector2(382, 535), new Vector2(220, 100), GameState.Dead, 'E'));
            /*stateButtonList.Add(newbut);ewbut = new stateButtons(new Vector2(283, 560), new Vector2(187, 82), GameState.Dead, 'E');

            newbut.LoadContent(Content, "exit");
            stateButtonList.Add(newbut);*/ //next button         aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
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
            startMenu.LoadContent(Content, "WelcomeScreen","HelpScreen");
            startMenu.LoadContent(Content, "", "nullVoidDead");
            deathMenu.LoadContent(Content, "Dead", "nullVoidDead");
        }

        protected override void Update(GameTime gameTime)
        {
            ks1 = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Rectangle mousePosition = new Rectangle(mouseState.X, mouseState.Y, 0, 0);
            switch (currentState)
            {
                case (GameState.StartMenu)://welcome user
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition)&& (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            switch (button.buttonAction)
                            {
                                case 'b':
                                    currentState = GameState.Shop;
                                    break;
                                case 'h':
                                    currentState = GameState.Help;
                                    startMenu.displayFg = true;
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case GameState.Help:
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            switch (button.buttonAction)
                            {
                                case 'H':
                                    startMenu.displayFg = false ;
                                    currentState = GameState.StartMenu;
                                    break;
                            }
                            break;
                        }
                        if (ks1.IsKeyDown(Keys.Escape) && startMenu.displayFg == true)
                        {
                            startMenu.displayFg = false;
                        }
                    }
                    break;
                case GameState.Shop:
                    toExit = false;
                    TheHero.rotation = 0f;
                    if (!levelGen)//generate 1 new level and wait
                    {
                        int vars = rand.Next(0,availableWalls.Length);
                        wallType = availableWalls[vars];
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
                    TheHero.collected.Clear();

                    break;
                case GameState.InGame:// run game code
                    // update all entities
                    TheHero.update();
                    StaminaBar.update(TheHero.Stamina);
                    HealthBar.update(TheHero.checkHealth());
                    ShieldBar.update(TheHero.shield);
                    TheHero.gainHealth(1);
                    foreach (Collectable col in collectables)
                    {
                        col.update();
                        if (col.Edge.Intersects(TheHero.Edge))
                        {
                            col.collect(TheHero);
                            Vector2 dis = new Vector2(col.Edge.Center.X, col.Edge.Center.Y) - TheHero.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                            if (Math.Abs(dis.X) > Math.Abs(dis.Y))//move player away depending on what side is further on collision
                            {
                                TheHero.setLocation(new Vector2(dis.X * -0.125f + TheHero.getLocation().X, TheHero.getLocation().Y));
                            }
                            else
                            {
                                TheHero.setLocation(new Vector2(TheHero.getLocation().X, dis.Y * -0.125f + TheHero.getLocation().Y));
                            }
                        }
                    }
                    
                    foreach (Monster monster in monsters)
                    {
                        monster.update(TheHero, maze);
                        Vector2 anglemath = monster.getLocation() - TheHero.getLocation();
                        double angle = Math.Atan(anglemath.X / -anglemath.Y);
                        if (monster.getLocation().Y > TheHero.getLocation().Y)
                        {
                            angle= angle+ 3.14;
                        }
                        
                        monster.rotation = +angle;
                        if (monster.rotation < 0)
                        {
                            monster.rotation = 3.14*2;
                        }
                        if (monster.rotation > 3.14 * 2)
                        {
                            monster.rotation = -(3.14 / 2);
                        }
                        

                    }
                    
                    foreach (var monster in monsters)
                    {
                        gameObjects.Clear();
                        ObjInGame newObj = new ObjInGame();
                        newObj.objectEdge = monster.Edge;
                        newObj.objectLocation = monster.getLocation();
                        newObj.name = 'M';
                        gameObjects.Add(newObj);
                        monster.lineOfSight = shotsFired(monster.rotation+3.14f, TheHero.getLocation());
                        bool shootable = shotsFired(TheHero.rotation, TheHero.getLocation());
                        
                        if ((mouseState.LeftButton == ButtonState.Pressed) && (mouseState2.LeftButton == ButtonState.Released) && shootable)
                        {

                            monster.loseHealth(TheHero.Damage);
                        }
                    }
                    gameObjects.Clear();
                    foreach (ExitDoor exitDoor in Exits)
                    {
                        toExit = exitDoor.update(TheHero.collected);
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
                            if (toExit)
                            {
                                currentState = GameState.win;
                            }
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
                            newObj.name = wallType;
                            gameObjects.Add(newObj);
                        }
                        rayCast(660, wallType);
                        foreach (var coll in collectables)
                        {
                            if (coll.CollectableType == 'K')
                            {
                                ObjInGame newObj = new ObjInGame();
                                newObj.objectEdge = coll.Edge;
                                newObj.objectLocation = coll.getLocation();
                                newObj.name = 'K';
                                gameObjects.Add(newObj);
                            }
                        }
                        rayCast(400, 'K');
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
                        rayCast(400, 'E');
                        
                        gameObjects.Clear();
                    }
                    if (TheHero.checkHealth() <= 0 || (ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter)))
                    {
                        currentState = GameState.Dead;
                    }
                    break;
                case GameState.Dead:
                    levelGen = false;
                    levelNumber = 0;
                    monsters.Clear();
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            switch (button.buttonAction)
                            {
                                case 'R':
                                    currentState = GameState.StartMenu;
                                    break;
                                case 'E':
                                    Exit();
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case GameState.win:
                    levelGen = false;
                    monsters.Clear();
                    currentState = GameState.Shop;
                    break;
                default:
                    break;
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
            mouseState2 = mouseState;

            base.Update(gameTime);
        }

        private void createEntities()
        {
            Random rnd = new Random();
            collectables.Clear();
            monsters.Clear();
            walls.Clear();
            Exits.Clear();
            //assignes amount of spawns of each type by using dead end cont
            int deadEndCount = 0;
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeHeight; j++)
                {
                    if (maze[i,j] == 3)
                    {
                        deadEndCount++;
                    }
                }
            }
            int exitMax = rnd.Next(1,deadEndCount);//ensures taht theres allways an exit to a level at the detriment to the monster count
            int keymax = exitMax+1;
            deadEndCount -= exitMax;
            int monsterMax = deadEndCount;
            if (deadEndCount>levelNumber)
            {
                 monsterMax = rnd.Next(levelNumber, deadEndCount);
            }
            int monsterCount = 0;
            int exitCount = 0;
            int keyCount = 0;
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
                            if (rnd.Next(0, 32 - i - j) == 1 && keyCount < keymax )//alg to define locations of keys, gets more common with furthur distance
                            {
                                Key newKey = new Key(new Vector2(i * 40, j * 40), 'K');
                                newKey.LoadContent(Content);
                                collectables.Add(newKey);
                                keyCount++;
                            }
                            break;
                        case 3:
                            if (exitCount == 0 || (rnd.Next(0,keymax-1) == 1 && exitCount < exitMax))//ensures 1 exit spawn
                            {
                                ExitDoor newDoor = new ExitDoor(new Vector2((i * 40) - 15, (j * 40) - 15));//spawn door at end of corridor and offset to centre of a tile
                                newDoor.LoadContent(Content);
                                Exits.Add(newDoor);
                                exitCount++;
                                maze[i, j] = 5;
                            }
                            else if (monsterCount == 0 || (rnd.Next(0, 4) == 1 && monsterCount < monsterMax))//ensures 1 monster after 1 exit
                            {
                                Monster newMonster = new Monster(new Vector2(i * 40, j * 40), 1, 5 * (int)levelNumber);//spawn monster at end of corridor
                                newMonster.LoadContent(Content);
                                monsters.Add(newMonster);
                                monsterCount++;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            if (keyCount == 0)//garuntee a key
            {
                Key newKey = new Key(new Vector2(40,120), 'K');
                newKey.LoadContent(Content);
                collectables.Add(newKey);
                keyCount++;
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
                    startMenu.draw(spriteBatch);
                    break;
                case GameState.Help:
                    this.IsMouseVisible = true;
                    startMenu.draw(spriteBatch);
                    
                    break;

                case GameState.Shop:
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
                            for (int i = 0; i < collectables.Count; i++)
                            {
                                collectables[i].draw(spriteBatch);
                            }
                            //draw hero on top
                            TheHero.draw(spriteBatch);
                            TheRay.draw(spriteBatch);
                            break;
                        case Dimension.D3://3d representation
                            List<wall3d> CURRENT = new List<wall3d>();
                            foreach (wall3d wall in walls3d)
                            {
                                if (wall.type == wallType)
                                {
                                    wall.draw(spriteBatch);
                                }
                                CURRENT.Add(wall);
                            }
                            foreach (wall3d wall in walls3d)
                            {
                                if (wall.type != wallType && wall.type != 'M')
                                {
                                    wall.draw(spriteBatch);
                                }
                                CURRENT.Add(wall);
                            }
                            foreach (wall3d wall in walls3d)
                            {
                                if (wall.type != wallType)
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
                    deathMenu.draw(spriteBatch);
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
                Vector2 distanceTraveled = Ray.cast(i, TheHero, TheRay, gameObjects, ref centreDis, castLength, ref objHit, toHit,wallType);
                if (distanceTraveled != new Vector2(castLength, castLength))
                {
                    wall3d newSlice = new wall3d(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit); //wall3d.generate3dWall(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit);
                    newSlice.LoadContent(Content,toExit);
                    walls3d.Add(newSlice);
                }
            }
        }
        public bool shotsFired(double rotation, Vector2 loc)
        {
            float speed = 1f;
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