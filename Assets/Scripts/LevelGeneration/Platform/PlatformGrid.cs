using Unity.Mathematics;

public struct PlatformGrid
{
  public const int UNBREAKABLE = 0;
  public const int KILLABLE = 1;
  public const int BREAKABLE = 2;
  public static readonly int3 emptyCell = new int3(-1, -1, -1);

  public int3[] gridMatrix;

  public PlatformGrid(int gridLength)
  {
    gridMatrix = new int3[gridLength];

    // initialize all grid cells as empty
    for (int g=0; g < gridLength; g++)
      gridMatrix[g] = emptyCell;
  }

  public int GetGridHeightAtUnit(int unit)
  {
    int height = 0;
    for (int h=0; h < 3; h++)
    {
      if (gridMatrix[unit][h] != -1) height++;
      else break;
    }

    return height;
  }
}