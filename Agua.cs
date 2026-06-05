using UnityEngine;

public class Agua : MonoBehaviour
{
    public float cantidadAgua = 40f;

    public float aguaMaxima = 100f;

    public float nutrientes = 15f;

    public float regeneracionAgua = 0.2f;

    public float velocidadCambio = 5f;

    public Vector3 tamañoMinimo =
        new Vector3(1f, 0.2f, 1f);

    public Vector3 tamañoMaximo =
        new Vector3(4f, 0.5f, 4f);

    void Update()
    {
        cantidadAgua +=
            regeneracionAgua *
            Time.deltaTime;

        cantidadAgua =
            Mathf.Clamp(
                cantidadAgua,
                0,
                aguaMaxima
            );

        ActualizarTamaño();
    }

    void ActualizarTamaño()
    {
        float porcentaje =
            cantidadAgua / aguaMaxima;

        Vector3 tamañoObjetivo =
            Vector3.Lerp(
                tamañoMinimo,
                tamañoMaximo,
                porcentaje
            );

        transform.localScale =
            Vector3.Lerp(
                transform.localScale,
                tamañoObjetivo,
                Time.deltaTime *
                velocidadCambio
            );
    }
}