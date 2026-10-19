using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    public GestorNivel gestor;   // El gestor del nivel

    private bool yaActivada = false;

    void OnTriggerEnter(Collider other)
    {
        if (yaActivada) return;

        ControladorPersonaje ardilla = other.GetComponentInParent<ControladorPersonaje>();
        if (ardilla == null) return;

        yaActivada = true;

        if (gestor != null)
            gestor.Completar();   // Avisa que llegó a tiempo
        else
            Debug.LogWarning("La Meta no tiene asignado el GestorNivel.");
    }
}

