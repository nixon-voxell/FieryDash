using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class CrateSpawner : AbstractObstacleSpawner
{
  private void CheckGridHeight(ref int[] array, ref PlatformGrid platformGrid)
  {
    Array.Sort(array);
    for (int i=1; i < array.Length -1; i++)
    {
      // check if there are 3 spikes in a row
      if (array[i] == (array[i -1] + 1) && array[i] == (array[i+1] - 1))
        array[i+1] = -1;
    }
  }

  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);

    CheckGridHeight(ref indices, ref platformGrid);

    for (int i = 0; i < indices.Length; i++)
    {
      if (indices[i] == -1) continue;

      int height = platformGrid.GetGridHeightAtUnit(indices[i]);
      if (height < 2)
      {
        platformGrid.InsertCell(indices[i], height, ObstacleType.Breakable);
        int poolIdx = NextPoolIdx();
        _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
      }
    }
  }
}
