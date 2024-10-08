﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;


namespace LiminalSpaceMazeGame
{
    abstract class MovingObject : StationaryObject
    {

        private int speed;
        private int health;
        private int sprint;
        private int attack;
        private int damage;


        protected int Speed { get => speed; set => speed = value; }
        protected int Health { get => health; set => health = value; }
        public int Sprint { get => sprint; set => sprint = value; }
        public int Attack { get => attack; set => attack = value; }
        public int Damage { get => damage; set => damage = value; }

        public override void update()
        {

        }
        protected virtual void SendAttack()
        {

        }
        protected virtual void gainHealth()
        {

        }
        protected virtual void LoseHealth()
        {

        }
        public override void LoadContent(ContentManager Content)
        { }

        protected override void checkDeath()
        {

        }
        public override void draw(SpriteBatch spriteBatch)
        {

        }
    }
}
