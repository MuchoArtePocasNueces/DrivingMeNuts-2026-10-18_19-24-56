using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    [Header("Canvas de nivel completado")]
    public GameObject panelCompletado;   // El panel/canvas que aparece al ganar

    private bool yaLlego = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo tocó la meta: " + other.name);  // Para depurar

        if (yaLlego) return;

        ControladorPersonaje ardilla = other.GetComponentInParent<ControladorPersonaje>();
        if (ardilla == null)
        {
            Debug.Log("Pero no era la ardilla");
            return;
        }

        yaLlego = true;
        Debug.Log("¡NIVEL COMPLETADO!");

        // Muestra el canvas de completado
        if (panelCompletado != null)
            panelCompletado.SetActive(true);

        // Opcional: congela el juego
        Time.timeScale = 0f;
    }
}
