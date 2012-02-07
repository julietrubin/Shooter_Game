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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections;



namespace Shooter
{
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        private KeyboardState mKeyboard; 
        private WorldObject mPlayer;
        private World mWorld;
        private Texture2D mPlayerTexture;
        private Texture2D mPlayerBulletTexture; 
        private float mSpeed;
        private Vector2 mBulletSpeed;
        int i = 50;


        public Player(Game game)
            : base(game)
        {
         
        }

        public override void Initialize()
        {
            mKeyboard = new KeyboardState();
            mSpeed = 0.3f;
            mBulletSpeed = new Vector2(0.5f, 0); 
            base.Initialize();
        }

        public void LoadContent()
        {
            mPlayerTexture = Game.Content.Load<Texture2D>("blue");
            mPlayerBulletTexture = Game.Content.Load<Texture2D>("spark");
            mPlayer = new WorldObject(mPlayerTexture);
            //mPlayer.Position = new Vector2(0, 0);
            mWorld.SetPlayerComponent(mPlayer);
        }


        internal void SetWorldComponent(World world)
        {
            mWorld = world;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaY = 0;
            float deltaX = 0;
            mKeyboard = Keyboard.GetState();
            if (mKeyboard.IsKeyDown(Keys.Escape))
                Game.Exit();

            if (mKeyboard.IsKeyDown(Keys.Down))
            {
                deltaY = gameTime.ElapsedGameTime.Milliseconds * mSpeed;
            }
            if (mKeyboard.IsKeyDown(Keys.Up))
            {
                deltaY = gameTime.ElapsedGameTime.Milliseconds * -mSpeed;
            }
            if (mKeyboard.IsKeyDown(Keys.Left))
            {
                deltaX = gameTime.ElapsedGameTime.Milliseconds * -mSpeed;
            }
            if (mKeyboard.IsKeyDown(Keys.Right))
            {
                deltaX = gameTime.ElapsedGameTime.Milliseconds * mSpeed;
            }
            if (mKeyboard.IsKeyDown(Keys.A))
            {
                int batch = 10; 
                i = i > batch ? 0 : i + 1;
                if (i == batch)
                {
                    mWorld.addPlayerBullet(new WorldObject(mPlayerBulletTexture));
                }
                
                   
               
            }

            mWorld.changeInPlayerPosition(new Vector2(deltaX, deltaY));
            Vector2 bulletDeltaX = mBulletSpeed * gameTime.ElapsedGameTime.Milliseconds;
            mWorld.changeInPlayerBulletPosition(bulletDeltaX);
            base.Update(gameTime);
        }

    }
}