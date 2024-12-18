using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
            
        }

        public virtual void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public void collect(Hero theHero)
        {
            theHero.collected.Add(CollectableType);
        }
    }
}
