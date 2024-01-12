using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockOn : MonoBehaviour
{
    public void SetLockOnTarget(LockOnTarget target)
    {
        transform.localPosition = target.GetWorldPosition();
    }
}
