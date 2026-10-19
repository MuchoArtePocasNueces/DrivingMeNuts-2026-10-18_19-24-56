using UnityEngine;

public class CamaraSeguidora : MonoBehaviour
{
    public Transform objetivo;          // El personaje a seguir
    public float desfaseX = 2f;         // Qué tan adelante del personaje mira la cámara
    public float suavizado = 5f;        // Qué tan suave sigue
    public float alturaFija = 1f;       // Altura constante de la cámara (eje Y)
    public float profundidad = -10f;    // Distancia en Z (para 3D)

    private float xMaxima;              // La X más a la derecha alcanzada (no retrocede)

    void Start()
    {
        // Empieza en la posición actual de la cámara
        xMaxima = transform.position.x;
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Hacia dónde debería ir en X (el personaje + un poco adelante)
        float xDeseada = objetivo.position.x + desfaseX;

        // Solo avanza a la derecha: nunca baja de la X máxima alcanzada
        if (xDeseada > xMaxima)
            xMaxima = xDeseada;

        // Posición final: X que solo avanza, Y fija, Z fija
        Vector3 posicionDeseada = new Vector3(xMaxima, alturaFija, profundidad);

        // Movimiento suave solo en X
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
    }
}
