using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GameLibrary.Utilities
{
  public class HighScoreManager
  {
    private XmlManager<List<HighScore>> xml;
    private string highscorePath;

    public List<HighScore> HighScores { get; private set; }

    public HighScoreManager(string relativePath)
    {
      string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
      this.xml = new XmlManager<List<HighScore>>();
      this.highscorePath = $@"{path}\{relativePath}";
      LoadScores();
    }

    #region Protected Methods

    protected void LoadScores()
    {
      try
      {
        HighScores = xml.Load(highscorePath);
      }
      catch (Exception exception)
      {
        HighScores = new List<HighScore>();
      }
    }

    #endregion

    #region Public Methods

    public virtual HighScore GetHighestScore()
    {
      return (HighScores.Count == 0) ? CreateEmptyHighScore(0) : HighScores[0];
    }

    public int GetRanking(int score)
    {
      int rank = HighScores.Count + 1;

      for (int index = 0; index < HighScores.Count; ++index)
      {
        if (score > HighScores[index].Score)
        {
          rank = index + 1;
          break;
        }
      }

      return rank;
    }

    public void SaveHighScore(int rank, HighScore score)
    {
      HighScores.Insert(rank - 1, score);

      for (int index = 0; index < HighScores.Count; ++index)
      {
        HighScores[index].Position = index + 1;
      }

      xml.Save(highscorePath, HighScores);
    }

    public List<HighScore> GetTopTen()
    {
      List<HighScore> list = new List<HighScore>();

      for (int index = 0; index < HighScores.Count && index < 10; ++index)
      {
        list.Add(HighScores[index]);
      }

      //Fill in the top 10 if there are not enough positions yet.
      int remaining = 10 - list.Count;

      for (int count = 0; count < remaining; ++count)
      {
        list.Add(CreateEmptyHighScore(list.Count + 1));
      }

      return list;
    }

    #endregion

    #region Private Methods

    private HighScore CreateEmptyHighScore(int position)
    {
      return new HighScore()
      {
        Position = position,
        Initials = "---",
        Score    = 0,
        Player   = 0,
        Recorded = DateTime.Now
      };
    }

    #endregion
  }
}