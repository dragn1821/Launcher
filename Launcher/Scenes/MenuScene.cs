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
using Microsoft.Xna.Framework.Input;
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

        private const int PIXELS_FOR_IMAGE_POSITIONING = 80;
        private const int SELECTED_PADDING             = 20;

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
        private ResizableSprite selectedImage;
        private ResizableSprite previousImage1;
        private ResizableSprite previousImage2;
        private ResizableSprite nextImage1;
        private ResizableSprite nextImage2;
        private Rectangle selectedImageHighlight;
        private Texture2D pixel;
        private float selectedImageHighlightAlpha;
        private Rectangle titleBackground;
        private float titleBackgroundAlpha;
        private Sprite logo;

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
            this.logo = new Sprite()
            {
                ResourceName = "stl-arcade-jam-logo"
            };
            this.gameInfo = new GameInfo();

            this.baseDirectory  = game.Settings.GameDirectory.Replace('/', '\\');
            this.selectedImageHighlightAlpha = 0.7f;
            this.titleBackgroundAlpha        = 0.6f;

            bounce = new Bounce(currentGameTitle, 0.05f, 1.15f);
        }

        public override void LoadContent()
        {
            LoadGames();
            LoadGamePlays();
            backgroundSprite.LoadContent(content);
            logo.LoadContent(content);
            currentGameTitle.LoadContent(content);
            gameInfo.LoadContent(content);
            pixel = content.Load<Texture2D>("1px");

            foreach (ResizableSprite sprite in gameImages)
            {
                sprite.LoadContent(content);
            }

            int x = ScreenWidth - game.Settings.ImageWidth - PIXELS_FOR_IMAGE_POSITIONING - SELECTED_PADDING;

            selectedImageHighlight = new Rectangle(x, (ScreenHeight / 2) - (game.Settings.ImageHeight / 2) - SELECTED_PADDING, game.Settings.ImageWidth + (SELECTED_PADDING * 2), game.Settings.ImageHeight + (SELECTED_PADDING * 2));
            titleBackground        = new Rectangle(0, 47, ScreenWidth, 106);
            logo.Position          = new Vector2((x / 2) - (logo.Width / 2), ScreenHeight - logo.Height);

            gameInfo.MaxGames = games.Count;

            UpdateCurrentGameSelection();
            bounce.Start();
        }

        public override void UnloadContent()
        {
            if (currentProcess != null)
            {
                CloseGame("UnloadContent");
            }

            base.UnloadContent();
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

                        UpdateGameSelection();
                    }
                    else if (controllers[index].IsDirectionDown())
                    {
                        ++selectedIndex;

                        if (selectedIndex >= games.Count)
                        {
                            selectedIndex = 0;
                        }

                        UpdateGameSelection();
                    }

                    //Select a game to play:
                    if (controllers[index].IsButton1Pressed() || controllers[index].IsButton2Pressed() || game.KeyboardInput.IsKeyPressed(Keys.Escape))
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
            logo.Draw(gameTime, spriteBatch);
            previousImage2.Draw(gameTime, spriteBatch);
            previousImage1.Draw(gameTime, spriteBatch);
            nextImage2.Draw(gameTime, spriteBatch);
            nextImage1.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(pixel, selectedImageHighlight, Color.White * selectedImageHighlightAlpha);
            selectedImage.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(pixel, titleBackground, Color.Black * titleBackgroundAlpha);
            currentGameTitle.Draw(gameTime, spriteBatch);
            gameInfo.Draw(gameTime, spriteBatch);
        }

        #region Event Handlers

        private void CurrentProcess_Exited(object sender, EventArgs e)
        {
            game.Log.WriteLine($"Exiting game: {Path.Combine(baseDirectory, games[selectedIndex].Slug, games[selectedIndex].Executable)}");
            currentProcess = null;
            isInputActive = true;
            PlayMusic();
        }
        
        #endregion

        #region Private Methods

        private int GetPreviousIndex(int number)
        {
            int newIndex = selectedIndex - number;

            if (newIndex < 0)
            {
                newIndex = gameImages.Count - (newIndex * -1);
            }

            return newIndex;
        }

        private int GetNextIndex(int number)
        {
            int newIndex = number + selectedIndex;

            if (newIndex >= gameImages.Count)
            {
                newIndex = Math.Abs(gameImages.Count - newIndex);
            }

            return newIndex;
        }

        private void UpdateGameTitle(GameEntry entry)
        {
            currentGameTitle.Text     = entry.Title;
            currentGameTitle.Origin   = new Vector2(currentGameTitle.Width / 2, currentGameTitle.Height / 2);
            currentGameTitle.Position = new Vector2((currentGameTitle.Width / 2) + 100, 100);
        }
               
        private void UpdateGameSelection()
        {
            UpdateCurrentGameSelection();            
            PlayClick();
            game.Log.WriteLine($"Changed selected game: {Path.Combine(baseDirectory, games[selectedIndex].Slug, games[selectedIndex].Executable)}");
        }

        private void UpdateCurrentGameSelection()
        {
            //Get selected image and resize/reposition.
            selectedImage              = gameImages[selectedIndex];
            selectedImage.TargetWidth  = game.Settings.ImageWidth;
            selectedImage.TargetHeight = game.Settings.ImageHeight;
            selectedImage.Position     = new Vector2(ScreenWidth - game.Settings.ImageWidth - PIXELS_FOR_IMAGE_POSITIONING, (ScreenHeight / 2) - (game.Settings.ImageHeight / 2));

            //Get one level before and after selected image and resize/reposition.
            if (gameImages.Count > 2)
            { 
                previousImage1              = gameImages[GetPreviousIndex(1)];
                previousImage1.TargetWidth  = game.Settings.ImageWidth - PIXELS_FOR_IMAGE_POSITIONING;
                previousImage1.TargetHeight = game.Settings.ImageHeight - PIXELS_FOR_IMAGE_POSITIONING;
                previousImage1.Position     = new Vector2(selectedImage.Position.X + (PIXELS_FOR_IMAGE_POSITIONING / 2), selectedImage.Position.Y - PIXELS_FOR_IMAGE_POSITIONING);

                nextImage1              = gameImages[GetNextIndex(1)];
                nextImage1.TargetWidth  = game.Settings.ImageWidth - PIXELS_FOR_IMAGE_POSITIONING;
                nextImage1.TargetHeight = game.Settings.ImageHeight - PIXELS_FOR_IMAGE_POSITIONING;
                nextImage1.Position     = new Vector2(selectedImage.Position.X + (PIXELS_FOR_IMAGE_POSITIONING / 2), selectedImage.Position.Y + (PIXELS_FOR_IMAGE_POSITIONING * 2));
            }

            //Get two levels before and after selected image and resize/reposition.
            if (gameImages.Count > 5)
            { 
                previousImage2              = gameImages[GetPreviousIndex(2)];
                previousImage2.TargetWidth  = game.Settings.ImageWidth - (PIXELS_FOR_IMAGE_POSITIONING * 2);
                previousImage2.TargetHeight = game.Settings.ImageHeight - (PIXELS_FOR_IMAGE_POSITIONING * 2);
                previousImage2.Position     = new Vector2(selectedImage.Position.X + PIXELS_FOR_IMAGE_POSITIONING, selectedImage.Position.Y - (PIXELS_FOR_IMAGE_POSITIONING * 2));

                nextImage2              = gameImages[GetNextIndex(2)];
                nextImage2.TargetWidth  = game.Settings.ImageWidth - (PIXELS_FOR_IMAGE_POSITIONING * 2);
                nextImage2.TargetHeight = game.Settings.ImageHeight - (PIXELS_FOR_IMAGE_POSITIONING * 2);
                nextImage2.Position     = new Vector2(selectedImage.Position.X + PIXELS_FOR_IMAGE_POSITIONING, selectedImage.Position.Y + (PIXELS_FOR_IMAGE_POSITIONING * 4));
            }

            UpdateGameTitle(games[selectedIndex]);
            gameInfo.CurrentGame = selectedIndex;
            gameInfo.GameEntry   = games[selectedIndex];
            gameInfo.GamePlay    = FindGamePlay(games[selectedIndex], GetGamePath(games[selectedIndex]));
        }

        private void LaunchGame()
        {
            try
            {
                if (currentProcess != null)
                {
                    CloseGame("LaunchGame");
                }
                               
                game.Log.WriteLine($"Launching game: {Path.Combine(baseDirectory, games[selectedIndex].Slug, games[selectedIndex].Executable)}");
                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = Path.Combine(baseDirectory, games[selectedIndex].Slug);
                info.FileName         = games[selectedIndex].Executable;
                currentProcess        = new Process();
                currentProcess.StartInfo = info;
                currentProcess.EnableRaisingEvents = true;
                currentProcess.Exited += new EventHandler(CurrentProcess_Exited);
                currentProcess.Start();
            }
            catch(Exception exception)
            {
                game.Log.WriteException(exception);
                CurrentProcess_Exited(this, new EventArgs());                
            }
        }

        private void CloseGame(string methodName)
        {
            currentProcess.Close();
            currentProcess = null;
            game.Log.WriteLine($"{methodName}: Exiting game: {Path.Combine(baseDirectory, games[selectedIndex].Slug, games[selectedIndex].Executable)}");
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
                string filename = Path.Combine(directory, game.Settings.MetaDataFileName);

                if (File.Exists(filename))
                {
                    GameEntry entry  = jsonGameEntries.Load(filename);
                    entry.Executable = entry.Executable.Replace('/', '\\');
                    games.Add(entry);                    

                    string imageFile = Path.Combine(directory, entry.Image_URL);
                    
                    if (File.Exists(imageFile) == false)
                    {
                        imageFile = "Content\\default-image.png";
                    }

                    gameImages.Add(new ResizableSprite(game.GraphicsDevice)
                    {
                        ResourceName = imageFile.Replace('/', '\\')
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
            return Path.Combine(baseDirectory, entry.Slug, entry.Executable);
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