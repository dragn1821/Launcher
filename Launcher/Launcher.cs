﻿using GameLibrary;
using GameLibrary.Audio;
using GameLibrary.Graphics;
using GameLibrary.Input;
using GameLibrary.Interfaces;
using GameLibrary.Scenes;
using GameLibrary.Utilities;
using Launcher.Components;
using Launcher.Scenes;
using Logger;
using Logger.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Launcher
{
    public class Launcher : Game, IGame
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Scene currentScene = null;        
        private Dictionary<int, Controller> controllers;
        private SoundManager sound;

        #region IGame implementation

        public Scene NextScene                 { private get; set; }
        public Camera Camera                   { get; private set; }
        public int ScreenWidth                 { get { return Camera.Width; } }
        public int ScreenHeight                { get { return Camera.Height; } }       
        public SoundManager Sound              { get { return sound; } }
        public GraphicsDisplay GraphicsDisplay { get; private set; }
        public Settings Settings               { get; private set; }
        public ILog Log                        { get; private set; }
        public KeyboardInput KeyboardInput     { get; private set; }

        #endregion

        public Launcher()
        {
            graphics              = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {       
            Window.Position            = new Point(0, 0);
            Window.IsBorderless        = true;
            JsonManager<Settings> json = new JsonManager<Settings>();
            Settings                   = json.Load("settings.json");
            Log                        = new Log("Log", Settings.LogPath);
            Camera                     = new Camera(GraphicsDevice, Settings.CameraWidth, Settings.CameraHeight);
            GraphicsDisplay            = new GraphicsDisplay(graphics, Window, Camera);
            GraphicsDisplay.SetResolution(Settings.ScreenWidth, Settings.ScreenHeight);
            Log.WriteLine("====> Starting Launcher.");
            Log.WriteLine("====> Initialization complete.");
            ValidateSettings();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch   = new SpriteBatch(GraphicsDevice);
            KeyboardInput = new KeyboardInput();
            controllers   = new Dictionary<int, Controller>();
            sound         = new Sounds();
            sound.LoadContent(Content);

            controllers.Add(1, new Controller(
                new KeyboardMapping(
                    Keys.Up,
                    Keys.Right,
                    Keys.Down,
                    Keys.Left,
                    Keys.OemQuestion,
                    Keys.OemPeriod),
                KeyboardInput));

            controllers.Add(2, new Controller(
                new KeyboardMapping(
                    Keys.W,
                    Keys.D,
                    Keys.S,
                    Keys.A,
                    Keys.D1,
                    Keys.OemTilde),
                KeyboardInput));

            NextScene = new LauncherSplashScene(this, Content, controllers);

            Log.WriteLine("====> LoadContent complete.");
        }

        protected override void UnloadContent()
        {
            currentScene.UnloadContent();
            Content.Unload();
            Log.WriteLine("====> UnloadContent complete.");
            Log.WriteLine("====> Exiting Launcher.");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardInput.Update(gameTime);

            if (KeyboardInput.IsKeyPressed(Keys.End))
            {
                Exit();
            }

            try
            { 
                if (NextScene != null)
                {
                    if (currentScene != null)
                    {
                        currentScene.UnloadContent();
                    }

                    currentScene = NextScene;
                    NextScene    = null;

                    currentScene.LoadContent();
                }
            
                currentScene.Update(gameTime);
            }
            catch(Exception exception)
            {
                Log.WriteException(exception);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Camera.Draw(gameTime, spriteBatch, currentScene);
            GraphicsDisplay.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrEmpty(Settings.GameDirectory) || Directory.Exists(Settings.GameDirectory) == false)
            {
                throw new Exception("The 'GameDirectory' must have a value and point to an existing directory of games.");
            }
        }
    }
}