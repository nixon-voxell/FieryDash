using System;

public class SpikeSpawner : AbstractObstacleSpawner
{
  private void CheckSpike(ref int[] indices, ref PlatformGrid platformGrid)
  {
    Array.Sort(indices);
    for (int i=1; i < indices.Length -1; i++)
    {
      // check if there are 3 spikes in a row
      if (indices[i] == (indices[i -1] + 1) && indices[i] == (indices[i+1] - 1))
        indices[i+1] = -1;
    }
  }
  

  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);

    /// check for consequential of 3 number
    CheckSpike(ref indices, ref platformGrid);

    for (int i = 0; i < indices.Length; i++)
    {
      if (indices[i] == -1) continue;
      int height = platformGrid.GetGridHeightAtUnit(indices[i]);
      platformGrid.InsertCell(indices[i], height, ObstacleType.Killable);
      int poolIdx = NextPoolIdx();
      _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
    }
  }
}