using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f; // Distance to interact with objects
    [SerializeField] private LayerMask interactableLayer; // Layer mask for interactable objects
    private Camera playerCamera; // Reference to the player's camera
    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found! Make sure your camera has the 'MainCamera' tag.");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);
            var interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                Debug.Log("No InteractableObject script on hit object.");
            }
        }
        else
        {
            Debug.Log("Nothing hit.");
        }
    }
}
