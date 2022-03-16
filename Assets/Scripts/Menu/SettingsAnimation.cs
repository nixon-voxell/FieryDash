using UnityEngine;

public class SettingsAnimation : MonoBehaviour
{
  [SerializeField] private CanvasGroup _bgGroup;
  [SerializeField] private float _backgroundDarken = 0.5f;

  [SerializeField] private Transform _settingsBoard;
  [SerializeField] private float _animateDuration;
  [SerializeField] private float _boardInY;
  private float _boardOutY;

  private void Start()
  {
    _bgGroup.alpha = 0.0f;
    _boardOutY = _settingsBoard.localPosition.y;
  }

  public void Open() 
  {
    _settingsBoard.LeanMoveLocal(new Vector3(
      _settingsBoard.localPosition.x, _boardInY, 0.0f
    ), _animateDuration).setEaseOutBack();

    _bgGroup.LeanAlpha(_backgroundDarken, _animateDuration).setEaseOutQuad();
    _bgGroup.blocksRaycasts = true;
  }

  public void Close()
  {
    _settingsBoard.LeanMoveLocal(new Vector3(
      _settingsBoard.localPosition.x, _boardOutY, 0.0f
    ), _animateDuration).setEaseInBack();

    _bgGroup.LeanAlpha(0.0f, _animateDuration).setEaseOutQuad();
    _bgGroup.blocksRaycasts = false;
  }
}
