using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField] private LevelSettings[] _levelSettings;

  // curr speed of the game
  public float CurrSpeed => _currSpeed;
  private float _currSpeed;

  // actual distance traveled by the player
  private float DistTraveled => _distTraveled;
  private float _distTraveled;
}