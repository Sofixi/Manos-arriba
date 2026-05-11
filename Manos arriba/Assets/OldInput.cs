
using UnityEngine;

public class OldInput : MonoBehaviour
{
    // Se declaran las variables
    public float horizontal;
    public float vertical;

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
        // Se almacena el eje raw (-1, 0- 1) del eje horizontal (ver en Unity "Axis"), en la variable.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Se escribe en consola el resultado

    }

    // Método para visibilizar la presión de botón
    public void GetInputButton()
    {
        // Si el sistema detecta que se presiona la letra "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Se escribe en consola el textor "Shoot"
            Debug.Log("Shoot");
        }
    }

}
