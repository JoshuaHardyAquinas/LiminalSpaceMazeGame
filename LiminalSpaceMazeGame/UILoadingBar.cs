using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.DataAnnotations;

namespace LiminalSpaceMazeGame
{
    internal class UILoadingBar:StationaryObject
    {
        private int maxLength;
        private int maxValue;
        private int currentValue;
        private int Height;
        Color barcolour;
        private Rectangle rectangle;
        public bool display = true;

        public UILoadingBar(Vector2 startingLocation, int maxV, int maxL, int height,Color colour)
        {
            barcolour = colour;
            maxLength = maxL;
            maxValue = maxV;
            Height = height;
            setLocation(startingLocation);
        }
        public void update(int current)
        {
            currentValue = (current * maxLength) / maxValue;
            rectangle = new Rectangle((int)getLocation().X, (int)getLocation().Y, currentValue, Height);
        }
        public void update(int current,int newmax)
        {
            maxValue = newmax;
            update(current);
        }
        public void LoadContent(ContentManager Content,GraphicsDevice device)
        {
            Texture = new Texture2D(device,1,1);//load 
            Texture.SetData(new Color[] { barcolour });
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            if (display)
            {
                spriteBatch.Draw(Texture, rectangle, new Rectangle(0, 0, 100, 50), Color.White);
            }
        }
    }
}
