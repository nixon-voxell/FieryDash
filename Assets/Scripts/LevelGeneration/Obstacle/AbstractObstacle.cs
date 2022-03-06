using UnityEngine;

public abstract class AbstractObstacle : MonoBehaviour
{
  private AbstractObstacleSpawner _spawner;

  /// <summary>Spawn in obstacle based on platform and spawn index.</summary>
  /// <param name="platform">target platform to spawn on</param>
  /// <param name="index">spawn index</param>
  public virtual void Spawn(ref Platform platform, int index)
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
    Debug.Log("Reinitialize");
    transform.SetParent(_spawner.transform);
    transform.position = Vector3.zero;
    transform.localScale = Vector3.one;
    gameObject.SetActive(false);
  }
}

public enum ObstacleType
{
  Unbreakable = 0,
  Killable = 1,
  Breakable = 2
}