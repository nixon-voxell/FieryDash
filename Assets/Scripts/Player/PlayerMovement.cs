using UnityEngine;
using Unity.Mathematics;
using Voxell.Inspector;

public class PlayerMovement : MonoBehaviour
{
  [Tooltip("Rigidbody will be activated when the player dies.")]
  [SerializeField] private Rigidbody2D _rigidbody;
  [SerializeField] private LayerMask _solidLayer;
  [SerializeField] private LayerMask _killableLayer;

  [Header("Input Action KeyCodes")]
  [SerializeField] private KeyCode _jumpKeyCode;
  [SerializeField] private KeyCode _dashKeyCode;

  [Header("Movement Settings")]
  [SerializeField] private float _mass = 1.0f;
  [SerializeField] private float3 _gravity = new float3(0.0f, -9.81f, 0.0f);
  [SerializeField] private float _maxVelocity = 10.0f;
  [SerializeField, Range(0.8f, 1.0f)] private float _damping = 0.98f;
  [SerializeField] private float _jumpImpulse;
  [SerializeField] private float _dashImpulse;
  [SerializeField] private float _dashDuration;
  [SerializeField] private float _dashCooldown;
  [SerializeField, Range(1, 3)] private int _jumpCount = 2;
  [SerializeField] private float _landOffset = 0.015f;

  [Header("States")]
  [SerializeField, InspectOnly] private bool _isGrounded;
  [SerializeField, InspectOnly] private bool _groundDetected;
  private float3 _velocity;
  private float3 _initialPosition, _predPosition;
  private int _jumpsMade;
  private float _dashTimer, _dashCooldownTimer;

  // vars
  private const float BOXCAST_HEIGHT = 0.1f;
  private float _halfHeight;
  private Vector2 _boxCastSize, _boxCastOrigin;
  private RaycastHit2D _raycastHit;

  private void Start()
  {
    _rigidbody.isKinematic = true;
    _boxCastSize = new Vector2(transform.localScale.x, BOXCAST_HEIGHT);
    _halfHeight = transform.localScale.y*0.5f;
    _groundDetected = false;

    // initialize states
    _isGrounded = false;
    _velocity = 0.0f;
    _initialPosition = transform.position;
    _predPosition = _initialPosition;
    _jumpsMade = 0;
    _dashTimer = 0.0f;
  }

  private void Update()
  {
    float dt = Time.deltaTime;

    _boxCastOrigin = (Vector2)transform.position - new Vector2(0.0f, _halfHeight);
    _raycastHit = Physics2D.BoxCast(
      _boxCastOrigin, _boxCastSize, 0.0f, Vector2.down, 5.0f, _solidLayer
    );

    _groundDetected = _raycastHit.collider != null;
    _isGrounded = _raycastHit.distance < _landOffset && _groundDetected;


    _initialPosition = transform.position;
    _predPosition = transform.position;

    // reduce one addtional jumps if it is already on the air
    if (!_isGrounded) _jumpsMade = math.max(_jumpsMade, 1);

    HandleJump();
    HandleDash();

    // apply gravity and external force
    _velocity += _gravity * dt;
    // remove downwards velocity if it is already grounded
    if (_isGrounded)
    {
      _velocity.y = math.max(_velocity.y, 0.0f);
      _jumpsMade = 0;
    }

    // freeze vertically when dashing
    if (_dashTimer > 0.0f)
    {
      _dashTimer -= dt;
      _velocity.y = 0.0f;
    } else _velocity.x *= 0.5f;

    if (_dashCooldownTimer > 0.0f) _dashCooldownTimer -= dt;

    PBD.ApplyExternalForce(ref _predPosition, ref _velocity, dt);
    

    // continuous collision detection and correction
    if (_groundDetected)
    {
      float fallHeight = _initialPosition.y - _predPosition.y;
      float corr = math.max(fallHeight - _raycastHit.distance - BOXCAST_HEIGHT*0.5f, 0.0f);
      _predPosition.y += corr;
    }

    // udpate velocity
    PBD.UpdateVelocity(in _initialPosition, in _predPosition, out _velocity, dt);
    _velocity *= _damping;
    _velocity = math.clamp(_velocity, -_maxVelocity, _maxVelocity);

    transform.position = _predPosition;
  }

  private void HandleJump()
  {
    if (!Input.GetKeyDown(_jumpKeyCode)) return;
    if (_jumpsMade++ < _jumpCount) _velocity.y = _jumpImpulse;
  }

  private void HandleDash()
  {
    if (!Input.GetKeyDown(_dashKeyCode)) return;
    if (_dashTimer > 0.0f || _dashCooldownTimer > 0.0f) return;
    _velocity.x += _dashImpulse;
    _dashTimer = _dashDuration;
    _dashCooldownTimer = _dashCooldown;
  }

  private void Die()
  {
    _rigidbody.isKinematic = false;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    // if it hits a killable obstacle, dies
    if ((1 << collision.gameObject.layer & _killableLayer) != 0) Die();
  }

  private void OnDrawGizmos()
  {
    if (_raycastHit.point == Vector2.zero) return;
    Gizmos.DrawLine(_boxCastOrigin, _raycastHit.point);
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(_raycastHit.point, _boxCastSize);
  }
}
