using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class mathx
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool approximately(float a, float b)
    {
      // If a or b is zero, compare that the other is less or equal to epsilon.
      // If neither a or b are 0, then find an epsilon that is good for
      // comparing numbers at the maximum magnitude of a and b.
      // Floating points have about 7 significant digits, so
      // 1.000001f can be represented while 1.0000001f is rounded to zero,
      // thus we could use an epsilon of 0.000001f for comparing values close to 1.
      // We multiply this epsilon by the biggest magnitude of a and b.
      return math.abs(b - a) < math.max(0.000001f * math.max(math.abs(a), math.abs(b)), math.EPSILON * 8);
    }

    /// <summary>Check if number is close to zero.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool approximately_zero(float a) => math.abs(a) < 0.000001f;
  }
}