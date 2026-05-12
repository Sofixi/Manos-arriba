
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [HideInInspector] public Transform checkPoint;

    public Transform startPos;

    // Altura mínima antes de morir
    public float deathHeight = -20f;

    void Start()
    {
        // El primer checkpoint es el inicio
        checkPoint = startPos;

        // Mueve jugador al inicio
        transform.position = startPos.position;
    }

    void Update()
    {
        CarReset();

        CheckFallDeath();
    }

    // Respawn manual con R
    public void CarReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = checkPoint.position;
        }
    }

    // Detecta si cayó al vacío
    void CheckFallDeath()
    {
        // Si cae debajo de cierta altura
        if (transform.position.y < deathHeight)
        {
            CharacterController cc =
            GetComponent<CharacterController>();

            // Desactiva controller
            cc.enabled = false;

            // Teletransporta
            transform.position = checkPoint.position;

            // Reactiva controller
            cc.enabled = true;
        }
    }
}