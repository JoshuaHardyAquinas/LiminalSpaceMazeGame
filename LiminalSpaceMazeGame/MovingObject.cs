using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace LiminalSpaceMazeGame
{
    abstract class MovingObject : StationaryObject
    {

        private float speed;
        private int health;
        private int sprint;
        private int attack;
        private int damage;


        protected float Speed { get => speed; set => speed = value; }
        protected int Health { get => health; set => health = value; }
        public int Sprint { get => sprint; set => sprint = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Damage { get => damage; set => damage = value; }

        public override void update()
        {
            Edge = new Rectangle((int)getLocation().X - Texture.Width / 2, (int)getLocation().Y - Texture.Height / 2, Texture.Width, Texture.Height);
            checkDeath();
        }
        protected virtual void SendAttack()
        {

        }
        protected virtual void gainHealth()
        {

        }
        protected virtual void LoseHealth()
        {

        }
        public override void LoadContent(ContentManager Content)
        { }

        protected override void checkDeath()
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
