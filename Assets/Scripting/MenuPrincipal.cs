using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Carga el nivel 1 (por nombre)
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    // Cierra el juego (funciona en el build, no en el editor)
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
