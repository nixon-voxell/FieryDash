using UnityEngine;
using Unity.Mathematics;

public partial class Player
{
  private bool _landed;
  private bool _deathOccured;

  public void Respawn()
  {
    transform.position = _startTransform.position;
    transform.localScale = _startTransform.localScale;
    transform.rotation = _startTransform.rotation;
    _landed = false;
    _deathOccured = false;

    Start();
  }

  private void HandleJump()
  {
    if (!Input.GetKeyDown(_jumpKeyCode)) return;
    if (_jumpsMade++ < _jumpCount)
    {
      _velocity.y = _jumpImpulse;
      _squashStretchAnimator.Play("Jump");
      _audioSource.PlayOneShot(_jumpClip);
    } 
  }

  private void HandleDash()
  {
    if (!Input.GetKeyDown(_dashKeyCode)) return;
    if (_dashTimer > 0.0f || _dashCooldownTimer > 0.0f) return;
    _velocity.x += _dashImpulse;
    _dashTimer = _dashDuration;
    _dashCooldownTimer = _dashCooldown;
    _audioSource.PlayOneShot(_dashClip);
  }

  private void LateUpdate()
  {
    float dt = Time.deltaTime;
    if (dt == 0.0f || !GameManager.GameStarted) return;
    float3 currPosition = transform.position;
    if (currPosition.y < 0.0f || currPosition.x < -SceneLoader.GameManager.OffScreenLimit) _dead = true;
    if (_dead)
    {
      if (!_deathOccured)
      {
        Die();
        _deathOccured = true;
      }
      return;
    } else _deathOccured = false;

    // reduce one addtional jumps if it is already on the air
    if (!IsGrounded) _jumpsMade = math.max(_jumpsMade, 1);

    _initialPosition = currPosition;
    _predPosition = currPosition;

    // apply gravity and external force
    _velocity += _gravity * dt;

    HandleJump();
    HandleDash();

    // remove downwards velocity if it is already grounded
    if (_isActualGrounded)
    {
      _squashStretchAnimator.Play("Land");
      _velocity.y = math.max(_velocity.y, 0.0f);
      _jumpsMade = 0;
      if (!_landed)
      {
        _audioSource.PlayOneShot(_landClip);
        _landed = true;
      }
    } else _landed = false;

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

    ObstacleCheck(_initialPosition.xy);
    if (_obstacleDetected)
    {
      Transform colliderTransform = _right_raycastHit.collider.transform;
      float colliderWidth = _right_raycastHit.collider.bounds.size.x;
      float colliderPos = colliderTransform.position.x;
      float selfWidth = transform.localScale.x;
      float selfPos = _predPosition.x;
      float corrPos = colliderPos - colliderWidth*0.5f - selfWidth*0.5f - _contactOffset;
      _predPosition.x = math.min(_predPosition.x, corrPos);
    }

    _groundedTimer -= dt;
    GroundCheck(_predPosition.xy);

    // udpate velocity
    PBD.UpdateVelocity(in _initialPosition, in _predPosition, out _velocity, dt);
    _velocity *= _damping;
    _velocity = math.clamp(_velocity, -_maxVelocity, _maxVelocity);

    transform.position = _predPosition;

    UpdateBendingMaterials();
  }
}

[System.Serializable]
public struct BendingMaterial
{
  public Material material;
  public float restPosition;
  public float bendingPosition;
}