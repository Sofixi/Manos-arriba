using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelFinalFinal : MonoBehaviour
{
    [Header("Paneles finales")]
    public GameObject panelVictoriaP1;
    public GameObject panelVictoriaP2;
    public GameObject panelEmpate;

    [Header("Puntajes del jugador 1")]
    public TextMeshProUGUI p1ScoreText;

    [Header("Puntajes del jugador 2")]
    public TextMeshProUGUI p2ScoreText;

    [Header("Botón para salir")]
    public string menuSceneName = "MenuPrincipal";

    void Start()
    {
        MostrarResultadoFinal();
    }

    public void MostrarResultadoFinal()
    {
        int puntosP1 = PlayerPrefs.GetInt("PuntajeFinalP1", 0);
        int puntosP2 = PlayerPrefs.GetInt("PuntajeFinalP2", 0);

        // Ocultar los tres paneles
        panelVictoriaP1.SetActive(false);
        panelVictoriaP2.SetActive(false);
        panelEmpate.SetActive(false);

        // Mostrar los puntajes acumulados en el panel que corresponda
        if (p1ScoreText != null)
            p1ScoreText.text = puntosP1.ToString();

        if (p2ScoreText != null)
            p2ScoreText.text = puntosP2.ToString();

        // Elegir al ganador
        if (puntosP1 > puntosP2)
        {
            panelVictoriaP1.SetActive(true);
        }
        else if (puntosP2 > puntosP1)
        {
            panelVictoriaP2.SetActive(true);
        }
        else
        {
            panelEmpate.SetActive(true);
        }
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}