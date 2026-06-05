using UnityEngine;

public class Hoja : MonoBehaviour
{
    public float nutrientes = 10f;

    public float tiempoDescomposicion = 50f;

    [Range(0f, 1f)]
    public float probabilidadGerminacion = 0.10f;

    public GameObject prefabPlanta;

    void Start()
    {
        Invoke(
            nameof(Descomponerse),
            tiempoDescomposicion
        );
    }

    void Descomponerse()
    {
        if (
            prefabPlanta != null
            && Random.value <
            probabilidadGerminacion
        )
        {
            GameObject nuevaPlanta =
                Instantiate(
                    prefabPlanta,
                    transform.position,
                    Quaternion.identity
                );

            Planta planta =
                nuevaPlanta.GetComponent<Planta>();

            if (planta != null)
            {
                planta.nutrientes = 1f;
            }
        }

        Destroy(gameObject);
    }
}