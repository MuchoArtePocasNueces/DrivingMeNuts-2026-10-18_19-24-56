using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camara;

    void Start()
    {
        if (Camera.main != null)
            camara = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (camara == null) return;
        // El sprite siempre encara a la cámara
        transform.forward = camara.forward;
    }
}
