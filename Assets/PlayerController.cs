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
        //slash.performed += ctx => cameraLens.FieldOfView = cutCameraFOV;
        slash.performed += ctx => StartZoom(true);

        slash.canceled += ctx => laser.SetActive(false);
        slash.canceled += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = defaultDamping;
        //slash.canceled += ctx => cameraLens.FieldOfView = defaultFOV;
        slash.canceled += ctx => StartZoom(false);
    }

    private void StartZoom(bool zoomingIn)
    {
        StopAllCoroutines();

        if (zoomingIn)
            StartCoroutine(ZoomIn());
        else
            StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomIn()
    {
        float interpolationVal = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            interpolationVal += 0.05f;

            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, cutCameraFOV, interpolationVal);

            if (interpolationVal >= 1f && mainCamera.m_Lens.FieldOfView >= defaultFOV)
                break;
        }

        mainCamera.m_Lens.FieldOfView = defaultFOV;
    }

    IEnumerator ZoomOut()
    {
        float interpolationVal = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            interpolationVal += 0.05f;

            mainCamera.m_Lens.FieldOfView = Mathf.Lerp(cutCameraFOV, defaultFOV, interpolationVal);

            if (interpolationVal >= 1f && mainCamera.m_Lens.FieldOfView <= cutCameraFOV)
                break;
        }

        mainCamera.m_Lens.FieldOfView = cutCameraFOV;
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
