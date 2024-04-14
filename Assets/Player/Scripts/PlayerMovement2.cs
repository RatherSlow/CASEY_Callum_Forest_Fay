using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    CharacterController characterController;
    InputActions inputActions;
    InputAction moveAction;
    InputAction lookAction;
    InputAction fireAction;
    InputAction jumpAction;

    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public float moveSpeed;
    public float jumpSpeed = 10f;
    public float gravity = 20.0f;
    public float lookUpClamp = 30f;
    public float lookDownClamp = 60f;

    [Header("Spell Casting")]
    public GameObject spell;
    public float spellForce;

    [Header("References")]
    public Transform cameraContainer;
    public Transform attackPoint;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isJumping = false;

    void Awake()
    {
        inputActions = new InputActions();
        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        fireAction = inputActions.Player.Fire;
        jumpAction = inputActions.Player.Jump;
        jumpAction.started += _ => isJumping = true;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        moveAction.Enable();
        lookAction.Enable();
        fireAction.Enable();
        jumpAction.Enable();
    }

    void Update()
    {
        HandleMovement();
        HandleVisionControl();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        move = transform.TransformDirection(move);
        move *= moveSpeed;

        if (isJumping)
        {
            move.y = jumpSpeed;
            isJumping = false;
        }

        move.y -= gravity * Time.deltaTime;
        characterController.Move(move * Time.deltaTime);
    }

    void HandleVisionControl()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        cameraContainer.Rotate(Vector3.left * mouseY);

        Vector3 currentRotation = cameraContainer.localRotation.eulerAngles;
        currentRotation.x = ClampRotationAngle(currentRotation.x, -lookUpClamp, lookDownClamp);
        cameraContainer.localRotation = Quaternion.Euler(currentRotation);
    }

    float ClampRotationAngle(float angle, float min, float max)
    {
        if (angle < 180f)
            return Mathf.Clamp(angle, min, max);
        else
            return Mathf.Clamp(angle, 360f + min, 360f - max);
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //public void OnLook(InputAction.CallbackContext context)
    //{
    //    lookInput = context.ReadValue<Vector2>();
    //}

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            FireSpell();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumping = true;
        }
    }

    void FireSpell()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //Ray hits something?
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 attackDirection = targetPoint - attackPoint.position;

        GameObject currentSpell = Instantiate(spell, attackPoint.position, Quaternion.identity);
        currentSpell.transform.forward = attackDirection.normalized;

        currentSpell.GetComponent<Rigidbody>().AddForce(attackDirection.normalized * spellForce, ForceMode.Impulse);
    }
}