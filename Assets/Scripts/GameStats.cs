using UnityEngine;
using TMPro;

public class GameStats : MonoBehaviour
{
  [SerializeField] private GameManager _gameManager;
  [SerializeField] private TextMeshProUGUI _textMeshPro;

  private void Update()
  {
    _textMeshPro.text = $"{_gameManager.CurrScore}m\n{_gameManager.ScoreSpeed.ToString("0.00")}";
  }
}
