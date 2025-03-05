using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Threading;
using System.IO;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework.Audio;
using System.Xml.Linq;
using System.Linq;


namespace LiminalSpaceMazeGame
{
    //test push
    public class Game1 : Game
    {
        //activate object classes
        KeyboardState ks1, ks2;
        Hero TheHero;
        Ray TheRay;
        UI TheUI;
        UI crosshair;
        UI keyUI;
        UI levelDisplay;

        //music classes
        Song mainSong;
        bool playsong = true;
        bool playfx = true;
        bool playing = false;

        //menu and ui classes
        GenerateMaze TheMaze;
        SpriteFont GameFont;
        stateClass startMenu;
        stateClass ShopMenu;
        stateClass SettingsMenu;
        stateClass leaderboardMenu;
        stateClass deathMenu;
        UILoadingBar StaminaBar;
        UILoadingBar HealthBar;
        UILoadingBar ShieldBar;
        UILoadingBar MonsterHealthBar;

        UILoadingBar sensitivityBar;

        //level info
        public int levelNumber = 1;
        public bool levelGen = false;

        //object lists
        List<Monster> monsters = new List<Monster>();

        List<Wall> walls = new List<Wall>();
        List<ObjInGame> gameObjects = new List<ObjInGame>();

        List<Wall3d> walls3d = new List<Wall3d>();
        List<Collectable> collectables = new List<Collectable>();

        public GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;

        List<ExitDoor> Exits = new List<ExitDoor>();

        List<stateButtons> stateButtonList = new List<stateButtons>();

        List<AudioSound> soundEffects = new List<AudioSound>();

        //maze cretion
        int[,] maze;
        int mazeHeight = 17;
        int mazeWidth = 17;

        //sensitivity items
        int senseBar = 33;
        bool toExit = false;
        bool nameChange = false;
        int countToChange = 0;

        //maze walls
        char wallType = ' ';
        char[] availableWalls = { 'W', 'w', 'v' };
        Random rand = new Random();
        List<NameValue> Playerleaderboards = new List<NameValue>();
        public List<NameValue> ShopItems = new List<NameValue>();

        //monster default health
        int monsterHealth = 1;
        //default key state
        Keys down = Keys.None;

        //default mouse states
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
            level,
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
        //default game and dimension states
        public GameState currentState = GameState.Settings;
        Dimension CurrentDimension = Dimension.D3;
        //screen
        Vector2 gameResolution = new Vector2(720, 720);

