using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public float radius = 0.5f; //radius of interactable area --- IN THE FUTURE ASSIGN THIS FROM AN ITEM CLASS AS EACH ITEM WILL HAVE A DIFFERENT INTERACT RADIUS
    public Transform interactionPoint; //transform of where interaction will occur from

    private void OnDrawGizmosSelected()
    {
        if(interactionPoint == null) //if no interaction point has been set
        {
            interactionPoint = transform; //set interaction point to this object's own transform
        }

        Gizmos.color = Color.yellow; //assign colour to yellow
        Gizmos.DrawWireSphere(interactionPoint.position, radius); //draw a sphere to show interactable area
    }

    public virtual void Interact()
    {
        Debug.Log("Player interacted with: " + transform.name); //debug to say the player interacted with the item
    }
}
