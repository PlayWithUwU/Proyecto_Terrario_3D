using UnityEngine;

public class TierraFusionVisual : MonoBehaviour
{
    public float nutrientes = 50f;

    public void EfectoFusion(Vector3 posicion)
    {
        Debug.Log(
            "Fusión en: " + posicion +
            " | Nutrientes: " + nutrientes
        );
    }
}