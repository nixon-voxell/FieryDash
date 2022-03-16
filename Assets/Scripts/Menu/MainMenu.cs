using UnityEngine;

public class MainMenu : MonoBehaviour
{
  [SerializeField] private CanvasGroup _gameplayGroup;
  [SerializeField] private float _animateDuration;

  [Header("Button")]
  [SerializeField] private Transform[] _buttonTransforms;
  [SerializeField] private float _buttonOutX;
  private float _buttonInX;

  [Header("Title")]
  [SerializeField] private Transform _titleTransform;
  [SerializeField] private float _titleOutY;
  private float _titleInY;

  private void Start()
  {
    _buttonInX = _buttonTransforms[0].localPosition.x;
    _titleInY = _titleTransform.localPosition.y;
  }

  public void PlayGame()
  {
    SceneLoader.GameManager.LoadGame();

    _titleTransform.LeanMoveLocalY(_titleOutY, _animateDuration).setEaseInBack().setIgnoreTimeScale(true);

    float delayTime = 0.0f;
    for (int b=0; b < _buttonTransforms.Length; b++)
    {
      _buttonTransforms[b].LeanMoveLocalX(
        _buttonOutX, _animateDuration
      ).setDelay(delayTime).setEaseInBack().setIgnoreTimeScale(true);
      delayTime += 0.2f;
    }
    _gameplayGroup.LeanAlpha(1.0f, _animateDuration).setEaseOutQuad().setDelay(delayTime).setIgnoreTimeScale(true);
  }

  public void StopGame()
  {
    SceneLoader.GameManager.StopGame();
    _gameplayGroup.LeanAlpha(0.0f, _animateDuration).setEaseOutQuad().setIgnoreTimeScale(true);
  }

  public void QuitGame()
  {
    Debug.Log("Quit");
    Application.Quit();
  }
}
