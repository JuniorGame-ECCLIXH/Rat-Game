using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    // Define the layer index to set when in the trigger
    [SerializeField] private LayerMask newLayer;

    // Store the original layer index before changing it
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            // Change the layer index to the newLayerIndex
            int layerIndex = (int)Mathf.Log(newLayer.value, 2);
            ChangeLayer(layerIndex, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            // Revert the layer index to the originalLayerIndex
            int layerIndex = (int)Mathf.Log(playerLayer.value, 2);
            ChangeLayer(layerIndex, other.gameObject);
        }
    }

    private void ChangeLayer(int newLayerIndex, GameObject player)
    {
        // Change the layer index of the GameObject
        player.layer = newLayerIndex;
    }
}
