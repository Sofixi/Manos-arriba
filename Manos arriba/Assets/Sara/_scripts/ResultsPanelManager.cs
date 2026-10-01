using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ResultsPanelManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject resultsPanel;


    // =====================================================
    // PLAYER 1
    // =====================================================

    [Header("Player 1")]

    public TextMeshProUGUI p1SimilarityText;

    public TextMeshProUGUI p1IngredientPointsText;

    public TextMeshProUGUI p1StealPointsText;

    public TextMeshProUGUI p1SimilarityPointsText;

    public TextMeshProUGUI p1ScoreText;

    public Image p1FillImage;


    // =====================================================
    // PLAYER 2
    // =====================================================

    [Header("Player 2")]

    public TextMeshProUGUI p2SimilarityText;

    public TextMeshProUGUI p2IngredientPointsText;

    public TextMeshProUGUI p2StealPointsText;

    public TextMeshProUGUI p2SimilarityPointsText;

    public TextMeshProUGUI p2ScoreText;

    public Image p2FillImage;


    // =====================================================
    // MANAGERS
    // =====================================================

    [Header("Managers")]

    public ScoreManager scoreManager;

    public RecipeManager recipeManager;

    public FinalResultsManager finalResultsManager;


    // Nombre de la siguiente escena
    public string nextSceneName;


    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        // Ocultar panel al iniciar
        resultsPanel.SetActive(false);

        // Reiniciar barras
        p1FillImage.fillAmount = 0;
        p2FillImage.fillAmount = 0;
    }


    // =====================================================
    // MOSTRAR RESULTADOS
    // =====================================================

    public void ShowResults()
    {
        // Activar panel
        resultsPanel.SetActive(true);


        // =================================================
        // PLAYER 1
        // =================================================

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


        // Puntos por ingredientes

        p1IngredientPointsText.text =
            "Ingredientes: +"
            + scoreManager.player1IngredientPoints;


        // Puntos por robos

        p1StealPointsText.text =
            "Robados: +"
            + scoreManager.player1StealPoints;


        // Puntos por similitud

        p1SimilarityPointsText.text =
            "Similitud: +"
            + scoreManager.player1SimilarityPoints;


        // =================================================
        // PLAYER 2
        // =================================================

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


        // Puntos por ingredientes

        p2IngredientPointsText.text =
            "Ingredientes: +"
            + scoreManager.player2IngredientPoints;


        // Puntos por robos

        p2StealPointsText.text =
            "Robados: +"
            + scoreManager.player2StealPoints;


        // Puntos por similitud

        p2SimilarityPointsText.text =
            "Similitud: +"
            + scoreManager.player2SimilarityPoints;
    }


    // =====================================================
    // SIGUIENTE RONDA
    // =====================================================

    public void NextRound()
    {
        // Si NO hay siguiente escena
        // entonces mostrar panel final

        if (string.IsNullOrEmpty(nextSceneName))
        {
            // Ocultar panel resultados
            resultsPanel.SetActive(false);

            // Mostrar panel final
            finalResultsManager.ShowFinalResults();

            return;
        }


        // Cargar siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }


    // =====================================================
    // ANIMAR PORCENTAJE
    // =====================================================

    IEnumerator AnimatePercentage(
        TextMeshProUGUI text,
        float targetValue
    )
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
                current.ToString("F0")
                + "%";

            yield return null;
        }
    }


    // =====================================================
    // ANIMAR PUNTAJE
    // =====================================================

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

            text.text = current.ToString();

            yield return null;
        }
    }


    // =====================================================
    // ANIMAR BARRA
    // =====================================================

    IEnumerator AnimateFill(
        Image image,
        float targetFill
    )
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

