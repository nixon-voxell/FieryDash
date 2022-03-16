using UnityEngine;

public class GameStopper : MonoBehaviour
{
  [SerializeField] private MainMenu _mainMenu;
  [SerializeField] private AnimationCurve _animationCurve;
  [SerializeField] private float _animationDuration;

  [SerializeField] private Transform _leftCover, _rightCover;

  [SerializeField] private Transform open_leftCover, open_rightCover;
  [SerializeField] private Transform close_leftCover, close_rightCover;

  private float _animationTime;
  private float _animationValue;
  private float _incrementMultiplier;

  private void OnEnable()
  {
    _leftCover.localPosition = open_leftCover.localPosition;
    _rightCover.localPosition = open_rightCover.localPosition;
    _animationTime = 0.0f;
    _animationValue = 0.0f;
    _incrementMultiplier = 1.0f;
  }

  private void Update()
  {
    float dt = Time.deltaTime;

    _animationTime += dt * _incrementMultiplier;
    _animationValue = Mathf.InverseLerp(0.0f, _animationDuration, _animationTime);

    if (_animationValue >= 1.0f)
    {
      _animationValue = 1.0f;
      _animationTime = _animationDuration;
      _incrementMultiplier = -1.0f;

      SceneLoader.GameManager.ReloadGame();
      _mainMenu.HideGameplayUI();
    }

    float evalulatedValue = _animationCurve.Evaluate(_animationValue);
    Vector3 leftCoverPos = Vector3.Lerp(
      open_leftCover.localPosition, close_leftCover.localPosition, evalulatedValue
    );
    Vector3 rightCoverPos = Vector3.Lerp(
      open_rightCover.localPosition, close_rightCover.localPosition, evalulatedValue
    );

    _leftCover.localPosition = leftCoverPos;
    _rightCover.localPosition = rightCoverPos;

    if (_animationValue <= 0.0f) gameObject.SetActive(false);
  }

  private void OnDisable()
  {
    _mainMenu.StopGame();
  }
}
