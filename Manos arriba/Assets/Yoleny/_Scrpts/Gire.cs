using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gire : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement jugador = other.GetComponent<PlayerMovement>();

        if (jugador == null)
        {
            jugador = other.GetComponentInParent<PlayerMovement>();
        }

        if (jugador != null)
        {
            jugador.MorirPorObstaculo();
        }
    }
}