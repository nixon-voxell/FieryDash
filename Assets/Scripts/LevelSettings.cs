using UnityEngine;

[CreateAssetMenu]
public class LevelSettings : ScriptableObject
{
  public float minSpeed = 1.0f;
  public float maxSpeed = 2.0f;
  public AnimationCurve transitionCurve;
  public float totalDistance;
}