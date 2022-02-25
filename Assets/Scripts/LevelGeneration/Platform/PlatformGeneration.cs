using UnityEngine;
using Voxell.Inspector;

public class PlatformGeneration : MonoBehaviour
{
  private const int PLATFORM_POOL_COUNT = 10;

  [SerializeField, Tooltip("Reference to game manager.")] private GameManager _gameManager;
  [SerializeField] private GameObject _platformPrefab;

  [SerializeField] private PlatformConfig _widthConfig = new PlatformConfig(0.1f, 5.0f, 10.0f);
  [SerializeField] private PlatformConfig _heightConfig = new PlatformConfig(0.3f, 1.0f, 5.0f);
  [SerializeField, Range(1.0f, 5.0f)] private float _minGap, _maxGap;

  // a storage of platform objects to be enabled/disabled
  private Platform[] _platformPool;
  private int _platformIdx;
  // platform's edge on the right most side of the screen
  private float _lastPlatformReach;
  private float _lastPlatformWidth;

  private void Awake()
  {
    Init();
    _platformPool = new Platform[PLATFORM_POOL_COUNT];
    for (int p=0; p < PLATFORM_POOL_COUNT; p++)
    {
      GameObject platformObj = Instantiate<GameObject>(_platformPrefab, transform);
      platformObj.SetActive(false);
      _platformPool[p] = platformObj.GetComponent<Platform>();
      _platformPool[p].Init(ref _gameManager);
    }
    _lastPlatformReach = -_gameManager.OffScreenLimit;
    _lastPlatformWidth = 0.0f;
  }

  public void Init()
  {
    _widthConfig.ReSeed();
    _heightConfig.ReSeed();
  }

  private void Update()
  {
    if (_lastPlatformReach < _gameManager.OffScreenLimit)
    {
      GenerateNextPlatform();

      _lastPlatformWidth = _platformPool[_platformIdx].transform.localScale.x;

      // x axis location
      float newLocation = _lastPlatformReach + _lastPlatformWidth*0.5f;
      _platformPool[_platformIdx].transform.position = new Vector2(newLocation, 0.0f);

      _lastPlatformReach = newLocation + _lastPlatformWidth*0.5f;
      _lastPlatformReach += Random.Range(_minGap, _maxGap);
    }
  }

  private void LateUpdate()
  {
    _lastPlatformReach -= _gameManager.DeltaDist;
  }

  [Button]
  private void GenerateNextPlatform()
  {
    // modulo to round it up back to zero when max pool count is reached
    _platformIdx = ++_platformIdx % PLATFORM_POOL_COUNT;
    _platformPool[_platformIdx].gameObject.SetActive(true);

    float width, height;
    GeneratePlatformWidthHeight(out width, out height);
    _platformPool[_platformIdx].transform.localScale = new Vector3(width, height, 0.0f);
  }

  private void GeneratePlatformWidthHeight(out float width, out float height)
  {
    width = Mathf.RoundToInt(_widthConfig.GetRandomLength());
    height = _heightConfig.GetRandomLength();
  }
}