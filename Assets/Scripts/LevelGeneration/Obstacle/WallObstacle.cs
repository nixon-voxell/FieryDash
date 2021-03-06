using UnityEngine;

public class WallObstacle : AbstractObstacle
{
  public override void Spawn(ref Platform platform, int index, float height)
  {
    base.Spawn(ref platform, index, height);
    Transform platformT = platform.transform;
    transform.position = new Vector3(
      platformT.position.x - platformT.localScale.x*0.5f + index + 0.5f,
      height, 0.0f
    );
  }
}