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

    public RecipeManager recipeManager;

    public FinalResultsManager finalResultsManager;


    [Header("Rounds")]

    // Nombre de la siguiente escena
    public string nextSceneName;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        // Si no asignaste ScoreManager manualmente,
        // intentar encontrar el que persiste entre escenas.

        if (scoreManager == null)
        {
            scoreManager =
                ScoreManager.Instance;
        }


        // Ocultar panel
        resultsPanel.SetActive(false);


        // Reiniciar barras visuales
        p1FillImage.fillAmount = 0;

        p2FillImage.fillAmount = 0;
    }


    // =========================================================
    // MOSTRAR RESULTADOS
    // =========================================================

    public void ShowResults()
    {
        // Verificar ScoreManager
        if (scoreManager == null)
        {
            scoreManager =
                ScoreManager.Instance;
        }


        if (scoreManager == null)
        {
            Debug.LogError(
                "ResultsPanelManager: No se encontró ScoreManager."
            );

            return;
        }


        // Activar panel
        resultsPanel.SetActive(true);


        // -----------------------------------------------------
        // PLAYER 1
        // -----------------------------------------------------

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


        // -----------------------------------------------------
        // PLAYER 2
        // -----------------------------------------------------

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


    // =========================================================
    // SIGUIENTE RONDA
    // =========================================================

    public void NextRound()
    {
        if (scoreManager == null)
        {
            scoreManager =
                ScoreManager.Instance;
        }


        // -----------------------------------------------------
        // ¿ES LA ÚLTIMA RONDA?
        // -----------------------------------------------------

        if (
            scoreManager != null &&
            scoreManager.currentRound >=
            scoreManager.totalRounds
        )
        {
            Debug.Log(
                "Última ronda completada."
            );

            // Ocultar resultados
            resultsPanel.SetActive(false);


            // Mostrar resultado final
            if (finalResultsManager != null)
            {
                finalResultsManager.ShowFinalResults();
            }
            else
            {
                Debug.LogError(
                    "No hay FinalResultsManager asignado."
                );
            }


            return;
        }


        // -----------------------------------------------------
        // TODAVÍA HAY OTRA RONDA
        // -----------------------------------------------------

        if (scoreManager != null)
        {
            scoreManager.AdvanceRound();
        }


        // Verificar nombre de escena
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError(
                "No se ha asignado nextSceneName."
            );

            return;
        }


        // Cargar siguiente escena
        SceneManager.LoadScene(
            nextSceneName
        );
    }


    // =========================================================
    // ANIMAR PORCENTAJE
    // =========================================================

    IEnumerator AnimatePercentage(
        TextMeshProUGUI text,
        float targetValue
    )
    {
        float current = 0;


        while (current < targetValue)
        {
            current +=
                Time.deltaTime * 25f;


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


        // Asegurar valor final
        text.text =
            "Similitud: "
            + targetValue.ToString("F0")
            + "%";
    }


    // =========================================================
    // ANIMAR PUNTAJE
    // =========================================================

    IEnumerator AnimateScore(
        TextMeshProUGUI text,
        int targetValue
    )
    {
        int current = 0;


        while (current < targetValue)
        {
            current +=
                Mathf.CeilToInt(
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


        // Asegurar valor final
        text.text =
            "Puntaje: "
            + targetValue;
    }


    // =========================================================
    // ANIMAR BARRA
    // =========================================================

    IEnumerator AnimateFill(
        Image image,
        float targetFill
    )
    {
        float current = 0;


        while (current < targetFill)
        {
            current +=
                Time.deltaTime;


            if (current > targetFill)
            {
                current = targetFill;
            }


            image.fillAmount =
                current;


            yield return null;
        }


        // Asegurar valor final
        image.fillAmount =
            targetFill;
    }
}