using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using sharpPDF;
using sharpPDF.Fonts;
using sharpPDF.Enumerators;
using Voxell;

public class ScoreManager : MonoBehaviour
{
  private const int MAX_SCORE_COUNT = 10;
  [System.Serializable]
  private class Score : System.IComparable<Score>
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

    UpdateHighScoreDisplay();
    #if !UNITY_EDITOR
    _highScoreSaver.WriteData(new ScoreCollection(_highScores));

    pdfDocument document = new pdfDocument("Player Report", "Fiery Dash");
    pdfPage page = document.addPage();
    pdfAbstractFont headerFont = document.getFontReference(predefinedFont.csTimesBold);
    pdfAbstractFont subHeaderFont = document.getFontReference(predefinedFont.csTimes);
    pdfAbstractFont contentFont = document.getFontReference(predefinedFont.csCourier);
    page.addText("Fiery Dash - Player Report", 60, 720, headerFont, 28);
    page.addText("Top 10 Highest Scores!", 80, 650, subHeaderFont, 20);

    int yPos = 600;
    for (int hs=0; hs < _highScores.Count; hs++)
    {
      if (_highScores[hs].score == 0) continue;

      string number = (hs+1).ToString();
      string content = $"{number}. Score: {_highScores[hs].score}m, Time Taken: {_highScores[hs].timeTaken}s";
      page.addText(content, 80, yPos, contentFont, 16);
      yPos -= 20;
    }

    document.createPDF(FileUtilx.GetStreamingAssetFilePath("Player Report.pdf"));

    document = null;
    page = null;
    headerFont = null;
    subHeaderFont = null;
    #endif
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
