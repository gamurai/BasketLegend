using UnityEngine;
using UnityEngine.InputSystem;

public class BasketballGameManager : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float ballDistance = 2f;
    public LayerMask basketLayer = 1;
    
    [Header("Shooting Settings")]
    public float maxForce = 25f;
    public float trajectoryTimeStep = 0.1f;
    public int trajectorySteps = 30;
    public LineRenderer trajectoryLine;
    
    [Header("UI Feedback")]
    public float minSwipeDistance = 50f;
    public Camera playerCamera;
    
    private GameObject currentBall;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isDragging = false;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    
    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
            
        SetupInputActions();
        CreateBallPrefab();
        SetupTrajectoryLine();
    }
    
    private void SetupInputActions()
    {
        touchPositionAction = new InputAction("TouchPosition", InputActionType.PassThrough);
        touchPositionAction.AddBinding("<Pointer>/position");
        
        touchPressAction = new InputAction("TouchPress", InputActionType.Button);
        touchPressAction.AddBinding("<Pointer>/press");
        
        touchPositionAction.Enable();
        touchPressAction.Enable();
        
        touchPressAction.started += OnTouchStart;
        touchPressAction.canceled += OnTouchEnd;
    }
    
    private void CreateBallPrefab()
    {
        if (ballPrefab == null)
        {
            ballPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ballPrefab.name = "Basketball";
            
            var ballRenderer = ballPrefab.GetComponent<MeshRenderer>();
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.color = Color.red;
            ballRenderer.material = material;
            
            ballPrefab.AddComponent<Rigidbody>();
            ballPrefab.GetComponent<Rigidbody>().mass = 0.6f;
            ballPrefab.GetComponent<Rigidbody>().linearDamping = 0.1f;
            ballPrefab.GetComponent<Rigidbody>().angularDamping = 0.3f;
            
            var collider = ballPrefab.GetComponent<SphereCollider>();
            collider.material = CreateBouncyMaterial();
            
            ballPrefab.SetActive(false);
        }
    }
    
    private PhysicsMaterial CreateBouncyMaterial()
    {
        var bouncyMaterial = new PhysicsMaterial("Bouncy");
        bouncyMaterial.bounciness = 0.7f;
        bouncyMaterial.dynamicFriction = 0.3f;
        bouncyMaterial.staticFriction = 0.3f;
        bouncyMaterial.frictionCombine = PhysicsMaterialCombine.Average;
        bouncyMaterial.bounceCombine = PhysicsMaterialCombine.Maximum;
        return bouncyMaterial;
    }
    
    private void SetupTrajectoryLine()
    {
        if (trajectoryLine == null)
        {
            var trajectoryObject = new GameObject("TrajectoryLine");
            trajectoryLine = trajectoryObject.AddComponent<LineRenderer>();
        }
        
        var trajectoryMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        trajectoryMaterial.color = Color.yellow;
        trajectoryLine.material = trajectoryMaterial;
        trajectoryLine.startWidth = 0.05f;
        trajectoryLine.endWidth = 0.05f;
        trajectoryLine.positionCount = trajectorySteps;
        trajectoryLine.enabled = false;
        trajectoryLine.useWorldSpace = true;
    }
    
    private void Start()
    {
        SpawnNewBall();
    }
    
    private void Update()
    {
        if (isDragging && currentBall != null)
        {
            currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
            UpdateTrajectoryPreview();
        }
    }
    
    private void SpawnNewBall()
    {
        if (currentBall != null)
        {
            DestroyImmediate(currentBall);
        }
        
        Vector3 spawnPosition = playerCamera.transform.position + playerCamera.transform.forward * ballDistance;
        spawnPosition.y = playerCamera.transform.position.y - 0.2f;
        
        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        currentBall.SetActive(true);
        
        var rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    
    private void OnTouchStart(InputAction.CallbackContext context)
    {
        if (currentBall == null) return;
        
        startTouchPosition = touchPositionAction.ReadValue<Vector2>();
        isDragging = true;
        trajectoryLine.enabled = true;
    }
    
    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if (!isDragging || currentBall == null) return;
        
        Vector2 endTouchPosition = touchPositionAction.ReadValue<Vector2>();
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;
        
        if (swipeDelta.magnitude > minSwipeDistance)
        {
            LaunchBall(swipeDelta);
        }
        
        isDragging = false;
        trajectoryLine.enabled = false;
        
        Invoke(nameof(SpawnNewBall), 3f);
    }
    
    private void LaunchBall(Vector2 swipeDelta)
    {
        var rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        
        Vector3 force = CalculateLaunchForce(swipeDelta);
        rb.AddForce(force, ForceMode.VelocityChange);
        
        float torqueMagnitude = swipeDelta.magnitude * 0.1f;
        Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * torqueMagnitude;
        rb.AddTorque(torque, ForceMode.VelocityChange);
    }
    
    private Vector3 CalculateLaunchForce(Vector2 swipeDelta)
    {
        float normalizedSwipeX = Mathf.Clamp(swipeDelta.x / Screen.width, -1f, 1f);
        float normalizedSwipeY = Mathf.Clamp(swipeDelta.y / Screen.height, -1f, 1f);
        
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        Vector3 cameraUp = playerCamera.transform.up;
        
        Vector3 direction = cameraForward;
        direction += cameraRight * normalizedSwipeX * 0.5f;
        direction += cameraUp * (normalizedSwipeY * 0.3f + 0.3f);
        
        direction = direction.normalized;
        
        float forceMagnitude = Mathf.Clamp(swipeDelta.magnitude / 100f, 0.3f, 1f) * maxForce;
        
        return direction * forceMagnitude;
    }
    
    private void UpdateTrajectoryPreview()
    {
        Vector2 swipeDelta = currentTouchPosition - startTouchPosition;
        
        if (swipeDelta.magnitude < minSwipeDistance)
        {
            trajectoryLine.enabled = false;
            return;
        }
        
        trajectoryLine.enabled = true;
        Vector3 launchForce = CalculateLaunchForce(swipeDelta);
        
        Vector3 startPosition = currentBall.transform.position;
        Vector3 velocity = launchForce;
        
        for (int i = 0; i < trajectorySteps; i++)
        {
            float time = i * trajectoryTimeStep;
            Vector3 position = startPosition + velocity * time + 0.5f * Physics.gravity * time * time;
            trajectoryLine.SetPosition(i, position);
        }
    }
    
    private void OnDestroy()
    {
        touchPositionAction?.Disable();
        touchPressAction?.Disable();
        touchPositionAction?.Dispose();
        touchPressAction?.Dispose();
    }
}