using GameLibrary.Graphics;
using GameLibrary.Graphics.Effects;
using GameLibrary.Input;
using GameLibrary.Interfaces;
using GameLibrary.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameLibrary.Scenes
{
  public abstract class SplashScene : Scene
  {
    protected Sprite sprite;
    protected Timer timer;
    protected Fade fadeIn;
    protected Fade fadeOut;
    protected float startFadeOutDelay = 3f;

    public SplashScene(IGame game, ContentManager content, Dictionary<int, Controller> controllers, float startFadeInDelay = 1f) 
      : base(game, content, controllers)
    {
      sprite = new Sprite() { Alpha = 0f };

      timer           = new Timer();
      timer.Complete += OnTimerComplete;
      timer.Start(startFadeInDelay);

      fadeIn           = new Fade(sprite);
      fadeIn.Complete += OnFadeIn;

      fadeOut = new Fade(sprite, true);
      fadeOut.Complete += OnFadeOut;
    }

    #region Abstract Methods

    public abstract void NextScene();

    #endregion

    #region Event Handlers

    private void OnTimerComplete(object sender, EventArgs args)
    {
      if (fadeIn.IsComplete == false)
      {
        fadeIn.Start();
      }
      else
      {
        fadeOut.Start();
      }
    }

    private void OnFadeIn(object sender, EventArgs args)
    {
      timer.Start(startFadeOutDelay);
    }

    private void OnFadeOut(object sender, EventArgs args)
    {
      NextScene();
    }

    #endregion

    #region Overrides

    public override void LoadContent()
    {
      sprite.LoadContent(content);
      sprite.Position = new Vector2(
        (game.ScreenWidth / 2) - (sprite.Width / 2),
        (game.ScreenHeight / 2) - (sprite.Height / 2)
      );
    }

    public override void Update(GameTime gameTime)
    {
      if (fadeIn.IsComplete && fadeOut.IsComplete == false && fadeOut.IsRunning == false)
      {
        for (int player = 1; player <= controllers.Count; ++player)
        {
          if (controllers[player].IsButton1Pressed() || controllers[player].IsButton2Pressed())
          {
            timer.Stop();
            fadeOut.Start();
            break;
          }
        }
      }

      timer.Update(gameTime);
      fadeIn.Update(gameTime);
      fadeOut.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      sprite.Draw(gameTime, spriteBatch);
    }

    #endregion
  }
}