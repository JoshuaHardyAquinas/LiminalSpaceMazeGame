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
        public int Height;
        public string WallType;
        public Rectangle rectangle;
        public char type;
        private Vector2 centreDistance;
        /*public enum Direction //deprecated wall directionals
        {
            none,
            North,
            South,
            East,
            West
        }*/
        public wall3d(Vector2 displacement, int slice, Vector2 gameRes, Vector2 centreDis, char objHit)
        {
            double wallHieght = 8192 / Math.Sqrt(displacement.Y * displacement.Y + displacement.X * displacement.X); //reciprocal function to convert distance of the wall from the player to teh wall height
            
            centreDistance = centreDis;
            
            Vector2 location = new Vector2(slice * 4, gameRes.Y / 2 - (float)wallHieght / 2);//move slice to specific place on screen
            type = objHit;
            Height = Convert.ToInt32(wallHieght);
            setLocation(location);
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            switch(type)
            {
                case 'M':
                    Texture = Content.Load<Texture2D>(@"216123");
                    Width = 50;
                    break;
                case 'E':
                    Texture = Content.Load<Texture2D>(@"door_3d");
                    Width = 40;
                    break;
                case 'K':
                    Content.Load<Texture2D>(@"nullVoidDead");
                    Width = 20;
                    break;
                case 'W':
                    Texture = Content.Load<Texture2D>(@"3dWallTest");//load wall texture for wall slices
                    Width = 40;
                    break;
                default:
                    Texture = Content.Load<Texture2D>(@"nullVoidDead");//load wall texture for wall slices
                    Width = 40;
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
            spriteBatch.Draw(Texture, new Rectangle((int)getLocation().X,(int)getLocation().Y,4,Height), rectangle, Color.White);//draws and streatches texture to its correct size from its original
        }
    }
}
