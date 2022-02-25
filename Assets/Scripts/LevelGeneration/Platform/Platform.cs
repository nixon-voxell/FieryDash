using UnityEngine;

public class Platform : MonoBehaviour
{
  [SerializeField] private GameManager _gameManager;

  private void Update()
  {
    gameObject.SetActive(IsInScreen());
  }

  private void LateUpdate()
  {
    transform.position = new Vector2(
      transform.position.x - _gameManager.DeltaDist, transform.position.y
    );
  }

  public void Init(ref GameManager gameManager)
  {
    _gameManager = gameManager;
  }

  private bool IsInScreen()
  {
    float platformEnd = transform.position.x + transform.localScale.x*0.5f;
    return platformEnd > -_gameManager.OffScreenLimit;
  }
}