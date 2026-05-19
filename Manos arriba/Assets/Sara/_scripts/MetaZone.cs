using UnityEngine;

public class MetaZone : MonoBehaviour
{
    // Referencia al TimeController
    public TimeController timeController;

    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        // Evitar múltiples activaciones
        if (finished)
        {
            return;
        }

        // Revisar si es jugador
        if (other.CompareTag("Player"))
        {
            finished = true;

            Debug.Log(
                other.name +
                " llegó a la meta"
            );

            // Terminar ronda
            timeController.StopTime();

            // Calcular resultados
            timeController.scoreManager
            .CalculateRoundResults();

            // Mostrar panel resultados
            timeController.resultsPanelManager
            .ShowResults();

            // Detener jugadores
            PlayerMovement[] players =
            FindObjectsOfType<PlayerMovement>();

            foreach (PlayerMovement player in players)
            {
                player.enabled = false;
            }
        }
    }
}