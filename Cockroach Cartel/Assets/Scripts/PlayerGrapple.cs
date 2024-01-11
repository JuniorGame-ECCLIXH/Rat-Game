using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    [SerializeField] private float grappleSpeed;
    [SerializeField] private float playerHeight = 2;
    /*temp serialize*/
    [SerializeField] private GrapplePoint grappleTarget;
    [SerializeField] private PlayerController controller;
    private Vector3 targetPos;
    private bool isGrappling = false;

    private void Start()
    {
        if(!controller)
        {
            controller = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            MoveGrapple();
        }
    }

    private void FixedUpdate()
    {
        if(isGrappling)
        {
            JumpToPoint();

            if(Vector3.Distance(new Vector3(transform.position.x, transform.position.y - playerHeight/2, 
                transform.position.z), targetPos) < 0.5f)
            {
                isGrappling = false;
                controller.EnablePlayerMovement();
            }
        }
    }

    public bool IsGrappling() => isGrappling;

    private void JumpToPoint()
    {
        float desiredY = Mathf.Abs(targetPos.z - transform.position.z);
        Vector3 desiredPos = new Vector3(targetPos.x, targetPos.y + desiredY + playerHeight/2, targetPos.z);
        transform.position = Vector3.MoveTowards(transform.position, desiredPos, grappleSpeed * Time.deltaTime);
    }

    private void MoveGrapple()
    {
        targetPos = grappleTarget.GetEndPoint();
        isGrappling = true;
        controller.DisablePlayerMovement();
    }
}
