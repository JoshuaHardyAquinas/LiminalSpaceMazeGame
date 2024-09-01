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
using System.Reflection.Metadata.Ecma335;

namespace LiminalSpaceMazeGame
{
    internal class Hero : MovingObject
    {
        float PI = 3.141592f;//pie for radians/rotation
        public double FOV;
        public Hero(double fov)
        {
            // constructor
            Location = new Vector2(60, 60);//player spawn location
            Movement = new Vector2(0, 0);//no movment for player to begin with
            rotation = 0;//starting rotation
            FOV = fov;
        }
        public override void update()
        {
            //creates player edge
            Edge = new Rectangle((int)Location.X-Texture.Width/2, (int)Location.Y-Texture.Height/2, Texture.Width, Texture.Height);
            move();
            die();
        }
        protected void move()
        {
            //checks for keyboard inputs
            KeyboardState ks = Keyboard.GetState();
            //calculates layer speed with possibility to add sprinting
            int speedMultiplyer = 1;
            int speed = 1;
            speed = speed * speedMultiplyer;
            //reset movment
            Movement.X = 0;
            Movement.Y = 0;
            //for player movment and rotation
            if (ks.IsKeyDown(Keys.A))//rotation using radians
            {
                rotation = rotation - PI / 16f;//used pi/16 for smoother rotation in comparison to a larger value
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation = rotation + PI / 16f;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Movement.X = speed * (float)Math.Sin(rotation);//trig to edit players directional movment
                Movement.Y = -speed * (float)Math.Cos(rotation);
            }
            if (ks.IsKeyDown(Keys.W))
            {
                Movement.X = -speed * (float)Math.Sin(rotation);// --//--
                Movement.Y = speed * (float)Math.Cos(rotation);
            }
            //resets rotation if it goes above 2 or below 0 to keep accuracy high
            if (rotation > PI * 2f)//pi*2 for full circle in radians
            {
                rotation = 0;
            }
            if (rotation < 0)
            {
                rotation = PI * 2f;
            }
            Location = Location + Movement;//move player
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, rotation, new Vector2(Texture.Width/2f, Texture.Height/2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture
            Texture = Content.Load<Texture2D>(@"2d_Hero");
        }
        public void spawn()
        {
            Location = new Vector2 (60, 60);
        }
        protected override void die()
        {
            if (Health <= 0)//player dies if health reaches 0
            {
                //die
            }
        }
    }
}
