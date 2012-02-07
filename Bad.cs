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

    public class Bad : Microsoft.Xna.Framework.GameComponent
    {
        List<WorldObject> mBad; 
        Texture2D mBadTexture; 
        private World mWorld;
        int n = 0;
        List<Vector2> mBadBulletList;
        Random rand = new Random();

        public Bad(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            mBadBulletList = new List<Vector2>();
            base.Initialize();
        }
        public void LoadContent()
        {
            mBadTexture = Game.Content.Load<Texture2D>("ball");
            mBad = new List<WorldObject>();
            mWorld.SetBadComponent(mBad);
            
        }

        internal void SetWorldComponent(World world)
        {
            mWorld = world;
        }


        private void addBad(Vector2 v)
        {
            mBadBulletList.Add(mWorld.findBulletVelocity(v));
            WorldObject mBad0 = new WorldObject(mBadTexture);
            mBad0.Position = v;
            mBad.Add(mBad0);
            

        }

        public override void Update(GameTime gameTime)
        {
            if (n == 0)
            {
                int side = rand.Next(3);
                float X = rand.Next(mWorld.mBounds.Left, mWorld.mBounds.Right);
                float Y = rand.Next(mWorld.mBounds.Top, mWorld.mBounds.Bottom);
                if (side == 0)
                {
                    addBad(new Vector2(mWorld.mBounds.Left, Y));

                }
                else if (side == 1)
                    addBad(new Vector2(mWorld.mBounds.Right, Y));
                else if (side ==2)
                    addBad(new Vector2(X, mWorld.mBounds.Top));
                else
                    addBad(new Vector2(X, mWorld.mBounds.Bottom));
            }
                n = n == 0? 50 : n - 1;
            for (int i = 0; i < mBad.Count; i++)
            {
                mBad.ElementAt(i).Position += mBadBulletList.ElementAt(i) * gameTime.ElapsedGameTime.Milliseconds; 
              
            }
                base.Update(gameTime);
        }
    }
}