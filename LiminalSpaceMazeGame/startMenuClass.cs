using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LiminalSpaceMazeGame
{
    internal class startMenuClass
    {
        private Texture2D background;
        private Texture2D helpMenu;
        public bool displayHelpMenu;

        public startMenuClass()
        {
            
        }
        public void update()
        {

        }
        public void LoadContent(ContentManager Content)
        {
            //load player texture
            background = Content.Load<Texture2D>(@"WelcomeScreen");
            helpMenu = Content.Load<Texture2D>(@"HelpScreen");//  WWWWWWWWWWAAAAAAAAAAAA
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0,0), Color.White);
            spriteBatch.Draw(helpMenu, new Vector2(0, 0), Color.White);// WWAAAAAAAAAAAAAAAAAA

        }
    }
}
