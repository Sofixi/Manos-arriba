using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    // Multiplicador de velocidad
    public float speedMultiplier = 2f;

    // Tiempo que dura el efecto
    public float duration = 5f;

    // Para evitar activarlo varias veces
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        // Evita múltiples activaciones
        if (activated) return;

        // Buscar PlayerMovement
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            activated = true;

            // Desactivar collider
            GetComponent<Collider>().enabled = false;

            // Ocultar objeto
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (mesh != null)
                mesh.enabled = false;

            // Iniciar efecto
            StartCoroutine(ApplySpeedBoost(player));
        }
    }

    IEnumerator ApplySpeedBoost(PlayerMovement player)
    {
        // Guardar velocidad original
        float originalSpeed = player.speed;

        // Aumentar velocidad
        player.speed *= speedMultiplier;

        Debug.Log("Power Up activado");

        float timeLeft = duration;

        // Contador regresivo
        while (timeLeft > 0)
        {
            Debug.Log("Tiempo restante del boost: " + timeLeft.ToString("F1") + " segundos");

            yield return new WaitForSeconds(1f);

            timeLeft -= 1f;
        }

        // Regresar velocidad normal
        player.speed = originalSpeed;

        Debug.Log("Power Up terminado");

        // Destruir objeto
        Destroy(gameObject);
    }
}