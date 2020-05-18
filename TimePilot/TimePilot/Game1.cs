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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rand; //pass this in everywhere

        Player player;
        int resetTimer;

        int enemySpawnTimer;
        List<Enemy> enemies;

        Texture2D enemyImg;
        Texture2D damagedImg;
        Texture2D bulletImg;
        Texture2D backgroundImg;
        Texture2D explosionImg;

        Rectangle backgroundRect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            rand = new Random();

            player = new Player(new Rectangle(500, 500, 64, 64));
            resetTimer = -1;

            enemySpawnTimer = 120;
            enemies = new List<Enemy>();

            backgroundRect = new Rectangle(-5000, -5000, 10000, 10000);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.texture = this.Content.Load<Texture2D>("player");

            enemyImg = this.Content.Load<Texture2D>("enemy");
            damagedImg = this.Content.Load<Texture2D>("damaged");
            bulletImg = this.Content.Load<Texture2D>("bullet");
            backgroundImg = this.Content.Load<Texture2D>("background");
            explosionImg = this.Content.Load<Texture2D>("explosion_player");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            enemySpawnTimer--;
            if (enemies.Count < 10)
            {
                if (enemySpawnTimer <= 0)
                {
                    enemies.Add(new Enemy(rand));
                    enemySpawnTimer = 30;
                }
            }

            if (resetTimer < 0)
            {
                player.Update(GamePad.GetState(PlayerIndex.One), enemies);

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update();

                    //check enemy got hit
                    for (int b = 0; b < player.bullets.Count; b++)
                    {
                        if (player.bullets[b].getCollisionRectangle().Intersects(enemies[i].getCollisionRectangle()))
                        {
                            player.bullets.RemoveAt(b);
                            enemies[i].health--;
                            enemies[i].dmgAnimTimer = 30000;
                        }
                    }

                    //check player hit by enemy
                    for (int b = 0; b < enemies[i].bullets.Count; b++)
                    {
                        if (player.getCollisionRectangle().Intersects(enemies[i].bullets[b].getCollisionRectangle()))
                        {
                            resetTimer = 70;
                            player.bullets.Clear();
                            player.lives--;
                            for (int e = 0; e < enemies.Count; e++)
                            {
                                enemies[e].randomize();
                                enemies[e].bullets.Clear();
                            }
                        }
                    }

                    if (enemies[i].health == 0)
                    {
                        enemies.RemoveAt(i);
                        i--;
                    }
                }
            }

            if (resetTimer >= 0)
            {
                resetTimer--;
            }

            backgroundRect.X += (int)player.moveX;
            backgroundRect.Y += (int)player.moveY;

            if (backgroundRect.Left >= 0)
                backgroundRect.X = -9000;

            else if (backgroundRect.Right <= 1000)
                backgroundRect.X = 0;

            if (backgroundRect.Top >= 0)
                backgroundRect.Y = -9000;

            else if (backgroundRect.Bottom <= 1000)
                backgroundRect.Y = 0;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImg, backgroundRect, Color.White);

            //call draws
            if (resetTimer < 0)
            {
                player.Draw(spriteBatch, bulletImg);
            }
            else if (resetTimer > 30)
            {
                spriteBatch.Draw(explosionImg, new Rectangle(player.getCollisionRectangle().X - 32, player.getCollisionRectangle().Y, 128, 64), new Rectangle(64 * (3 - ((resetTimer - 30) / 10)), 0, 64, 32), Color.White);
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch, enemyImg, damagedImg, bulletImg);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}