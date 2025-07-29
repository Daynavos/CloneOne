using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    public InputActionAsset asset;
    
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _upThrust;
    private InputAction _downThrust;
    
    private Vector2 _moveValue;
    private Vector2 _lookValue;
    private float _upThrustInput;
    private float _downThrustInput;

    [SerializeField] private float moveAmount = 5;
    [SerializeField] private float lookAmount = 5;
    [SerializeField] private float upThrustInput = 5;
    [SerializeField] private float downThrustInput = 5;
    
    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        asset.FindActionMap("Flying").Enable();
    }

    private void OnDisable()
    {
        asset.FindActionMap("Flying").Disable(); 
    }
    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _upThrust = InputSystem.actions.FindAction("UpThrust");
        _downThrust = InputSystem.actions.FindAction("DownThrust");
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();
        _lookValue = _lookAction.ReadValue<Vector2>();
        
    }

    void FixedUpdate()
    {
        MovingBasic();
        Looking();
    }

    private void MovingBasic()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * _moveValue.y *);
    }

    private void Looking()
    {
        
    }
}
