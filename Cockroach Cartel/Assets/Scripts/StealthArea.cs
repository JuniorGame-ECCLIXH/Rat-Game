using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerStealth player = other.GetComponent<PlayerStealth>();
            player.SetStealthed();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStealth player = other.GetComponent<PlayerStealth>();
            player.UnsetStealthed();
        }
    }
}
