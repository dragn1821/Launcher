using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace GameLibrary.Graphics.Effects
{
  public class Flash
  {
    private Sprite sprite;
    private Timer timer;
    private double delay;
    private bool showing = true;

    public bool IsRunning { get; private set; }

    public Flash(Sprite sprite, double delay, bool showSprite = true)
    {
      this.sprite    = sprite;
      this.showing   = showSprite;
      this.delay     = delay;
      this.IsRunning = false;

      timer           = new Timer();
      timer.Complete += OnTimerComplete;
    }

    public void Start()
    {
      if (IsRunning == false)
      {
        IsRunning = true;
        timer.Start(delay);
      }
    }

    public void Stop(bool showSprite = true)
    {
      if (IsRunning)
      {
        IsRunning = false;
        timer.Stop();

        if (showSprite)
        {
          showing = true;
          sprite.Alpha = 1f;
        }
        else
        {
          showing = false;
          sprite.Alpha = 0f;
        }
      }
    }

    public void Update(GameTime gameTime)
    {
      timer.Update(gameTime);
      sprite.Alpha = (showing) ? 1 : 0;
    }

    #region Event Handlers

    private void OnTimerComplete(object sender, EventArgs args)
    {
      showing = !showing;
      timer.Start(delay);
    }

    #endregion
  }
}
