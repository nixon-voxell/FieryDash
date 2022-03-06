using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillableObstaclesPool : MonoBehaviour
{   
  [System.Serializable]
  public class Pool 
  {
    public string tag;
    public GameObject prefab;
    public int amount; 
  }
  public static KillableObstaclesPool Instance;

  public List<Pool> pools;
  public Dictionary<string, Queue<GameObject>> poolDictionary;
  private void Awake()
  {
    Instance = this;

    //creating a pool of objects before placement
    poolDictionary = new Dictionary<string, Queue<GameObject>>();

    foreach (Pool pool in pools)
    {
      Queue<GameObject> objectPool = new Queue<GameObject>();

      for (int p = 0; p < pool.amount; p++)
      {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
      }

      poolDictionary.Add(pool.tag, objectPool);
    }

  }
  public GameObject SpawningFromPool (string tag, Vector3 position, GameObject gameObject)
  {
    GameObject objectToSpawn = poolDictionary[tag].Dequeue();

    objectToSpawn.SetActive(true);
    objectToSpawn.transform.parent = gameObject.transform;
    objectToSpawn.transform.position = position;

    IPooledObjects pooledObj = objectToSpawn.GetComponent<IPooledObjects>();
    if (pooledObj != null)
    {
      pooledObj.OnObjectSpawn();
    }

    poolDictionary[tag].Enqueue(objectToSpawn);
    
    return objectToSpawn;
  }
}
