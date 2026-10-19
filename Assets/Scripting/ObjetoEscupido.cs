using UnityEngine;

public class ObjetoEscupido : MonoBehaviour
{
    public float tiempoVida = 5f; // Segundos antes de desaparecer

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }
}
