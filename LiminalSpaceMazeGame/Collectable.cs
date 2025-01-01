using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace LiminalSpaceMazeGame
{
    internal class Collectable : StationaryObject
    {
        protected char CollectableType;
        protected bool collected;
        public Collectable()
        {
            
        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, objWidth, objHeight), Color.Yellow);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public virtual void collect(Hero theHero)
        {
            theHero.collected.Add(CollectableType);
        }
    }
}
