using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace LiminalSpaceMazeGame
{
    public class wall3d : StationaryObject
    {
        protected static Random rnd = new Random();
        private int Width;
        public int Height;
        public string WallType;
        public Rectangle rectangle;
        public char type;
        private Vector2 centreDistance;
        public double distanceFromHero;
        public wall3d(Vector2 displacement, int slice, Vector2 gameRes, Vector2 centreDis, char objHit)
        {
            distanceFromHero = Math.Sqrt(displacement.Y * displacement.Y + displacement.X * displacement.X);
            int wallHieght;
            try
            {
                wallHieght = Convert.ToInt32(8192 / distanceFromHero); //reciprocal function to convert distance of the wall from the player to teh wall height
            }
            catch
            {
                wallHieght = 0;
            }


            centreDistance = centreDis;

            Vector2 location = new Vector2(slice * 4, gameRes.Y / 2 - (float)wallHieght / 2);//move slice to specific place on screen
            type = objHit;
            Height = Convert.ToInt32(wallHieght);
            setLocation(location);
        }
        public override void update()
        {

        }
        public void LoadContent(ContentManager Content, bool ex)
        {
            Width = 40;
            switch (type)
            {
                case 'M':
                    Texture = Content.Load<Texture2D>(@"216123");
                    Width = 50;
                    break;
                case 'E':
                    if (ex)
                    {
                        Texture = Content.Load<Texture2D>(@"exitOpen");
                    }
                    else
                    {
                        Texture = Content.Load<Texture2D>(@"door_3d");
                    }
                    break;
                case 'K':
                    Texture = Content.Load<Texture2D>(@"key_3d");
                    break;
                case 'C':
                    Texture = Content.Load<Texture2D>(@"Coin_3d");
                    break;
                case 'W':
                    Texture = Content.Load<Texture2D>(@"3dWall1");
                    break;
                case 'w':
                    Texture = Content.Load<Texture2D>(@"3dWall2");
                    break;
                case 'v':
                    Texture = Content.Load<Texture2D>(@"3dWall3");
                    break;
                default:
                    Texture = Content.Load<Texture2D>(@"nullVoidDead");//load wall texture for wall slices
                    break;
            }
        }
        public new void draw(SpriteBatch spriteBatch)
        {
            int textureSlice;
            if (Math.Abs(centreDistance.X) <= Math.Abs(centreDistance.Y))//direction that the distance from the centre is closest
            {
                textureSlice = Width - (int)centreDistance.X;//subtract 40 from the value, allows us to select a slice from the texture
            }
            else
            {
                textureSlice = Width - (int)centreDistance.Y;//do in the y direction to create its slice too stops it from being drawn wrong
            }
            rectangle = new Rectangle(textureSlice * 4, 0, 4, 400);
            //draw in location
            spriteBatch.Draw(Texture, new Rectangle((int)getLocation().X, (int)getLocation().Y, 4, Height), rectangle, Color.White);//draws and streatches texture to its correct size from its original
        }
    }
}
