using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
  public int currScore => (int)(_gameManager.DistTraveled * distanceMultiplier);
  [SerializeField] private GameManager _gameManager;
  [SerializeField] private float distanceMultiplier = 1.0f;
  [SerializeField] private TextMeshProUGUI _textMeshPro;

  private void Update()
  {
    _textMeshPro.text = currScore.ToString();
  }
}
