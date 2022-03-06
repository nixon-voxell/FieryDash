using UnityEngine;

public abstract class AbstractObstacleSpawner : MonoBehaviour
{
  [SerializeField] private int _poolCount = 30;
  [SerializeField] private GameObject _obstaclePrefab;
  [SerializeField] private SpawnCondition[] _spawnConditions;

  private protected AbstractObstacle[] _obstaclePool;
  private protected int _poolIdx;

  private protected virtual void Awake()
  {
    _obstaclePool = new AbstractObstacle[_poolCount];
    for (int p=0; p < _poolCount; p++)
    {
      GameObject obstacleObj = Instantiate<GameObject>(_obstaclePrefab, transform);
      obstacleObj.SetActive(false);
      _obstaclePool[p] = obstacleObj.GetComponent<AbstractObstacle>();
      _obstaclePool[p].Init(this);
    }

    _poolIdx = 0;
  }

  /// <summary>Generate obstacles based on the platform grid.</summary>
  /// <param name="platformGrid">the platform grid</param>
  /// <returns>new platform grid after generating new obstacles</returns>
  public abstract PlatformGrid GenerateObstacle(ref PlatformGrid platformGrid);

  private protected int[] GenerateRandomIndices(int unitCount)
  {
    // obtain largest possible spawn condition given a unit count
    int spawnCondIdx = 0;
    for (int s=0; s < _spawnConditions.Length; s++)
    {
      if (unitCount >= _spawnConditions[s].unitCount)
        spawnCondIdx = s;
      else break;
    }

    SpawnCondition spawnCond = _spawnConditions[spawnCondIdx];
    int obstacleCount = Random.Range(0, spawnCond.spawnRange + 1);
    int[] obstacleIndices = new int[obstacleCount];

    // initialize indices with -1 (which means empty slot)
    for (int o=0; o < obstacleCount; o++)
      obstacleIndices[o] = -1;

    for (int o=0; o < obstacleCount; o++)
    {
      int proposedIndex;
      do
      {
        proposedIndex = Random.Range(0, unitCount + 1);
      } while (ArrayContainsIndex(ref obstacleIndices, proposedIndex));

      obstacleIndices[o] = proposedIndex;
    }

    return obstacleIndices;
  }

  /// <summary>Check if a given index exists in a given array.</summary>
  /// <returns>true if exsits, else false</returns>
  private protected bool ArrayContainsIndex(ref int[] array, int idx)
  {
    for (int i=0; i < array.Length; i++)
      if (array[i] == idx) return true;

    return false;
  }

  /// <summary>Increment pool index while making sure that it loops back when loop count is reached.</summary>
  private protected int NextPoolIdx()
  {
    return _poolIdx++ % _poolCount;
  }

  private protected virtual void OnValidate()
  {
    int lastUnitCount = -1;
    for (int s=0; s < _spawnConditions.Length; s++)
    {
      if (_spawnConditions[s].unitCount <= lastUnitCount)
        _spawnConditions[s].unitCount = Mathf.Min(20, lastUnitCount + 1);

      lastUnitCount = _spawnConditions[s].unitCount;
    }
  }
}

[System.Serializable]
public struct SpawnCondition
{
  [Range(0, 20)] public int unitCount;
  public int spawnRange;
}