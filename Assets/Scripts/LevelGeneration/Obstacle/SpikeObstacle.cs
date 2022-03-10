using UnityEngine;
public class SpikeObstacle : AbstractObstacle
{
  public override void Spawn(ref Platform platform, int index, int height)
  {
    base.Spawn(ref platform, index, height);
    Transform platformT = platform.transform;
    transform.position = new Vector3(
      platformT.position.x - platformT.localScale.x*0.5f + index + 0.5f,
      platformT.localScale.y * 0.5f + height + transform.lossyScale.y * 1.0f, 0.0f
    );
  }
}