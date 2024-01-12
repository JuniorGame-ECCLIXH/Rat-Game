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
    private bool lockedOn = false;

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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, lockOnRange);
    }

    public void SetFollowCamera()
    {
        cameraAnimator.Play("Follow Camera");
        lockedOn = false;
    }

    private void CheckForTarget()
    {
        Collider[] lockOnCheck = Physics.OverlapSphere(transform.position, lockOnRange, lockOnLayer);
        float currentBest = 0;
        LockOnTarget bestTarget = null;

        foreach(Collider target in lockOnCheck)
        {
            LockOnTarget lockTarget = target.GetComponent<LockOnTarget>(); //ensures we have a lock on target script
            if (lockTarget)
            {
                float dotProduct = Vector3.Dot(target.transform.position, playerCamera.transform.forward); //how in line the target is with camera forward
                if (dotProduct > currentBest)
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
            GrapplePoint grapplePoint = bestTarget.GetComponent<GrapplePoint>();

            if(grapplePoint)
            {
                GetComponent<PlayerGrapple>().SetGrapplePoint(grapplePoint);
            }
        }
    }
}
