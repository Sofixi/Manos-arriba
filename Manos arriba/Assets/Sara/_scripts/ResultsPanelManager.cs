using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ResultsPanelManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject resultsPanel;

    [Header("Player 1")]
    public TextMeshProUGUI p1SimilarityText;
    public TextMeshProUGUI p1ScoreText;
    public Image p1FillImage;

    [Header("Player 2")]
    public TextMeshProUGUI p2SimilarityText;
    public TextMeshProUGUI p2ScoreText;
    public Image p2FillImage;

    [Header("Managers")]
    public ScoreManager scoreManager;

    // Nombre de la siguiente escena
    public string nextSceneName;

    void Start()
    {
        // Ocultar panel al iniciar
        resultsPanel.SetActive(false);

        // Reiniciar barras
        p1FillImage.fillAmount = 0;
        p2FillImage.fillAmount = 0;
    }

    // Mostrar resultados
    public void ShowResults()
    {
        // Activar panel
        resultsPanel.SetActive(true);

        // PLAYER 1

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

        StartCoroutine(
            AnimateFill(
                p1FillImage,
                scoreManager.player1Similarity / 100f
            )
        );

        // PLAYER 2

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

        StartCoroutine(
            AnimateFill(
                p2FillImage,
                scoreManager.player2Similarity / 100f
            )
        );
    }

    // Botón siguiente ronda
    public void NextRound()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator AnimatePercentage(
    TextMeshProUGUI text,
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
        TextMeshProUGUI text,
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

    IEnumerator AnimateFill(
        Image image,
        float targetFill)
    {
        float current = 0;

        while (current < targetFill)
        {
            current += Time.deltaTime;

            if (current > targetFill)
            {
                current = targetFill;
            }

            image.fillAmount = current;

            yield return null;
        }
    }
}