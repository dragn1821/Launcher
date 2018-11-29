using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameLibrary.Graphics
{
  public class AnimatedSprite : DisplayObject
  {
    private Texture2D texture;
    private List<AnimatedFrame> frames;
    private int frameIndex;
    private double time;

    //public Vector2 Origin      { get; set; }
    public bool IsPlaying      { get; private set; }
    public bool IsLooping      { get; set; }
    public float FrameTime     { get; set; }
    public int FrameCount      { get { return frames.Count; } }
    public Rectangle Rectangle { get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); } }
    public bool IsComplete     { get { return IsLooping == false && frameIndex == frames.Count - 1; } }

    public AnimatedSprite(int frameWidth, int frameHeight) : base()
    {
      this.frames    = new List<AnimatedFrame>();
      this.Width     = frameWidth;
      this.Height    = frameHeight;
      this.Origin    = Vector2.Zero;
      this.IsPlaying = false;
      this.FrameTime = 0.1f;
    }

    public override void LoadContent(ContentManager content)
    {
      if (string.IsNullOrEmpty(ResourceName))
      {
        throw new ArgumentNullException("The resource name is required for an Animated Sprite.");
      }

      texture = content.Load<Texture2D>(ResourceName);
      frames  = GetFrames();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if (IsPlaying)
      {
        time += gameTime.ElapsedGameTime.TotalSeconds;

        while(time > FrameTime)
        {
          time -= FrameTime;

          if (IsLooping)
          {
            frameIndex = (frameIndex + 1) % frames.Count;
          }
          else
          {
            //only play the animation once.
            frameIndex = Math.Min(frameIndex + 1, frames.Count - 1);
          }
        }

        frames[frameIndex].Draw(spriteBatch, Position, Origin);
      }
    }

    public Vector2 GetCenter()
    {
      if (texture == null)
      {
        throw new Exception("You must load the texture before getting the center.");
      }

      return new Vector2(Position.X + (Width / 2), Position.Y + (Height / 2));
    }

    public void Play()
    {
      IsPlaying  = true;
      frameIndex = 0;
      time       = 0;
    }

    public void Stop()
    {
      IsPlaying = false;
    }

    #region Private Methods

    private List<AnimatedFrame> GetFrames()
    {
      List<AnimatedFrame> frames = new List<AnimatedFrame>();

      for (int y = 0; y < texture.Height; y += Height)
      {
        for (int x = 0; x < texture.Width; x += Width)
        {
          Rectangle source = new Rectangle(x, y, Width, Height);
          frames.Add(new AnimatedFrame(texture, source));
        }
      }

      return frames;
    }

    #endregion
  }
}