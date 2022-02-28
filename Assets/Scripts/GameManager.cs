using UnityEngine;
using Unity.Mathematics;
using Voxell.Inspector;

public class GameManager : MonoBehaviour
{
  [SerializeField] private AnimationCurve _incrementalCurve;
  [SerializeField] private float _incrementSpeed = 0.1f;
  private float _incrementValue;
  [SerializeField] private LevelSettings[] _levelSettings;
  [SerializeField] private Camera _mainCamera;

  [SerializeField] Material[] _scrollingMaterials;
  private static readonly int XScrolling = Shader.PropertyToID("_XScrolling");

  [SerializeField] Transform _playerTransform;
  [SerializeField] float _resetPlayerXPosSpeed;
  private float _playerOriginXPos;

  // current level index
  public int LevelIdx => _levelIdx;
  private int _levelIdx;

  // curr speed of the game
  public float CurrSpeed => _currSpeed;
  private float _currSpeed;
  private float _targetSpeed;

  // actual distance traveled by the player
  public float DistTraveled => _distTraveled;
  private float _distTraveled;

  // change in distance traveled
  public float DeltaDist => _deltaDist;
  private float _deltaDist;

  public float OffScreenLimit => _offScreenLimit;
  private float _offScreenLimit;

  private void Awake()
  {
    _incrementValue = 0.0f;
    _currSpeed = 0.0f;
    _distTraveled = 0.0f;
    _offScreenLimit = _mainCamera.orthographicSize / 9 * 16;
    _playerOriginXPos = _playerTransform.position.x;
  }

  private void Update()
  {
    float dt = Time.deltaTime;
    // keep increment value between 0 and 1
    _incrementValue = math.saturate(_incrementValue + dt*_incrementSpeed);
    _currSpeed = math.lerp(_currSpeed, _targetSpeed, _incrementalCurve.Evaluate(_incrementValue));

    _deltaDist = dt * _currSpeed;

    // if player is being offset to the front due to dash, move faster and move the player backwards
    float3 playerPos = _playerTransform.position;
    if (playerPos.x > _playerOriginXPos)
    {
      float targetXPos = math.lerp(playerPos.x, _playerOriginXPos, dt * _resetPlayerXPosSpeed);
      _deltaDist += playerPos.x - targetXPos;
      playerPos.x = targetXPos;
      _playerTransform.position = playerPos;
    }

    _distTraveled += _deltaDist;

    for (int m=0; m < _scrollingMaterials.Length; m++)
      _scrollingMaterials[m].SetFloat(XScrolling, _distTraveled*0.5f);
  }

  [Button]
  public void LoadLevel(int levelIdx = 0)
  {
    _incrementValue = 0.0f;
    _targetSpeed = _levelSettings[levelIdx].minSpeed;
    _levelIdx = levelIdx;
  }
}