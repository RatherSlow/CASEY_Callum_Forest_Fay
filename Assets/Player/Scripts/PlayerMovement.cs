using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Transform playerContainer, cameraContainer;

    [Header("Settings")]
    public float mouseSensitivity = 100f;    
    public float moveSpeed;
    public float jumpSpeed = 10f;
    public float gravity = 20.0f;
    public float lookUpClamp = -90f;
    public float lookDownClamp = 90f;

    [Header("Spell Casting")]
    public GameObject spell;
    public float spellForce;

    [Header("References")]
    public Transform attackPoint;

    private Vector3 moveDirection = Vector3.zero;
   

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        VisionControl();
        Movement();
    }

    private void VisionControl()
    {
        // Rotate player based on mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player horizontally
        playerContainer.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        cameraContainer.Rotate(Vector3.left * mouseY);

        // Ensure camera does not over-rotate vertically
        Vector3 currentRotation = cameraContainer.localRotation.eulerAngles;
        currentRotation.x = Mathf.Clamp(currentRotation.x, 0f, 25f); // Adjust the maximum vertical rotation here
        cameraContainer.localRotation = Quaternion.Euler(currentRotation);

        // Check for player input to cast spell
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for casting spell
        {
            target(); // Call target method to cast spell
        }
    }

    private void Movement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= moveSpeed;
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void target()
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
