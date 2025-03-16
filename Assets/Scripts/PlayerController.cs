using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float AnimBlendSpeed = 8.9f;

    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    [SerializeField] private float UpperLimit = -40f;
    [SerializeField] private float BottomLimit = 70f;
    [SerializeField] private float MouseSensitivity = 21.9f;

    private Rigidbody _playerRigidbody;

    private InputManager _inputManager;

    private Animator _animator;
    private bool _hasAnimator;

    private int _xVelHash;
    private int _yVelHasH;

    private const float _walkSpeed = 2f;
    private const float _runSpeed = 6f;

    private Vector2 _currentVelocity;
    private float _xRotation;

    private void Start()
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();

        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHasH = Animator.StringToHash("Y_Velocity");
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        CamMovements();
    }

    private void Move()
    {
        if (!_hasAnimator) 
            return;

        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if (_inputManager.Move == Vector2.zero) targetSpeed = 0.1f;

        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, AnimBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = _currentVelocity.x - _playerRigidbody.linearVelocity.x;
        var zVelDifference = _currentVelocity.y - _playerRigidbody.linearVelocity.z;

        _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, zVelDifference)), ForceMode.VelocityChange);

        _animator.SetFloat(_xVelHash, _currentVelocity.x);
        _animator.SetFloat(_yVelHasH, _currentVelocity.y);
    }

    private void CamMovements()
    {
        if (!_hasAnimator) return;

        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        Camera.position = CameraRoot.position;


        _xRotation -= Mouse_Y * MouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperLimit, BottomLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * MouseSensitivity * Time.smoothDeltaTime, 0));
    }

}
