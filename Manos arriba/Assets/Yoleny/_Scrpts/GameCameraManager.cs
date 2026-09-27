using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameCameraManager : MonoBehaviour
{
    [Header("CÁMARAS")]

    public Camera cinematicCamera;
    public Camera playerCamera1;
    public Camera playerCamera2;

    [Header("JUGADORES")]

    public PlayerMovement player1;
    public PlayerMovement player2;

    [Header("UI")]

    public UIManager uiManager;

    public TMP_Text countdownText;

    [Header("TIMELINE")]

    public PlayableDirector director;
    [Header("TIMER")]
    public TimeController timeController;

    [Header("TIEMPOS")]

    public float numberDuration = 1f;
    public float startMessageDuration = 1f;

    // =====================================================
    // START
    // =====================================================

    private void Start()
    {
        // =============================================
        // CÁMARA CINEMÁTICA
        // =============================================

        if (cinematicCamera != null)
        {
            cinematicCamera.gameObject.SetActive(true);
        }

        // =============================================
        // CÁMARAS DE JUGADORES APAGADAS
        // =============================================

        if (playerCamera1 != null)
        {
            playerCamera1.gameObject.SetActive(false);
        }

        if (playerCamera2 != null)
        {
            playerCamera2.gameObject.SetActive(false);
        }

        // =============================================
        // BLOQUEAR JUGADORES
        // =============================================

        if (player1 != null)
        {
            player1.SetMovementEnabled(false);
        }

        if (player2 != null)
        {
            player2.SetMovementEnabled(false);
        }

        // =============================================
        // OCULTAR HUD
        // =============================================

        if (uiManager != null)
        {
            uiManager.HideHUD();
        }

        // =============================================
        // OCULTAR CONTADOR
        // =============================================

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        // =============================================
        // ESPERAR TIMELINE
        // =============================================

        if (director != null)
        {
            director.stopped += TerminarCinematica;
        }
    }

    // =====================================================
    // TERMINA CINEMÁTICA
    // =====================================================

    private void TerminarCinematica(
        PlayableDirector obj
    )
    {
        // =============================================
        // APAGAR CÁMARA CINEMÁTICA
        // =============================================

        if (cinematicCamera != null)
        {
            cinematicCamera.gameObject.SetActive(false);
        }

        // =============================================
        // ACTIVAR CÁMARAS DE JUGADORES
        // =============================================

        if (playerCamera1 != null)
        {
            playerCamera1.gameObject.SetActive(true);
        }

        if (playerCamera2 != null)
        {
            playerCamera2.gameObject.SetActive(true);
        }

        // =============================================
        // EMPEZAR CUENTA REGRESIVA
        // =============================================

        StartCoroutine(Countdown());
    }

    // =====================================================
    // CUENTA REGRESIVA
    // =====================================================

    private IEnumerator Countdown()
    {
        if (countdownText == null)
        {
            StartCompetition();
            yield break;
        }

        countdownText.gameObject.SetActive(true);

        // =============================================
        // 3
        // =============================================

        countdownText.text = "3";

        yield return new WaitForSecondsRealtime(
            numberDuration
        );

        // =============================================
        // 2
        // =============================================

        countdownText.text = "2";

        yield return new WaitForSecondsRealtime(
            numberDuration
        );

        // =============================================
        // 1
        // =============================================

        countdownText.text = "1";

        yield return new WaitForSecondsRealtime(
            numberDuration
        );

        // =============================================
        // A COCINAR
        // =============================================

        countdownText.text = "¡A COCINAR!";

        yield return new WaitForSecondsRealtime(
            startMessageDuration
        );

        // =============================================
        // INICIAR COMPETENCIA
        // =============================================

        countdownText.gameObject.SetActive(false);

        StartCompetition();
    }

    // =====================================================
    // INICIAR COMPETENCIA
    // =====================================================

    private void StartCompetition()
    {
        Debug.Log("=================================");
        Debug.Log("¡A COCINAR! COMPETENCIA INICIADA");
        Debug.Log("TIME SCALE: " + Time.timeScale);
        Debug.Log("=================================");

        // Asegurar que el juego está corriendo
        Time.timeScale = 1f;

        // Permitir movimiento
        if (player1 != null)
            player1.SetMovementEnabled(true);

        if (player2 != null)
            player2.SetMovementEnabled(true);

        // Mostrar HUD
        if (uiManager != null)
            uiManager.ShowHUD();

        // Iniciar contador
        if (timeController != null)
        {
            timeController.StartTime();
        }
        else
        {
            Debug.LogError("¡¡NO HAY TimeController ASIGNADO EN GameCameraManager!!");
        }
    }
    // =====================================================
    // DESTROY
    // =====================================================

    private void OnDestroy()
    {
        if (director != null)
        {
            director.stopped -= TerminarCinematica;
        }
    }
}