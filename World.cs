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
    public class World : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch mSpriteBatch;
        List<WorldObject> mEnemyList;
        WorldObject mPlayer;
        List<WorldObject> mPlayerBullets;
        List<WorldObject> mBad;
        public Rectangle mBounds { get; set; }
        Random rand = new Random();
        //int i = 0;
        //List<WorldObject> mBadBullets; 
        WorldObject Blow;
        bool blown = false;
        bool alive = true;
        int lives = 3;
        int score = 0;
        SpriteFont font;
        int stall = 8;

        public World(Game game)
            : base(game)
        {
        }


        protected override void LoadContent()
        {
            mSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Blow = new WorldObject(Game.Content.Load<Texture2D>("blow"));
            font = Game.Content.Load<SpriteFont>("SpriteFont1");

            base.LoadContent();
        }

        public override void Initialize()
        {
            mPlayerBullets = new List<WorldObject>();
            base.Initialize();
        }
        
        public void AddEnemy(WorldObject enemy)
        {
            Vector2 Position = new Vector2(mBounds.Right - enemy.LowerRightCorner.Y, rand.Next(mBounds.Bottom - (int)enemy.LowerRightCorner.X));
            Rectangle newEnemy = newRec(Position, enemy.LowerRightCorner);
            foreach (WorldObject w in mEnemyList)
            {
                if (w.Bounds().Intersects(newEnemy))
                {
                    return; 
                }
            }
            enemy.Position = Position; 
            mEnemyList.Add(enemy);
        }

        public void addPlayerBullet(WorldObject bullet)
        {
            bullet.Position = mPlayer.Position + mPlayer.LowerRightCorner;
            mPlayerBullets.Add(bullet);
        }
        
        internal void SetPlayerComponent(WorldObject player)
        {
            mPlayer = player;
            mPlayer.Position = new Vector2(mBounds.Right/2, mBounds.Bottom/2);
        }

        internal void SetEnemyComponent(List<WorldObject> enemyList)
        {
            mEnemyList = enemyList;
        }

        internal void SetBadComponent(List<WorldObject> bad)
        {
            mBad = bad;
        }

        internal void changeInEnemyPosition(Vector2 delta)
        {
            WorldObject w;
            for (int n = 0; n < mEnemyList.Count; n++)
            {
                w = mEnemyList.ElementAt(n);
                if (!changeInPosition(w, delta))
                    mEnemyList.Remove(w);
            }
        }

        internal void changeInPlayerBulletPosition(Vector2 delta)
        {
            WorldObject w; 
            for (int n = 0; n < mPlayerBullets.Count; n++)
            {
                w = mPlayerBullets.ElementAt(n);
                if (!changeInPosition(w, delta))
                    mPlayerBullets.Remove(w);
            }
            
        }

        private Rectangle newRec(Vector2 v, Vector2 v2)
        {
            return new Rectangle((int)v.X, (int)v.Y, (int)v2.X, (int)v2.Y);
        }

        private bool changeInPosition(WorldObject w, Vector2 delta)
        {
            Vector2 newPosition = w.Position + delta;
            Rectangle rec = newRec(newPosition, w.LowerRightCorner);
            if (!mBounds.Intersects(rec))
            {
                return false; 
            }
            w.Position = newPosition;
            return true;    
        }

        internal void changeInPlayerPosition(Vector2 delta)
        {
            Vector2 newPosition = mPlayer.Position + delta;
            Vector2 newLowerRight = newPosition + mPlayer.LowerRightCorner;
            if (newPosition.X > mBounds.Left
                && newLowerRight.X < mBounds.Right)
            {
                mPlayer.Position += new Vector2(delta.X, 0); 
            }
            if (newPosition.Y > mBounds.Top
                && newLowerRight.Y < mBounds.Bottom)
            {
                mPlayer.Position += new Vector2(0, delta.Y);
            }

        }

        internal void setBounds(Rectangle bounds)
        {
            mBounds = bounds; 
        }

        private int collision(List<WorldObject> list1, List<WorldObject> list2)
        {
            int count = 0;
            WorldObject object1;
            WorldObject object2;
            for (int i = 0; i < list1.Count; i++)
            {
                object1 = list1.ElementAt(i);
                for (int n = 0; n < list2.Count; n++)
                {
                    object2 = list2.ElementAt(n);
                    if (object1.Bounds().Intersects(object2.Bounds()))
                    {
                        list1.Remove(object1);
                        list2.Remove(object2);
                        count++;
                    }
                }
            }
            return count; 
        }

        private List<WorldObject> collision(List<WorldObject> list1, WorldObject w)
        {
            List<WorldObject> v = new List<WorldObject>(); 
            WorldObject object1;
            for (int i = 0; i < list1.Count; i++)
            {
                object1 = list1.ElementAt(i);
                if (object1.Bounds().Intersects(w.Bounds()))
                {
                    v.Add(object1); 
                }
            }
            return v; 
           
        }

        public Vector2 findBulletVelocity(Vector2 position)
        {
            bool xneg = false;
            bool yneg = false;

            Vector2 dir = mPlayer.Position - position;

            if (dir.X < 0)
            {
                xneg = true;
                dir.X = -dir.X;
            }
            if (dir.Y < 0)
            {
                yneg = true;
                dir.Y = -dir.Y;
            }
            float X = dir.X / (dir.X + dir.Y);
            float Y = 1 - X;

            float Xdir = X * .4f;
            float Ydir = Y * .4f;
            Xdir = xneg ? -Xdir : Xdir;
            Ydir = yneg ? -Ydir : Ydir;

            return new Vector2(Xdir, Ydir);
        }

        public override void Update(GameTime gameTime)
        {
            if (lives >= 0)
            {
                blown = false;
                int count = collision(mEnemyList, mPlayerBullets);
                score += 2 * count;
                foreach (WorldObject w in collision(mEnemyList, mPlayer))
                {
                    mEnemyList.Remove(w);
                    score += 2;
                }
                if (alive)
                {
                    List<WorldObject> blowUp = collision(mBad, mPlayer);
                    if (blowUp.Count > 0)
                    {
                        alive = false;
                        blown = true;
                        Blow.Position = mPlayer.Position;
                    }
                }
                else
                {
                    if (stall == 0)
                    {
                        alive = true;
                        mPlayer.Position = new Vector2(mBounds.Right / 2, mBounds.Bottom / 2);
                        lives--;
                    }
                    stall = stall == 0 ? 8 : stall - 1; 
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mSpriteBatch.Begin();
            if (lives >= 0)
            {
                if (alive)
                {
                    mPlayer.Draw(mSpriteBatch);

                }
                if (blown)
                {
                    Blow.Draw(mSpriteBatch);
                }
                foreach (WorldObject w in mBad)
                    w.Draw(mSpriteBatch);
                foreach (WorldObject w in mEnemyList)
                {
                    w.Draw(mSpriteBatch);
                }
                foreach (WorldObject w in mPlayerBullets)
                {
                    w.Draw(mSpriteBatch);
                }

                mSpriteBatch.DrawString(font, "score " + score.ToString(),
                    new Vector2(mBounds.Right - 100, mBounds.Bottom - 20), Color.Red);
                mSpriteBatch.DrawString(font, "lives " + lives.ToString(),
                    new Vector2(mBounds.Right - 200, mBounds.Bottom - 20), Color.Red);
            }

            else
            {
                mSpriteBatch.DrawString(font, "score " + score.ToString(),
                    new Vector2(mBounds.Right / 2 - 20, mBounds.Bottom / 2 - 20), Color.Red);
            }

   
            mSpriteBatch.End();
            base.Draw(gameTime);
        }

    }
}