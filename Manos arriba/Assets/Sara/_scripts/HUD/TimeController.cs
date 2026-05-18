using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [Header("Tiempo")]
    public float tiempoInicial = 60f;

    // Tiempo actual
    private float tiempoActual;

    [Header("UI")]
    public TextMeshProUGUI textoTiempo;

    // Barra visual del tiempo
    public Image barraTiempo;

    [Header("Estado")]

    // Revisar si el tiempo sigue corriendo
    private bool corriendo = true;

    //[Header("Managers")]

    // Referencia al ScoreManager
    //public ScoreManager scoreManager;

    void Start()
    {
        // Reinicia tiempo al iniciar
        tiempoActual = tiempoInicial;
    }

    void Update()
    {
        // Si el tiempo está detenido
        if (!corriendo)
        {
            return;
        }

        // Restar tiempo
        tiempoActual -= Time.deltaTime;

        // Evitar negativos
        if (tiempoActual <= 0)
        {
            tiempoActual = 0;

            // Detener tiempo
            corriendo = false;

            // Ejecutar final de ronda
            FinDelTiempo();
        }

        // Actualizar interfaz
        ActualizarUI();
    }

    // Método que actualiza HUD del tiempo
    void ActualizarUI()
    {
        // Obtener minutos
        int minutos =
        Mathf.FloorToInt(tiempoActual / 60f);

        // Obtener segundos
        int segundos =
        Mathf.FloorToInt(tiempoActual % 60f);

        // Mostrar tiempo en texto
        textoTiempo.text =
        minutos.ToString("00")
        + ":"
        + segundos.ToString("00");

        // Normalizar tiempo
        float t =
        tiempoActual / tiempoInicial;

        // Actualizar barra
        barraTiempo.fillAmount = t;

        // Cambiar color según tiempo
        barraTiempo.color =
        Color.Lerp(
            Color.red,
            Color.green,
            t
        );
    }

    // Método que ocurre cuando se acaba el tiempo
    void FinDelTiempo()
    {
        Debug.Log("Se acabó el tiempo");

        // Calcular resultados finales
        //scoreManager.CalculateRoundResults();

        // Detener movimiento de jugadores
        StopPlayers();
    }

    // Método para detener ronda manualmente
    public void StopTime()
    {
        // Detener tiempo
        corriendo = false;

        Debug.Log("Ronda terminada");
    }

    // Método para reiniciar tiempo
    public void ReiniciarTiempo()
    {
        Debug.Log("Reinicia");

        // Reinicia contador
        tiempoActual = tiempoInicial;

        // Reactiva tiempo
        corriendo = true;
    }

    // Método que detiene movimiento jugadores
    void StopPlayers()
    {
        // Busca todos los PlayerMovement
        PlayerMovement[] players =
        FindObjectsOfType<PlayerMovement>();

        // Recorre jugadores
        foreach (PlayerMovement player in players)
        {
            // Desactivar script movimiento
            player.enabled = false;
        }
    }
}
