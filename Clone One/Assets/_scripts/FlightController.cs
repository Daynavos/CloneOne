using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    public InputActionAsset asset;
    
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _upThrust;
    private InputAction _downThrust;
    
    private Vector2 _moveVector;
    private Vector2 _lookVector;
    private float _upThrustInputFloat;
    private float _downThrustInputFloat;

    [SerializeField] private float moveMultiplier = 5;
    [SerializeField] private float lookMultiplier = 5;
    [SerializeField] private float upThrustMultiplier = 2;
    [SerializeField] private float downThrustMultiplier = 2;
    
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
        _moveVector = _moveAction.ReadValue<Vector2>();
        _lookVector = _lookAction.ReadValue<Vector2>();
        _upThrustInputFloat = _upThrust.ReadValue<float>();
        _downThrustInputFloat = _downThrust.ReadValue<float>();
    }

    void FixedUpdate()
    {
        MovingBasic();
        Looking();
        UpThrust();
        DownThrust();
    }

    private void MovingBasic()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * _moveVector.y + transform.right * _moveVector.x );
    }

    private void UpThrust()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.up * (_upThrustInputFloat * upThrustMultiplier));
    }

    private void DownThrust()
    {
        _rigidbody.MovePosition(_rigidbody.position + transform.up * (_downThrustInputFloat * downThrustMultiplier * -1));

    }

    private void Looking()
    {
        
    }
}
