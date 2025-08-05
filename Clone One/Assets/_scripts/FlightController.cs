using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightController : MonoBehaviour
{
    public GameObject ship;
    public GameObject planet;
    public float rotationSpeed = 20f;
    public float turnSpeed = 90f;
    public float zoomSpeed = 10f;
    public float minScale = 0.1f;
    public float maxScale = 30f;

    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private float zoomInput;
    
    public Camera cam;
    private Vector3 clickHitPoint;
    private Quaternion targetRotation;
    private bool isRotating = false;

    private float shootForce = 5f;
    public GameObject bullet;

    private LineRenderer lineRenderer;
    private bool isAbducting = false;
    
    public GameObject LeftClickStateObject;
    private LeftClickState leftClickState;

    public GameObject GameManagerObj;
    private GameStateMan gameStateMan;
    
    public float speed = 1.0f;
    
    void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Flying.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Flying.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Flying.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();
        controls.Flying.Zoom.canceled += ctx => zoomInput = 0;
    }


    void OnEnable() => controls.Flying.Enable();
    void OnDisable() => controls.Flying.Disable();

    void Start()
    {
        
        gameStateMan = GameManagerObj.GetComponent<GameStateMan>();
        leftClickState = LeftClickStateObject.GetComponent<LeftClickState>();
        
        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;
        
        lineRenderer.enabled = false;
    }
    void Update()
    {
        //MOVE
        planet.transform.Rotate(Vector3.forward.normalized *moveInput.x * turnSpeed * Time.deltaTime, Space.World);
        
        //TURN
        planet.transform.Rotate(ship.transform.right.normalized, -moveInput.y * rotationSpeed * Time.deltaTime, Space.World);

        //ZOOM
        if (zoomInput != 0)
        {
            Vector3 newScale = planet.transform.localScale + Vector3.one * zoomInput * zoomSpeed * Time.deltaTime;
            newScale = Vector3.Max(Vector3.one * minScale, Vector3.Min(Vector3.one * maxScale, newScale));
            planet.transform.localScale = newScale;
        }
        
        //MOVE with CLICK
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("planet"))
                {
                    RotatePlanetToClick(hit);
                }
            }
        }

        //SHOOT or ABDUCT
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //SHOOT
            if (leftClickState.currentLCstate == LeftClickState.leftClickState.bullet)
            {
                Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

                Vector3 direction;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    direction = hit.point - ship.transform.position;
                }
                else
                {
                    direction = ray.direction;
                }

                GameObject newBubble = Instantiate(bullet, ship.transform.position, Quaternion.identity);
                Rigidbody newBubbleRb = newBubble.GetComponent<Rigidbody>();
                newBubbleRb.linearVelocity = direction * shootForce;
            }
            //ABDUCT
            if (leftClickState.currentLCstate == LeftClickState.leftClickState.beams)
            { 
                isAbducting = true; 
                lineRenderer.enabled = true;
            }
            //NONE
            if (leftClickState.currentLCstate == LeftClickState.leftClickState.none)
            {
                Debug.Log("none");
                return;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isAbducting = false;
            lineRenderer.enabled = false;
        }

        if (isAbducting)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, ship.transform.position);
                lineRenderer.SetPosition(1, hit.point);
                
                if (hit.collider.gameObject.CompareTag("Bear"))
                {
                    Debug.Log("bear");
                    var bearObject = hit.collider.gameObject; 
                    float step = speed * Time.deltaTime;
                    bearObject.transform.position = Vector3.MoveTowards(bearObject.transform.position, ship.transform.position, step);

                    gameStateMan.goToMap();
                }
            }
        }
        
        if (isRotating)
        {
            planet.transform.rotation = Quaternion.RotateTowards(
                planet.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            
            if (Quaternion.Angle(planet.transform.rotation, targetRotation) < 0.1f)
            {
                planet.transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    void RotatePlanetToClick(RaycastHit hit)
    {
        Vector3 hitPoint = hit.point;
        Vector3 planetCenter = planet.transform.position;

        Vector3 fromDirection = (hitPoint - planetCenter).normalized;
        Vector3 toDirection = (ship.transform.position - planetCenter).normalized;

        Quaternion rotationDelta = Quaternion.FromToRotation(fromDirection, toDirection);
            
        targetRotation = rotationDelta * planet.transform.rotation;
        isRotating = true;
    }


}
