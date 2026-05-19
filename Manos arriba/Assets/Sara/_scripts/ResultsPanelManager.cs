using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        // Animaciones Player 1
        StartCoroutine(
            AnimatePercentage(
                p1SimilarityText,
                scoreManager.player1Similarity
            )
        );

        StartCoroutine(
            AnimateScore(
                p1ScoreText,
                scoreManager.player1Score
            )
        );

        // Animaciones Player 2
        StartCoroutine(
            AnimatePercentage(
                p2SimilarityText,
                scoreManager.player2Similarity
            )
        );

        StartCoroutine(
            AnimateScore(
                p2ScoreText,
                scoreManager.player2Score
            )
        );
    }

    // Botón siguiente ronda
    public void NextRound()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator AnimatePercentage(
    TMPro.TextMeshProUGUI text,
    float targetValue)
    {
        float current = 0;

        while (current < targetValue)
        {
            current += Time.deltaTime * 25f;

            if (current > targetValue)
            {
                current = targetValue;
            }

            text.text =
            "Similitud: "
            + current.ToString("F0")
            + "%";

            yield return null;
        }
    }

    IEnumerator AnimateScore(
        TMPro.TextMeshProUGUI text,
        int targetValue)
    {
        int current = 0;

        while (current < targetValue)
        {
            current += Mathf.CeilToInt(
                Time.deltaTime * 200f
            );

            if (current > targetValue)
            {
                current = targetValue;
            }

            text.text =
            "Puntaje: "
            + current;

            yield return null;
        }
    }

}