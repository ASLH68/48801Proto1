using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] float cutCameraDamping;
    float defaultDamping;
    [SerializeField] float cutCameraFOV;
    float defaultFOV;
  
    PlayerControls playerControls;
    InputAction move, slash, jump, reset, quit;

    Rigidbody rb;
    CinemachineVirtualCamera mainCamera;
    CinemachineTransposer transposer;
    [SerializeField] GameObject laser;

    Vector2 moveDirection;
    Vector3 velocity;
    
    bool isGrounded = true;
    float groundDistance = 0.3f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundChecker;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = gameObject.GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<CinemachineVirtualCamera>();
        transposer = mainCamera.GetCinemachineComponent<CinemachineTransposer>();
        defaultDamping = transposer.m_XDamping;
        defaultFOV = mainCamera.m_Lens.FieldOfView;
        //laser = mainCamera.transform.GetChild(0).gameObject;

        playerControls = new PlayerControls();
        playerControls.BasicControls.Enable();

        move = playerControls.FindAction("Move");
        slash = playerControls.FindAction("Slash");
        jump = playerControls.FindAction("Jump");
        reset = playerControls.FindAction("Reset");
        quit = playerControls.FindAction("Quit");

        move.performed += ctx => moveDirection = move.ReadValue<Vector2>();
        move.canceled += ctx => moveDirection = move.ReadValue<Vector2>();

        slash.performed += ctx => laser.SetActive(true);
        slash.performed += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = cutCameraDamping;
        slash.performed += ctx => mainCamera.m_Lens.FieldOfView = cutCameraFOV;
        slash.canceled += ctx => laser.SetActive(false);
        slash.canceled += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = defaultDamping;
        slash.canceled += ctx => mainCamera.m_Lens.FieldOfView = defaultFOV;
    }

    // Update is called once per frame
    void Update()
    {
        // Player Movement
        velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        velocity = velocity.normalized;
        rb.AddForce(velocity * moveSpeed);

        float speed = rb.velocity.magnitude;
        if (speed > 5)
        {
            float brakeSpeed = speed - 5;
            Vector3 brakingVelocity = rb.velocity.normalized * brakeSpeed;
            rb.AddForce(-brakingVelocity);
        }

        // Ground Check
        if (!isGrounded)
            isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

        // Jump
        if (jump.IsPressed() && isGrounded)
        {
            isGrounded = false;
            //float verticalVelocity = Mathf.Sqrt(jumpHeight *  Physics.gravity.y);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }

        if (reset.IsPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (quit.IsPressed())
        {
            Application.Quit();
        }

        // Player Rotation
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
    }
}
