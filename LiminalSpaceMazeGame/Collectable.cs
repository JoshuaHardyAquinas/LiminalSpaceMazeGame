using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LiminalSpaceMazeGame
{
    internal class Collectable : StationaryObject
    {
        public char CollectableType;
        public bool isCollected;
        public Collectable()
        {
            
        }
        public override void update()
        {
            if (isCollected)
            {
                setLocation(new Vector2(-40, -40));
                Edge = new Rectangle((int)getLocation().X, (int)getLocation().Y, objWidth, objHeight);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)
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
            theHero.collected.Add(CollectableType);
            isCollected = true;
        }
    }
}
