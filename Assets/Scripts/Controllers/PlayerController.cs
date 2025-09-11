using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerAnimationsController _playerAnimationsController;
    private Animator _animator;
    private CharacterController _controller;

    private float _horizontalInput;
    private float _verticalInput;

    private int _animIDSpeedX;
    private int _animIDSpeedY;
    private int _animIDJumping;
    private int _animRunning;

    private void Awake()
    {
        _playerAnimationsController = GetComponent<PlayerAnimationsController>();

        _animator = GetComponent<Animator>();

        _controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        inputManager = InputManager.Instance;

        inputManager.InputControl.Player.Move.performed += ctx =>
        {
            SetMovementInputs(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y);
        };

        inputManager.InputControl.Player.Move.canceled += ctx =>
        {
            SetMovementInputs(0, 0);
        };

        inputManager.InputControl.Player.Running.started += ctx =>
        {
            _playerAnimationsController.SetBoolAnimator(_animRunning, true);
        };

        inputManager.InputControl.Player.Running.canceled += ctx =>
        {
            _playerAnimationsController.SetBoolAnimator(_animRunning, false);
        };

        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeedX = Animator.StringToHash("SpeedX");
        _animIDSpeedY = Animator.StringToHash("SpeedY");
        _animIDJumping = Animator.StringToHash("Jumping");
        _animRunning = Animator.StringToHash("Running");
    }

    private void SetMovementInputs(float horizontal, float vertical)
    {
        _horizontalInput = horizontal;
        _verticalInput = vertical;
    }

    private void Update()
    {
        _playerAnimationsController.SetFloatAnimator(_animIDSpeedX, _horizontalInput);
        _playerAnimationsController.SetFloatAnimator(_animIDSpeedY, _verticalInput);
    }

    void OnAnimatorMove()
    {
        if (_controller.enabled)
        {
            _controller.Move(_animator.deltaPosition);                
        }
    }
}
