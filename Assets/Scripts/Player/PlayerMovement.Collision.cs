using UnityEngine;
using Unity.Mathematics;

public partial class PlayerMovement
{
  private void GroundCheck(in float2 position)
  {
    _down_boxCastOrigin = position - new float2(0.0f, _halfHeight);
    _down_raycastHit = Physics2D.BoxCast(
      _down_boxCastOrigin, _down_boxCastSize, 0.0f, Vector2.down, 5.0f, _solidLayer
    );

    Collider2D collider = _down_raycastHit.collider;
    _groundDetected = collider != null;
    _isGrounded = _down_raycastHit.distance < _contactOffset && _groundDetected;
  }

  private void ObstacleCheck(in float2 position)
  {
    _right_boxCastOrigin = position + new float2(_halfHeight, 0.0f);
    _right_raycastHit = Physics2D.BoxCast(
      _right_boxCastOrigin, _right_boxCastSize, 0.0f, Vector2.right, 5.0f, _solidLayer
    );

    Collider2D collider = _right_raycastHit.collider;
    _obstacleDetected = collider != null;
    _isObstructed = _right_raycastHit.distance < _contactOffset && _obstacleDetected;
  }

  private bool CollisionIsLayer(in int collisionLayer, in LayerMask layerMask)
    => (1 << collisionLayer & layerMask) != 0;

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (CollisionIsLayer(collision.gameObject.layer, _killableLayer))
      _dead = true;
  }
}