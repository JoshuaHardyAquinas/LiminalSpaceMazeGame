﻿using Microsoft.Xna.Framework.Content;
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
        private Rectangle rectangle;
        
        public UILoadingBar(Vector2 startingLocation, int maxV, int maxL,int height)
        {
            maxLength = maxL;
            maxValue = maxV;
            Height = height;
            spawn(startingLocation);
        }
        public void update(int current)
        {
            currentValue = current*(maxLength/maxValue);
            rectangle = new Rectangle((int)getLocation().X, (int)getLocation().Y, currentValue, Height);
            
        }
        public override void LoadContent(ContentManager Content)
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, getLocation(), Color.AliceBlue);
        }
    }
}
