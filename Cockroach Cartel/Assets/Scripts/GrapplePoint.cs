using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Transform endPoint;

    public Vector3 GetEndPoint() => endPoint.position;
}
