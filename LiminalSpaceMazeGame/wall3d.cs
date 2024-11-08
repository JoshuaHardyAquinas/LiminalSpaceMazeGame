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
            setLocation(location);
            rectangle = new Texture2D(device, Width, Height);

            Color[] pixelData = new Color[Width * Height];//upload texture of pixels to the rectangle for printing
            rectangle.SetData(pixelData);
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            
        }
        public new void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            spriteBatch.Draw(Texture,new Rectangle((int)getLocation().X,(int)getLocation().Y,4,Height),rectangle, Color.White);
            spriteBatch.Draw(Texture,new Rectangle((int)getLocation().X, (int)getLocation().Y, 4, Height),rectangle,Color.Wheat)
        }
        static public wall3d generate3dWall(Vector2 displacement, int slice, Vector2 gameRes, GraphicsDevice device,Vector2 centreDis)
        {
            double hieght = Math.Sqrt(displacement.Y * displacement.Y +  displacement.X * displacement.X);
            double wallHieght = 8192 / hieght; //reciprical function to convert distance of the wall from the player to teh wall hieght
            Vector2 location = new Vector2(slice * 4, gameRes.Y / 2 - (float)wallHieght / 2);//move slice to specific place on screen
            return new wall3d(4, Convert.ToInt32(wallHieght), location, device, 1); ;//return so it can be added to the list
        }
    }
}
