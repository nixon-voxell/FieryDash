using UnityEngine;
using Voxell.Inspector;

public class Player : MonoBehaviour
{
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private float _jumpForce;
  [SerializeField, InspectOnly] private bool _isGrounded = false;
  private int _noOfJumps = 0;

  private Rigidbody2D _rigidbody;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (_isGrounded == true)
      {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(Vector2.up * _jumpForce);
        _isGrounded = false;
        _noOfJumps = 1;
      } else if (_noOfJumps == 1)
      {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(Vector2.up * _jumpForce);
        _noOfJumps = 2;
      }
    }
    if (Input.GetKeyDown(KeyCode.D))
    {
      _rigidbody.AddForce(Vector2.right * _jumpForce);       
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    // if it hits the ground, set the _isGrounded toggle to true
    if ((1 << collision.gameObject.layer & _groundLayer.value) != 0)
      _isGrounded = true;
  }
}
