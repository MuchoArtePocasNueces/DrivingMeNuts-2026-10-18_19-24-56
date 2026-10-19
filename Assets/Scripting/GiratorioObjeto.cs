using UnityEngine;

public class GiratorioObjeto : MonoBehaviour
{
    [Header("Rotación")]
    public Vector3 velocidadRotacion = new Vector3(0f, 90f, 0f); // Grados por segundo en cada eje

    [Header("Flotar (opcional)")]
    public bool flotar = true;
    public float alturaFlotar = 0.25f;   // Cuánto sube y baja
    public float velocidadFlotar = 2f;   // Qué tan rápido flota

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Gira constantemente
        transform.Rotate(velocidadRotacion * Time.deltaTime, Space.World);

        // Flota suavemente arriba y abajo (efecto de objeto "mágico")
        if (flotar)
        {
            float desplazamiento = Mathf.Sin(Time.time * velocidadFlotar) * alturaFlotar;
            transform.position = posicionInicial + new Vector3(0f, desplazamiento, 0f);
        }
    }
}
