using UnityEngine;
using System.Collections;
using Random = System.Random;

public class CrateObstacle : AbstractObstacle
{
  [Header("PowerUp Config")]
  [SerializeField, Range(1, 5)] private int _spawnPowerUpChance;
  Random random = new Random();
  
  public void DecidePowerUpSpawn()
  {
    int spawn = -1;
    float[] _spawnPowerUpChanceList = {0.2f,0.4f,0.6f,0.8f,1.0f};
    double randomDouble = random.NextDouble();
    float randomFloat = (float) randomDouble;
    if (randomFloat < _spawnPowerUpChanceList[_spawnPowerUpChance-1])
    {
      spawn = 1;
    }
    else
    {
      spawn = -1;
    }
    if (spawn == 1)
    {
      PowerUps.SpawnPowerUps();
    }
  }
 
  public override void Spawn(ref Platform platform, int index, float height)
  {
    base.Spawn(ref platform, index, height);
    Transform platformT = platform.transform;
    transform.position = new Vector3(
      platformT.position.x - platformT.localScale.x*0.5f + index + 0.5f,
      platformT.localScale.y*0.5f + height + transform.lossyScale.y*0.5f, 0.0f
    );
  }
}
