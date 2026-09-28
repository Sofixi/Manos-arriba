using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodilloMovimiento : MonoBehaviour
{
    [Header("PUNTOS")]
    public Transform puntoA;
    public Transform puntoB;

    [Header("VELOCIDAD")]
    public float velocidadMovimiento = 2f;

    [Header("ROTACIÓN")]
    public float velocidadRotacion = 360f;

    private Transform destino;

    void Start()
    {
        destino = puntoB;
    }

    void Update()
    {
        // Movimiento A B
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino.position,
            velocidadMovimiento * Time.deltaTime
        );

        // Cuando llega al destino, cambia de dirección
        if (Vector3.Distance(transform.position, destino.position) < 0.01f)
        {
            destino = destino == puntoA ? puntoB : puntoA;
        }

        // Rotación constante sobre su propio eje
        transform.Rotate(
            Vector3.right * velocidadRotacion * Time.deltaTime,
            Space.Self
        );
    }
}