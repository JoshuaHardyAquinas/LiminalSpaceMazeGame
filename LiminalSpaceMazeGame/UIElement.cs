using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    internal class UIElement:StationaryObject
    {
        public UIElement()
        {
            spawn(new Vector2(0, 695));
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture
            Texture = Content.Load<Texture2D>(@"ui");
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, getLocation(), Color.AliceBlue);
        }
    }
}
