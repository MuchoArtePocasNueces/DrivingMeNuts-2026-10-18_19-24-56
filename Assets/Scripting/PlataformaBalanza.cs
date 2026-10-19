using UnityEngine;

public class PlataformaBalanza : MonoBehaviour
{
    [Header("Peso necesario para bajar")]
    public float pesoNecesario = 5f;     // La ardilla debe pesar esto o más

    [Header("Movimiento de la balanza")]
    public float distanciaBajada = 3f;   // Cuánto baja cuando hay peso suficiente
    public float velocidadBajada = 2f;   // Qué tan rápido baja/sube

    private Vector3 posicionArriba;      // Posición original (sin peso)
    private Vector3 posicionAbajo;       // Posición hundida (con peso)
    private ControladorPersonaje ardillaEncima;

    void Start()
    {
        posicionArriba = transform.position;
        posicionAbajo = posicionArriba - new Vector3(0f, distanciaBajada, 0f);
    }

    void FixedUpdate()
    {
        // Decide el destino: ¿hay peso suficiente encima?
        bool hayPesoSuficiente = false;

        if (ardillaEncima != null)
        {
            float peso = ardillaEncima.PesoTotal();
            hayPesoSuficiente = peso >= pesoNecesario;
        }

        Vector3 destino = hayPesoSuficiente ? posicionAbajo : posicionArriba;

        // Mueve la plataforma suavemente hacia el destino
        transform.position = Vector3.MoveTowards(
            transform.position,
            destino,
            velocidadBajada * Time.fixedDeltaTime
        );
    }

    // La ardilla se sube
    void OnCollisionEnter(Collision collision)
    {
        ControladorPersonaje ardilla = collision.collider.GetComponentInParent<ControladorPersonaje>();
        if (ardilla != null)
        {
            ardillaEncima = ardilla;
            ardilla.transform.SetParent(transform); // Viaja con la plataforma

            float peso = ardilla.PesoTotal();
            if (peso >= pesoNecesario)
                Debug.Log("Peso suficiente (" + peso + "). La balanza baja.");
            else
                Debug.Log("Peso insuficiente (" + peso + " de " + pesoNecesario + "). Necesitas cargar más.");
        }
    }

    // La ardilla se baja
    void OnCollisionExit(Collision collision)
    {
        ControladorPersonaje ardilla = collision.collider.GetComponentInParent<ControladorPersonaje>();
        if (ardilla != null)
        {
            if (ardilla.transform.parent == transform)
                ardilla.transform.SetParent(null);
            ardillaEncima = null;
        }
    }

    // Dibuja el recorrido de la balanza en el editor
    void OnDrawGizmosSelected()
    {
        Vector3 arriba = Application.isPlaying ? posicionArriba : transform.position;
        Vector3 abajo = arriba - new Vector3(0f, distanciaBajada, 0f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(arriba, new Vector3(1f, 0.1f, 1f)); // Posición arriba
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(abajo, new Vector3(1f, 0.1f, 1f));  // Posición abajo
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(arriba, abajo);
    }
}
