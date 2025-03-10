﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LiminalSpaceMazeGame
{
    internal class AudioSound
    {
        private SoundEffect music;
        private string musicName;
        public AudioSound(string name)
        {
            musicName = name;
        }
        public void loadContent(ContentManager Content)
        {
            music = Content.Load<SoundEffect>(musicName);
        }
        public void play(bool playcase)
        {
            if (playcase)
            {
                music.Play();
            }
        }
    }
}
