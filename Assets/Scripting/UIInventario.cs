using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIInventario : MonoBehaviour
{
    public ControladorPersonaje jugador;    // El personaje con el inventario
    public TextMeshProUGUI textoInventario; // El texto en pantalla
    public GameObject panelInventario;      // El panel que se muestra/oculta

    private bool visible = false;

    void Start()
    {
        // Empieza oculto
        if (panelInventario != null)
            panelInventario.SetActive(false);
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        // Abrir / cerrar con la tecla I
        if (teclado.iKey.wasPressedThisFrame)
        {
            visible = !visible;
            if (panelInventario != null)
                panelInventario.SetActive(visible);
        }

        // Si está visible, actualiza el contenido
        if (visible)
            ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (jugador == null || textoInventario == null) return;

        string texto = "INVENTARIO (" + jugador.objetosGuardados.Count + ")\n";
        float pesoTotal = 0f;

        foreach (ObjetoGuardado o in jugador.objetosGuardados)
        {
            string nombre = o.prefab != null ? o.prefab.name : "¿?";
            texto += "- " + nombre + " (peso " + o.peso + ")\n";
            pesoTotal += o.peso;
        }

        texto += "Peso total: " + pesoTotal;
        textoInventario.text = texto;
    }
}
