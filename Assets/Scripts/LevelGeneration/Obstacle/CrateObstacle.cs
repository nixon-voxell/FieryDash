using UnityEngine;
using System.Collections;
using Random = System.Random;

public class CrateObstacle : AbstractObstacle
{
  [Header("PowerUp Config")]
  [SerializeField, Range(1, 100)] private int _spawnPowerUpChance;
  Random random = new Random();
  
  public void DecidePowerUpSpawn(/*ref powerups*/)
  {
    int spawn = -1;
    float spawnPowerUpChanceFloat = (float)_spawnPowerUpChance;
    spawnPowerUpChanceFloat /= 100;
    double randomDouble = random.NextDouble();
    float randomFloat = (float) randomDouble;
    if (randomFloat < spawnPowerUpChanceFloat)
    {
      spawn = 1;
    }
    else
    {
      spawn = -1;
    }
    if (spawn == 1)
    {
      //spawnpowerup function
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
