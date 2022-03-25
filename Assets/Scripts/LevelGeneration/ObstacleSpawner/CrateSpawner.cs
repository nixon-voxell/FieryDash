using Unity.Mathematics;

public class CrateSpawner : AbstractObstacleSpawner
{
  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);


    for (int i = 0; i < indices.Length; i++)
    {
      int3[] gridMatrix = platformGrid.GridMatrix;

      int height = platformGrid.GetGridHeightAtUnit(indices[i]);
      if (height == 1)
      {
        if (gridMatrix[indices[i]][0] != (int)ObstacleType.Killable)
        {
          platformGrid.InsertCell(indices[i], height, ObstacleType.Breakable);
          int poolIdx = NextPoolIdx();
          _obstaclePool[poolIdx].Spawn(ref platform, indices[i], height);
        }
      }
    }
  }
}
