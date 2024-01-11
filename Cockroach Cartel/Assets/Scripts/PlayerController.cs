using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //basic setup
    private const float RUN_MULTIPLIER = 3f; //this should be accessable in editor
    [SerializeField] private Transform cam;
    [SerializeField] private bool lockCam = true;
    [Header("Movement")]
    [SerializeField] private bool run; //is hard set through code, no value in editor
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private Vector2 speedLimmit = new Vector2(12, 12); //this should be a float
    [Header("Jump")]
    [SerializeField] private int jumps = 1;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Transform playerBase;
    private float groundSphereRadius = 0.05f;
    private int jumpsUsed = 0;
    private bool canJump;
    private bool grounded;
    private Vector2 heading;
    private Rigidbody rb;



    private void Start()
    {
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        rb = GetComponent<Rigidbody>();
        //playerBase.position = new Vector3(playerBase.position.x, playerBase.position.y - groundSphereRadius, playerBase.position.z);
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //user input
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        heading = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(Input.GetAxis("Jump") > 0 && canJump && grounded)
        {
            Jump();
            canJump = false;
        }
        else if (Input.GetAxis("Jump") == 0)
        {
            canJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (grounded)
        {
            float moveSpeed = (run ? speed * RUN_MULTIPLIER : speed);
            Vector3 movement = cam.right * heading.x +
            cam.forward * heading.y;

            movement = movement.normalized * moveSpeed;

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            if (rb.velocity.magnitude > 0)
            {
                Vector3 facePosition = new Vector3(rb.velocity.x, 0, rb.velocity.z) + transform.position;
                transform.LookAt(facePosition); //apply blending for smoother rotation
            }
        }

/*        if(lockCam)
        {
            //face away from cam
            Vector3 facePos = new Vector3(cam.position.x, this.transform.position.y, cam.position.z);
            this.transform.LookAt(facePos);
            this.transform.Rotate(0, 180, 0);
            //what if we want the player to be able to rotate the camera without the player moving?
        }*/

        if (Physics.CheckSphere(playerBase.position, groundSphereRadius))
        {
            jumpsUsed = 0;
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = grounded ? Color.green : Color.red;

        Gizmos.DrawWireSphere(playerBase.position, groundSphereRadius);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce * 100);

        jumpsUsed++;
    }
}
