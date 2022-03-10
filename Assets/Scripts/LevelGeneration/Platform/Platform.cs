using UnityEngine;

public class Platform : MonoBehaviour
{
  [SerializeField] private GameManager _gameManager;

  private void Update()
  {
    if (IsOffScreen())
    {
      gameObject.SetActive(false);
      AbstractObstacle[] obstacles = transform.GetComponentsInChildren<AbstractObstacle>();
      for (int o=0; o < obstacles.Length; o++)
        obstacles[o].Reinitialize();
    }
    transform.position = new Vector2(
      transform.position.x - _gameManager.DeltaDist, transform.position.y
    );
  }

  public void Init(ref GameManager gameManager)
  {
    _gameManager = gameManager;
  }

  private bool IsOffScreen()
  {
    float platformEnd = transform.position.x + transform.localScale.x*0.5f;
    return platformEnd < -_gameManager.OffScreenLimit;
  }
}