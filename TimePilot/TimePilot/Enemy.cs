using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TimePilot
{
    class Enemy
    {
        public int xVel;
        public int yVel;
        public float angle;
        public List<Bullet> bullets;
        public Rectangle rect;
        Random ran;
        public int health;
        public int dmgAnimTimer;
        public bool damaged;
        public int bulletTimer;

        public Enemy() : this(new Random())
        {
        }

        public Enemy(Random r)
        {
            bullets = new List<Bullet>();
            rect = new Rectangle(0, 0, 48, 48);
            ran = r;
            health = 2;
            bulletTimer = 60;
            randomize();
        }

        //dont worry about this it works
        public void randomize()
        {
            int spawnSide = ran.Next(0, 4);

            switch (spawnSide)
            {
                //0 is top of the screen, go clockwise for the rest.
                case 0:
                    rect.X = ran.Next(50, 950);
                    rect.Y = -100;
                    xVel = ran.Next(0, 2) == 0 ? -3 : 3;
                    angle = xVel == -3 ? 225 : 135;
                    yVel = 3;
                    break;
                case 1:
                    rect.X = 1100;
                    rect.Y = ran.Next(50, 950);
                    xVel = -3;
                    yVel = ran.Next(0, 2) == 0 ? -3 : 3;
                    angle = yVel == -3 ? 315 : 225;
                    break;
                case 2:
                    rect.X = ran.Next(50, 950);
                    rect.Y = 1100;
                    xVel = ran.Next(0, 2) == 0 ? -3 : 3;
                    angle = xVel == -3 ? 315 : 45;
                    yVel = -3;
                    break;
                case 3:
                    rect.X = -100;
                    rect.Y = ran.Next(50, 950);
                    xVel = 3;
                    yVel = ran.Next(0, 2) == 0 ? -3 : 3;
                    angle = yVel == -3 ? 45 : 135;
                    break;
                default:
                    rect.X = 0;
                    rect.Y = 0;
                    break;
            }
        }


        public void Update()
        {
            rect.X += xVel;
            rect.Y += yVel;
            if (dmgAnimTimer != 0)
            {
                dmgAnimTimer--;
            }
            if (rect.X > 1200 || rect.Right < -200 || rect.Y > 1200 || rect.Bottom < -200)
            {
                this.randomize();
            }

            bulletTimer--;
            if (bulletTimer == 0)
            {
                bullets.Add(new Bullet(rect.X, rect.Y, xVel * 1.7f, yVel * 1.7f, Color.Yellow));
                bulletTimer = 60;
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Update())
                    bullets.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch sb, Texture2D img, Texture2D damaged, Texture2D bulletImg)
        {
            if (dmgAnimTimer != 0 && dmgAnimTimer % 10 == 0)
            {
                sb.Draw(damaged, rect, null, Color.White, MathHelper.ToRadians(angle), new Vector2(0, 0), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(img, rect, null, Color.White, MathHelper.ToRadians(angle), new Vector2(0, 0), SpriteEffects.None, 0);
            }

            foreach (Bullet b in bullets)
            {
                b.Draw(sb, bulletImg);
            }
        }

        public Rectangle getCollisionRectangle()
        {
            return new Rectangle(rect.X - (rect.Width / 2), rect.Y - (rect.Height / 2), rect.Width, rect.Height);
        }
    }
}
