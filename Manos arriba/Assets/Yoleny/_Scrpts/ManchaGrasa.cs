using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManchaGrasa : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1. Algo entró en la grasa: " + other.name);

        PlayerMovement jugador =
            other.GetComponentInParent<PlayerMovement>();

        if (jugador != null)
        {
            Debug.Log("2. Jugador detectado: " + jugador.name);
            jugador.ActivarGrasa();
        }
        else
        {
            Debug.LogWarning(
                "No se encontró PlayerMovement en " + other.name
            );
        }
    }
}