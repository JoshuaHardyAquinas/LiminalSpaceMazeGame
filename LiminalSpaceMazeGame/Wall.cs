using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    public class Wall : StationaryObject
    {
        public Wall(int i, int j)
        {
            //set location and use the texture size as a grid
            spawn(new Vector2(i * 40 - 20, j * 40 - 20));
            objWidth = 40;
            objHeight = 40;
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            //texture and hitbox
            Texture = Content.Load<Texture2D>(@"wall");
            base.LoadContent(Content);
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, objWidth, objHeight), Color.White);
        }
    }
}
