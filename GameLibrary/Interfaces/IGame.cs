using GameLibrary.Audio;
using GameLibrary.Graphics;
using GameLibrary.Scenes;
using Logger.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary.Interfaces
{
    public interface IGame
    {
        int ScreenWidth                 { get; }
        int ScreenHeight                { get; }
        Scene NextScene                 { set; }
        SoundManager Sound              { get; }
        GraphicsDevice GraphicsDevice   { get; }
        GraphicsDisplay GraphicsDisplay { get; }
        Settings Settings               { get; }
        ILog Log                        { get; }
    }
}