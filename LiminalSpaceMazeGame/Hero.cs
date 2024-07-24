using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms.VisualStyles;
using SharpDX.Direct3D9;

namespace LiminalSpaceMazeGame
{
    internal class Hero : MovingObject
    {
        public enum Direction
        {
            none,
            down,
            left,
            right,
            up,
        }
        private Vector2 Movement;
        float PI = 3.141592f;

        public Hero()
        {
            // constructor
            Location = new Vector2(0, 0);
            Movement = new Vector2(0, 0);
            rotation = 0;
        }
        public override void update()
        {
            //Edge = new Rectangle((int)Location.X, (int)Location.Y, 32, 54);

            KeyboardState ks = Keyboard.GetState();
            int speedMultiplyer = 1;
            int speed = 1;
            speed = speed * speedMultiplyer;
            if (ks.IsKeyDown(Keys.A) && Location.X > 0)
            {
                rotation = rotation - PI/16f;
            }
            if (ks.IsKeyDown(Keys.D) && Location.X < 1000)
            {
                rotation = rotation + PI/16f;
            }
            if (ks.IsKeyDown(Keys.S) && Location.Y < 1000)
            {
                Movement.X = speed * (float)Math.Cos(rotation);
                Movement.Y = speed * (float)Math.Sin(rotation);
                /*if (ks.IsKeyDown(Keys.A))
                {
                    Movement.X = -speed;
                }
                if (ks.IsKeyDown(Keys.D))
                {
                    Movement.X = speed;
                }*/
            }
           if (ks.IsKeyDown(Keys.W) && Location.X > 0)
            {
                Movement.X = -speed * (float)Math.Cos(rotation);
                Movement.Y = -speed * (float)Math.Sin(rotation);
                /*if (ks.IsKeyDown(Keys.A))
                {
                    Movement.X = -speed;
                }
                if (ks.IsKeyDown(Keys.D))
                {
                    Movement.X = speed;
                }*/
            }
            if (!ks.IsKeyDown(Keys.A) && !ks.IsKeyDown(Keys.W) && !ks.IsKeyDown(Keys.S) && !ks.IsKeyDown(Keys.D))
            {
                Movement.X = 0;
                Movement.Y = 0;
            }
            if (rotation > PI*2f)
            {
                rotation = 0;
            }
            if (rotation < 0)
            {
                rotation = PI*2f;
            }
            Location = Location + Movement;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, rotation, new Vector2(Texture.Width / 2f, Texture.Height / 2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public override void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("Yellow_Box");
        }
        protected override void spawn()
        {
            base.spawn();
        }
        protected override void die()
        {

        }
    }
}
