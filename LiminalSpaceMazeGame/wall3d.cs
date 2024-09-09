using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;


namespace LiminalSpaceMazeGame
{

    public class wall3d: StationaryObject {
        protected static Random rnd = new Random();
        private int Width;
        private int Height;
        public Texture2D rectangle;
        public wall3d(int width,int hieght, Vector2 location, GraphicsDevice device, int decay)
        {
            Width = width;
            Height = hieght+1;
            Location = location;
            rectangle = new Texture2D(device, Width, Height);

            Color[] pixelData = new Color[Width * Height];//upload texture of pixels to the rectange for printing
            for (int i = 0; i < pixelData.Length; ++i)
            {
                int num = 0;//rnd.Next(-1, decay);
                if (num == 0)
                {
                    pixelData[i] = Color.GreenYellow;//set each pixels data, in future can turn this into a texture map and use math to darken and hue
                }
                else
                {
                    pixelData[i] = Color.Green;
                }
            }
            rectangle.SetData(pixelData);
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public new void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            
            spriteBatch.Draw(rectangle, Location, Color.White);
        }
        static public wall3d generate3dWall(Vector2 displacment, int slice, Vector2 gameRes, GraphicsDevice device)
        {
            int XDist = Convert.ToInt32(Math.Abs(displacment.X));//turn displacment into distance for easy calculation
            int YDist = Convert.ToInt32(Math.Abs(displacment.Y));

            int hieght;
            if (XDist > YDist)//we want to use the longer disatnce to figure out how far away teh wall is as the x and y distances change with rotation
            {
                hieght = XDist;
            }
            else
            {
                hieght = YDist;
            }
            int wallHieght = 4096 / hieght + 20; //reciprical function to convert distance of the wall from the player to teh wall hieght, tunes specifically by me
            Vector2 location = new Vector2(slice * 3 + 50, gameRes.Y / 2 - wallHieght / 2);//move slice to specific place on screen

            wall3d newWall = new wall3d(3, wallHieght, location, device, 1);//create physical wall entity
            return newWall;//return so it can be added to the list
        }
    }
}
