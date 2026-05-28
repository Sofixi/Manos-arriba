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

    void Start()
    {
        HideAllPanels();

        // SI YA INICIÓ EL JUEGO
        // significa que esta es otra ronda
        if (gameStarted)
        {
            // Mostrar pantalla receta
            recipePanel.SetActive(true);

            // Música de espera / menú
            AudioManager.Instance.PlayMenuMusic();

            // Pausar gameplay
            Time.timeScale = 0f;
        }
        else
        {
            // Primera vez -> Main Menu
            mainMenuPanel.SetActive(true);

            AudioManager.Instance.PlayMenuMusic();

            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        // Pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Oculta todos los paneles
    void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);

        hudPanel.SetActive(false);

        pausePanel.SetActive(false);

        warningPanel.SetActive(false);

        resultsPanel.SetActive(false);

        finalWinnerPanel.SetActive(false);

        tutorialPanel.SetActive(false);

        recipePanel.SetActive(false);
    }

    IEnumerator ShowTemporaryPanel(
        GameObject panel,
        float duration
    )
    {
        panel.SetActive(true);

        yield return new WaitForSecondsRealtime(duration);

        panel.SetActive(false);
    }

    // BOTÓN PLAY DEL MENÚ
    public void StartGame()
    {
        // Ya pasó menú principal
        gameStarted = true;

        HideAllPanels();

        // Mostrar HUD
        hudPanel.SetActive(true);

        // Música gameplay
        AudioManager.Instance.PlayGameplayMusic();

        // Mostrar tutorial temporal
        ShowTutorialPanel();

        // Empezar gameplay
        Time.timeScale = 1f;
    }

    // Reiniciar ronda
    public void RestartRound()
    {
        Time.timeScale = 1f;

        Scene currentScene =
        SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }

    // Pausa
    public void TogglePause()
    {
        // No pausar si HUD no está activo
        if (!hudPanel.activeSelf)
        {
            return;
        }

        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Mostrar pantalla receta manualmente
    public void ShowRecipeScreen()
    {
        HideAllPanels();

        recipePanel.SetActive(true);

        AudioManager.Instance.PlayMenuMusic();

        Time.timeScale = 0f;
    }

    // Advertencia tiempo
    public void ShowWarningPanel()
    {
        StartCoroutine(
            ShowTemporaryPanel(
                warningPanel,
                3f
            )
        );
    }

    // Tutorial temporal
    public void ShowTutorialPanel()
    {
        StartCoroutine(
            ShowTemporaryPanel(
                tutorialPanel,
                5f
            )
        );
    }

    // Ocultar advertencia
    public void HideWarning()
    {
        warningPanel.SetActive(false);
    }

    // Mostrar resultados ronda
    public void ShowResults()
    {
        resultsPanel.SetActive(true);
    }

    // Mostrar ganador final
    public void ShowFinalWinner()
    {
        finalWinnerPanel.SetActive(true);
    }

    // Volver al menú principal
    public void ReturnToMenu()
    {
        gameStarted = false;

        HideAllPanels();

        mainMenuPanel.SetActive(true);

        AudioManager.Instance.PlayMenuMusic();

        Time.timeScale = 0f;
    }

    // Salir juego
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Salir del juego");
    }

    // Sonido botones
    public void PlayButtonSound()
    {
        AudioManager.Instance.PlaySFX(
            AudioManager.Instance.buttonClick
        );
    }
}