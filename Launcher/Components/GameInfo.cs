using GameLibrary.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Launcher.Components
{
    public class GameInfo
    {
        private Label playCount;
        private Label minPlayers;
        private Label maxPlayers;
        private Label gameNumber;
        private int maxGames;

        public GameEntry GameEntry
        {
            set
            {
                minPlayers.Text = $"Min Players: {value.Min_Players}";
                maxPlayers.Text = $"Max Players: {value.Max_Players}";
            }
        }

        public GamePlay GamePlay
        {
            set
            {
                playCount.Text = $"Play Count:  {value.PlayCount}";
            }
        }

        public int CurrentGame
        {
            set
            {
                gameNumber.Text = $"Game {value + 1} of {maxGames}";
            }
        }

        public int MaxGames
        {
            set
            {
                maxGames = value;
            }
        }

        public GameInfo()
        {
            playCount = new Label()
            {
                ResourceName = "Fonts/emulogic",
                Text         = "Play Count:  0",
                Color        = Color.WhiteSmoke,
                Position     = new Vector2(100, 300)
            };

            minPlayers = new Label()
            {
                ResourceName = "Fonts/emulogic",
                Text         = "Min Players: 1",
                Color        = Color.LightGray,
                Position     = new Vector2(100, 350)
            };

            maxPlayers = new Label()
            {
                ResourceName = "Fonts/emulogic",
                Text         = "Max Players: 2",
                Color        = Color.DarkGray,
                Position     = new Vector2(100, 400)
            };

            gameNumber = new Label()
            {
                ResourceName = "Fonts/emulogic",
                Text         = "Game 0 of 0",
                Color        = Color.LightGray,
                Position     = new Vector2(100, 450)
            };
        }

        public void LoadContent(ContentManager content)
        {
            playCount.LoadContent(content);
            minPlayers.LoadContent(content);
            maxPlayers.LoadContent(content);
            gameNumber.LoadContent(content);
        }
                
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playCount.Draw(gameTime, spriteBatch);
            minPlayers.Draw(gameTime, spriteBatch);
            maxPlayers.Draw(gameTime, spriteBatch);
            gameNumber.Draw(gameTime, spriteBatch);
        }
    }
}