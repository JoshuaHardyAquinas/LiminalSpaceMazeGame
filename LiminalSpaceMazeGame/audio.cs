using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LiminalSpaceMazeGame
{
    internal class AudioSound
    {
        SoundEffect music;
        public AudioSound()
        {

        }
        public void loadContent(ContentManager Content, string loc)
        {
            music = Content.Load<SoundEffect>(loc);
        }
        public void play()
        {
            music.Play();
        }
    }
}
