using UnityEngine;

public class Platform : MonoBehaviour
{
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
      transform.position.x - SceneLoader.GameManager.DeltaDist, transform.position.y
    );
  }

  private bool IsOffScreen()
  {
    float platformEnd = transform.position.x + transform.localScale.x*0.5f;
    return platformEnd < -SceneLoader.GameManager.OffScreenLimit;
  }
}