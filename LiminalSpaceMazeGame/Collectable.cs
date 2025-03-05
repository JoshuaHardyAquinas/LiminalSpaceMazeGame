using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    abstract class Collectable : StationaryObject//abstract class for all collectables, can be extended into objects like the coin and the key and more in the future
    {
        public char CollectableType;
        public bool isCollected;
        public Collectable()
        {

        }
        public override void update()
        {
            if (isCollected)//set outside the map if it was collected
            {
                setLocation(new Vector2(-40, -40));
                Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, objWidth, objHeight);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)//draw if not collected
            {
                spriteBatch.Draw(Texture, new Rectangle((int)getLocation().X, (int)getLocation().Y, objWidth, objHeight), null, Color.White);
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public virtual void collect(Hero theHero)
        {
            //generic collect 
            theHero.collected.Add(CollectableType);
            isCollected = true;
        }
    }
}
