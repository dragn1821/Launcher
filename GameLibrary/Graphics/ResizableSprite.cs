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
        private int targetWidth;
        private int targetHeight;
        private Rectangle rectangle;
        private Texture2D barTexture;

        public ResizableSprite(GraphicsDevice graphicsDevice) : base()
        {
            this.graphicsDevice = graphicsDevice;
            this.targetWidth    = 0;
            this.targetHeight   = 0;
            this.barTexture     = new Texture2D(graphicsDevice, 1, 1);
            this.barTexture.SetData(new[] { Color.Black });
        }

        public void SetTargetSize(int width, int height)
        {
            targetWidth  = width;
            targetHeight = height;
            UpdateAspectRatio();
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
                texture      = Texture2D.FromStream(graphicsDevice, fileStream);
                Width        = texture.Width;
                targetWidth  = texture.Width;
                Height       = texture.Height;
                targetHeight = texture.Height;                
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int width  = (targetWidth == 0) ? texture.Width : targetWidth;
            int height = (targetHeight == 0) ? texture.Height : targetHeight;

            spriteBatch.Draw(barTexture, new Rectangle((int)Position.X, (int)Position.Y, width, height), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(rectangle.X + (int)Position.X, rectangle.Y + (int)Position.Y, rectangle.Width, rectangle.Height), new Rectangle(0, 0, texture.Width, texture.Height), Color * Alpha);
        }

        #endregion

        #region Private Methods

        private void UpdateAspectRatio()
        {
            float sourceAspect = Width / (float)Height;
            float targetAspect = targetWidth / (float)targetHeight;

            if (targetAspect <= sourceAspect)
            {
                int presentHeight = (int)((targetWidth / sourceAspect) + 0.5f);
                int barHeight     = (targetHeight - presentHeight) / 2;
                rectangle         = new Rectangle(0, barHeight, targetWidth, presentHeight);
            }
            else
            {
                int presentWidth = (int)((targetHeight * sourceAspect) + 0.5f);
                int barWidth     = (targetWidth - presentWidth) / 2;
                rectangle        = new Rectangle(barWidth, 0, presentWidth, targetHeight);
            }
        }

        #endregion
    }
}