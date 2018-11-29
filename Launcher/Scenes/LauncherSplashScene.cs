using GameLibrary.Input;
using GameLibrary.Interfaces;
using GameLibrary.Scenes;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Launcher.Scenes
{
    public class LauncherSplashScene : SplashScene
    {
        public LauncherSplashScene(IGame game, ContentManager content, Dictionary<int, Controller> controllers) : base(game, content, controllers)
        {            
            sprite.ResourceName = "splashscene";
        }

        #region Overrides

        public override void NextScene()
        {
            game.NextScene = new MenuScene(game, content, controllers);
        }

        #endregion
    }
}