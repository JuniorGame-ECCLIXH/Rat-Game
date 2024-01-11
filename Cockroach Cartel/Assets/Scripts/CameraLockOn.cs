using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOn : MonoBehaviour
{
    [SerializeField] private LockOnTarget target;

    private void Start()
    {
        transform.localPosition = target.GetWorldPosition();
    }
}
