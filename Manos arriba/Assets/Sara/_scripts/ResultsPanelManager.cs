using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanelManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject resultsPanel;

    [Header("Player 1")]
    public TextMeshProUGUI p1SimilarityText;
    public TextMeshProUGUI p1ScoreText;

    [Header("Player 2")]
    public TextMeshProUGUI p2SimilarityText;
    public TextMeshProUGUI p2ScoreText;

    [Header("Managers")]
    public ScoreManager scoreManager;

    // Nombre de la siguiente escena
    public string nextSceneName;

    void Start()
    {
        // Ocultar panel al iniciar
        resultsPanel.SetActive(false);
    }

    // Mostrar resultados
    public void ShowResults()
    {
        // Activar panel
        resultsPanel.SetActive(true);

        // Mostrar similitud P1
        p1SimilarityText.text =
        "Similitud: "
        + scoreManager.player1Similarity.ToString("F0")
        + "%";

        // Mostrar score P1
        p1ScoreText.text =
        "Puntaje: "
        + scoreManager.player1Score;

        // Mostrar similitud P2
        p2SimilarityText.text =
        "Similitud: "
        + scoreManager.player2Similarity.ToString("F0")
        + "%";

        // Mostrar score P2
        p2ScoreText.text =
        "Puntaje: "
        + scoreManager.player2Score;
    }

    // Botón siguiente ronda
    public void NextRound()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}