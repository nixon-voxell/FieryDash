using UnityEngine;
using TMPro;

public class GameStats : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _textMeshPro;

  private void Update()
  {
    if (GameManager.GameStarted)
    {
      _textMeshPro.text = $"{SceneLoader.GameManager.CurrScore}m\n";
      _textMeshPro.text += $"{SceneLoader.GameManager.ScoreSpeed.ToString("0.00")}m/s";
    }
  }
}
