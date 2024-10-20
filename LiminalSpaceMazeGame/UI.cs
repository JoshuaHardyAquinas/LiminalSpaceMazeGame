using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    internal class UI:StationaryObject
    {
       
        public UI()
        {
            Location = new Vector2(0, 670);
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
            spriteBatch.Draw(Texture,Location,Color.AliceBlue);
        }

    }
}
