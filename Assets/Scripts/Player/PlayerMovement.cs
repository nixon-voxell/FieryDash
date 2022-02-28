using UnityEngine;
using Unity.Mathematics;
using Voxell.Inspector;

public partial class PlayerMovement : MonoBehaviour
{
  [SerializeField] private LayerMask _solidLayer;
  [SerializeField] private LayerMask _killableLayer;

  [Header("Input Action KeyCodes")]
  [SerializeField] private KeyCode _jumpKeyCode;
  [SerializeField] private KeyCode _dashKeyCode;

  [Header("Physics Settings")]
  // [SerializeField] private float _mass = 1.0f;
  [SerializeField] private float3 _gravity = new float3(0.0f, -9.81f, 0.0f);
  [SerializeField] private float _maxVelocity = 10.0f;
  [SerializeField, Range(0.8f, 1.0f)] private float _damping = 0.98f;
  [SerializeField] private float _contactOffset = 0.015f;

  [Header("Movement Settings")]
  [SerializeField, Range(1, 3)] private int _jumpCount = 2;
  [SerializeField] private float _jumpImpulse;
  [SerializeField] private float _dashImpulse;
  [SerializeField] private float _dashDuration;
  [SerializeField] private float _dashCooldown;
  [SerializeField, Range(0.0f, 1.0f)] private float _dashDecayFactor = 0.6f;

  [Header("States")]
  [SerializeField, InspectOnly] private bool _isGrounded;
  [SerializeField, InspectOnly] private bool _groundDetected;
  [SerializeField, InspectOnly] private bool _isObstructed;
  [SerializeField, InspectOnly] private bool _obstacleDetected;
  // physics state
  private float3 _velocity;
  private float3 _initialPosition, _predPosition;
  // jump state
  private int _jumpsMade;
  // dash state
  public bool IsDashing => _dashTimer > 0.0f;
  private float _dashTimer;
  public float DashCooldownTimer => _dashCooldownTimer;
  private float _dashCooldownTimer;
  // player state
  public bool Dead => _dead;
  private bool _dead;

  // vars
  private const float BOXCAST_HEIGHT = 0.1f;
  private float _halfHeight;
  private Vector2 _down_boxCastOrigin, _down_boxCastSize;
  private Vector2 _right_boxCastOrigin, _right_boxCastSize;
  private RaycastHit2D _down_raycastHit, _right_raycastHit;

  private void Start()
  {
    _down_boxCastSize = new Vector2(transform.localScale.x - _contactOffset, BOXCAST_HEIGHT);
    _right_boxCastSize = new Vector2(BOXCAST_HEIGHT, transform.localScale.y - _contactOffset);
    _halfHeight = transform.localScale.y*0.5f;
    _groundDetected = false;

    // initialize states
    _isGrounded = false;
    _velocity = 0.0f;
    _initialPosition = transform.position;
    _predPosition = _initialPosition;
    _jumpsMade = 0;
    _dashTimer = 0.0f;
    _dashCooldownTimer = 0.0f;
    _dead = false;
  }

  private void Die()
  {
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    // if it hits a killable obstacle, dies
    if ((1 << collision.gameObject.layer & _killableLayer) != 0) Die();
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    if (_down_raycastHit.collider != null)
    {
      Gizmos.DrawLine(_down_boxCastOrigin, _down_raycastHit.point);
      Gizmos.color = Color.green;
      Gizmos.DrawWireCube(_down_raycastHit.point, _down_boxCastSize);
    }

    Gizmos.color = Color.white;
    if (_right_raycastHit.collider != null)
    {
      Gizmos.DrawLine(_right_boxCastOrigin, _right_raycastHit.point);
      Gizmos.color = Color.green;
      Gizmos.DrawWireCube(_right_raycastHit.point, _right_boxCastSize);
    }
  }
}
