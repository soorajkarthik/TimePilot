using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TimePilot
{
    class Bullet
    {
        public Rectangle rect; //get img from main
        public float dx, dy;
        public float velX, velY;
        public Color col;

        public Bullet()
        {

        }

        public Bullet(Rectangle rect, float velX, float velY, Color col)
        {
            this.rect = rect;
            dx = rect.X;
            dy = rect.Y;
            this.velX = velX;
            this.velY = velY;
            this.col = col;
        }


        public Bullet(int x, int y, float velX, float velY, Color col)
        {
            this.rect = new Rectangle(x, y, 8, 8);
            dx = x;
            dy = y;
            this.velX = velX;
            this.velY = velY;
            this.col = col;
        }

        // returns true if out of bounds
        public bool Update()
        {
            dx += velX;
            dy += velY;

            rect.X = (int)dx;
            rect.Y = (int)dy;

            return dx < 0 || dx + 8 > 1000 || dy < 0 || dy > 1000;
        }

        public void Draw(SpriteBatch batch, Texture2D img)
        {
            batch.Draw(img, rect, col);
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle(rect.X - (rect.Width / 2), rect.Y - (rect.Height / 2), rect.Width, rect.Height);
        }
    }
}