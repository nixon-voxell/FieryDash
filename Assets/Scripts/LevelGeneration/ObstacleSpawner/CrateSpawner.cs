using Unity.Mathematics;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class CrateSpawner : AbstractObstacleSpawner
{
  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);


    for (int i = 0; i < indices.Length; i++)
    {
      if (indices[i] == -1) continue;

      int height = platformGrid.GetGridHeightAtUnit(indices[i]);
      if (height < 2)
      {
        int3[] gridMatrix = platformGrid.GetGridMatrix();
        if (gridMatrix[indices[i]][0] == 0)
        {
          platformGrid.InsertCell(indices[i], height, ObstacleType.Breakable);
          int poolIdx = NextPoolIdx();
          _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
        }
        else if (gridMatrix[indices[i]][0] == -1)
        {
          if (gridMatrix[indices[i] -1][0] != 0 && gridMatrix[indices[i] -1][0] != 1)
          {
            platformGrid.InsertCell(indices[i], height, ObstacleType.Breakable);
            int poolIdx = NextPoolIdx();
            _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
          }
        }
      }
    }
  }
}
