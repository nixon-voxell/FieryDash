using UnityEngine;
using Voxell.Inspector;

public class PlatformGeneration : MonoBehaviour
{
  [SerializeField, Tooltip("Reference to game manager.")] private GameManager _gameManager;
  [SerializeField] private GameObject _platformPrefab;

  [SerializeField] private PlatformConfig _widthConfig = new PlatformConfig(0.1f, 5.0f, 10.0f);
  [SerializeField] private PlatformConfig _heightConfig = new PlatformConfig(0.3f, 1.0f, 5.0f);

  // a storage of platform objects to be enabled/disabled
  private GameObject[] _platformPool;

  private void Awake()
  {
    _widthConfig.ReSeed();
    _heightConfig.ReSeed();
  }

  private void Update()
  {
    // 
  }
}