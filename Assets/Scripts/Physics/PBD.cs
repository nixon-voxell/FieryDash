using Unity.Mathematics;

public static class PBD
{
  public static void ApplyExternalForce(
    ref float3 position, ref float3 velocity, in float dt
  )
  {
    // velocity
    position += velocity * dt;
  }

  public static void UpdateVelocity(
    in float3 initialPosition, in float3 predPosition, out float3 velocity, in float dt
  )
  {
    velocity = (predPosition - initialPosition) / dt;
  }
}