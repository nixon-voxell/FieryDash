using UnityEngine;

public class CooldownTimer : MonoBehaviour
{
  [SerializeField] private Material _material;

  private static readonly int Cooldown = Shader.PropertyToID("_Cooldown");

  private void Update()
  {
    _material.SetFloat(
      Cooldown,
      2.0f - SceneLoader.GameManager.player.DashCooldownTimer/
      SceneLoader.GameManager.player.DashCooldown*2.0f
    );
  }

  private void OnDisable()
  {
    _material.SetFloat(Cooldown, 0.0f);
  }
}
