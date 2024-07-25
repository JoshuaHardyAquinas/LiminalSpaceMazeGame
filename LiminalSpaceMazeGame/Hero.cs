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
        private int textureHieght = 20;
        private int textureWidth = 20;
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
            Location = new Vector2(60, 60);
            Movement = new Vector2(0, 0);
            rotation = 0;
            
        }
        public override void update()
        {
            Rectangle Edge = new Rectangle((int)Location.X, (int)Location.Y, textureHieght, textureWidth);
            move();
            die();
        }
        protected void move()
        {
            KeyboardState ks = Keyboard.GetState();
            int speedMultiplyer = 1;
            int speed = 1;
            speed = speed * speedMultiplyer;
            Movement.X = 0;
            Movement.Y = 0;
            if (ks.IsKeyDown(Keys.A))
            {
                rotation = rotation - PI / 16f;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation = rotation + PI / 16f;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Movement.X = speed * (float)Math.Sin(rotation);
                Movement.Y = -speed * (float)Math.Cos(rotation);
            }
            if (ks.IsKeyDown(Keys.W))
            {

                Movement.X = -speed * (float)Math.Sin(rotation);
                Movement.Y = speed * (float)Math.Cos(rotation);
            }
            if (rotation > PI * 2f)
            {
                rotation = 0;
            }
            if (rotation < 0)
            {
                rotation = PI * 2f;
            }
            if (Location.X < 0)
            {
                Location.X = 0;
            }
            if (Location.Y < 0)
            {
                Location.Y = 0;
            }
            Location = Location + Movement;
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, textureWidth, textureHieght), Color.White, rotation, new Vector2(textureWidth/2f, textureHieght/2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public override void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>(@"2d_Hero");
        }
        protected override void spawn()
        {
            base.spawn();
        }
        protected override void die()
        {
            if (Health <= 0)
            {
                //die
            }
        }
    }
}
