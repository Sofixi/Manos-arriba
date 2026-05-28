using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSlow : MonoBehaviour
{
    public float slowMultiplier = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement =
            other.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.speed *= slowMultiplier;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement =
            other.GetComponent<PlayerMovement>();

            if (movement != null)
            {
                movement.speed /=
                slowMultiplier;
            }
        }
    }
}