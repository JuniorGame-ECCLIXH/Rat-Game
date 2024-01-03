using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //setup
    public Transform target;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float distance;
    //cam movment
    [SerializeField] private float angle;
    private float holdAngle;
    private float angleShift = 3;

    void Update()
    {
        //set rotation of direction, then rotate by mouse
        KeepAngle();

        if (Input.GetAxis("Mouse X") > 0)
        {
            angle += angleShift;
        }
        else if (Input.GetAxis("Mouse X") < 0)
        {
            angle -= angleShift;
        }

        //set position of cam
        this.transform.position = target.position + (direction.normalized * distance);

        //set cam target and look at, edit Y by mouse
        Vector3 tempTarget = target.position;
        if (Input.GetAxis("Mouse Y") > 0)
        {
            holdAngle += angleShift/50;
        }
        else if (Input.GetAxis("Mouse Y") < 0)
        {
            holdAngle -= angleShift/50;
        }

        tempTarget.y += holdAngle;

        this.transform.LookAt(tempTarget);

        
    }

    private void KeepAngle()
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = Mathf.Sin(angle * Mathf.Deg2Rad);
        direction.x = x;
        direction.z = z;
    }
}
