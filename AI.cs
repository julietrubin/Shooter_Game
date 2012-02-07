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


namespace Shooter
{
    public class AI : Microsoft.Xna.Framework.GameComponent
    {
        List<WorldObject> mEnemyList;
        Texture2D mEnemyTexture;  
        private World mWorld;
        private Random mRandom; 
        private Vector2 mSpeed;
        private int i = 0;
        private int batch;

        public AI(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            mRandom = new Random();
            mEnemyList = new List<WorldObject>();
            mWorld.SetEnemyComponent(mEnemyList);
            mSpeed = new Vector2(0.03f, 0.0f);
            batch = mRandom.Next(50, 250);
            base.Initialize();
        }

        public void LoadContent()
        {
            mEnemyTexture = Game.Content.Load<Texture2D>("crab");
            CreateEnemies();
            
        }

        public void CreateEnemies()
        {
            WorldObject nextEnemy = new WorldObject(mEnemyTexture);
            mWorld.AddEnemy(nextEnemy);
        }

        internal void SetWorldComponent(World world)
        {
            mWorld = world;
        }

        public override void Update(GameTime gameTime)
        {
            i = i > batch ? 0 : i + 1;
            if (i == batch)
            {
                CreateEnemies();
                batch = mRandom.Next(20, 100);
            }
            Vector2 delta = -mSpeed * gameTime.ElapsedGameTime.Milliseconds;
            mWorld.changeInEnemyPosition(delta);
         
            base.Update(gameTime);
        }
    }
}