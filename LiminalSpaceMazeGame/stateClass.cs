using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace LiminalSpaceMazeGame
{
    internal class stateClass
    {
        private Texture2D background;
        private Texture2D forground;
        public bool displayFg;
        public void LoadContent(ContentManager Content, string bg, string fg)
        {
            //load player texture
            background = Content.Load<Texture2D>(@bg);
            forground = Content.Load<Texture2D>(@fg);
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            if (displayFg)
            {
                spriteBatch.Draw(forground, new Vector2(0, 0), Color.White);
            }
        }
    }
}
