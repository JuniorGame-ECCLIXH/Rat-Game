using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTarget : MonoBehaviour
{
    private Vector3 worldPos;

    private void Start()
    {
        worldPos = transform.position;
    }

    public Vector3 GetWorldPosition() => worldPos;

}
