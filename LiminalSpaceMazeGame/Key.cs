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
            isCollected = false;
            spawn(spawnLoc-new Vector2(10,10));
            CollectableType = type;
            objWidth = 20;
            objHeight = objWidth;
        }
        public override void update()
        {
            base.update();
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            try
            {
                Texture = Content.Load<Texture2D>(@"key");
            }
            catch
            {
                Texture = Content.Load<Texture2D>(@"nullVoidDead");
            }
            
        }
        public override void collect(Hero theHero)
        {
            theHero.collected.Add(CollectableType);
            theHero.points += 50;
            isCollected = true;
        }
    }
}