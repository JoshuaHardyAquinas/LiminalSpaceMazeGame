using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Windows.Forms;

namespace LiminalSpaceMazeGame
{
     class Monster : MovingObject
    {
        private int textnum = 0;
        public Monster(Vector2 startingLoc, int text)
        {
            spawn(startingLoc);
            textnum = 0;
            rotation = 0f;
        }
        public void update(Hero theHero)
        {
            base.update();
            //creates player edge
            if (theHero.getLocation().X>getLocation().X)
            {
                Movement.X = 1;
            }
            else
            {
                Movement.X = -1;
            }
            if (theHero.getLocation().Y > getLocation().Y)
            {
                Movement.Y = 1;
            }
            else
            {
                Movement.Y = -1;
            }
            
            //rotation = PI / 32;
            //Movement.X = 1 * (float)Math.Sin(rotation);//trig to edit players directional movement
            //Movement.Y = -1* (float)Math.Cos(rotation);
            setLocation(getLocation()+Movement);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load monster texture
            if (textnum == 0)
            {
                Texture = Content.Load<Texture2D>(@"Monster2d");
            }
            else
            {
                throw new Exception("ID10T");
            }
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, (float)rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }

    }
}
