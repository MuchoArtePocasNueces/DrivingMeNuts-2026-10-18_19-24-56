using UnityEngine;

public class CamaraSeguidora : MonoBehaviour
{
    public Transform objetivo;          // El personaje a seguir
    public float desfaseX = 2f;         // Qué tan adelante mira la cámara
    public float suavizado = 5f;        // Qué tan suave sigue
    public float profundidad = -10f;    // Distancia en Z

    [Header("Seguir horizontal (X)")]
    public bool soloAvanzaDerecha = true;  // Si true, la cámara no retrocede

    [Header("Seguir vertical (Y)")]
    public bool seguirY = true;
    public float desfaseY = 1f;         // Altura extra sobre el personaje
    public float alturaMinima = 1f;     // La cámara no baja de aquí (suelo base)

    private float xMaxima;

    void Start()
    {
        xMaxima = transform.position.x;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // --- Horizontal (X) ---
        float xDeseada = objetivo.position.x + desfaseX;

        if (soloAvanzaDerecha)
        {
            if (xDeseada > xMaxima)
                xMaxima = xDeseada;
            xDeseada = xMaxima;  // No retrocede
        }

        // --- Vertical (Y) ---
        float yDeseada = transform.position.y; // Por defecto, se queda igual
        if (seguirY)
        {
            yDeseada = objetivo.position.y + desfaseY;
            if (yDeseada < alturaMinima)
                yDeseada = alturaMinima;  // No baja del suelo base
        }

        // Posición final
        Vector3 posicionDeseada = new Vector3(xDeseada, yDeseada, profundidad);

        // Movimiento suave
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
