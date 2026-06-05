using UnityEngine;

public class GeneradorPlantas : MonoBehaviour
{
    public GameObject prefabPlanta;

    public Transform sueloBase;

    public float tamañoAreaX = 40f;

    public float tamañoAreaZ = 40f;

    public float tiempoGeneracion = 20f;

    public int maximoPlantas = 20;

    private float cronometro;

    void Update()
    {
        cronometro += Time.deltaTime;

        if (cronometro >= tiempoGeneracion)
        {
            GenerarPlanta();

            cronometro = 0f;
        }
    }

    void GenerarPlanta()
    {
        Planta[] plantas =
            FindObjectsByType<Planta>(
                FindObjectsSortMode.None
            );

        if (plantas.Length >= maximoPlantas)
            return;

        float x =
            Random.Range(
                -tamañoAreaX / 2f,
                tamañoAreaX / 2f
            );

        float z =
            Random.Range(
                -tamañoAreaZ / 2f,
                tamañoAreaZ / 2f
            );

        Vector3 posicion =
            new Vector3(
                x,
                sueloBase.position.y + 1f,
                z
            );

        Instantiate(
            prefabPlanta,
            posicion,
            Quaternion.identity
        );
    }
}
