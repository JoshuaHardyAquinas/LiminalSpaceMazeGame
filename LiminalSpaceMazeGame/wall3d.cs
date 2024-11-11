using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System.Net.Security;


namespace LiminalSpaceMazeGame
{

    public class wall3d: StationaryObject 
    {
        protected static Random rnd = new Random();
        private int Width;
        private int Height;
        public string WallType;
        public Rectangle rectangle;
        public enum Direction
        {
            none,
            North,
            South,
            East,
            West
        }
        public wall3d(int width,int height, Vector2 location, int textureSlice)
        {

            Height = height;
            setLocation(location);
            rectangle = new Rectangle(textureSlice*4,0,4,400);
        }
        public override void update()
        {

        }
        public void LoadContent(ContentManager Content, char type)
        {
            switch(type)
            {
                case 'M':
                    Texture = Content.Load<Texture2D>(@"216123");
                    break;
                default:
                    Texture = Content.Load<Texture2D>(@"3dWallTest");
                    break;
            }
        }
        public new void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            spriteBatch.Draw(Texture, new Rectangle((int)getLocation().X,(int)getLocation().Y,4,Height), rectangle, Color.White);
        }
        static public wall3d generate3dWall(Vector2 displacement, int slice, Vector2 gameRes,Vector2 centreDis)
        {
            double hieght = Math.Sqrt(displacement.Y * displacement.Y +  displacement.X * displacement.X);
            double wallHieght = 8192 / hieght; //reciprical function to convert distance of the wall from the player to teh wall hieght
            double height = Math.Sqrt(displacement.Y * displacement.Y + displacement.X * displacement.X);
            int textureSlice = 0;
            if (Math.Abs(centreDis.X) <= Math.Abs(centreDis.Y))
            {
                textureSlice = 40 - (int)centreDis.X;
            }
            else
            {
                textureSlice = 40 - (int)centreDis.Y;
            }
            Vector2 location = new Vector2(slice*4, gameRes.Y / 2 - (float)wallHieght / 2);//move slice to specific place on screen
            return new wall3d(4, Convert.ToInt32(wallHieght), location,textureSlice); ;//return so it can be added to the list
        }
    }
}
