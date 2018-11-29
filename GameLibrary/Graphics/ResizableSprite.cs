using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace GameLibrary.Graphics
{
    public class ResizableSprite : Sprite
    {
        private GraphicsDevice graphicsDevice;

        public int TargetWidth  { get; set; }
        public int TargetHeight { get; set; }

        public ResizableSprite(GraphicsDevice graphicsDevice) : base()
        {
            this.graphicsDevice = graphicsDevice;
        }

        #region Overrides

        public override void LoadContent(ContentManager content)
        {
            if (string.IsNullOrEmpty(ResourceName))
            {
                throw new ArgumentNullException("The resource name is required for a Sprite.");
            }

            using (FileStream fileStream = new FileStream(ResourceName, FileMode.Open))
            {
                texture = Texture2D.FromStream(graphicsDevice, fileStream);
                Width   = texture.Width;
                Height  = texture.Height;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int width  = (TargetWidth == 0) ? texture.Width : TargetWidth;
            int height = (TargetHeight == 0) ? texture.Height : TargetHeight;

            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, width, height), new Rectangle(0, 0, texture.Width, texture.Height), Color * Alpha);
        }

        #endregion
    }
}