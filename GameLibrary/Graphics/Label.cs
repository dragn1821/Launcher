using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameLibrary.Graphics
{
  public class Label : DisplayObject
  {
    private SpriteFont font;
    private string text;

    public string Text
    {
      get
      {
        return text;
      }
      set
      {
        text = value;

        if (font != null)
        {
          MeasureString();
        }
      }
    }

    public Label() : base()
    { }

    #region Overrides

    public override void LoadContent(ContentManager content)
    {
      if (string.IsNullOrEmpty(ResourceName))
      {
        throw new ArgumentNullException("The resource name is required for a Label.");
      }

      font = content.Load<SpriteFont>(ResourceName);
      MeasureString();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      MeasureString();
      spriteBatch.DrawString(font, Text, Position, Color * Alpha, Rotation, Origin, Scale, SpriteEffects.None, 0f);
    }

    #endregion

    #region Private Methods

    private void MeasureString()
    {
      Vector2 size = font.MeasureString(Text);
      Width        = (int)size.X;
      Height       = (int)size.Y;
    }

    #endregion
  }
}