using UnityEngine;

[System.Serializable]
public class ObjetoGuardado
{
    public GameObject prefab;  // Lo que se escupe
    public float peso;         // Lo que pesa

    public ObjetoGuardado(GameObject prefab, float peso)
    {
        this.prefab = prefab;
        this.peso = peso;
    }
}
