using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField] private LevelSettings[] _levelSettings;
  [SerializeField] private Camera _mainCamera;

  // curr speed of the game
  public float CurrSpeed => _currSpeed;
  private float _currSpeed;

  // actual distance traveled by the player
  public float DistTraveled => _distTraveled;
  private float _distTraveled;

  public float OffScreenLimit => _offScreenLimit;
  private float _offScreenLimit;

  private void Awake()
  {
    _offScreenLimit = _mainCamera.orthographicSize / 9 * 16 + 1;
  }
}