using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpikeObstacle : AbstractObstacle
{
  [SerializeField] private Light2D _light;

  public override void Spawn(ref Platform platform, int index, float height)
  {
    base.Spawn(ref platform, index, height);
    Transform platformT = platform.transform;
    transform.position = new Vector3(
      platformT.position.x - platformT.localScale.x*0.5f + index + 0.5f,
      platformT.localScale.y * 0.5f + height + transform.lossyScale.y * 1.0f, 0.0f
    );
  }

  private void Update()
  {
    float dt = Time.deltaTime*2.0f;
    if (!GameManager.GameStarted)
      _light.intensity = Mathf.Lerp(_light.intensity, 0.0f, dt);
    else _light.intensity = Mathf.Lerp(_light.intensity, 0.2f, dt);
  }
}