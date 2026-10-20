using UnityEngine;

public class MusicaPeso : MonoBehaviour
{
    [Header("Música")]
    public AudioSource musica;              // Audio Source con la música en loop

    [Header("Jugador")]
    public ControladorPersonaje jugador;    // La ardilla (para leer el peso)

    [Header("Ajuste de velocidad por peso")]
    public float pitchNormal = 1f;          // Velocidad normal (sin peso)
    public float pitchMinimo = 0.6f;        // Lo más lento (mucho peso)
    public float pesoMaximo = 20f;          // Peso que se considera "máximo"
    public float suavizado = 2f;            // Qué tan suave cambia la velocidad

    void Start()
    {
        // Asegura que la música esté sonando en loop
        if (musica != null)
        {
            musica.loop = true;
            if (!musica.isPlaying)
                musica.Play();
        }
    }

    void Update()
    {
        if (musica == null || jugador == null) return;

        // Lee el peso total de la ardilla
        float peso = jugador.PesoTotal();

        // Calcula el pitch: a más peso, más lento (pitch más bajo)
        float t = Mathf.Clamp01(peso / pesoMaximo);
        float pitchObjetivo = Mathf.Lerp(pitchNormal, pitchMinimo, t);

        // Cambia la velocidad suavemente (sin saltos bruscos)
        musica.pitch = Mathf.Lerp(musica.pitch, pitchObjetivo, suavizado * Time.unscaledDeltaTime);
    }
}
