using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameLibrary.Graphics
{
    public class Sprite : DisplayObject, ICloneable
    {
        protected Texture2D texture;

        public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); } }

        public Sprite() : base()
        { }

        #region Overrides

        public override void LoadContent(ContentManager content)
        {
            if (string.IsNullOrEmpty(ResourceName))
            {
                throw new ArgumentNullException("The resource name is required for a Sprite.");
            }

            texture = content.Load<Texture2D>(ResourceName);
            Width = texture.Width;
            Height = texture.Height;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color * Alpha, Rotation, Origin, Scale, SpriteEffects.None, 0f);
        }

        #endregion

        #region Public Methods

        public object Clone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        #endregion
    }
}