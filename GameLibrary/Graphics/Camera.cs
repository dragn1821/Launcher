using GameLibrary.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
  //This class represents the ideal screen resolution.
  //All screens will be drawn to this size.
  public class Camera
  {
    private GraphicsDevice graphics;

    public int Width                   { get; private set; }
    public int Height                  { get; private set; }
    public float AspectRatio           { get { return Width / (float)Height; } }
    public RenderTarget2D RenderTarget { get; private set; }

    public Camera(GraphicsDevice graphics, int width, int height)
    {
      this.graphics     = graphics;
      this.Width        = width;
      this.Height       = height;
      this.RenderTarget = new RenderTarget2D(graphics, Width, Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Scene scene)
    {
      graphics.SetRenderTarget(RenderTarget);
      graphics.Clear(Color.Black);
      spriteBatch.Begin();

      if (scene != null)
      {
        scene.Draw(gameTime, spriteBatch);
      }

      spriteBatch.End();
    }
  }
}
