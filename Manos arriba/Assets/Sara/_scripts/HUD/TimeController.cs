using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoTiempo;
    public UIManager uiManager;

    // Barra visual del tiempo
    public Image barraTiempo;

    [Header("Estado")]
    private float tiempoActual;
    private float tiempoInicial;
    private bool corriendo = false;

    [Header("Managers")]
    public ScoreManager scoreManager;
    public ResultsPanelManager resultsPanelManager;

    private bool warningTriggered = false;


    void Start()
    {
        // Leer el tiempo que escribiste en el texto
        tiempoInicial = ObtenerTiempoDelTexto();

        tiempoActual = tiempoInicial;

        // El timer espera al 3, 2, 1
        corriendo = false;

        ActualizarUI();
    }


    void Update()
    {
        if (!corriendo)
        {
            return;
        }

        // Restar tiempo
        tiempoActual -= Time.deltaTime;


        // Warning cuando queden 30 segundos
        if (tiempoActual <= 30f && !warningTriggered)
        {
            warningTriggered = true;

            if (uiManager != null)
            {
                uiManager.ShowWarningPanel();
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(
                    AudioManager.Instance.warningSFX
                );

                AudioManager.Instance.PlayWarningMusic();
            }
        }


        // Evitar negativos
        if (tiempoActual <= 0)
        {
            tiempoActual = 0;

            corriendo = false;

            FinDelTiempo();
        }


        ActualizarUI();
    }


    // =========================================================
    // INICIAR TIMER
    // =========================================================

    public void StartTime()
    {
        Debug.Log("=================================");
        Debug.Log("TIME CONTROLLER: INICIANDO");
        Debug.Log("Tiempo inicial: " + tiempoInicial);
        Debug.Log("=================================");

        // Volver al tiempo que colocaste en el texto
        tiempoActual = tiempoInicial;

        warningTriggered = false;

        corriendo = true;

        ActualizarUI();
    }


    // =========================================================
    // LEER TIEMPO DEL TEXTO
    // =========================================================

    float ObtenerTiempoDelTexto()
    {
        if (textoTiempo == null)
        {
            Debug.LogError("No hay Texto Tiempo asignado.");
            return 0f;
        }

        string texto = textoTiempo.text.Trim();

        string[] partes = texto.Split(':');

        if (partes.Length != 2)
        {
            Debug.LogError(
                "El tiempo debe tener formato MM:SS. Ejemplo: 01:30"
            );

            return 0f;
        }

        int minutos;
        int segundos;

        if (!int.TryParse(partes[0], out minutos) ||
            !int.TryParse(partes[1], out segundos))
        {
            Debug.LogError(
                "No se pudo leer el tiempo. Usa formato MM:SS."
            );

            return 0f;
        }

        return (minutos * 60f) + segundos;
    }


    // =========================================================
    // ACTUALIZAR HUD
    // =========================================================

    void ActualizarUI()
    {
        int minutos =
            Mathf.FloorToInt(tiempoActual / 60f);

        int segundos =
            Mathf.FloorToInt(tiempoActual % 60f);


        if (textoTiempo != null)
        {
            textoTiempo.text =
                minutos.ToString("00")
                + ":"
                + segundos.ToString("00");
        }


        if (tiempoInicial > 0)
        {
            float t =
                tiempoActual / tiempoInicial;


            if (barraTiempo != null)
            {
                barraTiempo.fillAmount = t;

                barraTiempo.color =
                    Color.Lerp(
                        Color.red,
                        Color.green,
                        t
                    );
            }
        }
    }


    // =========================================================
    // FINAL DE LA RONDA
    // =========================================================

    void FinDelTiempo()
    {
        Debug.Log("Se acabó el tiempo");


        if (scoreManager != null)
        {
            scoreManager.CalculateRoundResults();
        }


        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.roundEndSFX
            );

            AudioManager.Instance.PlayVictoryMusic();
        }


        if (resultsPanelManager != null)
        {
            resultsPanelManager.ShowResults();
        }


        StopPlayers();
    }


    // =========================================================
    // DETENER TIEMPO
    // =========================================================

    public void StopTime()
    {
        corriendo = false;

        Debug.Log("Ronda terminada");
    }


    // =========================================================
    // REINICIAR TIEMPO
    // =========================================================

    public void ReiniciarTiempo()
    {
        Debug.Log("Reinicia");

        tiempoActual = tiempoInicial;

        corriendo = true;

        warningTriggered = false;

        ActualizarUI();
    }


    // =========================================================
    // DETENER JUGADORES
    // =========================================================

    void StopPlayers()
    {
        PlayerMovement[] players =
            FindObjectsOfType<PlayerMovement>();


        foreach (PlayerMovement player in players)
        {
            player.enabled = false;
        }
    }
}