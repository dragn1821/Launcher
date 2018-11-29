using Microsoft.Xna.Framework;
using System;

namespace GameLibrary.Utilities
{
  public class Timer
  {
    private double currentTime;
    private double secondsToComplete;

    public delegate void TimerComplete(object sender, EventArgs args);

    public event TimerComplete Complete;

    public bool IsRunning { get; private set; }

    #region Public Methods

    public void Start(double secondsToComplete)
    {
      this.currentTime       = 0;
      this.secondsToComplete = secondsToComplete;
      this.IsRunning         = true;
    }

    public void Stop()
    {
      IsRunning = false;
    }

    public void Update(GameTime gameTime)
    {
      if (IsRunning)
      {
        currentTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (currentTime >= secondsToComplete)
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