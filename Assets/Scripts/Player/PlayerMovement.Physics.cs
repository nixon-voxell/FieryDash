using UnityEngine;
using Unity.Mathematics;

public partial class PlayerMovement
{
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

  private void LateUpdate()
  {
    float dt = Time.deltaTime;
    float3 currPosition = transform.position;
    if (currPosition.y < 0.0f || currPosition.x < -_gameManager.OffScreenLimit) _dead = true;
    if (_dead) { Die(); return; }

    // reduce one addtional jumps if it is already on the air
    if (!_isGrounded) _jumpsMade = math.max(_jumpsMade, 1);

    _initialPosition = currPosition;
    _predPosition = currPosition;

    // apply gravity and external force
    _velocity += _gravity * dt;

    HandleJump();
    HandleDash();

    // remove downwards velocity if it is already grounded
    if (_isGrounded)
    {
      _velocity.y = math.max(_velocity.y, 0.0f);
      _jumpsMade = 0;
    }

    // remove downwards velocity when dashing
    if (IsDashing)
    {
      _dashTimer -= dt;
      _velocity.y = math.max(_velocity.y, 0.0f);
    } else _velocity.x *= _dashDecayFactor; // exponentially decrease horizontal velocity after dashing

    if (_dashCooldownTimer > 0.0f) _dashCooldownTimer -= dt;

    PBD.ApplyExternalForce(ref _predPosition, ref _velocity, dt);

    // continuous collision detection and correction
    if (_groundDetected)
    {
      float fallHeight = _initialPosition.y - _predPosition.y;
      float corr = math.max(fallHeight - _down_raycastHit.distance - BOXCAST_THICKNESS*0.5f, 0.0f);
      _predPosition.y += corr;
    }

    ObstacleCheck(_predPosition.xy);
    if (_isObstructed)
    {
      Transform colliderTransform = _right_raycastHit.collider.transform;
      float colliderWidth = colliderTransform.localScale.x;
      float colliderPos = colliderTransform.position.x;
      float selfWidth = transform.localScale.x;
      float selfPos = _predPosition.x;
      float corrPos = colliderPos - colliderWidth*0.5f - selfWidth*0.5f - _contactOffset;
      _predPosition.x = math.min(_predPosition.x, corrPos);
    }

    GroundCheck(_predPosition.xy);

    // udpate velocity
    PBD.UpdateVelocity(in _initialPosition, in _predPosition, out _velocity, dt);
    _velocity *= _damping;
    _velocity = math.clamp(_velocity, -_maxVelocity, _maxVelocity);

    transform.position = _predPosition;
  }
}