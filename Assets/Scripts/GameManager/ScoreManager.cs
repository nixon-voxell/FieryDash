using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
  private const int MAX_SCORE_COUNT = 10;
  [System.Serializable]
  private class Score : IComparable<Score>
  {
    public int score;
    public int timeTaken;

    public Score(int score, int timeTaken)
    {
      this.score = score;
      this.timeTaken = timeTaken;
    }

    public Score()
    {
      this.score = 0;
      this.timeTaken = 0;
    }

    public int CompareTo(Score other)
    {
      return Comparer<float>.Default.Compare(this.score, other.score);
    }
  }

  [System.Serializable]
  private class ScoreCollection
  {
    public Score[] scores;

    public ScoreCollection(List<Score> scores)
    {
      this.scores = scores.ToArray();
    }
  }

  [SerializeField] private Saver<ScoreCollection> _highScoreSaver;
  // up to 10 high scores only
  [SerializeField] private List<Score> _highScores;
  [SerializeField] private TextMeshProUGUI _textMeshPro;
  [SerializeField] private float _pulsingRate;
  [SerializeField] private float _pulsingSize;

  private Vector3 defaultScale;

  private void Start()
  {
    defaultScale = _textMeshPro.transform.localScale;

    ScoreCollection savedHighScore = _highScoreSaver.ReadFile();
    if (savedHighScore != null) _highScores = savedHighScore.scores.ToList();
    else _highScores = new List<Score> { new Score() };

    UpdateHighScoreDisplay();
  }

  /// <summary>Store top 10 highest score in descending order (highest score first)</summary>
  public void StoreScore(int score, int timeTaken)
  {
    Score newScore = new Score(score, timeTaken);

    _highScores.Add(newScore);
    _highScores.Sort();
    _highScores.Reverse();
    if (_highScores.Count > MAX_SCORE_COUNT) _highScores.RemoveAt(MAX_SCORE_COUNT);

    _highScoreSaver.WriteData(new ScoreCollection(_highScores));
    UpdateHighScoreDisplay();
  }

  private void UpdateHighScoreDisplay()
  {
    _textMeshPro.text = $"High Score: {_highScores[0].score}m\n";
    _textMeshPro.text += $"Time Taken: {_highScores[0].timeTaken}s";
  }

  private void Update()
  {
    _textMeshPro.transform.localScale = defaultScale + defaultScale *
      (0.5f + Mathf.Sin(Time.time * _pulsingRate) * 0.5f) * _pulsingSize;
  }
}
