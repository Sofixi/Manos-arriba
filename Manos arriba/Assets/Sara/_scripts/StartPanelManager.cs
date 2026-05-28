using UnityEngine;

public class StartGamePanel : MonoBehaviour
{
    [Header("Panel de inicio")]
    public GameObject startPanel;

    [Header("Objetos a activar al comenzar")]
    public GameObject[] gameplayObjects;

    void Start()
    {
        // Mostrar panel al iniciar
        startPanel.SetActive(true);

        // Desactivar gameplay
        foreach (GameObject obj in gameplayObjects)
        {
            obj.SetActive(false);
        }

        // Pausar juego
        Time.timeScale = 0f;
    }

    // Botón COMENZAR
    public void StartGame()
    {
        // Ocultar panel
        startPanel.SetActive(false);

        // Activar gameplay
        foreach (GameObject obj in gameplayObjects)
        {
            obj.SetActive(true);
        }

        // Reanudar juego
        Time.timeScale = 1f;
    }
}
