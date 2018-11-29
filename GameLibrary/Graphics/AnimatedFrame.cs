using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
  public class AnimatedFrame
  {
    private Texture2D texture;
    private Rectangle sourceRectangle;

    public AnimatedFrame(Texture2D texture, Rectangle sourceRectangle)
    {
      this.texture         = texture;
      this.sourceRectangle = sourceRectangle;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin)
    {
      spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
    }
  }
}