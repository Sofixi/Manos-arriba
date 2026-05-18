using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    // Fuerza del trampolín
    public float jumpForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró");

        // Revisar si es el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado");

            // Obtener PlayerMovement
            PlayerMovement player =
            other.GetComponent<PlayerMovement>();

            // Si existe el componente
            if (player != null)
            {
                // Aplicar impulso
                player.JumpBoost(jumpForce);

                Debug.Log("Boost aplicado");
            }
        }
    }
}