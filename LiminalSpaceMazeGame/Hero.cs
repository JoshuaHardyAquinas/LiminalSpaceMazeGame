﻿using Microsoft.Xna.Framework.Content;
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
using SharpDX.XAudio2;

namespace LiminalSpaceMazeGame
{
    internal class Hero : MovingObject
    {
        private int MaxHealth;
        private int staminaMax;
        private int stamina;
        private int MaxShield = 100;
        private int Shield;
        private int fOV;
        bool cooldown;
        public List<char> collected = new List<char>();

        public int FOV { get => fOV; set => fOV = value; }
        public int Stamina { get => stamina; set => stamina = value; }
        public int StaminaMax { get => staminaMax; set => staminaMax = value; }
        public int maxHealth { get => MaxHealth; set => MaxHealth = value; }
        public int shield { get => Shield; set => Shield = value; }

        public Hero(int fov, int MxSt, int MxHlth)
        {
            // constructor
            staminaMax = MxSt;
            Health = MxHlth;
            MaxHealth = MxHlth;
            spawn(new Vector2(60, 60));
            Movement = new Vector2(0, 0);//no movment for player to begin with
            changeRotation = 0;
            rotation = 0;//starting rotation
            FOV = fov;
            stamina = staminaMax/2;
            cooldown = false;
            
        }
        public override void update()
        {
            //creates player edge
            Edge = new Rectangle((int)getLocation().X-Texture.Width/2, (int)getLocation().Y-Texture.Height/2, Texture.Width, Texture.Height);
            move();
        }
        protected void move()
        {
            //checks for keyboard inputs
            KeyboardState ks = Keyboard.GetState();
            //calculates layer speed with possibility to add sprinting
            int speedMultiplyer = 1;
            int speed = 1;
            if (stamina < 1200 && (stamina>20 || !cooldown))
            {
                stamina = stamina+2;
            }
            if (ks.IsKeyDown(Keys.LeftShift) && !cooldown)
            {
                speedMultiplyer = 2;
                stamina = stamina - 20;
            }
            if (stamina < 0)
            {
                cooldown = true;
            }
            if (cooldown == true)
            {
                stamina++;
            }
            if (stamina > 180)
            {
                cooldown = false;
            }
            speed = speed * speedMultiplyer;
            //reset movement
            Movement.X = 0;
            Movement.Y = 0;
            changeRotation = 0;
            //for player movment and rotation
            if (ks.IsKeyDown(Keys.A))//rotation using radians
            {
                changeRotation = - PI / 32f;//used pi/16 for smoother rotation in comparison to a larger value
            }
            if (ks.IsKeyDown(Keys.D))
            {
                changeRotation = PI / 32f;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                Movement.X = speed * (float)Math.Sin(rotation);//trig to edit players directional movement
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
            rotation = rotation + changeRotation;
            setLocation(getLocation() + Movement);//movePlayer
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw player including rotation
            spriteBatch.Draw(Texture, getLocation(), new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, (float)rotation, new Vector2(Texture.Width/2f, Texture.Height/2f), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public override void LoadContent(ContentManager Content)
        {
            //load player texture
            Texture = Content.Load<Texture2D>(@"2d_Hero");
        }
        public void loseHealth(int value)
        {
            Health -= value;

        }
        public int checkHealth()
        {
            return Health;
        }
        public int checkStamina()
        {
            return stamina;
        }

        public void editStats(char stat, int vaue) { 
        }

        public void gainHealth(int value)
        {
            Health += value;
            if (Health > maxHealth)
            {
                Health = maxHealth;
            }
        }

        public override void spawn(Vector2 loc)
        {
            base.spawn(loc);
            Health = maxHealth;
        }
    }
}
