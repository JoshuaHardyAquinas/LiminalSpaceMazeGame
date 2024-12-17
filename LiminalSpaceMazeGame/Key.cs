using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace LiminalSpaceMazeGame
{
    internal class Key : Collectable
    {
        public Key(Vector2 spawnLoc, char type)
        {
            spawn(spawnLoc);
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