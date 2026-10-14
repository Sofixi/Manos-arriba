
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelFinalFinal : MonoBehaviour
{
    [Header("Paneles finales")]
    public GameObject panelVictoriaP1;
    public GameObject panelVictoriaP2;
    public GameObject panelEmpate;

    [Header("Puntajes")]
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    [Header("Volver al menú")]
    public string menuSceneName = "Nivel 1";
    public GameObject panelMenuPrincipal;

    void Start()
    {
        MostrarResultadoFinal();
    }

    public void MostrarResultadoFinal()
    {
        int puntosP1 = PlayerPrefs.GetInt("PuntajeFinalP1", 0);
        int puntosP2 = PlayerPrefs.GetInt("PuntajeFinalP2", 0);

        if (panelVictoriaP1 != null)
            panelVictoriaP1.SetActive(false);

        if (panelVictoriaP2 != null)
            panelVictoriaP2.SetActive(false);

        if (panelEmpate != null)
            panelEmpate.SetActive(false);

        if (p1ScoreText != null)
            p1ScoreText.text = puntosP1.ToString();

        if (p2ScoreText != null)
            p2ScoreText.text = puntosP2.ToString();

        if (puntosP1 > puntosP2)
            panelVictoriaP1.SetActive(true);
        else if (puntosP2 > puntosP1)
            panelVictoriaP2.SetActive(true);
        else
            panelEmpate.SetActive(true);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;

        // Volver a la escena donde está el menú principal.
        SceneManager.LoadScene(menuSceneName);

        // Al cargar Nivel 1, UIManager debe mostrar el menú.
        PlayerPrefs.SetInt("VolverAlMenu", 1);
        PlayerPrefs.Save();
    }
}
