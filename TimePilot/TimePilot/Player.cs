using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimePilot
{
    class Player
    {

        public Rectangle rect;
        public Texture2D texture;
        public float angle;
        public List<Bullet> bullets;
        public int lives;
        public float moveX;
        public float moveY;
        int tics;

        public Player()
        {

        }

        public Player(Rectangle r)
        {
            rect = r;
            bullets = new List<Bullet>();
            lives = 3;
            moveX = 0;
            moveY = 0;
            tics = 0;
        }

        public Player(Rectangle r, Texture2D t)
        {
            rect = r;
            texture = t;
            bullets = new List<Bullet>();
            lives = 3;
            moveX = 0;
            moveY = 0;
            tics = 0;
        }

        public void Update(GamePadState gps, List<Enemy> enemies)
        {


            if (gps.ThumbSticks.Left.Y != 0 || gps.ThumbSticks.Left.X != 0)
            {
                moveX = gps.ThumbSticks.Left.X * -5;
                moveY = gps.ThumbSticks.Left.Y * 5;
                angle = (float)Math.Atan2(gps.ThumbSticks.Left.Y, gps.ThumbSticks.Left.X) * -1 + MathHelper.PiOver2;

            }

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].dx += (int)moveX;
                bullets[i].dy += (int)moveY;

                if (bullets[i].Update())
                    bullets.RemoveAt(i);
            }


            foreach (Enemy e in enemies)
            {
                e.rect.X += (int)moveX;
                e.rect.Y += (int)moveY;

                foreach (Bullet b in e.bullets)
                {
                    b.dx += (int)moveX;
                    b.dy += (int)moveY;
                }
            }

            if (tics % 15 == 0)
                bullets.Add(new Bullet(rect.X, rect.Y, (float)Math.Cos(angle - MathHelper.PiOver2) * 9, (float)Math.Sin(angle - MathHelper.PiOver2) * 9, Color.Black));

            tics++;
        }

        public void Draw(SpriteBatch sb, Texture2D bulletImg)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(sb, bulletImg);
            }

            sb.Draw(texture, rect, null, Color.White, angle, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);


        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle(rect.X - (rect.Width / 2), rect.Y - (rect.Height / 2), rect.Width, rect.Height);
        }

    }
}