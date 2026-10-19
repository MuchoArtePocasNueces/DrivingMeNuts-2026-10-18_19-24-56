using UnityEngine;
using System.Collections;

public class PlataformaRompible : MonoBehaviour
{
    [Header("Peso que la rompe")]
    public float pesoMinimoParaRomper = 5f;

    [Header("Tiempos")]
    public float tiempoAntesDeRomper = 1f;
    public float tiempoParaReaparecer = 3f;

    [Header("Aviso de temblor")]
    public bool temblarAntesDeRomper = true;
    public float intensidadTemblor = 0.05f;

    private bool rompiendose = false;
    private Vector3 posicionOriginal;
    private Renderer render;
    private Collider col;

    void Start()
    {
        posicionOriginal = transform.position;
        render = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rompiendose) return;

        ControladorPersonaje ardilla = collision.collider.GetComponentInParent<ControladorPersonaje>();
        if (ardilla == null) return;

        float peso = ardilla.PesoTotal();
        Debug.Log("Ardilla pisó la plataforma. Peso total: " + peso);

        if (peso >= pesoMinimoParaRomper)
        {
            Debug.Log("¡Demasiado peso! La plataforma temblará y se romperá.");
            StartCoroutine(RomperYReaparecer());
        }
    }

    IEnumerator RomperYReaparecer()
    {
        rompiendose = true;

        // 1. Tiembla como aviso
        float tiempo = 0f;
        while (tiempo < tiempoAntesDeRomper)
        {
            if (temblarAntesDeRomper)
            {
                Vector3 temblor = new Vector3(
                    Random.Range(-intensidadTemblor, intensidadTemblor),
                    Random.Range(-intensidadTemblor, intensidadTemblor),
                    0f
                );
                transform.position = posicionOriginal + temblor;
            }
            tiempo += Time.deltaTime;
            yield return null;
        }

        // 2. Se "rompe": vuelve a su sitio y se apaga (no se destruye)
        transform.position = posicionOriginal;
        OcultarPlataforma(true);
        Debug.Log("¡Plataforma rota! Reaparecerá en " + tiempoParaReaparecer + "s");

        // 3. Espera el tiempo de reaparición
        yield return new WaitForSeconds(tiempoParaReaparecer);

        // 4. Reaparece: se vuelve a encender
        OcultarPlataforma(false);
        rompiendose = false;
        Debug.Log("Plataforma restablecida.");
    }

    void OcultarPlataforma(bool oculta)
    {
        if (render != null) render.enabled = !oculta;
        if (col != null) col.enabled = !oculta;
    }
}
