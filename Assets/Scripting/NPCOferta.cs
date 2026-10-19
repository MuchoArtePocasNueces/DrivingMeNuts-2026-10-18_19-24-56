using UnityEngine;
using UnityEngine.InputSystem;

public class NPCOferta : MonoBehaviour
{
    [Header("Oferta")]
    public GameObject objetoQueOfrece;   // El prefab que regala (ej: ObjetoEscupible)
    public string textoDialogo = "¿Quieres este objeto? (Z: aceptar / X: rechazar)";

    private bool dialogoActivo = false;
    private ControladorPersonaje jugador;

    void Update()
    {
        if (!dialogoActivo) return;

        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        // Aceptar con Z
        if (teclado.zKey.wasPressedThisFrame)
        {
            Aceptar();
        }
        // Rechazar con X
        else if (teclado.xKey.wasPressedThisFrame)
        {
            Rechazar();
        }
    }

    // Cuando algo entra en la zona del NPC
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al trigger del NPC: " + other.name);

        ControladorPersonaje cp = other.GetComponentInParent<ControladorPersonaje>();
        if (cp != null)
        {
            jugador = cp;
            dialogoActivo = true;
            Debug.Log(textoDialogo);
        }
        else
        {
            Debug.Log("Pero no era el personaje (no tiene ControladorPersonaje)");
        }
    }

    // Cuando se aleja
    void OnTriggerExit(Collider other)
    {
        ControladorPersonaje cp = other.GetComponentInParent<ControladorPersonaje>();
        if (cp != null)
        {
            dialogoActivo = false;
            jugador = null;
        }
    }

public float pesoDelObjeto = 1f; // Peso del objeto que ofrece el NPC

void Aceptar()
{
    if (jugador != null && objetoQueOfrece != null)
    {
        jugador.Guardar(objetoQueOfrece, pesoDelObjeto);
        Debug.Log("Objeto aceptado y guardado.");
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
