/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Nick Grinsteasd
// Creation Date :     
//
// Brief Description : A 3D player controller with options to jump and cut with
                       a laser.
*****************************************************************************/
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

    bool isMoving = false;
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
        move.performed += ctx => isMoving = true;
        move.canceled += ctx => moveDirection = move.ReadValue<Vector2>();
        move.canceled += ctx => isMoving = false;
        //move.performed += ctx => rb.velocity = 
        //    new Vector3(move.ReadValue<Vector2>().x * moveSpeed, rb.velocity.y, move.ReadValue<Vector2>().y * moveSpeed);
        //move.canceled += ctx => rb.velocity = new Vector3(0, rb.velocity.y, 0);

        slash.performed += ctx => laser.SetActive(true);
        slash.performed += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = cutCameraDamping;
        //slash.performed += ctx => cameraLens.FieldOfView = cutCameraFOV;
        slash.performed += ctx => StartZoom(true);

        slash.canceled += ctx => laser.SetActive(false);
        slash.canceled += ctx => transposer.m_XDamping = transposer.m_YDamping = transposer.m_ZDamping = defaultDamping;
        //slash.canceled += ctx => cameraLens.FieldOfView = defaultFOV;
        slash.canceled += ctx => StartZoom(false);
    }

    /// <summary>
    /// Initiates a camera zoom when inputs are recieved
    /// </summary>
    /// <param name="zoomingIn">Determines which coroutine to start</param>
    private void StartZoom(bool zoomingIn)
    {
        StopAllCoroutines();

        if (zoomingIn)
            StartCoroutine(ZoomIn());
        else
            StartCoroutine(ZoomOut());
    }

    /// <summary>
    /// Zooms the camera from defaultFOV to cutCameraFOV
    /// </summary>
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

    /// <summary>
    /// Zooms the camera from cutCameraFOV back to defaultFOV
    /// </summary>
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

    void FixedUpdate()
    {
        // Player Movement
        if (isMoving)
        {
            velocity = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            velocity = velocity.normalized;
            velocity *= moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            //rb.AddForce(velocity * moveSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

//float speed = rb.velocity.magnitude;
//if (speed > 5)
//{
//    float brakeSpeed = speed - 5;
//    Vector3 brakingVelocity = rb.velocity.normalized * brakeSpeed;
//    rb.AddForce(-brakingVelocity);
//}

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
