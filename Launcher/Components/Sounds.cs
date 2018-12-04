using GameLibrary.Audio;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Launcher.Components
{
    public class Sounds : SoundManager
    {
        public override void LoadContent(ContentManager content)
        {
            //music
            songs.Add("music", content.Load<Song>("arcade_dash"));

            //sound effects
            soundEffects.Add("click", content.Load<SoundEffect>("click"));
        }
    }
}