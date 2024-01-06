using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    public static bool IsStealthed;
    [SerializeField] private MeshRenderer _playerMesh;
    [SerializeField] private Material _stealthMaterial;
    private Material _playerMaterial;

    private void Start()
    {
        _playerMaterial = _playerMesh.material;
        IsStealthed = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleStealth();
        }
    }

    public void ToggleStealth()
    {
        IsStealthed = !IsStealthed;
        _playerMesh.material = IsStealthed ? _stealthMaterial : _playerMaterial;
    }
}
