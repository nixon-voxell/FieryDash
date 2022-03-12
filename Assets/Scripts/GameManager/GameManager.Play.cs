using UnityEngine;
using Voxell.Inspector;

public partial class GameManager
{
  [Button]
  public void LoadGame(int levelIdx = 0)
  {
    _incrementValue = 0.0f;
    _targetSpeed = _levelSettings[levelIdx].minSpeed;
    _levelIdx = levelIdx;
    _gameStarted = true;
  }

  [Button]
  public void StopGame()
  {
    for (int sm=0; sm < _scrollingMaterials.Length; sm++)
    {
      for (int m=0; m < _scrollingMaterials[sm].materials.Length; m++)
      _scrollingMaterials[sm].materials[m].SetFloat(XScrolling, 0.0f);
    }

    _distTraveled = 0.0f;
    _gameStarted = false;
  }
}