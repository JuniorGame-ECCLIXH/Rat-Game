using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockOn : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CameraLockOn cameraTarget;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private float lockOnRange = 5;
    [SerializeField] private LayerMask lockOnLayer;
    [SerializeField] private LayerMask playerLayer;
    private bool lockedOn = false;
    private Transform target;
    private PlayerGrapple playerGrapple;

    private void Start()
    {
        playerGrapple = GetComponent<PlayerGrapple>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) //temp key
        {
            if(!lockedOn)
            {
                CheckForTarget();
                if (lockedOn)
                {
                    cameraAnimator.Play("Lock On Camera");
                }
            }
            else
            {
                SetFollowCamera();
            }
        }

        if (target)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            Vector3 direction = (target.position - transform.position).normalized;

            if (distance > lockOnRange || Physics.Raycast(transform.position, direction, distance, ~playerLayer))
            {
                SetFollowCamera();
            }
            
            Debug.DrawRay(transform.position, direction * distance, Color.cyan);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);
    }

    public void SetFollowCamera()
    {
        cameraAnimator.Play("Follow Camera");
        playerGrapple.SetGrapplePoint(null);
        lockedOn = false;
        target = null;
    }

    private void CheckForTarget()
    {
        Collider[] lockOnCheck = Physics.OverlapSphere(transform.position, lockOnRange, lockOnLayer);
        float currentBest = float.MinValue;
        LockOnTarget bestTarget = null;

        foreach(Collider target in lockOnCheck)
        {
            LockOnTarget lockTarget = target.GetComponent<LockOnTarget>(); //ensures we have a lock on target script
            if (lockTarget)
            {
                Vector3 targetPosition = target.transform.position;
                Vector3 direction = (targetPosition - transform.position);
                float dotProduct = Vector3.Dot(targetPosition, playerCamera.transform.forward); //how in line the target is with camera forward
                float distance = Vector3.Distance(transform.position, targetPosition);
                
                if (dotProduct > currentBest && !Physics.Raycast(transform.position, direction, distance, ~playerLayer))
                {
                    currentBest = dotProduct;
                    cameraTarget.SetLockOnTarget(lockTarget);
                    lockedOn = true;
                    bestTarget = lockTarget;
                }
            }
        }

        //not the best way, but the best I could think of
        if(bestTarget)
        {
            target = bestTarget.transform;
            
            GrapplePoint grapplePoint = bestTarget.GetComponent<GrapplePoint>();

            if(grapplePoint)
            {
                playerGrapple.SetGrapplePoint(grapplePoint);
            }
        }
    }
}
