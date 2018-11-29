using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
  //This class represents the user's screen resolution.
  //The camera size will be scaled up to whatever the user chooses.
  public class GraphicsDisplay
  {
    private GraphicsDeviceManager graphics;
    private GameWindow window;
    private Camera camera;
    private Rectangle screenRectangle;

    public int Width         { get; private set; }
    public int Height        { get; private set; }
    public float AspectRatio { get; private set; }
    public bool IsFullScreen { get { return graphics.IsFullScreen; } }

    public GraphicsDisplay(GraphicsDeviceManager graphics, GameWindow window, Camera camera)
    {
      this.graphics = graphics;
      this.window   = window;
      this.camera   = camera;
      UpdateProperties();
    }

    public void SetResolution(int width, int height)
    {
      graphics.PreferredBackBufferWidth = width;
      graphics.PreferredBackBufferHeight = height;
      graphics.ApplyChanges();
      UpdateProperties();
    }

    public void SetFullScreen(bool isFullScreen)
    {
      graphics.IsFullScreen = isFullScreen;
      graphics.ApplyChanges();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      graphics.GraphicsDevice.SetRenderTarget(null);
      graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
      spriteBatch.Draw(camera.RenderTarget, screenRectangle, Color.White);
      spriteBatch.End();
    }

    private void UpdateProperties()
    {
      Width       = window.ClientBounds.Width;
      Height      = window.ClientBounds.Height;
      AspectRatio = Width / (float)Height;

      if (AspectRatio <= camera.AspectRatio)
      {
        int presentHeight = (int)((Width / camera.AspectRatio) + 0.5f);
        int barHeight     = (Height - presentHeight) / 2;
        screenRectangle   = new Rectangle(0, barHeight, Width, presentHeight);
      }
      else
      {
        int presentWidth = (int)((Height * camera.AspectRatio) + 0.5f);
        int barWidth     = (Width - presentWidth) / 2;
        screenRectangle  = new Rectangle(barWidth, 0, presentWidth, Height);
      }
    }
  }
}