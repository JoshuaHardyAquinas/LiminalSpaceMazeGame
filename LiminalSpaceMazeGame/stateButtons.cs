using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static LiminalSpaceMazeGame.Game1;

namespace LiminalSpaceMazeGame
{
    internal class stateButtons : StationaryObject
    {
        public GameState activeState;
        public char buttonAction;
        public stateButtons(Vector2 location, Vector2 size, GameState state, char action)
        {
            buttonAction = action;
            activeState = state;
            spawn(location);
            Edge = new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y);
            objWidth = (int)size.X;
            objHeight = (int)size.Y;
        }
        public override void update()
        {

        }
        public void LoadContent(ContentManager Content, string texture)
        {
            Texture = Content.Load<Texture2D>(@texture);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Edge, Color.White);
        }
    }
}
