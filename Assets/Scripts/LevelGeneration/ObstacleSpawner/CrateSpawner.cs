using Unity.Mathematics;

public class CrateSpawner : AbstractObstacleSpawner
{
  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);


    for (int i = 0; i < indices.Length; i++)
    {
      int3[] gridMatrix = platformGrid.GridMatrix;

      int idx = math.clamp(indices[i], 1, platformGrid.UnitCount - 2);

      int height = platformGrid.GetGridHeightAtUnit(idx);
      if (height == 2) continue;

      if (height == 1)
      {
        // insert if the base block is a wall
        if (gridMatrix[idx][0] != (int)ObstacleType.Killable)
        {
          platformGrid.InsertCell(idx, height, ObstacleType.Breakable);
          int poolIdx = NextPoolIdx();
          _obstaclePool[poolIdx].Spawn(ref platform, idx, height);
        }
      } else if (gridMatrix[idx - 1][0] == -1 && gridMatrix[idx + 1][0] == -1)
      {
        // insert if there is no obstacle at the front and back of the block
        platformGrid.InsertCell(idx, height, ObstacleType.Breakable);
        int poolIdx = NextPoolIdx();
        _obstaclePool[poolIdx].Spawn(ref platform, idx, height);
      }
    }
  }
}
