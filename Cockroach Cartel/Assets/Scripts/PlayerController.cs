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
        playerBase.position = new Vector3(playerBase.position.x, playerBase.position.y - groundSphereRadius, playerBase.position.z);
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

        //this can be simplified, getAxis is limited to -1,1, and for more instant reaction use GetAxisRa
        if(Input.GetAxis("Horizontal") > 0)
        {
            heading.x = 1;
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            heading.x = -1;
        }
        else
        {
            heading.x = 0;
        }

        //this can be combined with the x axis
        if (Input.GetAxis("Vertical") > 0)
        {
            heading.y = 1;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            heading.y = -1;
        }
        else
        {
            heading.y = 0;
        }

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
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.velocity += transform.right * heading.x * (run ? speed * RUN_MULTIPLIER : speed);
        rb.velocity += transform.forward * heading.y * (run ? speed * RUN_MULTIPLIER : speed);

        if(lockCam)
        {
            //face away from cam
            Vector3 facePos = new Vector3(cam.position.x, this.transform.position.y, cam.position.z);
            this.transform.LookAt(facePos);
            this.transform.Rotate(0, 180, 0);
            //what if we want the player to be able to rotate the camera without the player moving?
        }

        if(Physics.CheckSphere(playerBase.position, groundSphereRadius))
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
        if(true)
            Gizmos.DrawWireSphere(playerBase.position, groundSphereRadius);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce * 100);

        jumpsUsed++;
    }
}
