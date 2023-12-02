using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public float radius = 0.5f; //radius of interactable area --- IN THE FUTURE ASSIGN THIS FROM AN ITEM CLASS AS EACH ITEM WILL HAVE A DIFFERENT INTERACT RADIUS

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; //assign colour to yellow
        Gizmos.DrawWireSphere(transform.position, radius); //draw a sphere to show interactable area
    }

    public void DeleteItem()
    {
        this.gameObject.SetActive(false); //disable current game object
    }
}
