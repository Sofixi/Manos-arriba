using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // Detecta si ya pasó por menú principal
    private static bool gameStarted = false;

    [Header("Panels")]

    public GameObject mainMenuPanel;

    public GameObject recipePanel;

    public GameObject hudPanel;

    public GameObject pausePanel;

    public GameObject tutorialPanel;

    public GameObject warningPanel;

    public GameObject resultsPanel;

    public GameObject finalWinnerPanel;

    [Header("Estado")]

    private bool isPaused = false;

    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        HideAllPanels();

        // =============================================
        // SI YA INICIÓ EL JUEGO
        // =============================================

        if (gameStarted)
        {
            // Mostrar pantalla receta
            recipePanel.SetActive(true);

            // Música de espera / menú
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMenuMusic();
            }

            // Pausar gameplay
            Time.timeScale = 0f;
        }

        // =============================================
        // PRIMERA VEZ
        // =============================================

        else
        {
            mainMenuPanel.SetActive(true);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMenuMusic();
            }

            Time.timeScale = 0f;
        }
    }

    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        // Pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // =====================================================
    // OCULTAR TODOS LOS PANELES
    // =====================================================

    public void HideAllPanels()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (hudPanel != null)
            hudPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (warningPanel != null)
            warningPanel.SetActive(false);

        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        if (finalWinnerPanel != null)
            finalWinnerPanel.SetActive(false);

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (recipePanel != null)
            recipePanel.SetActive(false);
    }

    // =====================================================
    // MOSTRAR PANEL TEMPORAL
    // =====================================================

    IEnumerator ShowTemporaryPanel(
        GameObject panel,
        float duration
    )
    {
        if (panel == null)
            yield break;

        panel.SetActive(true);

        yield return new WaitForSecondsRealtime(duration);

        panel.SetActive(false);
    }

    // =====================================================
    // BOTÓN PLAY
    // =====================================================

    public void StartGame()
    {
        gameStarted = true;

        // Ocultar todo
        HideAllPanels();

        // IMPORTANTE:
        // NO mostramos el HUD todavía.
        // La cinemática y el contador van primero.

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameplayMusic();
        }

        // El juego puede comenzar a correr,
        // pero el HUD permanecerá oculto.
        Time.timeScale = 1f;
    }

    // =====================================================
    // MOSTRAR HUD
    // =====================================================

    public void ShowHUD()
    {
        if (hudPanel != null)
        {
            hudPanel.SetActive(true);
        }

        Debug.Log("HUD ACTIVADO");
    }

    // =====================================================
    // OCULTAR HUD
    // =====================================================

    public void HideHUD()
    {
        if (hudPanel != null)
        {
            hudPanel.SetActive(false);
        }

        Debug.Log("HUD DESACTIVADO");
    }

    // =====================================================
    // REINICIAR RONDA
    // =====================================================

    public void RestartRound()
    {
        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }

    // =====================================================
    // PAUSA
    // =====================================================

    public void TogglePause()
    {
        // No pausar si HUD no está activo
        if (hudPanel == null ||
            !hudPanel.activeSelf)
        {
            return;
        }

        isPaused = !isPaused;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // =====================================================
    // PANTALLA RECETA
    // =====================================================

    public void ShowRecipeScreen()
    {
        HideAllPanels();

        if (recipePanel != null)
        {
            recipePanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        Time.timeScale = 0f;
    }

    // =====================================================
    // ADVERTENCIA
    // =====================================================

    public void ShowWarningPanel()
    {
        StartCoroutine(
            ShowTemporaryPanel(
                warningPanel,
                3f
            )
        );
    }

    // =====================================================
    // TUTORIAL
    // =====================================================

    public void ShowTutorialPanel()
    {
        StartCoroutine(
            ShowTemporaryPanel(
                tutorialPanel,
                5f
            )
        );
    }

    // =====================================================
    // OCULTAR ADVERTENCIA
    // =====================================================

    public void HideWarning()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }

    // =====================================================
    // RESULTADOS
    // =====================================================

    public void ShowResults()
    {
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
        }
    }

    // =====================================================
    // GANADOR FINAL
    // =====================================================

    public void ShowFinalWinner()
    {
        if (finalWinnerPanel != null)
        {
            finalWinnerPanel.SetActive(true);
        }
    }

    // =====================================================
    // VOLVER AL MENÚ
    // =====================================================

    public void ReturnToMenu()
    {
        gameStarted = false;

        HideAllPanels();

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }

        Time.timeScale = 0f;
    }

    // =====================================================
    // SALIR
    // =====================================================

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Salir juego");
    }

    // =====================================================
    // SONIDO BOTONES
    // =====================================================

    public void PlayButtonSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                AudioManager.Instance.buttonClick
            );
        }
    }
}