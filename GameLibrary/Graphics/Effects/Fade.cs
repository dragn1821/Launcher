using Microsoft.Xna.Framework;
using System;

namespace GameLibrary.Graphics.Effects
{
  public class Fade
  {
    private Sprite sprite;
    private float fadeStep;

    public delegate void FadeComplete(object sender, EventArgs args);

    public event FadeComplete Complete;

    public bool IsRunning   { get; private set; }
    public bool IsComplete  { get; private set; }
    public bool IsFadingOut { get; private set; }

    public Fade(Sprite sprite, bool isFadingOut = false)
    {
      this.sprite      = sprite;
      this.IsFadingOut = isFadingOut;
      this.fadeStep    = 0.01f;
      this.IsComplete  = false;
    }

    #region Public Methods

    public void Start(float? fadeStep = null)
    {
      sprite.Alpha  = (IsFadingOut) ? 1f : 0f;
      this.fadeStep = (fadeStep == null) ? this.fadeStep : fadeStep.Value;
      IsRunning     = true;
      IsComplete    = false;
    }

    public void Stop()
    {
      IsRunning  = false;
      IsComplete = true;
    }

    public void Update(GameTime gameTime)
    {
      if (IsRunning)
      {
        sprite.Alpha += MathHelper.Lerp(0, 1, fadeStep) * ((IsFadingOut) ? -1 : 1);

        if ((IsFadingOut == true && sprite.Alpha <= 0f) || (IsFadingOut == false && sprite.Alpha >= 1f))
        {
          Stop();

          if (Complete != null)
          {
            Complete(this, new EventArgs());
          }
        }
      }
    }

    #endregion
  }
}