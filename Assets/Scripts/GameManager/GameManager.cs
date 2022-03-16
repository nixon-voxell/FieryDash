using UnityEngine;
using Unity.Mathematics;

public partial class GameManager : MonoBehaviour
{
  [SerializeField] private AnimationCurve _incrementalCurve;
  [SerializeField] private float _incrementSpeed = 0.1f;
  private float _incrementValue;
  [SerializeField] private LevelSettings[] _levelSettings;
  [SerializeField] private float _distanceMultiplier;
  [SerializeField] private Camera _mainCamera;

  [SerializeField] ScrollingMaterial[] _scrollingMaterials;
  private static readonly int XScrolling = Shader.PropertyToID("_XScrolling");

  [SerializeField] public Transform playerTransform;
  [SerializeField] public Player player;
  [SerializeField] private float _resetPlayerXPosSpeed;
  private float _playerOriginXPos;

  // current level index
  public int LevelIdx => _levelIdx;
  private int _levelIdx;

  // curr speed of the game
  public float ScoreSpeed => _currSpeed * _distanceMultiplier;
  public float CurrSpeed => _currSpeed;
  private float _currSpeed;
  private float _targetSpeed;

  // actual distance traveled by the player
  public int CurrScore => (int)(_distTraveled * _distanceMultiplier);
  public float DistTraveled => _distTraveled;
  private float _distTraveled;

  // change in distance traveled
  public float DeltaDist => _deltaDist;
  private float _deltaDist;

  public float OffScreenLimit => _offScreenLimit;
  private float _offScreenLimit;

  public static bool GameStarted => _gameStarted;
  private static bool _gameStarted;

  private void Start()
  {
    _incrementValue = 0.0f;
    _currSpeed = 0.0f;
    _distTraveled = 0.0f;
    _offScreenLimit = _mainCamera.orthographicSize / 9 * 16;
    _playerOriginXPos = playerTransform.position.x;
    _gameStarted = false;
  }

  private void Update()
  {
    float dt = Time.deltaTime;
    // keep increment value between 0 and 1
    _incrementValue = math.saturate(_incrementValue + dt*_incrementSpeed);
    _currSpeed = math.lerp(_currSpeed, _targetSpeed, _incrementalCurve.Evaluate(_incrementValue));

    // target speed reached, now we increment speed based on distance
    if (_gameStarted)
    {
      LevelSettings lvlSettings = _levelSettings[_levelIdx];
      if (lvlSettings.endDistance <= (float)CurrScore)
      {
        _targetSpeed = math.lerp(
          lvlSettings.minSpeed,
          lvlSettings.maxSpeed,
          lvlSettings.transitionCurve.Evaluate(
            ((float)CurrScore - lvlSettings.startDistance) / (lvlSettings.endDistance - lvlSettings.startDistance)
          )
        );
      }

      if ((float)CurrScore >= lvlSettings.endDistance && _levelIdx <= _levelSettings.Length)
        _levelIdx++;
    }

    _deltaDist = dt * _currSpeed;

    // if player is being offset to the front due to dash, move faster and move the player backwards
    float3 playerPos = playerTransform.position;
    if (playerPos.x > _playerOriginXPos)
    {
      float targetXPos = math.lerp(playerPos.x, _playerOriginXPos, dt * _resetPlayerXPosSpeed);
      _deltaDist += playerPos.x - targetXPos;
      playerPos.x = targetXPos;
      playerTransform.position = playerPos;
    }

    _distTraveled += _deltaDist;

    for (int sm=0; sm < _scrollingMaterials.Length; sm++)
    {
      for (int m=0; m < _scrollingMaterials[sm].materials.Length; m++)
      _scrollingMaterials[sm].materials[m].SetFloat(
        XScrolling, _distTraveled*_scrollingMaterials[sm].speed*0.5f
      );
    }
  }

  private void OnDisable() => ResetMaterials();
}

[System.Serializable]
public struct ScrollingMaterial
{
  public Material[] materials;
  public float speed;
}