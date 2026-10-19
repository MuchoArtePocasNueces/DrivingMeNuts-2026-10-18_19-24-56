using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GestorNivel : MonoBehaviour
{
    [Header("Tiempo")]
    public float tiempoLimite = 30f;   // Segundos para llegar a la meta

    [Header("UI")]
    public TextMeshProUGUI textoTiempo;      // Muestra el cronómetro
    public TextMeshProUGUI textoResultado;   // Muestra COMPLETADO / FAIL
    public GameObject panelResultado;        // Panel que aparece al terminar

    private float tiempoRestante;
    private bool nivelTerminado = false;

    void Start()
    {
        tiempoRestante = tiempoLimite;

        if (panelResultado != null)
            panelResultado.SetActive(false); // Oculto al empezar
    }

    void Update()
    {
        if (nivelTerminado)
        {
            // Al terminar, permitir reiniciar con R
            Keyboard teclado = Keyboard.current;
            if (teclado != null && teclado.rKey.wasPressedThisFrame)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // Cuenta regresiva
        tiempoRestante -= Time.deltaTime;

        if (textoTiempo != null)
            textoTiempo.text = "Tiempo: " + Mathf.Max(0, Mathf.CeilToInt(tiempoRestante));

        // ¿Se acabó el tiempo?
        if (tiempoRestante <= 0f)
            Fallar();
    }

    // La llama la Meta cuando el jugador llega a tiempo
    public void Completar()
    {
        if (nivelTerminado) return;
        nivelTerminado = true;
        MostrarResultado("¡COMPLETADO!", Color.green);
    }

    void Fallar()
    {
        if (nivelTerminado) return;
        nivelTerminado = true;
        MostrarResultado("FAIL", Color.red);
    }

    void MostrarResultado(string mensaje, Color color)
    {
        Debug.Log(mensaje);

        if (panelResultado != null)
            panelResultado.SetActive(true);

        if (textoResultado != null)
        {
            textoResultado.text = mensaje + "\n(R para reintentar)";
            textoResultado.color = color;
        }
    }
}
