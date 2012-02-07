using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class WorldObject
    {
        public Vector2 Position { get; set; }
        public Vector2 LowerRightCorner { get; private set; }
        private Texture2D mTexture;

        public Rectangle Bounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)LowerRightCorner.X, (int)LowerRightCorner.Y);
        }
        public WorldObject(Texture2D texture)
        {
            mTexture = texture; 
            LowerRightCorner = new Vector2(texture.Width, texture.Height);
        }


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(mTexture, Position, Color.White);
        }
    }
}
