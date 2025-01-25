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
using SharpDX.XAudio2;
using SharpDX.Direct2D1.Effects;

namespace LiminalSpaceMazeGame
{
    internal class Hero : MovingObject
    {
        public float sensitivity = 32;
        public float senseMin = 64;
        public float senseMax = 1;
        private int MaxHealth;
        private int startHealth;
        private int staminaMax;
        private int staminaStart;
        private int stamina;
        private int MaxShield = 100;
        private int Shield;
        private int ShieldStart;
        private int fOV;
        public int points;
        bool cooldown;
        public string name;
        public List<char> collected = new List<char>();

        public int FOV { get => fOV; set => fOV = value; }
        public int Stamina { get => stamina; set => stamina = value; }
        public int StaminaMax { get => staminaMax; set => staminaMax = value; }
        public int maxHealth { get => MaxHealth; set => MaxHealth = value; }
        public int shield { get => Shield; set => Shield = value; }
        public int ShieldMax { get => MaxShield; set => MaxShield = value; }

        public Hero(int fov, int MxSt, int MxHlth)
        {
            // constructor
            Damage = 10;
            staminaMax = MxSt;
            staminaStart = staminaMax;
            Health = MxHlth;
            MaxHealth = MxHlth;
            startHealth = MxHlth;
            shield = MaxShield / 4;
            ShieldStart = MaxShield;
            spawn(new Vector2(60, 60));
            Movement = new Vector2(0, 0);//no movment for player to begin with
            changeRotation = 0;
            rotation = 0;//starting rotation
            FOV = fov;
            stamina = staminaMax/2;
            cooldown = false;
            name = "";
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
                changeRotation = - PI / sensitivity;//used pi/16 for smoother rotation in comparison to a larger value
            }
            if (ks.IsKeyDown(Keys.D))
            {
                changeRotation = PI / sensitivity;
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
            if (shield > 0)
            {
                shield -= value;
                if (shield < 0)
                {
                    value = Math.Abs(shield);
                    shield = 0;
                }
            }
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

        public void editStats() {
            
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

        public void upgrade(NameValue item,int levelNum)
        {
            if (item.value*levelNum >= points)
            {
                points -= item.value*levelNum;
            }
            switch (item.name)
            {
                case "s":
                    shield = ShieldMax;
                    break;
                case "S":
                    staminaMax = (int)(staminaMax*Math.Sqrt(levelNum));
                    break;
                case "H":
                    staminaMax = (int)(staminaMax * Math.Sqrt(levelNum));
                    break;
            }
        }
        public void reset(char stat, int value, bool reset)
        {
            maxHealth = startHealth;
            MaxShield = ShieldStart;
            staminaMax = staminaStart;
        }
        public struct NameValue()
        {
            public string name;
            public int value;
        }
    }
}
