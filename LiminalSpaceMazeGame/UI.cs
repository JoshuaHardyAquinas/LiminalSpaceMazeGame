using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    public class UI : StationaryObject
    {
        private string TN;
        public bool drawUI = true;
        public UI(Vector2 loc, string textName)
        {
            TN = textName;
            spawn(loc);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture

            Texture = Content.Load<Texture2D>(@TN);
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if (drawUI)
            {
                spriteBatch.Draw(Texture, getLocation(), Color.AliceBlue);
            }
        }

    }
}
