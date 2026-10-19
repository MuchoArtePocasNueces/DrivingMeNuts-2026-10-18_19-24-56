using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class ControladorPersonaje : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadBase = 7f;
    public float fuerzaSaltoBase = 7f;
    public float factorRalentizacion = 0.3f;
    public float velocidadMinima = 1.5f;

    [Header("Salto con peso")]
    public float factorSaltoPorPeso = 0.2f;
    public float fuerzaSaltoMinima = 3f;

    [Header("Peso de la ardilla")]
    public float pesoBaseArdilla = 2f;

    [Header("Límite de cámara")]
    public bool usarLimiteIzquierdo = true;
    public float margenIzquierdo = 0.5f;   // Qué tan adentro del borde se detiene

    [Header("Detección de piso")]
    public Transform chequeoPiso;
    public float radioChequeoPiso = 0.1f;
    public LayerMask capaPiso;

    [Header("Escupir")]
    public Transform puntoEscupir;
    public float fuerzaEscupir = 10f;
    public float fuerzaEscupirArriba = 2f;

    [Header("Recoger")]
    public float radioRecoger = 1.5f;

    [Header("Animación")]
    public Animator animator;

    public List<ObjetoGuardado> objetosGuardados = new List<ObjetoGuardado>();

    private Rigidbody rb;
    private float entradaHorizontal;
    private bool enPiso;
    private bool mirandoDerecha = true;
    private bool quiereSaltar;
    private float velocidadActual;
    private float fuerzaSaltoActual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        RecalcularVelocidad();
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        LeerMovimiento(teclado);
        RevisarPiso();

        // Saltar: Barra Espaciadora
        if (teclado.spaceKey.wasPressedThisFrame && enPiso)
            quiereSaltar = true;

        // Lanzar / Escupir: Shift
        if (teclado.leftShiftKey.wasPressedThisFrame || teclado.rightShiftKey.wasPressedThisFrame)
            Escupir();

        // Recoger: E
        if (teclado.eKey.wasPressedThisFrame)
            Recoger();

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("velocidad", Mathf.Abs(entradaHorizontal));
            animator.SetBool("enElAire", !enPiso);
        }
    }

    void FixedUpdate()
    {
        Vector3 v = rb.linearVelocity;
        v.x = entradaHorizontal * velocidadActual;

        if (quiereSaltar)
        {
            v.y = fuerzaSaltoActual;
            quiereSaltar = false;
        }

        rb.linearVelocity = v;

        // Límite izquierdo: no dejar que el personaje salga por el borde izquierdo de la cámara
        if (usarLimiteIzquierdo && Camera.main != null)
        {
            Camera cam = Camera.main;
            float bordeIzquierdo;

            if (cam.orthographic)
            {
                float mitadAncho = cam.orthographicSize * cam.aspect;
                bordeIzquierdo = cam.transform.position.x - mitadAncho + margenIzquierdo;
            }
            else
            {
                float distancia = Mathf.Abs(cam.transform.position.z - transform.position.z);
                Vector3 bordeMundo = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, distancia));
                bordeIzquierdo = bordeMundo.x + margenIzquierdo;
            }

            if (transform.position.x < bordeIzquierdo)
            {
                Vector3 pos = transform.position;
                pos.x = bordeIzquierdo;
                transform.position = pos;

                if (rb.linearVelocity.x < 0)
                {
                    Vector3 vel = rb.linearVelocity;
                    vel.x = 0f;
                    rb.linearVelocity = vel;
                }
            }
        }
    }

    void LeerMovimiento(Keyboard teclado)
    {
        entradaHorizontal = 0f;
        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) entradaHorizontal -= 1f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) entradaHorizontal += 1f;

        if (entradaHorizontal > 0 && !mirandoDerecha) Voltear();
        else if (entradaHorizontal < 0 && mirandoDerecha) Voltear();

    }

    void RevisarPiso()
    {
        if (chequeoPiso == null) { enPiso = false; return; }
        enPiso = Physics.CheckSphere(chequeoPiso.position, radioChequeoPiso, capaPiso);
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;

        // Gira el personaje completo 180° en Y (sin usar flipX)
        if (mirandoDerecha)
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        else
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }



    void RecalcularVelocidad()
    {
        float pesoCargado = 0f;
        foreach (ObjetoGuardado o in objetosGuardados)
            pesoCargado += o.peso;

        velocidadActual = velocidadBase - (pesoCargado * factorRalentizacion);
        if (velocidadActual < velocidadMinima)
            velocidadActual = velocidadMinima;

        fuerzaSaltoActual = fuerzaSaltoBase - (pesoCargado * factorSaltoPorPeso);
        if (fuerzaSaltoActual < fuerzaSaltoMinima)
            fuerzaSaltoActual = fuerzaSaltoMinima;

        Debug.Log("Peso cargado: " + pesoCargado + " | Velocidad: " + velocidadActual + " | Salto: " + fuerzaSaltoActual);
    }

    public float PesoTotal()
    {
        float total = pesoBaseArdilla;
        foreach (ObjetoGuardado o in objetosGuardados)
            total += o.peso;
        return total;
    }

    void Escupir()
    {
        if (animator != null)
            animator.SetTrigger("escupiendo");

        if (objetosGuardados.Count == 0)
        {
            Debug.Log("Nada que escupir (vacío)");
            return;
        }

        int ultimo = objetosGuardados.Count - 1;
        ObjetoGuardado datos = objetosGuardados[ultimo];
        objetosGuardados.RemoveAt(ultimo);

        RecalcularVelocidad();

        if (datos.prefab == null)
        {
            Debug.LogWarning("Objeto guardado sin prefab asignado.");
            return;
        }

        float direccion = mirandoDerecha ? 1f : -1f;
        Vector3 posicion = puntoEscupir != null
            ? puntoEscupir.position
            : transform.position + new Vector3(direccion * 1.5f, 0.5f, 0f);

        GameObject escupido = Instantiate(datos.prefab, posicion, Quaternion.identity);

        Collider colEscupido = escupido.GetComponent<Collider>();
        Collider colPersonaje = GetComponent<Collider>();
        if (colEscupido != null && colPersonaje != null)
            Physics.IgnoreCollision(colEscupido, colPersonaje);

        Rigidbody rbEscupido = escupido.GetComponent<Rigidbody>();
        if (rbEscupido != null)
        {
            Vector3 impulso = new Vector3(direccion * fuerzaEscupir, fuerzaEscupirArriba, 0f);
            rbEscupido.AddForce(impulso, ForceMode.Impulse);
        }

        Debug.Log("¡Escupió! Quedan: " + objetosGuardados.Count);
    }

    void Recoger()
    {
        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioRecoger);

        ObjetoRecogible masCercano = null;
        float distanciaMin = Mathf.Infinity;

        foreach (Collider c in cercanos)
        {
            ObjetoRecogible recogible = c.GetComponent<ObjetoRecogible>();
            if (recogible != null)
            {
                float dist = Vector3.Distance(transform.position, c.transform.position);
                if (dist < distanciaMin)
                {
                    distanciaMin = dist;
                    masCercano = recogible;
                }
            }
        }

        if (masCercano == null)
        {
            Debug.Log("No hay nada que recoger cerca.");
            return;
        }

        GameObject paraGuardar = masCercano.prefabParaGuardar != null
            ? masCercano.prefabParaGuardar
            : masCercano.gameObject;

        Guardar(paraGuardar, masCercano.peso);

        Destroy(masCercano.gameObject);
        Debug.Log("Recogido: " + masCercano.name + " (peso " + masCercano.peso + ")");
    }

    public void Guardar(GameObject prefab, float peso)
    {
        objetosGuardados.Add(new ObjetoGuardado(prefab, peso));
        RecalcularVelocidad();
        Debug.Log("Guardado. Total objetos: " + objetosGuardados.Count);
    }

    void OnDrawGizmosSelected()
    {
        if (chequeoPiso != null)
        {
            Gizmos.color = enPiso ? Color.green : Color.red;
            Gizmos.DrawWireSphere(chequeoPiso.position, radioChequeoPiso);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radioRecoger);
    }
}

