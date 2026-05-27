using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    private static bool gameStarted = false;

    [Header("Panels")]

    public GameObject mainMenuPanel;

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

        if (gameStarted)
        {
            hudPanel.SetActive(true);

            Time.timeScale = 1f;
        }
        else
        {
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
    }

    IEnumerator ShowTemporaryPanel(
        GameObject panel,
        float duration
    )
    {
        panel.SetActive(true);

        yield return new WaitForSeconds(duration);

        panel.SetActive(false);
    }

    // Iniciar juego
    public void StartGame()
    {
        gameStarted = true;

        HideAllPanels();

        hudPanel.SetActive(true);

        AudioManager.Instance.PlayGameplayMusic();

        ShowTutorialPanel();

        Time.timeScale = 1f;
    }

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
        // No pausar si no está el HUD activo
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

    // Mostrar advertencia tiempo
    public void ShowWarningPanel()
    {
        StartCoroutine(
            ShowTemporaryPanel(
                warningPanel,
                3f
            )
        );
    }

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

    public void ReturnToMenu()
    {
        gameStarted = false;

        HideAllPanels();

        mainMenuPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // Salir juego
    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Salir del juego");
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlaySFX(
            AudioManager.Instance.buttonClick
        );
    }
}