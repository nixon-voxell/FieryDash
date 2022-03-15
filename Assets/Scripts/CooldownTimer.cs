using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
  [SerializeField] private Player _player;
  [SerializeField] private Material _material;

  private static readonly int Cooldown = Shader.PropertyToID("_Cooldown");

  private void Update()
  {
    _material.SetFloat(
      Cooldown,
      2.0f - _player.DashCooldownTimer/_player.DashCooldown*2.0f
    );
  }

  private void OnDisable()
  {
    _material.SetFloat(Cooldown, 0.0f);
  }
}
