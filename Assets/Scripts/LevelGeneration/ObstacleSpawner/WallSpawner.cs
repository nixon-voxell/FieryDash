using UnityEngine;

public class WallSpawner : AbstractObstacleSpawner
{
  public override void GenerateObstacle(ref PlatformGrid platformGrid, ref Platform platform)
  {
    int[] indices = GenerateRandomIndices(platformGrid.UnitCount);

    for (int i=0; i < indices.Length; i++)
    {
      platformGrid.InsertCell(indices[i], 0, ObstacleType.Unbreakable);
      int poolIdx = NextPoolIdx();
      _obstaclePool[poolIdx].Spawn(
        ref platform, indices[i],
        platform.transform.localScale.y*0.5f + transform.lossyScale.y*0.5f
      );
    }
  }
}