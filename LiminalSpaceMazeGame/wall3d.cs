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
    internal class wall3d: StationaryObject {
        private GraphicsDeviceManager _graphics;
        private int Width;
        private int Height;
        public wall3d(int width,int hieght, Vector2 location)
        {
            Width = width;
            Height = hieght;
            Location = location;
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            Texture2D rectangle = new Texture2D(_graphics.GraphicsDevice, Width, Height);

            Color[] pixelData = new Color[Width * Height];//upload texture of pixels to the rectange for printing
            for (int i = 0; i < pixelData.Length; ++i)
            {
                pixelData[i] = Color.GreenYellow;//set each pixels data, in future can turn this into a texture map and use math to darken and hue
            }
            rectangle.SetData(pixelData);
            spriteBatch.Draw(rectangle, Location, Color.White);
        }
    }
}
