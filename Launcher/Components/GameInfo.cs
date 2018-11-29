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

        public GameInfo()
        {
            playCount = new Label()
            {
                ResourceName = "Fonts/GameInfo",
                Text         = "Play Count:  0",
                Position     = new Vector2(100, 300)
            };

            minPlayers = new Label()
            {
                ResourceName = "Fonts/GameInfo",
                Text         = "Min Players: 1",
                Position     = new Vector2(100, 350)
            };

            maxPlayers = new Label()
            {
                ResourceName = "Fonts/GameInfo",
                Text         = "Max Players: 2",
                Position     = new Vector2(100, 400)
            };
        }

        public void LoadContent(ContentManager content)
        {
            playCount.LoadContent(content);
            minPlayers.LoadContent(content);
            maxPlayers.LoadContent(content);
        }
                
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            playCount.Draw(gameTime, spriteBatch);
            minPlayers.Draw(gameTime, spriteBatch);
            maxPlayers.Draw(gameTime, spriteBatch);
        }
    }
}