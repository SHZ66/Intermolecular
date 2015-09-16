using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Intermolecular
{
    public class Texture2DWrapper
    {
        public Texture2D texture;
        public float Scale;
        public SpriteBatch spriteBatch;

        public Texture2DWrapper()
        {
            
        }
        
        public int Height {
            get { return texture.Height; }
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public void Draw(Vector2 position, float Rotation, float _scale = 1.0f)
        {
            spriteBatch.Draw(this.texture, position,
                             new Rectangle(0, 0, this.Width, this.Height),
                             Color.White,
                             Rotation,
                             new Vector2(this.Width / 2.0f, this.Height / 2.0f),
                             this.Scale * _scale,
                             SpriteEffects.None,
                             0.0f);
        }
    }
}
