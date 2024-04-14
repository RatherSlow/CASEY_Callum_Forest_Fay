using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    //InputActions inputActions;
    //InputAction jump;

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
    public Transform playerContainer;
    public Transform cameraContainer;
    public Transform attackPoint;

    float rotateX, rotateY;
    private Vector3 moveDirection = Vector3.zero;
    bool isClimbable = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        VisionControl();
        Movement();
    }

    private void VisionControl()
    {
        rotateX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotateY -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotateY = Mathf.Clamp(rotateY, -lookUpClamp, lookDownClamp);

        transform.Rotate(0f, rotateX, 0f);

        cameraContainer.transform.localRotation = Quaternion.Euler(rotateY, 0f, 0f);

        // Check for player input to cast spell
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for casting spell
        {
            target(); // Call target method to cast spell
            Debug.Log("Spellcast");
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
