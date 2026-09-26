using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameCameraManager : MonoBehaviour
{
    public Camera cinematicCamera;
    public Camera playerCamera1;
    public Camera playerCamera2;

    public GameObject gameUI;

    public PlayableDirector director;

    void Start()
    {
        [Header("CÁMARAS")]
        public Camera cinematicCamera;
    public Camera playerCamera1;
    public Camera playerCamera2;

    [Header("HUD")]
    public GameObject gameHUD;

    [Header("TIMELINE")]
    public PlayableDirector director;

    [Header("GAME MANAGER")]
    public GameManager gameManager;

    private void Start()
    {
        // =========================================
        // CINEMÁTICA
        // =========================================

        cinematicCamera.gameObject.SetActive(true);

        // Apagamos las cámaras del gameplay
        playerCamera1.gameObject.SetActive(false);
        playerCamera2.gameObject.SetActive(false);

        // Apagamos SOLO el HUD
        if (gameHUD != null)
            gameHUD.SetActive(false);

        // Esperamos a que termine el Timeline
        if (director != null)
            director.stopped += TerminarCinematica;
    }

    private void TerminarCinematica(PlayableDirector obj)
    {
        // =========================================
        // TERMINA CINEMÁTICA
        // =========================================

        // Apagar cámara cinematográfica
        if (cinematicCamera != null)
            cinematicCamera.gameObject.SetActive(false);

        // Activar cámaras de los jugadores
        if (playerCamera1 != null)
            playerCamera1.gameObject.SetActive(true);

        if (playerCamera2 != null)
            playerCamera2.gameObject.SetActive(true);

        // Mostrar HUD
        if (gameHUD != null)
            gameHUD.SetActive(true);

        // =========================================
        // INICIAR JUEGO
        // =========================================

        if (gameManager != null)
            gameManager.IniciarJuego();
    }

    private void OnDestroy()
    {
        if (director != null)
            director.stopped -= TerminarCinematica;
    }
}