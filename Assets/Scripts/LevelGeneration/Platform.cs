using UnityEngine;

public class Platform : MonoBehaviour
{
  private GameManager _gameManager;

  private void Update()
  {
    gameObject.SetActive(IsInScreen());
  }

  private bool IsInScreen()
  {
    float platformEnd = transform.position.x + transform.localScale.x*0.5f;
    return platformEnd > -12.0f;
  }
}