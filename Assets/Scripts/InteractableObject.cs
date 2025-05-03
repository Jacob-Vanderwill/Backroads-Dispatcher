using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string interactionMessage = "This is an interactable object.";

    public void Interact()
    {
        Debug.Log(interactionMessage);
        // You can add behavior here like opening a UI, starting dialogue, etc.
    }
}
