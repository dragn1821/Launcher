using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Graphics
{
  public abstract class DisplayObject
  {
    public string ResourceName { get; set; }
    public Vector2 Position    { get; set; }
    public float Scale         { get; set; }
    public float Rotation      { get; set; }
    public Vector2 Origin      { get; set; }
    public Color Color         { get; set; }
    public float Alpha         { get; set; }
    public int Width           { get; protected set; }
    public int Height          { get; protected set; }

    public DisplayObject()
    {
      this.ResourceName = string.Empty;
      this.Position     = Vector2.Zero;
      this.Scale        = 1f;
      this.Rotation     = 0f;
      this.Origin       = Vector2.Zero;
      this.Color        = Color.White;
      this.Alpha        = 1.0f;
    }

    #region Abstract Methods

    public abstract void LoadContent(ContentManager content);
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    #endregion
  }
}