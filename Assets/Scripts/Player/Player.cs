using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.Mathematics;
using Voxell.Inspector;

public partial class Player : MonoBehaviour
{
  private struct TransformStorage
  {
    public float3 position;
    public float3 localScale;
    public quaternion rotation;
  }

  private static readonly int UpperBending = Shader.PropertyToID("_UpperBending");

  [SerializeField] private LayerMask _solidLayer;
  [SerializeField] private LayerMask _killableLayer;
  [SerializeField] private LayerMask _breakableLayer;
  [SerializeField] private Light2D _light;
  [SerializeField] private ParticleSystem _deathFX;
  [SerializeField] private GameObject[] _renderers;

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
  [SerializeField] private Animator _squashStretchAnimator;
  [SerializeField, Range(1, 3)] private int _jumpCount = 2;
  [SerializeField] private float _jumpImpulse;
  [SerializeField] private float _dashImpulse;
  [SerializeField] private float _dashDuration;
  public float DashCooldown => _dashCooldown;
  [SerializeField] private float _dashCooldown;
  [SerializeField, Range(0.0f, 1.0f)] private float _dashDecayFactor = 0.6f;

  [Header("States")]
  [SerializeField, InspectOnly] private bool _isActualGrounded;
  [SerializeField] private float _groundedTime = 0.2f;
  [SerializeField, InspectOnly] private float _groundedTimer;
  public bool IsGrounded => _groundedTimer > 0.0f;
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
  private const float BOXCAST_THICKNESS = 0.1f;
  private float _halfHeight;
  private Vector2 _down_boxCastOrigin, _down_boxCastSize;
  private Vector2 _right_boxCastOrigin, _right_boxCastSize;
  private RaycastHit2D _down_raycastHit, _right_raycastHit;

  [Header("Bending")]
  [SerializeField] private BendingMaterial[] _bendingMaterials;

  [Header("Sound FX")]
  [SerializeField] private AudioSource _audioSource;
  [SerializeField] private AudioClip _jumpClip;
  [SerializeField] private AudioClip _landClip;
  [SerializeField] private AudioClip _dashClip;
  [SerializeField] private AudioClip _dieClip;
  [SerializeField] private AudioClip _crateDestroyedClip;

  private TransformStorage _startTransform;

  private void Awake()
  {
    _startTransform.position = transform.position;
    _startTransform.localScale = transform.localScale;
    _startTransform.rotation = transform.rotation;
    SceneLoader.GameManager.player = this;
  }

  private void Start()
  {
    _down_boxCastSize = new Vector2(transform.localScale.x - _contactOffset, BOXCAST_THICKNESS);
    _right_boxCastSize = new Vector2(BOXCAST_THICKNESS, transform.localScale.y - _contactOffset);
    _halfHeight = transform.localScale.y*0.5f;
    _groundDetected = false;

    // initialize states
    _velocity = 0.0f;
    _initialPosition = transform.position;
    _predPosition = _initialPosition;
    _jumpsMade = 0;
    _dashTimer = 0.0f;
    _dashCooldownTimer = 0.0f;
    _dead = false;
    _groundedTimer = 0.0f;
    _isActualGrounded = false;
  }

  private void Update()
  {
    float dt = Time.deltaTime*2.0f;
    if (!GameManager.GameStarted)
      _light.intensity = Mathf.Lerp(_light.intensity, 0.0f, dt);
    else _light.intensity = Mathf.Lerp(_light.intensity, 2.0f, dt);
  }

  public void Respawn()
  {
    transform.position = _startTransform.position;
    transform.localScale = _startTransform.localScale;
    transform.rotation = _startTransform.rotation;
    _landed = false;
    _deathOccured = false;

    Start();
    ResetMaterials();
    for (int r=0; r < _renderers.Length; r++) _renderers[r].SetActive(true);
  }

  private void Die()
  {
    _audioSource.PlayOneShot(_dieClip);
    SceneLoader.GameManager.StopGame();
    StartCoroutine(PlayEndScene(2.0f));
    _deathFX.transform.position = transform.position;
    _deathFX.Play(true);

    for (int r=0; r < _renderers.Length; r++) _renderers[r].SetActive(false);
  }

  private IEnumerator PlayEndScene(float delayTime)
  {
    yield return new WaitForSeconds(delayTime);
    SceneLoader.GameManager.gameStopper.gameObject.SetActive(true);
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

  private void UpdateBendingMaterials()
  {
    for (int bm=0; bm < _bendingMaterials.Length; bm++)
    {
      _bendingMaterials[bm].material.SetFloat(
        UpperBending, math.lerp(
          _bendingMaterials[bm].restPosition,
          _bendingMaterials[bm].bendingPosition,
          math.saturate(_velocity.x))
      );
    }
  }

  private void OnDisable() => ResetMaterials();

  private void ResetMaterials()
  {
    for (int bm=0; bm < _bendingMaterials.Length; bm++)
    {
      _bendingMaterials[bm].material.SetFloat(
        UpperBending, _bendingMaterials[bm].restPosition
      );
    }
  }
}