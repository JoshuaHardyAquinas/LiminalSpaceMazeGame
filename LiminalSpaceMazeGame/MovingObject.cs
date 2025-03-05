using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace LiminalSpaceMazeGame
{
    abstract class MovingObject : StationaryObject
    {

        private float speed;
        protected int Health;
        private int damage;
        public Vector2 Movement;


        public float Speed { get => speed; set => speed = value; }
        public int Damage { get => damage; set => damage = value; }

        public override void update()
        {
            Edge = new Rectangle((int)getLocation().X - Texture.Width / 2, (int)getLocation().Y - Texture.Height / 2, Texture.Width, Texture.Height);
            checkDeath();
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
