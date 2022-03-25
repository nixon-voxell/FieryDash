using Unity.Mathematics;

public struct PlatformGrid
{
  public static readonly int3 emptyCell = new int3(-1, -1, -1);

  public int3[] GridMatrix => _gridMatrix;
  public int UnitCount => _gridMatrix.Length;
  private int3[] _gridMatrix;

  public PlatformGrid(int gridLength)
  {
    _gridMatrix = new int3[gridLength];

    // initialize all grid cells as empty
    for (int g=0; g < gridLength; g++)
      _gridMatrix[g] = emptyCell;
  }

  public int GetGridHeightAtUnit(int unit)
  {
    int height = 0;
    for (int h=0; h < 3; h++)
    {
      if (_gridMatrix[unit][h] != -1) height++;
      else break;
    }

    return height;
  }

  public void InsertCell(int x, int y, ObstacleType obstacleType)
  {
    _gridMatrix[x][y] = (int)obstacleType;
  }
}