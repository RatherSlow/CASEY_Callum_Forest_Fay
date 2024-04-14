using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PortalEffect : MonoBehaviour
{
    public Transform exitPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Teleport the player to the exit portal's position
            other.transform.position = exitPortal.position;
            Debug.Log("Collided");
        }
    }
}
