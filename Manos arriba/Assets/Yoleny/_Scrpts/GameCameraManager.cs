using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameCameraManager : MonoBehaviour
{
    public Camera cinematicCamera;
    public Camera playerCamera1;
    public Camera playerCamera2;

    public PlayableDirector director;

    void Start()
    {
        // Cinemática activa
        cinematicCamera.gameObject.SetActive(true);

        // Cámaras de jugadores apagadas
        playerCamera1.gameObject.SetActive(false);
        playerCamera2.gameObject.SetActive(false);

        // Cuando termine el Timeline
        director.stopped += TerminarCinematica;
    }

    void TerminarCinematica(PlayableDirector obj)
    {
        // Apagar cámara cinematográfica
        cinematicCamera.gameObject.SetActive(false);

        // Encender cámaras de jugadores
        playerCamera1.gameObject.SetActive(true);
        playerCamera2.gameObject.SetActive(true);
    }
}