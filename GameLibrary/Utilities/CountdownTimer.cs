using Microsoft.Xna.Framework;
using System;

namespace GameLibrary.Utilities
{
  public class CountdownTimer
  {
    public delegate void TimerComplete(object sender, EventArgs args);

    public event TimerComplete Complete;

    public TimeSpan Value  { get; private set; }
    public bool IsRunning  { get; private set; }
    public bool IsComplete { get; private set; }

    public CountdownTimer(TimeSpan value)
    {
      this.Value = value;
    }

    #region Public Methods

    public void Start()
    {
      IsRunning  = true;
      IsComplete = false;
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
        if (Value.TotalSeconds > 0)
        {
          Value = Value.Subtract(new TimeSpan(gameTime.ElapsedGameTime.Ticks));

          if (Value.TotalSeconds < 0)
          {
            Value = new TimeSpan();
            Stop();

            if (Complete != null)
            {
              Complete(this, new EventArgs());
            }
          }
        }
      }
    }
    
    #endregion
  }
}