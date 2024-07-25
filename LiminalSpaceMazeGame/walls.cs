using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace LiminalSpaceMazeGame
{
    internal class walls:StationaryObject
    {
        int textureWidth = 40;
        int textureHieght = 40;
        public walls(int i,int j)
        {
            Location = new Vector2(i * textureWidth, j * textureHieght);
        }
        public override void update()
        {
            //TODO: COLIDE WITH EDGE OF PLAYER!
        }
        public override void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>(@"wall");
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, 40, 40), Color.White);
        }
    }
}
