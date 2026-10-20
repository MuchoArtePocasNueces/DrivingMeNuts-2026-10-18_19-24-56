using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPestanas : MonoBehaviour
{
    [Header("Panel principal del menú")]
    public GameObject panelMenu;          // Contenedor de todo el menú

    [Header("Las 3 pestañas")]
    public GameObject panelMapa;
    public GameObject panelInventario;
    public GameObject panelMenu3;         // La pestaña "Menú" con Salir

    [Header("Inventario")]
    public ControladorPersonaje jugador;
    public TextMeshProUGUI textoInventario;

    private bool abierto = false;

    void Start()
    {
        if (panelMenu != null) panelMenu.SetActive(false);
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado != null && (teclado.tabKey.wasPressedThisFrame || teclado.escapeKey.wasPressedThisFrame))
        {
            if (abierto) CerrarMenu();
            else AbrirMenu();
        }

        if (abierto && panelInventario != null && panelInventario.activeSelf)
            ActualizarInventario();
    }

    public void AbrirMenu()
    {
        abierto = true;
        if (panelMenu != null) panelMenu.SetActive(true);
        MostrarInventario();
        Time.timeScale = 0f;
    }

    public void CerrarMenu()
    {
        abierto = false;
        if (panelMenu != null) panelMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MostrarMapa()
    {
        OcultarPestanas();
        if (panelMapa != null) panelMapa.SetActive(true);
    }

    public void MostrarInventario()
    {
        OcultarPestanas();
        if (panelInventario != null) panelInventario.SetActive(true);
        ActualizarInventario();
    }

    public void MostrarMenu()
    {
        OcultarPestanas();
        if (panelMenu3 != null) panelMenu3.SetActive(true);
    }

    void OcultarPestanas()
    {
        if (panelMapa != null) panelMapa.SetActive(false);
        if (panelInventario != null) panelInventario.SetActive(false);
        if (panelMenu3 != null) panelMenu3.SetActive(false);
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo...");
        Time.timeScale = 1f;
        Application.Quit();
    }

    void ActualizarInventario()
    {
        if (jugador == null || textoInventario == null) return;

        string texto = "INVENTARIO (" + jugador.objetosGuardados.Count + ")\n\n";
        float pesoTotal = 0f;

        if (jugador.objetosGuardados.Count == 0)
            texto += "(vacío)\n";
        else
        {
            foreach (ObjetoGuardado o in jugador.objetosGuardados)
            {
                string nombre = o.prefab != null ? o.prefab.name : "¿?";
                texto += "- " + nombre + " (peso " + o.peso + ")\n";
                pesoTotal += o.peso;
            }
        }

        texto += "\nPeso total: " + pesoTotal;
        textoInventario.text = texto;
    }
}
