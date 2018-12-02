using GameLibrary.Input;
using GameLibrary.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameLibrary.Scenes
{
  public abstract class Scene
  {
    protected IGame game;
    protected ContentManager content;
    protected Dictionary<int, Controller> controllers;

    public int ScreenWidth  { get { return game.ScreenWidth; } }
    public int ScreenHeight { get { return game.ScreenHeight; } }


    public Scene(IGame game, ContentManager content, Dictionary<int, Controller> controllers)
    {
      this.game        = game;
      this.content     = new ContentManager(content.ServiceProvider, "Content");
      this.controllers = controllers;
    }

    #region Abstract Methods

    public abstract void LoadContent();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    #endregion

    #region Public Methods

    public virtual void UnloadContent()
    {
      content.Unload();
    }

    #endregion
  }
}