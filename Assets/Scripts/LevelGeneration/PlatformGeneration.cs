using UnityEngine;
using Voxell.Inspector;

public class PlatformGeneration : MonoBehaviour
{
  private const int PLATFORM_POOL_COUNT = 10;

  [SerializeField, Tooltip("Reference to game manager.")] private GameManager _gameManager;
  [SerializeField] private GameObject _platformPrefab;

  [SerializeField] private PlatformConfig _widthConfig = new PlatformConfig(0.1f, 5.0f, 10.0f);
  [SerializeField] private PlatformConfig _heightConfig = new PlatformConfig(0.3f, 1.0f, 5.0f);

  // a storage of platform objects to be enabled/disabled
  private Platform[] _platformPool;

  private void Awake()
  {
    Init();
    _platformPool = new Platform[PLATFORM_POOL_COUNT];
    for (int p=0; p < PLATFORM_POOL_COUNT; p++)
    {
      GameObject platformObj = Instantiate<GameObject>(_platformPrefab, transform);
      platformObj.SetActive(false);
      _platformPool[p] = platformObj.GetComponent<Platform>();
    }
  }

  public void Init()
  {
    _widthConfig.ReSeed();
    _heightConfig.ReSeed();
  }

  private void Update()
  {
  }
}