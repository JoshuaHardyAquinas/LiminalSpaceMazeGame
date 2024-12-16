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
        private char CollectableType;
        private bool collected;
        public Collectable(Vector2 locaction ,char type)
        {
            spawn(locaction);
            CollectableType = type;
        }
        public override void update()
        {

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public void collect(Hero theHero)
        {
            theHero.collected.Add(CollectableType);
        }
    }
}
