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
    public UIManager uiManager;

    // Barra visual del tiempo
    public Image barraTiempo;

    [Header("Estado")]

    // Revisar si el tiempo sigue corriendo
    private bool corriendo = true;

    [Header("Managers")]

    //Referencia al ScoreManager
    public ScoreManager scoreManager;

    //Ref al panel manager
    public ResultsPanelManager resultsPanelManager;

    private bool warningTriggered = false;

    void Start()
    {
        // Reinicia tiempo al iniciar
        tiempoActual = tiempoInicial;
    }

    void Update()
    {
        // Si el tiempo est� detenido
        if (!corriendo)
        {
            return;
        }

        // Restar tiempo
        tiempoActual -= Time.deltaTime;

        // Warning cuando queden 30 segundos
    if (tiempoActual <= 30f
        && !warningTriggered)
    {
        warningTriggered = true;

        uiManager.ShowWarningPanel();
    }

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

    // M�todo que actualiza HUD del tiempo
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

        // Cambiar color seg�n tiempo
        barraTiempo.color =
        Color.Lerp(
            Color.red,
            Color.green,
            t
        );
    }

    // M�todo que ocurre cuando se acaba el tiempo
    void FinDelTiempo()
    {
        Debug.Log("Se acab� el tiempo");

        // Calcula resultados finales
        scoreManager.CalculateRoundResults();

        // Mostrar panel resultados
        resultsPanelManager.ShowResults();

        // Detener movimiento de jugadores
        StopPlayers();
    }

    // M�todo para detener ronda manualmente
    public void StopTime()
    {
        // Detener tiempo
        corriendo = false;

        Debug.Log("Ronda terminada");
    }

    // M�todo para reiniciar tiempo
    public void ReiniciarTiempo()
    {
        Debug.Log("Reinicia");

        // Reinicia contador
        tiempoActual = tiempoInicial;

        // Reactiva tiempo
        corriendo = true;

        warningTriggered = false;
    }

    // M�todo que detiene movimiento jugadores
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
