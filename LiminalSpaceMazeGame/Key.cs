using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Net;
using Microsoft.Xna.Framework;

namespace LiminalSpaceMazeGame
{
    internal class Key : Collectable
    {
        public Key (Vector2 spawnLoc, char type)
        {
            collected = false;
            spawn(spawnLoc);
            CollectableType = type;
            objWidth = 20;
            objHeight = objWidth;
        }
        public override void update()
        {
            if (collected)
            {
                Edge = new Rectangle(0, 0, 0, 0);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Texture = Content.Load<Texture2D>(@"2dDoor");
        }
        public void collect(Hero theHero)
        {
            theHero.collected.Add(CollectableType);
            collected = true;
        }
    }
}