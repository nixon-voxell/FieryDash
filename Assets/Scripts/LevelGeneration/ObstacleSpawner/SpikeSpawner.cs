using System;
using Random = UnityEngine.Random;
using UnityEngine;
public class SpikeSpawner : AbstractObstacleSpawner
{
  private void CheckSpike(ref int[] array, ref PlatformGrid platformGrid)
  {
    Array.Sort(array);
    for (int i=1; i<array.Length -1; i++)
    {
      if (array[i] == (array[i-1] + 1) && array[i] == (array[i+1] - 1)) 
      {
        int index = array[i+1];
        do
        {
          index = Random.Range(0, platformGrid.UnitCount);
        } while (ArrayContainsIndex(ref array, index));
        array[i+1] = index;
      }

    }
  }
  

  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);

    /// check for consequential of 3 number
    CheckSpike(ref indices, ref platformGrid);

    for (int i = 0; i < indices.Length; i++)
    {
      int height = platformGrid.GetGridHeightAtUnit(indices[i]);
      platformGrid.InsertCell(indices[i], height, ObstacleType.Killable);
      int poolIdx = NextPoolIdx();
      _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
    }
  }
}