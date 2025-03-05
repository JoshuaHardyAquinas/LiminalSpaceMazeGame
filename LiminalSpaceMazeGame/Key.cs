using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LiminalSpaceMazeGame
{
    internal class Key : Collectable//object class for the key
    {
        private int value = 50;
        public Key(Vector2 spawnLoc, char type)
        {
            //set up key
            isCollected = false;
            spawn(spawnLoc - new Vector2(10, 10));
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
            //add to hero list if collected and disable object
            theHero.collected.Add(CollectableType);
            theHero.points += value;
            isCollected = true;
        }
    }
}