using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMagicScript : MonoBehaviour
{
    public GameObject Stairs;
    Vector3 targetPosition = new Vector3(0f, 1.5f, 13.5f);
    Vector3 startPosition = new Vector3(0f, -2.64f, 0f);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Spell"));
        Activate();
    }

    public void Activate()
    {
        Stairs.transform.position = targetPosition;
    }

}
