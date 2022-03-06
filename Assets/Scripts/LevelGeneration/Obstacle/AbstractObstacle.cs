using UnityEngine;

public abstract class AbstractObstacle : MonoBehaviour
{
  private AbstractObstacleSpawner _spawner;

  /// <summary>Spawn in obstacle based on platform and spawn index.</summary>
  /// <param name="platform">target platform to spawn on</param>
  /// <param name="index">spawn index</param>
  /// <param name="height">spawn height</param>
  public virtual void Spawn(Platform platform, int index, float height)
  {
    gameObject.SetActive(true);
    transform.SetParent(platform.transform);
  }

  public void Init(AbstractObstacleSpawner spawner)
  {
    _spawner = spawner;
  }

  /// <summary>Reset obstacle position, scale, and parent.</summary>
  public void Reinitialize()
  {
    transform.SetParent(_spawner.transform);
    transform.position = Vector3.zero;
    transform.localScale = Vector3.one;
    gameObject.SetActive(false);
  }
}