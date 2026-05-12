
using UnityEngine;

public class OldInput : MonoBehaviour
{
    // Variables para jugador 1
    public float horizontalP1;
    public float verticalP1;

    // Variables para jugador 2
    public float horizontalP2;
    public float verticalP2;

    //Variable para saltar
    public bool jumpP1;
    public bool jumpP2;

    // Se llama cada frame
    void Update()
    {
       // Se llaman los métodos para que funcionen

        GetInputFloat();
        GetInputButton();

    }

    // Método para visibilizar el vector 2
    public void GetInputFloat()
    {
        // Jugador 1
        horizontalP1 = Input.GetAxisRaw("Horizontal_P1");
        verticalP1 = Input.GetAxisRaw("Vertical_P1");

        // Jugador 2
        horizontalP2 = Input.GetAxisRaw("Horizontal_P2");
        verticalP2 = Input.GetAxisRaw("Vertical_P2");

        


    }

    // Método para visibilizar la presión de botón
    public void GetInputButton()
    {
        jumpP1 = Input.GetKeyDown(KeyCode.Space);

        jumpP2 = Input.GetKeyDown(KeyCode.Return);
    }

}
