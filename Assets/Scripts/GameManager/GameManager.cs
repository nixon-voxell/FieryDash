using UnityEngine;
using Unity.Mathematics;
using Voxell.Mathx;

using Random = UnityEngine.Random;

public partial class GameManager : MonoBehaviour
{
  [Header("Menu & Managers")]
  [SerializeField] private KeyCode _pauseKey;
  [SerializeField] private PopInAnimation _pauseMenuAnimation;
  [SerializeField] private VolumeManager _volumeManager;
  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioClip[] _audioClips;
  [SerializeField] private int _audioClipIdx;

  [Header("Game")]
  public GameStopper gameStopper;
  [SerializeField] private ScoreManager _scoreManager;
  [SerializeField] private AnimationCurve _incrementalCurve;
  [SerializeField] private float _incrementSpeed = 0.1f;
  private float _incrementValue;
  [SerializeField] private LevelSettings[] _levelSettings;
  [SerializeField] private float _distanceMultiplier;
  [SerializeField] private Camera _mainCamera;

  [SerializeField] ScrollingMaterial[] _scrollingMaterials;
  private static readonly int XScrolling = Shader.PropertyToID("_XScrolling");

  [SerializeField] public Player player;
  [SerializeField] public PlatformGeneration platformGeneration;
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
  public int CurrScore => (int)(AccurateCurrScore);
  public float AccurateCurrScore => _distTraveled * _distanceMultiplier;
  public float DistTraveled => _distTraveled;
  private float _distTraveled;
  private float _timeTaken;

  // change in distance traveled
  public float DeltaDist => _deltaDist;
  private float _deltaDist;

  public float OffScreenLimit => _offScreenLimit;
  private float _offScreenLimit;

  public static bool GameStarted => _gameStarted;
  private static bool _gameStarted;

  public static bool GamePaused => _gamePaused;
  private static bool _gamePaused;

  private void Start()
  {
    _incrementValue = 0.0f;
    _currSpeed = 0.0f;
    _distTraveled = 0.0f;
    _offScreenLimit = _mainCamera.orthographicSize / 9 * 16;
    _playerOriginXPos = player.transform.position.x;
    _gameStarted = false;
    _gamePaused = false;
    _audioClipIdx = 0;

    MathUtil.ShuffleArray<AudioClip>(ref _audioClips, (uint)Random.Range(1, 100));
    _audioSource.clip = _audioClips[_audioClipIdx];
    _audioSource.Play();
    NextAudioClipIdx();
  }

  private void LateUpdate()
  {
    if (_audioSource.isPlaying == false)
    {
      _audioSource.clip = _audioClips[_audioClipIdx];
      _audioSource.Play();
      NextAudioClipIdx();
    }
  }

  private void Update()
  {
    float dt = Time.deltaTime;

    if (!_gamePaused)
    {
      // keep increment value between 0 and 1
      _incrementValue = math.saturate(_incrementValue + dt*_incrementSpeed);
      _currSpeed = math.lerp(_currSpeed, _targetSpeed, _incrementalCurve.Evaluate(_incrementValue));
    }

    // target speed reached, now we increment speed based on distance
    if (_gameStarted)
    {
      LevelSettings lvlSettings = _levelSettings[_levelIdx];
      if (AccurateCurrScore <= lvlSettings.endScore)
      {
        _targetSpeed = math.lerp(
          lvlSettings.minSpeed,
          lvlSettings.maxSpeed,
          lvlSettings.transitionCurve.Evaluate(
            (AccurateCurrScore - (float)lvlSettings.startScore) /
            ((float)lvlSettings.endScore - (float)lvlSettings.startScore)
          )
        );
      }

      if (AccurateCurrScore >= lvlSettings.endScore && _levelIdx < _levelSettings.Length-1)
        _levelIdx++;

      if (Input.GetKeyDown(_pauseKey))
        if (!_gamePaused) PauseGame();
    }

    _deltaDist = dt * _currSpeed;

    // if player is being offset to the front due to dash, move faster and move the player backwards
    float3 playerPos = player.transform.position;
    if (playerPos.x > _playerOriginXPos)
    {
      float targetXPos = math.lerp(playerPos.x, _playerOriginXPos, dt * _resetPlayerXPosSpeed);
      _deltaDist += playerPos.x - targetXPos;
      playerPos.x = targetXPos;
      player.transform.position = playerPos;
    }

    _distTraveled += _deltaDist;
    _timeTaken += dt;

    for (int sm=0; sm < _scrollingMaterials.Length; sm++)
    {
      for (int m=0; m < _scrollingMaterials[sm].materials.Length; m++)
      _scrollingMaterials[sm].materials[m].SetFloat(
        XScrolling, _distTraveled*_scrollingMaterials[sm].speed*0.5f
      );
    }
  }

  private void OnDisable() => ResetMaterials();

  public void ResumeGame()
  {
    Time.timeScale = 1.0f;
    _gamePaused = false;
    _pauseMenuAnimation.Close();
    _volumeManager.DisablePauseCutoff();
  }

  public void PauseGame()
  {
    Time.timeScale = 0.0f;
    _gamePaused = true;
    _pauseMenuAnimation.Open();
    _volumeManager.EnablePauseCutoff();
  }

  private void NextAudioClipIdx()
  {
    _audioClipIdx = (_audioClipIdx + 1) % _audioClips.Length;
  }
}

[System.Serializable]
public struct ScrollingMaterial
{
  public Material[] materials;
  public float speed;
}