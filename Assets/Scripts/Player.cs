using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField] private float _jumpForce;
  [SerializeField] private bool _isGrounded = false;
  private int _noOfJumps = 0;

  private Rigidbody2D _rigidbody;

  private void Awake()
  {
    _rigidbody = GetComponent<Rigidbody2D>();
  }


  void Update()
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
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("Ground"))
    {
      if(_isGrounded == false) _isGrounded = true;
    }
  }
}
