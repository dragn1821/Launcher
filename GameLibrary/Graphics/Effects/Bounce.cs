using Microsoft.Xna.Framework;

namespace GameLibrary.Graphics.Effects
{
    public class Bounce
    {
        private DisplayObject obj;
        private float maxscale;
        private float step;

        private double time;
        private double delay;

        public bool IsRunning { get; private set; }
        public bool IsGrowing { get; private set; }

        public Bounce(DisplayObject obj, double delay, float maxScale)
        {
            this.obj      = obj;
            this.maxscale = maxScale;
            this.step     = 0.002f;
            this.time     = 0;
            this.delay    = delay;
        }

        public void Start(float? step = null)
        {
            if (IsRunning == false)
            {
                obj.Scale = 1f;
                this.step = (step == null) ? this.step : step.Value;
                IsRunning = true;
                IsGrowing = true;
            }
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Update(GameTime gameTime)
        {
            if (IsRunning)
            {
                time += gameTime.ElapsedGameTime.TotalSeconds;

                if (time > delay)
                {
                    if (obj.Scale < 1.0f || obj.Scale > maxscale)
                    {
                        IsGrowing = !IsGrowing;
                    }

                    int direction = (IsGrowing) ? 1 : -1;
                    obj.Scale    += (direction * step);
                    time          = 0;
                }
            }
        }
    }
}