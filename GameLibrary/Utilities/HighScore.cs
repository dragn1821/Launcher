using System;

namespace GameLibrary.Utilities
{
  [Serializable]
  public class HighScore
  {
    public int Position      { get; set; }
    public string Initials   { get; set; }
    public int Score         { get; set; }
    public int Player        { get; set; }
    public DateTime Recorded { get; set; }
  }
}