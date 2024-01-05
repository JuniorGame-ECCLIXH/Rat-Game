using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //basic setup
    private const float RUN_MULTIPLIER = 3f;
    [SerializeField] private Transform cam;
    [Header("Movement")]
    [SerializeField] private bool run;
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private Vector2 speedLimmit = new Vector2(12, 12);
    [Header("Jump")]
    [SerializeField] private int jumps = 1;
    private int jumpsUsed;
    [SerializeField] private float jumpForce = 5;
    private bool jump;
    private Vector2 heading;
    private Rigidbody rb;



    private void Start()
    {
        if(cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        rb = GetComponent<Rigidbody>();
        jumpsUsed = 0;
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

        if(Input.GetAxis("Jump") > 0)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    private void FixedUpdate()
    {
        //actual movement
        if(Mathf.Abs(rb.velocity.x) < speedLimmit.x)
        {
            rb.AddForce(transform.right * heading.x * (run ? speed * RUN_MULTIPLIER : speed));
        }
        if(Mathf.Abs(rb.velocity.z) < speedLimmit.y)
        {
            rb.AddForce(transform.forward * heading.y * (run ? speed * RUN_MULTIPLIER : speed));
        }



        if (jump && jumpsUsed < jumps)
        {
            rb.AddForce(Vector3.up * jumpForce * 100);

            jumpsUsed++;
        }

        //face away from cam
        Vector3 facePos = new Vector3(cam.position.x, this.transform.position.y, cam.position.z);
        this.transform.LookAt(facePos);
        this.transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == 3) // 3:Ground
        {
            //reset jumps if touch the ground, think about how to make it work more accuratly
            jumpsUsed = 0;
        }
    }
}
