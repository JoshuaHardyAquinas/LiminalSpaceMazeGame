using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LiminalSpaceMazeGame
{
    internal class Exit: StationaryObject
    {
        public bool exitAvailable = false;
        public int texturenumber = 1;
        public Exit(int i,int j)
        {
            spawn(new Vector2(i * 40, j * 40));
        }
        public void update(GameTime gameTime)
        {
            if (exitAvailable == true)
            {
                if (gameTime.ElapsedGameTime.Seconds % 5 == 0)
                {
                    texturenumber++;
                    if (texturenumber < 3)
                    {
                        texturenumber = 1;
                    }
                }
            }
            
        }

    }
}
