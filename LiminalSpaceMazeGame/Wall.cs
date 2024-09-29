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
    public class Wall:StationaryObject
    {
        public Wall(int i,int j)
        {
            //set location and use the texture size as a grid
            Location = new Vector2(i * 40, j * 40);
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            //texture and hitbox
            Texture = Content.Load<Texture2D>(@"wall");
            Edge = new Rectangle((int)Location.X, (int)Location.Y, Texture.Width, Texture.Height);
        }
        public new void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            spriteBatch.Draw(Texture, Location, Edge, Color.Wheat);
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, 40, 40), Color.White);
        }
    }
}
