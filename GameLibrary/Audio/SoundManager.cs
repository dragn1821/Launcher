using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GameLibrary.Audio
{
  public abstract class SoundManager
  {
    protected Dictionary<string, Song> songs;
    protected Dictionary<string, SoundEffect> soundEffects;

    public SoundManager()
    {
      songs        = new Dictionary<string, Song>();
      soundEffects = new Dictionary<string, SoundEffect>();
    }

    #region Abstract Methods

    public abstract void LoadContent(ContentManager content);

    #endregion

    public void PlaySong(string name, bool loop = false)
    {
      StopSong();
      MediaPlayer.Play(songs[name]);
      MediaPlayer.IsRepeating = loop;
    }

    public void StopSong()
    {
      MediaPlayer.Stop();
    }

    public void PlaySoundEffect(string name)
    {
      soundEffects[name].Play();
    }
  }
}
