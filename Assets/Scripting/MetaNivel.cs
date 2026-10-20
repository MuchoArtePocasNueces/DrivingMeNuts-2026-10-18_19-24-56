using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    [Header("Canvas de nivel completado")]
    public GameObject panelCompletado;   // El panel/canvas que aparece al ganar
    
    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonidoGanar;


    private bool yaLlego = false;

void OnTriggerEnter(Collider other)
{
    if (yaLlego) return;

    ControladorPersonaje ardilla = other.GetComponentInParent<ControladorPersonaje>();
    if (ardilla == null) return;

    yaLlego = true;
    Debug.Log("¡NIVEL COMPLETADO!");

    if (panelCompletado != null)
        panelCompletado.SetActive(true);

    // Sonido de ganar
    if (audioSource != null && sonidoGanar != null)
        audioSource.PlayOneShot(sonidoGanar);

    Time.timeScale = 0f;
}

}
