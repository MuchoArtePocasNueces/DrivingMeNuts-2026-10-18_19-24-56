using UnityEngine;
using UnityEngine.InputSystem;

public class NPCOferta : MonoBehaviour
{
    [Header("Oferta")]
    public GameObject objetoQueOfrece;
    public float pesoDelObjeto = 1f;
    public string textoDialogo = "¿Quieres este objeto? (Z: aceptar / X: rechazar)";

    [Header("Animación")]
    public Animator animatorNPC;   // Arrastra aquí el Animator del NPC (su Sprite)

    private bool dialogoActivo = false;
    private ControladorPersonaje jugador;

    void Update()
    {
        if (!dialogoActivo) return;

        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        // Aceptar con Z
        if (teclado.zKey.wasPressedThisFrame)
            Aceptar();
        // Rechazar con X
        else if (teclado.xKey.wasPressedThisFrame)
            Rechazar();
    }

    void OnTriggerEnter(Collider other)
    {
        ControladorPersonaje cp = other.GetComponentInParent<ControladorPersonaje>();
        if (cp != null)
        {
            jugador = cp;
            dialogoActivo = true;
            Debug.Log(textoDialogo);
        }
    }

    void OnTriggerExit(Collider other)
    {
        ControladorPersonaje cp = other.GetComponentInParent<ControladorPersonaje>();
        if (cp != null)
        {
            dialogoActivo = false;
            jugador = null;
        }
    }

    void Aceptar()
    {
        if (jugador != null && objetoQueOfrece != null)
        {
            jugador.Guardar(objetoQueOfrece, pesoDelObjeto);
            Debug.Log("Objeto aceptado y guardado.");

            // Animación del NPC al dar el objeto
            if (animatorNPC != null)
                animatorNPC.SetTrigger("dando");
        }
        dialogoActivo = false;
    }

    void Rechazar()
    {
        Debug.Log("Objeto rechazado.");
        dialogoActivo = false;
    }

    // Muestra el diálogo en pantalla cuando el jugador está cerca
    void OnGUI()
    {
        if (!dialogoActivo) return;

        GUIStyle estilo = new GUIStyle();
        estilo.fontSize = 24;
        estilo.normal.textColor = Color.white;

        GUI.Box(new Rect(Screen.width / 2 - 300, Screen.height - 120, 600, 80), "");
        GUI.Label(new Rect(Screen.width / 2 - 290, Screen.height - 110, 580, 60), textoDialogo, estilo);
    }
}