        public Game1()
        {
            //keyboard defaults
            down = Keys.None;
            ks1 = Keyboard.GetState();
            ks2 = Keyboard.GetState();
            this.IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            //change screen size
            _graphics.PreferredBackBufferWidth = (int)gameResolution.X;
            _graphics.PreferredBackBufferHeight = (int)gameResolution.Y;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            //create shop items
            NameValue newShopItem = new NameValue();
            newShopItem.name = "Stamina Cap";
            newShopItem.value = 20;
            newShopItem.used = 1;
            ShopItems.Add(newShopItem);
            newShopItem.name = "Health Cap";
            newShopItem.value = 50;
            newShopItem.used = 1;
            ShopItems.Add(newShopItem);
            newShopItem.name = "Shield Fill";
            newShopItem.value = 40;
            newShopItem.used = 1;
            ShopItems.Add(newShopItem);
        }
        protected override void Initialize()
        {
            //leaderboard
            StreamWriter sw = new StreamWriter(@"playerLeaderboard.txt", true);//creates player file if its the 1st boot
            sw.WriteLine("Jos, 2050");//default playrs to beat the score of
            sw.WriteLine("tim, 700");
            sw.WriteLine("Dal, 400");
            sw.Close();
            // read in leaderboard
            StreamReader sr = new StreamReader("playerLeaderboard.txt");
            for (int i = 0; i < 3; i++)
            {
                string[] readin = sr.ReadLine().Split(',');
                NameValue newItem = new NameValue();
                newItem.name = readin[0];
                newItem.value = int.Parse(readin[1]);
                Playerleaderboards.Add(newItem);
            }
            sr.Close();
            sr.Dispose();


            //create hero and maze object
            TheHero = new Hero(90, 1000, 100);
            TheMaze = new GenerateMaze();
            TheRay = new Ray();
            TheUI = new UI(new Vector2(0, 670), "ui");
            crosshair = new UI(new Vector2(340, 340), "crosshair");
            keyUI = new UI(new Vector2(TheUI.getLocation().X + 539f, TheUI.getLocation().Y + 5f), "key");
            levelDisplay = new UI(new Vector2(0, -720), "Level screen");
            StaminaBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 595f, TheUI.getLocation().Y + 20f), TheHero.StaminaMax, 112, 24, Color.Green);
            HealthBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 59f, TheUI.getLocation().Y + 18f), TheHero.maxHealth, 94, 27, Color.Red);
            ShieldBar = new UILoadingBar(new Vector2(TheUI.getLocation().X + 165F, TheUI.getLocation().Y + 18f), TheHero.ShieldMax, 94, 27, Color.Blue);
            MonsterHealthBar = new UILoadingBar(new Vector2(crosshair.getLocation().X, crosshair.getLocation().Y + 25f), monsterHealth, 20, 6, Color.Red);
            sensitivityBar = new UILoadingBar(new Vector2(185, 170), 64, 350, 101, Color.OrangeRed);
            startMenu = new stateClass();
            ShopMenu = new stateClass();
            SettingsMenu = new stateClass();
            leaderboardMenu = new stateClass();
            deathMenu = new stateClass();


            //create button objects

            //add new button script for size!
            /*
            startMenuButton newbut = new startMenuButton(new Vector2(283, 560), new Vector2(187, 82), GameState.Help, 'H');
            newbut.LoadContent(Content);
            startButtons.Add();
            */

            //main menu
            stateButtonList.Add(new stateButtons(new Vector2(85, 465), new Vector2(187, 82), GameState.StartMenu, 'b'));
            stateButtonList.Add(new stateButtons(new Vector2(85, 580), new Vector2(187, 82), GameState.StartMenu, 's'));
            stateButtonList.Add(new stateButtons(new Vector2(312, 465), new Vector2(187, 82), GameState.StartMenu, 'l'));
            stateButtonList.Add(new stateButtons(new Vector2(312, 580), new Vector2(187, 82), GameState.StartMenu, 'h'));

            //help menu
            stateButtonList.Add(new stateButtons(new Vector2(283, 560), new Vector2(187, 82), GameState.Help, 'H'));

            //dead
            stateButtonList.Add(new stateButtons(new Vector2(108, 535), new Vector2(220, 100), GameState.Dead, 'R'));
            stateButtonList.Add(new stateButtons(new Vector2(382, 535), new Vector2(220, 100), GameState.Dead, 'E'));
            //shop
            stateButtonList.Add(new stateButtons(new Vector2(528, 246), new Vector2(156, 64), GameState.Shop, 'S'));
            stateButtonList.Add(new stateButtons(new Vector2(528, 346), new Vector2(156, 64), GameState.Shop, 'H'));
            stateButtonList.Add(new stateButtons(new Vector2(528, 456), new Vector2(156, 64), GameState.Shop, 's'));
            stateButtonList.Add(new stateButtons(new Vector2(545, 570), new Vector2(175, 150), GameState.Shop, 'E'));

            //settings
            stateButtonList.Add(new stateButtons(new Vector2(70, 175), new Vector2(100, 100), GameState.Settings, 'S'));
            stateButtonList.Add(new stateButtons(new Vector2(545, 175), new Vector2(100, 100), GameState.Settings, 's'));
            stateButtonList.Add(new stateButtons(new Vector2(250, 598), new Vector2(220, 100), GameState.Settings, 'E'));
            stateButtonList.Add(new stateButtons(new Vector2(275, 360), new Vector2(220, 90), GameState.Settings, 'N'));
            stateButtonList.Add(new stateButtons(new Vector2(545, 353), new Vector2(85, 85), GameState.Settings, 'C'));
            stateButtonList.Add(new stateButtons(new Vector2(150, 485), new Vector2(200, 100), GameState.Settings, 'M'));
            stateButtonList.Add(new stateButtons(new Vector2(385, 485), new Vector2(200, 100), GameState.Settings, 'm'));

            //leaderboard
            stateButtonList.Add(new stateButtons(new Vector2(250, 598), new Vector2(220, 100), GameState.Leaderboard, 'E'));
            //end of buttons
            base.Initialize();
        }
        protected override void LoadContent()
        {
            //load all files in
            //textures
            spriteBatch = new SpriteBatch(GraphicsDevice);
            TheHero.LoadContent(Content);
            TheUI.LoadContent(Content);
            keyUI.LoadContent(Content);
            keyUI.drawUI = false;
            levelDisplay.LoadContent(Content);
            GameFont = Content.Load<SpriteFont>(@"File");
            StaminaBar.LoadContent(Content, GraphicsDevice);
            HealthBar.LoadContent(Content, GraphicsDevice);
            ShieldBar.LoadContent(Content, GraphicsDevice);
            MonsterHealthBar.LoadContent(Content, GraphicsDevice);
            sensitivityBar.LoadContent(Content, GraphicsDevice);
            startMenu.LoadContent(Content, "WelcomeScreen", "HelpScreen");
            ShopMenu.LoadContent(Content, "The Shop", "nullVoidDead");
            SettingsMenu.LoadContent(Content, "Settings", "nullVoidDead");
            leaderboardMenu.LoadContent(Content, "Leaderboard", "nullVoidDead");
            deathMenu.LoadContent(Content, "Dead", "nullVoidDead");
            crosshair.LoadContent(Content);
            //audio
            mainSong = Content.Load<Song>("dark-ambient");
            AudioSound newAudio = new AudioSound("select");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            newAudio = new AudioSound("coin2");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio); 
            newAudio = new AudioSound("pickupCoin");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            newAudio = new AudioSound("wrong");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            newAudio = new AudioSound("correct");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            newAudio = new AudioSound("explosion");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            //6
            newAudio = new AudioSound("roar");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            //7
            newAudio = new AudioSound("hurt");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            //8
            newAudio = new AudioSound("died");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            //9
            newAudio = new AudioSound("level complete");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
            //10
            newAudio = new AudioSound("no Ammo");
            newAudio.loadContent(Content);
            soundEffects.Add(newAudio);
        }

        protected override void Update(GameTime gameTime)
        {
            // main update routine, handles music then game states and keyboard/ mouse inputs
            if (playsong && !playing)//play or pause song depending on settings and sets the music to repeat if it is playing
            {
                MediaPlayer.Play(mainSong);
                MediaPlayer.IsRepeating = true;
                playing = true;
            }
            else if (!playsong) 
            {
                MediaPlayer.Pause();
                playing = false;
            }
            //get the keyboard and mouse state for the game
            ks1 = Keyboard.GetState();
            mouseState = Mouse.GetState();
            Rectangle mousePosition = new Rectangle(mouseState.X, mouseState.Y, 0, 0);//gets an entity of the mouse position to compare to the edges of buttons
            switch (currentState)//switches between game menus and in game screen defaults and switches off of a how did we get here screen (coz its literarily impossible to get here lol)
            {
                case GameState.StartMenu://welcome user and allow user to switch between menus
                    foreach (stateButtons button in stateButtonList)
                    {
                        // only activates button if you have pressed it once and your mouse was hovering over it
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            soundEffects[0].play(playfx);
                            switch (button.buttonAction)//switch between all button actions
                            {
                                case 'b':
                                    currentState = GameState.Shop;
                                    break;
                                case 'h':
                                    currentState = GameState.Help;
                                    startMenu.displayFg = true;
                                    break;
                                case 'l':
                                    currentState = GameState.Leaderboard;
                                    break;
                                case 's':
                                    currentState = GameState.Settings;
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case GameState.Help://help menu on top of main menu
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            soundEffects[0].play(playfx);
                            switch (button.buttonAction)
                            {
                                case 'H'://disable help menu and send back to start menu
                                    startMenu.displayFg = false;
                                    currentState = GameState.StartMenu;
                                    break;
                            }
                            break;
                        }
                        if (ks1.IsKeyDown(Keys.Escape) && startMenu.displayFg == true)// if escape is pressed then disable help menu
                        {
                            startMenu.displayFg = false;
                        }
                    }
                    break;
                case GameState.Settings:
                    sensitivityBar.update(senseBar);
                    foreach (stateButtons button in stateButtonList)//button switcher for levels with buttons activated
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed))// re enter settings menu
                        {
                            //activate settings
                            switch (button.buttonAction)
                            {
                                case 'E'://exit button
                                    if (mouseState2.LeftButton == ButtonState.Released && TheHero.name.Length>0)//cheks if name is correct lengths to exit
                                    {
                                        soundEffects[0].play(playfx);

                                        currentState = GameState.StartMenu;
                                    }
                                    else
                                    {
                                        soundEffects[3].play(playfx);
                                    }
                                    break;
                                case 's'://edit sensitivity
                                    if (TheHero.sensitivity > TheHero.senseMax)
                                    {
                                        senseBar++;
                                        TheHero.sensitivity -= 1;
                                    }
                                    break;
                                case 'S':
                                    if (TheHero.sensitivity < TheHero.senseMin)
                                    {
                                        senseBar--;
                                        TheHero.sensitivity += 1;
                                    }
                                    break;
                                case 'N':
                                    if (mouseState2.LeftButton != ButtonState.Pressed)//enable name editing
                                    {
                                        soundEffects[0].play(playfx);
                                    }
                                    nameChange = true;
                                    break;
                                case 'M':
                                    if (mouseState2.LeftButton != ButtonState.Pressed)//mute music
                                    {
                                        soundEffects[0].play(playfx);

                                        playsong = !playsong;
                                    }
                                    break;
                                case 'm':
                                    if (mouseState2.LeftButton != ButtonState.Pressed)//mute sound effects
                                    {
                                        soundEffects[0].play(playfx);
                                        playfx = !playfx;
                                    }
                                    break;
                                case 'C':
                                    if (mouseState2.LeftButton != ButtonState.Pressed)//disable name edditing
                                    {
                                        soundEffects[4].play(playfx);
                                        nameChange = false;
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                    if (nameChange)
                    {
                        string safe = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";//allowable letters 

                        if (ks1.IsKeyDown(Keys.Back) && ks2.IsKeyUp(Keys.Back) && TheHero.name.Length > 0)//remove letters
                        {
                            TheHero.name = TheHero.name.Remove(TheHero.name.Length - 1);
                        }
                        else if (TheHero.name.Length < 3 && (ks1.IsKeyDown(down) == !ks2.IsKeyDown(down))&& safe.Contains(down.ToString()))//add letters
                        {
                            TheHero.name = TheHero.name + down.ToString();
                        }
                        Keys[] KeyLogs = ks1.GetPressedKeys();


                        if (KeyLogs.Length > 0)//dont allow held down inputs
                        {
                            down = KeyLogs[0];
                        }

                    }
                    break;
                case GameState.Leaderboard:
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released) && button.buttonAction == 'E')
                        {
                            soundEffects[0].play(playfx);
                            currentState = GameState.StartMenu;
                        }
                    }
                    break;
                case GameState.Shop:
                    toExit = false;
                    TheHero.rotation = 0f;
                    foreach (stateButtons button in stateButtonList)
                    {
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            switch (button.buttonAction)
                            {
                                case 'E'://shop buttons and exit button
                                    soundEffects[0].play(playfx);
                                    currentState = GameState.level;
                                    break;
                                case 'S':
                                    ShopItems[0] = TheHero.Upgrade(ShopItems[0], levelNumber,soundEffects, playfx);//purchase item
                                    break;
                                case 'H':
                                    ShopItems[1] = TheHero.Upgrade(ShopItems[1], levelNumber, soundEffects, playfx);
                                    break;
                                case 's':
                                    ShopItems[2] = TheHero.Upgrade(ShopItems[2], levelNumber, soundEffects , playfx);
                                    break;
                            }
                            break;
                        }
                    }
                    break;
                case GameState.level:
                    if (!levelGen)//generate 1 new level and wait
                    {
                        int vars = rand.Next(0, availableWalls.Length);
                        wallType = availableWalls[vars];
                        maze = TheMaze.GenerateNewMaze(mazeWidth, mazeHeight);
                        maze[1, 1] = 1;
                        createEntities();
                        TheHero.spawn(new Vector2(40, 40));//put the hero back at its spawn location
                        levelGen = true;
                    }
                    countToChange += 2;
                    if (countToChange > 720)//animation
                    {
                        levelDisplay.setLocation(new Vector2(0, -720));
                        countToChange = 0;
                        currentState = GameState.InGame;
                    }
                    levelDisplay.setLocation(new Vector2(levelDisplay.getLocation().X, levelDisplay.getLocation().Y + 2));
                    walls3d.Clear();
                    break;
                case GameState.InGame:// run game code
                    // update all entities
                    TheHero.update();
                    if (ks1.IsKeyDown(Keys.NumPad2))//stop player movement if the map is open
                    {
                        CurrentDimension = Dimension.D2;
                    }
                    else
                    {
                        CurrentDimension = Dimension.D3;
                        TheHero.move();
                    }
                    StaminaBar.update(TheHero.Stamina, TheHero.StaminaMax);//print ui bars
                    HealthBar.update(TheHero.checkHealth(), TheHero.maxHealth);
                    ShieldBar.update(TheHero.shield, TheHero.ShieldMax);
                    TheHero.gainHealth(1);
                    Vector2 centreDis;
                    foreach (Collectable col in collectables)
                    {
                        col.update();
                        if (col.Edge.Intersects(TheHero.Edge))
                        {
                            col.collect(TheHero);
                            soundEffects[2].play(playfx);
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
                    foreach (var monster in monsters)//update monsters and check if they can see the player/ rotate them too
                    {
                        monster.update(TheHero);
                        Vector2 anglemath = monster.getLocation() - TheHero.getLocation();
                        double angle = Math.Atan(anglemath.X / -anglemath.Y);
                        if (monster.getLocation().Y > TheHero.getLocation().Y)
                        {
                            angle = angle + 3.14;
                        }
                        monster.rotation = +angle;//rotate monster
                        if (monster.rotation < 0)
                        {
                            monster.rotation = 3.14 * 2;
                        }
                        if (monster.rotation > 3.14 * 2)
                        {
                            monster.rotation = -(3.14 / 2);
                        }
                        gameObjects.Clear();
                        ObjInGame newObj = new ObjInGame();
                        newObj.objectEdge = monster.Edge;
                        newObj.objectLocation = monster.getLocation();
                        newObj.name = 'M';
                        gameObjects.Add(newObj);
                        if (monster.lineOfSight == false && monster.lineOfSight != shotsFired(monster.rotation + 3.14f, TheHero.getLocation(), 200))
                        {
                            soundEffects[6].play(playfx);
                        }
                        monster.lineOfSight = shotsFired(monster.rotation + 3.14f, TheHero.getLocation(), 200);//check line of sight
                        monster.shootable = shotsFired(TheHero.rotation, TheHero.getLocation(), 200);
                        MonsterHealthBar.update(monster.gethealth(), monster.gethealthMax());
                        if ((mouseState.LeftButton == ButtonState.Pressed) && (mouseState2.LeftButton == ButtonState.Released))
                        {
                            if (TheHero.Stamina >= 100)//player shooting
                            {
                                soundEffects[5].play(playfx);

                                TheHero.Stamina -= 100;
                                if (monster.shootable)
                                {
                                    monster.loseHealth(TheHero.Damage);
                                    if (monster.gethealth() < 0)
                                    {
                                        soundEffects[7].play(playfx);
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                soundEffects[10].play(playfx);
                            }
                            
                        }
                        
                    }
                    gameObjects.Clear();
                    foreach (ExitDoor exitDoor in Exits)//exits check if they should end a level
                    {
                        toExit = exitDoor.update(TheHero.collected);
                    }
                    
                    foreach (Wall wall in walls)//check all wall intersections
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
                    foreach (Monster monster in monsters)//check all monster intersections
                    {
                        monster.update();
                        if (!monster.dead)
                        {
                            if (monster.Edge.Intersects(TheHero.Edge))
                            {
                                centreDis = TheHero.getLocation() - monster.getLocation();//make variable to determine the vector distance away from the center of  the wall in question                                                                                  
                                if (Math.Abs(centreDis.X) > Math.Abs(centreDis.Y))//move monster away depending on what side is further on collision
                                {
                                    monster.setLocation(new Vector2(centreDis.X * -0.25f*monster.Speed + monster.getLocation().X, monster.getLocation().Y));
                                    TheHero.setLocation(new Vector2(centreDis.X * +0.125f + TheHero.getLocation().X, TheHero.getLocation().Y));
                                }
                                else
                                {
                                    monster.setLocation(new Vector2(monster.getLocation().X, centreDis.Y * -0.25f* monster.Speed + monster.getLocation().Y));
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
                    if (CurrentDimension == Dimension.D3)//raycast all objects in order if in 3d mode
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
                        foreach (var coll in collectables)
                        {
                            if (coll.CollectableType == 'C')
                            {
                                ObjInGame newObj = new ObjInGame();
                                newObj.objectEdge = coll.Edge;
                                newObj.objectLocation = coll.getLocation();
                                newObj.name = 'C';
                                gameObjects.Add(newObj);
                            }
                        }
                        rayCast(400, 'C');
                        foreach (var exit in Exits)
                        {
                            ObjInGame newObj = new ObjInGame();
                            newObj.objectEdge = exit.Edge;
                            newObj.objectLocation = exit.getLocation();
                            newObj.name = 'E';
                            gameObjects.Add(newObj);
                        }
                        rayCast(400, 'E');
                        foreach (var monster in monsters)
                        {
                            if (!monster.dead)
                            {
                                ObjInGame newObj = new ObjInGame();
                                newObj.objectEdge = monster.Edge;
                                newObj.objectLocation = monster.getLocation();
                                newObj.name = 'M';
                                gameObjects.Add(newObj);
                            }
                        }
                        rayCast(400, 'M');

                        gameObjects.Clear();
                    }
                    if (TheHero.checkHealth() <= 0 || ks1.IsKeyDown(Keys.Enter) && ks2.IsKeyUp(Keys.Enter))//check if player is dead
                    {
                        //create leaderboard entry
                        TheHero.points = TheHero.points*levelNumber;
                        soundEffects[8].play(playfx);
                        NameValue newinput = new NameValue();
                        newinput.name = TheHero.name;
                        newinput.value = TheHero.points;
                        Playerleaderboards.Insert(0, newinput);
                        //if leaderboard is empty fill it
                        for (int i = Playerleaderboards.Count; i < 3; i++)
                        {
                            newinput.name = "null";
                            newinput.value = 0;
                            Playerleaderboards.Add(newinput);
                        }
                        //sort leaderboard
                        for (int i = 0; i < Playerleaderboards.Count - 1; i++)
                        {
                            for (int j = 0; j < Playerleaderboards.Count - i - 1; j++)
                            {
                                if (Playerleaderboards[j].value <= Playerleaderboards[j + 1].value)
                                {
                                    (Playerleaderboards[j + 1], Playerleaderboards[j]) = (Playerleaderboards[j], Playerleaderboards[j + 1]);
                                }
                            }
                        }
                        //reset shop
                        ShopItems.Clear();
                        NameValue newShopItem = new NameValue();
                        newShopItem.name = "Stamina Cap";
                        newShopItem.value = 20;
                        newShopItem.used = 1;
                        ShopItems.Add(newShopItem);
                        newShopItem.name = "Health Cap";
                        newShopItem.value = 50;
                        newShopItem.used = 1;
                        ShopItems.Add(newShopItem);
                        newShopItem.name = "Shield Fill";
                        newShopItem.value = 40;
                        newShopItem.used = 1;
                        ShopItems.Add(newShopItem);
                        StreamWriter sw = new StreamWriter(@"playerLeaderboard.txt", false);
                        //save leaderboard file
                        for (int i = 0; i < 3; i++)
                        {
                            sw.WriteLine(Playerleaderboards[i].name + "," + Playerleaderboards[i].value);
                        }
                        //dispose
                        sw.Close();
                        sw.Dispose();
                        //change to dead state
                        currentState = GameState.Dead;
                    }
                    break;
                case GameState.Dead:
                    //reset level num
                    levelGen = false;
                    levelNumber = 1;
                    
                    monsters.Clear();
                    foreach (stateButtons button in stateButtonList)
                    {
                        //wait for user to press a button
                        if (button.activeState == currentState && button.Edge.Intersects(mousePosition) && (mouseState.LeftButton == ButtonState.Pressed && mouseState2.LeftButton == ButtonState.Released))
                        {
                            switch (button.buttonAction)
                            {
                                case 'R':

                                    TheHero.reset();
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
                    //set up next level
                    levelGen = false;
                    levelNumber++;
                    monsters.Clear();
                    TheHero.collected.Clear();
                    soundEffects[9].play(playfx);
                    currentState = GameState.Shop;
                    break;
                default:
                    break;
            }
            //update click to check for held down buttons
            mouseState2 = mouseState;
            ks2 = ks1;

            base.Update(gameTime);
        }


        private void createEntities()
        {
            Random rnd = new Random();
            //clear all entities
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
                    if (maze[i, j] == 3)
                    {
                        deadEndCount++;
                    }
                }
            }
            int exitMax = rnd.Next(1, deadEndCount);//ensures taht theres allways an exit to a level at the detriment to the monster count
            int keymax = exitMax + 1;//maximum amount of keys
            deadEndCount -= exitMax;
            int monsterMax = deadEndCount;
            if (deadEndCount > levelNumber)
            {
                monsterMax = rnd.Next(levelNumber, deadEndCount);
            }
            int monsterCount = 0;
            int exitCount = 0;
            int keyCount = 0;
            int coincount = 0;
            int coinMax = levelNumber + 1 / 2;
            for (int i = 0; i < mazeWidth; i++)//loop through all maze locations
            {
                for (int j = 0; j < mazeHeight; j++)
                {
                    switch (maze[i, j])
                    {
                        case 0://place walls
                            Wall newWall = new Wall(i, j);
                            newWall.LoadContent(Content);
                            walls.Add(newWall);
                            break;
                        case 2:
                            if (rnd.Next(0, 32 - i - j) == 1 && keyCount < keymax)//alg to define locations of keys, gets more common with furthur distance
                            {
                                Key newKey = new Key(new Vector2(i * 40, j * 40), 'K');
                                newKey.LoadContent(Content);
                                collectables.Add(newKey);
                                keyCount++;
                                break;// break so nothing else can spawn on that tile
                            }
                            if (rnd.Next(0, 32 - i - j) > 5 && coincount < coinMax && new Vector2(i, j) != new Vector2(1, 3))//alg to define locations of coins, do not need to guarantee a key
                            {
                                CoinItem coin = new CoinItem(new Vector2(i * 40, j * 40), 'C');
                                coin.LoadContent(Content);
                                collectables.Add(coin);
                                coincount++;
                            }
                            break;
                        case 3://place monsters and exits with a priority on exits
                            if (exitCount == 0 || (rnd.Next(0, keymax - 1) == 1 && exitCount < exitMax))//ensures 1 exit spawn
                            {
                                ExitDoor newDoor = new ExitDoor(new Vector2((i * 40) - 15, (j * 40) - 15));//spawn door at end of corridor and offset to centre of a tile
                                newDoor.LoadContent(Content);
                                Exits.Add(newDoor);
                                exitCount++;
                                maze[i, j] = 5;
                            }
                            else if (monsterCount == 0 || (rnd.Next(0, 4) == 1 && monsterCount < monsterMax))//ensures 1 monster after 1 exit
                            {
                                Monster newMonster = new Monster(new Vector2(i * 40, j * 40), 1, levelNumber * levelNumber,maze);//spawn monster at end of corridor
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
            if (keyCount == 0)//guarantee 1 key
            {
                Key newKey = new Key(new Vector2(40, 120), 'K');
                newKey.LoadContent(Content);
                collectables.Add(newKey);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentState)//draw according to what menu the game is on. set mouse visable or not from the level
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
                    this.IsMouseVisible = true;
                    //background
                    ShopMenu.draw(spriteBatch);
                    //points
                    spriteBatch.DrawString(GameFont, TheHero.points.ToString(), new Vector2(535,45), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    //shop items
                    spriteBatch.DrawString(GameFont, ShopItems[0].name, new Vector2(12, 235), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, (ShopItems[0].value * ShopItems[0].used * levelNumber).ToString(), new Vector2(345, 235), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, ShopItems[1].name, new Vector2(12, 335), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1); 
                    spriteBatch.DrawString(GameFont, (ShopItems[1].value * ShopItems[1].used * levelNumber).ToString(), new Vector2(345, 335), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, ShopItems[2].name, new Vector2(12, 435), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, (ShopItems[2].value * ShopItems[2].used * levelNumber).ToString(), new Vector2(345, 435), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    break;
                case GameState.level:
                    this.IsMouseVisible = true;
                    levelDisplay.draw(spriteBatch);
                    spriteBatch.DrawString(GameFont, levelNumber.ToString(), new Vector2(levelDisplay.getLocation().X + 480, levelDisplay.getLocation().Y + 1144), Color.White, 0f, new Vector2(1, 1), 2f, SpriteEffects.None, 1);
                    
                    break;
                case GameState.Settings:
                    this.IsMouseVisible = true;
                    SettingsMenu.draw(spriteBatch);
                    sensitivityBar.draw(spriteBatch);
                    if (TheHero.name != null)
                    {
                        spriteBatch.DrawString(GameFont, TheHero.name, new Vector2(290, 375), Color.Black, 0f, new Vector2(1, 1), 1.75f, SpriteEffects.None, 1);
                    }
                    if (playsong)
                    {
                        spriteBatch.DrawString(GameFont, "music", new Vector2(180, 500), Color.Green, 0f, new Vector2(1, 1), 2.25f, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.DrawString(GameFont, "music", new Vector2(180, 500), Color.Red, 0f, new Vector2(1, 1), 2.25f, SpriteEffects.None, 1);
                    }
                    if (playfx)
                    {
                        spriteBatch.DrawString(GameFont, "fx", new Vector2(400, 500), Color.Green, 0f, new Vector2(1, 1), 2.25f, SpriteEffects.None, 1);
                    }
                    else
                    {
                        spriteBatch.DrawString(GameFont, "fx", new Vector2(400, 500), Color.Red, 0f, new Vector2(1, 1), 2.25f, SpriteEffects.None, 1);
                    }
                    break;
                case GameState.Leaderboard:
                    this.IsMouseVisible = true;
                    //display leaderbard
                    leaderboardMenu.draw(spriteBatch);
                    spriteBatch.DrawString(GameFont, Playerleaderboards[0].name + " " + Playerleaderboards[0].value.ToString(), new Vector2(310, 205), Color.Gold, 0f, new Vector2(1, 1), 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, Playerleaderboards[1].name + " " + Playerleaderboards[1].value.ToString(), new Vector2(310, 320), Color.Silver, 0f, new Vector2(1, 1), 1.5f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(GameFont, Playerleaderboards[2].name + " " + Playerleaderboards[2].value.ToString(), new Vector2(310, 435), Color.OrangeRed, 0f, new Vector2(1, 1), 1.5f, SpriteEffects.None, 1);
                    break;
                case GameState.InGame:
                    this.IsMouseVisible = false;
                    GraphicsDevice.Clear(Color.Gray);
                    keyUI.drawUI = false;
                    foreach (char item in TheHero.collected)
                    {
                        if (item == 'K')
                        {
                            keyUI.drawUI = true;
                        }
                    }
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
                            for (int i = 0; i < walls3d.Count - 1; i++)//make a list of objects closer and further from the player
                            {
                                for (int j = 0; j < walls3d.Count - i - 1; j++)
                                {
                                    if (walls3d[j].distanceFromHero < walls3d[j + 1].distanceFromHero)
                                    {
                                        (walls3d[j + 1], walls3d[j]) = (walls3d[j], walls3d[j + 1]);
                                    }
                                }
                            }
                            foreach (Wall3d wall in walls3d)//draw any wall tile
                            {
                                wall.draw(spriteBatch);
                            }
                            //draw all other items on top
                            TheUI.draw(spriteBatch);
                            crosshair.draw(spriteBatch);
                            StaminaBar.draw(spriteBatch);
                            HealthBar.draw(spriteBatch);
                            ShieldBar.draw(spriteBatch);
                            //draw monster health bar if you can see one
                            MonsterHealthBar.display = false;

                            foreach (Monster monster in monsters)
                            {
                                if(monster.shootable == true)
                                {
                                    MonsterHealthBar.display = true;
                                    break;
                                }
                            }
                            MonsterHealthBar.draw(spriteBatch);
                            //draw key on inventory if available
                            keyUI.draw(spriteBatch);
                            spriteBatch.DrawString(GameFont, (TheHero.points).ToString(), new Vector2(TheUI.getLocation().X + 386f, TheUI.getLocation().Y + 19f), Color.Black);

                            break;
                    }
                    break;

                case GameState.Dead:
                    //draw deathscreen
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
        public void rayCast(int castLength, char toHit)//set up each ray to cast and then create a new slice
        {
            for (int i = -TheHero.FOV; i < TheHero.FOV; i++)
            {
                char objHit = ' ';
                Vector2 centreDis = new Vector2(0, 0);
                Vector2 distanceTraveled = Ray.Cast(i, TheHero, TheRay, gameObjects, ref centreDis, castLength, ref objHit, toHit, wallType);//process new cast distance
                if (distanceTraveled != new Vector2(castLength, castLength))
                {
                    Wall3d newSlice = new Wall3d(distanceTraveled, i + TheHero.FOV, gameResolution, centreDis, objHit);//add to a list to turn into a textured 3d slice later
                    newSlice.LoadContent(Content, toExit);
                    walls3d.Add(newSlice);
                }
            }
        }
        public bool shotsFired(double rotation, Vector2 loc, int range)//player shooting algorithm that uses a single ray from raycasting
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
        public struct NameValue()//leaderboard item and or shop item
        {
            public string name;
            public int value;
            public int used;
        }
    }
}