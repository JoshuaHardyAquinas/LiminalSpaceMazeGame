using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace LiminalSpaceMazeGame
{
    internal class ExitDoor : StationaryObject
    {
        public bool Available = false;
        public int textureNumber = 1;
        public ExitDoor(Vector2 loc)//spawn daw and give a default edge
        {
            spawn(loc);
            Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, 30, 30);
        }
        public bool update(List<char> collected)
        {
            //check if the player has a key, if so change texture and allow door open
            foreach (char collectable in collected)
            {
                if (collectable == 'K')
                {
                    Available = true;
                }
            }
            if (Available == true)//set texture
            {
                textureNumber = 2;
            }
            return Available;
        }
        public override void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>(@"2dDoor");
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)getLocation().X, (int)getLocation().Y, 30, 30), new Rectangle(30 * textureNumber - 30, 0, 30, 30), Color.White);
        }
    }
}
