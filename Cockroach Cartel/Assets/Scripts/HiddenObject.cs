using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    // Define the layer index to set when in the trigger
    [SerializeField] private int newLayerIndex;

    // Store the original layer index before changing it
    [SerializeField] private int originalLayerIndex;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            //Stores the original layer index of the player
            originalLayerIndex = other.gameObject.layer;
            // Change the layer index to the newLayerIndex
            ChangeLayer(newLayerIndex, other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player or any other relevant object
        if (other.CompareTag("Player"))
        {
            // Revert the layer index to the originalLayerIndex
            ChangeLayer(originalLayerIndex, other.gameObject);
        }
    }

    private void ChangeLayer(int newLayerIndex, GameObject player)
    {
        // Change the layer index of the GameObject
        player.layer = newLayerIndex;
    }
}
