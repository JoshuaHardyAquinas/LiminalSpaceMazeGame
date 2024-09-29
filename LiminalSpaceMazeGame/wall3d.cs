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
        public enum Direction
        {
            none,
            North,
            South,
            East,
            West
        }
        public wall3d(int width,int height, Vector2 location, GraphicsDevice device, int decay, Direction wallDirection)
        {
            Width = width;
            Height = height+1;
            Location = location;
            rectangle = new Texture2D(device, Width, Height);

            Color[] pixelData = new Color[Width * Height];//upload texture of pixels to the rectangle for printing
            for (int i = 0; i < pixelData.Length; ++i)
            {
                if (wallDirection == Direction.North)
                {
                    pixelData[i] = Color.Red;
                }
                else if (wallDirection == Direction.South)
                {
                    pixelData[i] = Color.Green;
                }
                else if (wallDirection == Direction.East)
                {
                    pixelData[i] = Color.Blue;
                }
                else if (wallDirection == Direction.West)
                {
                    pixelData[i] = Color.Yellow;
                }
                else
                {
                    pixelData[i] = Color.Black;
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
        static public wall3d generate3dWall(Vector2 displacement, int slice, Vector2 gameRes, GraphicsDevice device,Vector2 centreDis)
        {
            float XDist = Convert.ToInt32(Math.Abs(displacment.X));//turn displacment into distance for easy calculation
            float YDist = Convert.ToInt32(Math.Abs(displacment.Y));

            Direction cDirection = Direction.none;
            double hieght = Math.Sqrt(YDist*YDist + XDist*XDist);
            /*if (XDist > YDist)//we want to use the longer disatnce to figure out how far away teh wall is as the x and y distances change with rotation
            {
                hieght = XDist;
            }
            else
            {
                hieght = YDist;
            }*/
            
            if (Math.Abs(centreDis.X)>Math.Abs(centreDis.Y))
            {
                if (centreDis.X > 0)
                {
                    cDirection = Direction.East;
                }
                else
                {
                    cDirection = Direction.West;
                }
            }
            else
            {
                if (centreDis.Y > 0)
                {
                    cDirection = Direction.North;
                }
                else
                {
                    cDirection = Direction.South;
                }
            }
            double wallHieght = 4096 / hieght + 20; //reciprical function to convert distance of the wall from the player to teh wall hieght, tunes specifically by me
            Vector2 location = new Vector2(slice * 4, gameRes.Y / 2 - (float)wallHieght / 2);//move slice to specific place on screen
            wall3d newWall = new wall3d(4, Convert.ToInt32(wallHieght), location, device, 1, cDirection);//create physical wall entity
            return newWall;//return so it can be added to the list
        }
    }
}
