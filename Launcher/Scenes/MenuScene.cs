using GameLibrary.Graphics;
using GameLibrary.Graphics.Effects;
using GameLibrary.Input;
using GameLibrary.Interfaces;
using GameLibrary.Scenes;
using GameLibrary.Utilities;
using Launcher.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Launcher.Scenes
{
    public class MenuScene : Scene
    {
        #region DLL Imports for process management

        //https://stackoverflow.com/questions/7162834/determine-if-current-application-is-activated-has-focus
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        //https://stackoverflow.com/questions/2315561/correct-way-in-net-to-switch-the-focus-to-another-application
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        #endregion

        private JsonManager<GameEntry> jsonGameEntries;
        private JsonManager<List<GamePlay>> jsonGamePlays;
        private bool isInputActive = true;
        private Process currentProcess = null;
        private string baseDirectory;
        private List<GameEntry> games;
        private List<GamePlay> gamePlays;
        private int selectedIndex;
        private Label currentGameTitle;
        private List<ResizableSprite> gameImages;
        private Sprite backgroundSprite;
        private Bounce bounce;
        private GameInfo gameInfo;

        public MenuScene(IGame game, ContentManager content, Dictionary<int, Controller> controllers) : base(game, content, controllers)
        {
            this.jsonGameEntries  = new JsonManager<GameEntry>();
            this.jsonGamePlays    = new JsonManager<List<GamePlay>>();
            this.games            = new List<GameEntry>();
            this.gamePlays        = new List<GamePlay>();
            this.gameImages       = new List<ResizableSprite>();
            this.selectedIndex    = 0;
            this.currentGameTitle = new Label()
            {
                ResourceName = "Fonts/GameTitle",
                Text         = ""
            };
            this.backgroundSprite = new Sprite()
            {
                ResourceName = "background"
            };
            this.gameInfo = new GameInfo();

            this.baseDirectory = game.Settings.GameDirectory;

            bounce = new Bounce(currentGameTitle, 0.05f, 1.15f);
            PlayMusic();
        }

        public override void LoadContent()
        {
            LoadGames();
            LoadGamePlays();
            backgroundSprite.LoadContent(content);
            currentGameTitle.LoadContent(content);
            gameInfo.LoadContent(content);

            foreach (ResizableSprite sprite in gameImages)
            {
                sprite.LoadContent(content);
            }
                        
            gameInfo.GameEntry        = games[selectedIndex];
            gameInfo.GamePlay         = FindGamePlay(games[selectedIndex], GetGamePath(games[selectedIndex]));
            UpdateGameTitle(games[selectedIndex]);
            bounce.Start();
        }
        
        public override void Update(GameTime gameTime)
        {
            if (isInputActive)
            {
                for (int index = 1; index <= 2; ++index)
                {
                    //Change selected game:
                    if (controllers[index].IsDirectionUp())
                    {
                        --selectedIndex;

                        if (selectedIndex < 0)
                        {
                            selectedIndex = games.Count - 1;
                        }

                        UpdateGameTitle(games[selectedIndex]);
                        gameInfo.GameEntry = games[selectedIndex];
                        gameInfo.GamePlay  = FindGamePlay(games[selectedIndex], GetGamePath(games[selectedIndex]));

                        PlayClick();
                    }
                    else if (controllers[index].IsDirectionDown())
                    {
                        ++selectedIndex;

                        if (selectedIndex >= games.Count)
                        {
                            selectedIndex = 0;
                        }

                        UpdateGameTitle(games[selectedIndex]);
                        gameInfo.GameEntry = games[selectedIndex];
                        gameInfo.GamePlay  = FindGamePlay(games[selectedIndex], GetGamePath(games[selectedIndex]));

                        PlayClick();
                    }

                    //Select a game to play:
                    if (controllers[index].IsButton1Pressed() || controllers[index].IsButton2Pressed())
                    {
                        isInputActive = false;
                        StopMusic();
                        LaunchGame();
                        UpdateGamePlay();
                        break;
                    }
                }

                //If this menu application is not in focus while input is enabled (no games are running), then set it in focus.
                if (IsMenuInFocus() == false)
                {
                    SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
                }

                bounce.Update(gameTime);
            }
            else if (currentProcess != null && IsProcessInFocus() == false)
            {
                //Change the game process to be in focus.
                SetForegroundWindow(currentProcess.MainWindowHandle);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            backgroundSprite.Draw(gameTime, spriteBatch);
            gameImages[selectedIndex].Draw(gameTime, spriteBatch);
            currentGameTitle.Draw(gameTime, spriteBatch);
            gameInfo.Draw(gameTime, spriteBatch);
        }

        #region Event Handlers

        private void CurrentProcess_Exited(object sender, EventArgs e)
        {
            currentProcess = null;
            isInputActive = true;
            PlayMusic();
        }

        #endregion

        #region Private Methods

        private void UpdateGameTitle(GameEntry entry)
        {
            currentGameTitle.Text     = entry.Title;
            currentGameTitle.Origin   = new Vector2(currentGameTitle.Width / 2, currentGameTitle.Height / 2);
            currentGameTitle.Position = new Vector2((currentGameTitle.Width / 2) + 100, 100);

        }

        private void LaunchGame()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.WorkingDirectory = baseDirectory + games[selectedIndex].Slug + "\\";
            info.FileName         = games[selectedIndex].Executable;
            currentProcess        = new Process();
            currentProcess.StartInfo = info;
            currentProcess.EnableRaisingEvents = true;
            currentProcess.Exited += new EventHandler(CurrentProcess_Exited);
            currentProcess.Start();
        }
        
        private void PlayMusic()
        {
            game.Sound.PlaySong("music", true);
        }

        private void StopMusic()
        {
            game.Sound.StopSong();
        }

        private void PlayClick()
        {
            game.Sound.PlaySoundEffect("click");
        }

        private void LoadGames()
        {
            string[] directories = Directory.GetDirectories(baseDirectory);

            foreach(string directory in directories)
            {
                string filename = directory + game.Settings.MetaDataFileName;

                if (File.Exists(filename))
                {
                    GameEntry entry = jsonGameEntries.Load(filename);
                    games.Add(entry);

                    string imageFile = directory + "\\" + entry.Image_URL;
                    
                    if (File.Exists(imageFile) == false)
                    {
                        imageFile = "Content\\default-image.png";
                    }

                    gameImages.Add(new ResizableSprite(game.GraphicsDevice)
                    {
                        ResourceName = imageFile,
                        Position     = new Vector2(ScreenWidth - game.Settings.ImageWidth - 50, ScreenHeight - game.Settings.ImageHeight - 50),
                        TargetWidth  = game.Settings.ImageWidth,
                        TargetHeight = game.Settings.ImageHeight
                    });
                }
            }
        }

        private void LoadGamePlays()
        {
            gamePlays = jsonGamePlays.Load("game-plays.json");

            foreach(GameEntry entry in games)
            {
                string path   = GetGamePath(entry);
                GamePlay play = FindGamePlay(entry, path);

                if (play == null)
                {
                    gamePlays.Add(new GamePlay()
                    {
                        GamePath  = path,
                        PlayCount = 0
                    });
                }
            }
        }

        private void UpdateGamePlay()
        {
            GameEntry entry = games[selectedIndex];
            GamePlay play   = FindGamePlay(entry, GetGamePath(entry));

            if (play != null)
            {
                ++play.PlayCount;
                gameInfo.GamePlay = play;
                jsonGamePlays.Save("game-plays.json", gamePlays);
            }
        }

        private GamePlay FindGamePlay(GameEntry entry, string path)
        {            
            return gamePlays.Find(x => x.GamePath == path);
        }

        private string GetGamePath(GameEntry entry)
        {
            return game.Settings.GameDirectory + entry.Slug + "\\" + entry.Executable;
        }

        private bool IsProcessInFocus()
        {
            IntPtr activatedHandle = GetForegroundWindow();

            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            int activeProcessID;
            GetWindowThreadProcessId(activatedHandle, out activeProcessID);

            return (activeProcessID == currentProcess.Id);
        }

        private bool IsMenuInFocus()
        {
            IntPtr activatedHandle = GetForegroundWindow();

            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            int activeProcessID;
            GetWindowThreadProcessId(activatedHandle, out activeProcessID);

            return (activeProcessID == Process.GetCurrentProcess().Id);
        }

        #endregion
    }
}