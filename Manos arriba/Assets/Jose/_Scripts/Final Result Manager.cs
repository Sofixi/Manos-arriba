using TMPro;
using UnityEngine;

public class FinalResultsManager : MonoBehaviour
{
    [Header("Panel")]

    public GameObject finalPanel;

    [Header("Player 1")]

    public TextMeshProUGUI p1FinalScoreText;

    public GameObject p1WinnerImage;

    public GameObject p1LoserImage;

    [Header("Player 2")]

    public TextMeshProUGUI p2FinalScoreText;

    public GameObject p2WinnerImage;

    public GameObject p2LoserImage;

    [Header("Managers")]

    public ScoreManager scoreManager;

    

    void Start()
    {
        finalPanel.SetActive(false);
    }

    public void ShowFinalResults()
    {
        // Mostrar panel
        finalPanel.SetActive(true);

        // Mostrar scores
        p1FinalScoreText.text =
        "Score: "
        + scoreManager.player1TotalScore;

        p2FinalScoreText.text =
        "Score: "
        + scoreManager.player2TotalScore;

        // Apagar todo primero
        p1WinnerImage.SetActive(false);
        p1LoserImage.SetActive(false);

        p2WinnerImage.SetActive(false);
        p2LoserImage.SetActive(false);

        // Comparar scores
        if (scoreManager.player1TotalScore >
            scoreManager.player2TotalScore)
        {
            p1WinnerImage.SetActive(true);

            p2LoserImage.SetActive(true);
        }
        else if (scoreManager.player2TotalScore >
                 scoreManager.player1TotalScore)
        {
            p2WinnerImage.SetActive(true);

            p1LoserImage.SetActive(true);
        }
        else
        {
            // Empate
            p1WinnerImage.SetActive(true);

            p2WinnerImage.SetActive(true);
        }
    }
}