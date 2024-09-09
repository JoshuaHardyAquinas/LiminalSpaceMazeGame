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
    public class Wall:StationaryObject
    {
        public Wall(int i,int j)
        {
            //set location and use the texture size as a grid
            Location = new Vector2(i * 40, j * 40);
        }
        public override void update()
        {

        }
        public override void LoadContent(ContentManager Content)
        {
            //texture and hitbox
            Texture = Content.Load<Texture2D>(@"wall");
            Edge = new Rectangle((int)Location.X, (int)Location.Y, Texture.Width, Texture.Height);
        }
        public override void draw(SpriteBatch spriteBatch)
        {
            //draw in location
            spriteBatch.Draw(Texture, Location, new Rectangle(0, 0, 40, 40), Color.White);
        }
        public wall3d generate3dWall(Vector2 displacment, int slice, Vector2 gameRes, GraphicsDevice device)
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
            int wallHieght = 2048 / hieght; //reciprical function to convert distance of the wall from the player to teh wall hieght, tunes specifically by me
            Vector2 location = new Vector2(slice * 3 + 50, gameRes.Y / 2 - wallHieght / 2);//move slice to specific place on screen

            wall3d newWall = new wall3d(3, wallHieght, location, device, 1);//create physical wall entity
            return newWall;//return so it can be added to the list
        }
    }
}
